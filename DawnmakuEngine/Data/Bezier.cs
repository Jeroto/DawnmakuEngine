using System;
using System.Collections.Generic;
using System.Text;
using OpenTK;
using static DawnmakuEngine.DawnMath;
using OpenTK.Mathematics;

namespace DawnmakuEngine.Data
{
    public class Bezier
    {
        public List<Point> points;
        public float scale = 100;
        public bool autoSetPoints;

        [System.Serializable]
        public class Point
        {
            public Vector2 pos;
            public float time;
            public ushort waitTime;
            public float size;

            public Point(Vector2 pos_, float time_, ushort waitTime_ = 0, float size_ = 1)
            {
                pos = pos_;
                time = time_;
                waitTime = waitTime_;
                size = size_;
            }
        }

        public Bezier() { }

        public Bezier(Vector2 center)
        {
            points = new List<Point>
            {
                new Point(center + vec2Left, 0),
                new Point(center +(vec2Left + vec2Up) * 0.5f, 0),
                new Point(center+(vec2Right + vec2Down) * 0.5f, 0),
                new Point(center + vec2Right, 0.5f),
            };
            GameMaster.Log("Instantiated with " + NumPoints + " points");
        }

        public Point this[int i]
        {
            get
            {
                return points[i];
            }
        }

        public bool AutoSetPoints
        {
            get { return autoSetPoints; }
            set
            {
                if (autoSetPoints != value)
                {
                    autoSetPoints = value;
                    if (autoSetPoints)
                        AutoSetAllCtrlPoints();
                }
            }
        }

        public int NumPoints
        {
            get
            {
                return points.Count;
            }
        }

        public int NumSegments
        {
            get
            {
                return (points.Count - 4) / 3 + 1;
            }
        }

        public void AddSegment(Vector2 anchorPos)
        {
            float newTime = points[points.Count - 1].time + 0.5f;
            points.Add(new Point(points[points.Count - 1].pos * 2 - points[points.Count - 2].pos, newTime));
            points.Add(new Point((points[points.Count - 1].pos + anchorPos) * 0.5f, newTime));
            points.Add(new Point(anchorPos, newTime));

            if (AutoSetPoints)
                AutoSetAffectedCtrl(points.Count - 1);
        }

        public void SplitSegment(Vector2 anchorPos, int segIndex)
        {
            float newTime = (GetPointsInSegment(segIndex)[0].time + GetPointsInSegment(segIndex)[3].time) / 2;
            points.InsertRange(segIndex * 3 + 2, new Point[]
            { new Point(Vector2.Zero, newTime), new Point(anchorPos, newTime), new Point(Vector2.Zero, newTime) });


            if (AutoSetPoints)
                AutoSetAffectedCtrl(segIndex * 3 + 3);
            else
                AutoSetCtrlPoints(segIndex * 3 + 3);
        }

        public void DeleteSegment(int anchorIndex)
        {
            if (NumSegments <= 1)
                return;

            bool end = anchorIndex == points.Count - 1, start = anchorIndex == 0;

            if (anchorIndex == 0)
            {
                points.RemoveRange(0, 3);
            }
            else if (anchorIndex == points.Count - 1)
            {
                points.RemoveRange(anchorIndex - 2, 3);
            }
            else
            {
                points.RemoveRange(anchorIndex - 1, 3);
            }

            if (end)
            {
                AutoSetCtrlPoints(anchorIndex - 3);
                AutoSetStartAndEndCtrl();
            }
            else if (start)
            {
                AutoSetCtrlPoints(anchorIndex + 3);
                AutoSetStartAndEndCtrl();
            }
            else
            {
                AutoSetCtrlPoints(anchorIndex);
                AutoSetCtrlPoints(anchorIndex + 3);
            }
        }

        public Point[] GetPointsInSegment(int i)
        {
            return new Point[] { points[i * 3], points[i * 3 + 1], points[i * 3 + 2], points[i * 3 + 3] };
        }

        public float StartWaitTime(int i) { return points[i * 3].waitTime; }
        public float StartTimeOfSegment(int i) { return points[i * 3].time; }
        public float EndTimeOfSegment(int i) { return points[i * 3 + 3].time; }

