using System;
using System.Collections.Generic;
using System.Text;

namespace DawnmakuEngine.Data.JSONFormats
{
    public class JSONBezier
    {
        public float scale = 100;
        public bool autoSetPoints = false;
        
        public List<Point> pointsList = new List<Point>();

        public class Point
        {
            public JSONVec2 pos;
            public float time;
            public float waitTime;
        }
    }
}