        public void MovePoint(int i, Vector2 pos)
        {
            Vector2 moveDist = pos - points[i].pos;
            if (i % 3 == 0 || !autoSetPoints)
            {
                points[i].pos = pos;

                if (AutoSetPoints)
                {
                    AutoSetAffectedCtrl(i);
                    return;
                }
                else
                {
                    if (i % 3 == 0)
                    {

                        if (i + 1 < points.Count)
                            points[i + 1].pos += moveDist;
                        if (i - 1 >= 0)
                            points[i - 1].pos += moveDist;
                    }
                    else
                    {
                        bool nextPointAnchor = (i + 1) % 3 == 0;
                        int otherControl = nextPointAnchor ? i + 2 : i - 2;
                        int anchorIndex = (nextPointAnchor) ? i + 1 : i - 1;

                        if (otherControl >= 0 && otherControl < points.Count)
                        {
                            float dist = (points[anchorIndex].pos - points[otherControl].pos).Length;
                            Vector2 dir = (points[anchorIndex].pos - pos).Normalized();
                            points[otherControl].pos = points[anchorIndex].pos + dir * dist;
                        }
                    }
                }
            }
        }

        void AutoSetAffectedCtrl(int anchorIndex)
        {
            for (int i = anchorIndex - 3; i <= anchorIndex + 3; i += 3)
            {
                if (i >= 0 && i < points.Count)
                    AutoSetCtrlPoints(i);
            }

            AutoSetStartAndEndCtrl();
        }

        void AutoSetAllCtrlPoints()
        {
            for (int i = 0; i < points.Count; i += 3)
            {
                AutoSetCtrlPoints(i);
            }

            AutoSetStartAndEndCtrl();
        }

        void AutoSetCtrlPoints(int anchorIndex)
        {
            Vector2 anchorPos = points[anchorIndex].pos;
            Vector2 dir = Vector2.Zero;
            float[] neighborDist = new float[2];
            if (anchorIndex - 3 >= 0)
            {
                Vector2 offset = points[anchorIndex - 3].pos - anchorPos;
                dir += offset.Normalized();
                neighborDist[0] = offset.Length;
            }
            if (anchorIndex + 3 < points.Count)
            {
                Vector2 offset = points[anchorIndex + 3].pos - anchorPos;
                dir -= offset.Normalized();
                neighborDist[1] = -offset.Length;
            }
            dir.Normalize();
            for (int i = 0; i < 2; i++)
            {
                int ctrlIndex = anchorIndex + i * 2 - 1;
                if (ctrlIndex >= 0 && ctrlIndex < points.Count)
                    points[ctrlIndex].pos = anchorPos + dir * neighborDist[i] * 0.5f;
            }
        }

        void AutoSetStartAndEndCtrl()
        {
            points[1].pos = (points[0].pos + points[2].pos) * 0.5f;
            points[points.Count - 2].pos = (points[points.Count - 1].pos + points[points.Count - 3].pos) * 0.5f;
        }


        public static Vector2 QuadraticCurve(Vector2 a, Vector2 b, Vector2 c, float t)
        {
            Vector2 p0 = Lerp(a, b, t);
            Vector2 p1 = Lerp(b, c, t);
            return Lerp(p0, p1, t);
        }
        public static Vector2 CubicCurve(Vector2 a, Vector2 b, Vector2 c, Vector2 d, float t)
        {
            Vector2 p0 = QuadraticCurve(a, b, c, t);
            Vector2 p1 = QuadraticCurve(b, c, d, t);
            return Lerp(p0, p1, t);
        }
        /// <summary>
        /// Slower than PositionOnSegment, but requires less stored variables
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public Vector2 PositionOnCurve(float time)
        {
            Point[] segmentPoints = new Point[4];
            float nextTime = 1;
            for (int i = 0; i < NumPoints; i += 3)
            {
                if(time < points[i].time)
                {
                    nextTime = points[i].time;
                    i -= 3;

                    time -= points[i].time;
                    for (int p = 0; p < 4; p++)
                    {
                        segmentPoints[p] = points[i + p];
                    }
                    break;
                }
            }
            if (nextTime <= 0)
                nextTime = 1;

            return CubicCurve(segmentPoints[0].pos, segmentPoints[1].pos, segmentPoints[2].pos, segmentPoints[3].pos, time / nextTime) * scale;
        }
        /// <summary>
        /// Faster than PositionOnCurve(), but requires more storing of variables from calling object
        /// </summary>
        /// <param name="segment"></param>
        /// <returns></returns>
        public Vector2 PositionOnSegment(int segment, float time, bool overallTime)
        {
            Point[] segmentPoints = GetPointsInSegment(segment);
            float nextTime = segmentPoints[3].time;
            if (overallTime)
            {
                time -= segmentPoints[0].time;
                nextTime -= segmentPoints[0].time;
            }
            return CubicCurve(segmentPoints[0].pos, segmentPoints[1].pos, segmentPoints[2].pos, segmentPoints[3].pos, time / nextTime) * scale;
        }
    }
}
