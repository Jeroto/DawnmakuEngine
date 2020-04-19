using System;
using System.Collections.Generic;
using System.Text;
using OpenTK;

namespace DawnmakuEngine
{
    public static class DawnMath
    { //Note: All angles are calculated with 0 degrees being the vector 0,1,0 -- straight up
        public static Vector2 upDir = new Vector2(0, 1);

        public static float Repeat(float curVal, float maxVal, float minVal = 0)
        {
            while(curVal < minVal)
                curVal += maxVal;
            while (curVal > maxVal)
                curVal -= maxVal;
            return curVal;
        }
        public static int Repeat(int curVal, int maxVal, int minVal = 0)
        {
            while (curVal < minVal)
                curVal += maxVal;
            while (curVal > maxVal)
                curVal -= maxVal;
            return curVal;
        }
        public static long Repeat(long curVal, long maxVal, long minVal = 0)
        {
            while (curVal < minVal)
                curVal += maxVal;
            while (curVal > maxVal)
                curVal -= maxVal;
            return curVal;
        }

        public static int Round(float value)
        {
            return (int)Math.Round(value);
        }
        public static int Floor(float value)
        {
            return (int)Math.Floor(value);
        }
        public static int Ceil(float value)
        {
            return (int)Math.Ceiling(value);
        }

        public static float Sin(float angle)
        {
            return MathF.Sin(angle);
        }
        public static float Cos(float angle)
        {
            return MathF.Cos(angle);
        }
        public static int SinRounded(float angle)
        {
            return Round(Sin(angle));
        }
        public static int CosRounded(float angle)
        {
            return Round(Cos(angle));
        }

        public static float Lerp(float start, float end, float t)
        {
            t = Math.Clamp(t, 0, 1);
            return start * (1 - t) + end * t;
        }
        public static int LerpInt(int start, int end, float t)
        {
            t = Math.Clamp(t, 0, 1);
            return Round(start * (1 - t) + end * t);
        }
        public static Vector2 Lerp(Vector2 start, Vector2 end, float t)
        {
            t = Math.Clamp(t, 0, 1);
            start.X = start.X * (1 - t) + end.X * t;
            start.Y = start.Y * (1 - t) + end.Y * t;
            return start;
        }
        public static Vector3 Lerp(Vector3 start, Vector3 end, float t)
        {
            t = Math.Clamp(t, 0, 1);
            start.X = start.X * (1 - t) + end.X * t;
            start.Y = start.Y * (1 - t) + end.Y * t;
            start.Z = start.Z * (1 - t) + end.Z * t;
            return start;
        }
        public static Vector4 Lerp(Vector4 start, Vector4 end, float t)
        {
            t = Math.Clamp(t, 0, 1);
            start.X = start.X * (1 - t) + end.X * t;
            start.Y = start.Y * (1 - t) + end.Y * t;
            start.Z = start.Z * (1 - t) + end.Z * t;
            start.W = start.W * (1 - t) + end.W * t;
            return start;
        }


        public static float Dot(Vector2 first, Vector2 second)
        {
            return first.X * second.X + first.Y * second.Y;
        }

        public static float GetUnsignedAngleRad(Vector2 start, Vector2 end)
        {
            float dem = (float)Math.Sqrt(start.LengthFast * end.LengthFast);
            if (dem.CompareTo(0) == 0)
                return 0;
            float dot = Math.Clamp(Dot(start, end) / dem, -1f, 1f);

            return MathF.Acos(dot);
        }
        public static float GetSignedAngleRad(Vector2 start, Vector2 end)
        {
            return GetUnsignedAngleRad(start, end) * Math.Sign(start.X * end.Y - start.Y * end.X);
        }

        public static float GetUnsignedAngleDeg(Vector2 start, Vector2 end)
        {
            float dem = (float)Math.Sqrt(start.LengthFast * end.LengthFast);
            if (dem.CompareTo(0) == 0)
                return 0;
            float dot = Math.Clamp(Dot(start, end) / dem, -1f, 1f);

            return MathHelper.RadiansToDegrees(MathF.Acos(dot));
        }
        public static float GetSignedAngleDeg(Vector2 start, Vector2 end)
        {
            return MathHelper.RadiansToDegrees(GetUnsignedAngleRad(start, end) * Math.Sign(start.X * end.Y - start.Y * end.X));
        }


        //Calculates a position in a circle by generating a random angle value with a set radius and centerpoint
        public static Vector2 RandomCircle(Vector3 center, float radius)
        {
            //Creates random angle between 0 and 360 (0 and 2pi)
            float ang = GameMaster.Random(0f, 1f) * MathHelper.TwoPi;
            Vector2 pos;
            //Calculates the sin for the x position and cos for the y, multiplies it by the radius, and then adds the center x and y to get it to the correct point
            pos.X = center.X + radius * Sin(ang);
            pos.Y = center.Y + radius * Cos(ang);
            return pos;
        }

        //Same as above, except the center point is 0,0 -- good for if you only need relative coordinates
        public static Vector2 RandomCircle(float radius)
        {
            //Creates random angle between 0 and 360
            float ang = GameMaster.Random(0f, 1f) * 360;
            Vector2 pos;
            //Calculates the sin for the x position and cos for the y, and then multiplies it by the radius to get it to the correct point
            pos.X = radius * Sin(ang );
            pos.Y = radius * Cos(ang);
            return pos;
        }

        //Same as above, except the center point is 0,0 and the radius is 1 -- good for if you only need the direction
        public static Vector2 RandomCircle()
        {
            //Creates random angle between 0 and 360
            float ang = GameMaster.Random(0f, 1f) * 360;
            Vector2 pos;
            //Calculates the sin for the x position and the cos for the y
            pos.X = Sin(ang);
            pos.Y = Cos(ang);
            return pos;
        }

        //Calculates a position in a circle by taking an exact angle and uses a centerpoint and radius
        public static Vector2 CalculateCircleRad(Vector2 center, float angle, float radius = 1)
        {
            Vector2 pos;
            //Calculates the sin for the x position and cos for the y, multiplies it by the radius, and then adds the center x and y to get it to the correct point
            pos.X = center.X + radius * Sin(angle);
            pos.Y = center.Y + radius * Cos(angle);
            return pos;
        }

        //Same as above, except the center point is 0,0 -- good for if you only need relative coordinates 
        public static Vector2 CalculateCircleRad(float angle, float radius = 1)
        {
            Vector2 pos;
            //Calculates the sin for the x position and cos for the y, and then multiplies it by the radius to get it to the correct point
            pos.X = radius * Sin(angle);
            pos.Y = radius * Cos(angle);
            return pos;
        }

        //Calculates a position in a circle by taking an exact angle and uses a centerpoint and radius
        public static Vector2 CalculateCircleDeg(Vector2 center, float angle, float radius = 1)
        {
            angle = MathHelper.DegreesToRadians(angle);
            Vector2 pos;
            //Calculates the sin for the x position and cos for the y, multiplies it by the radius, and then adds the center x and y to get it to the correct point
            pos.X = center.X + radius * Sin(angle);
            pos.Y = center.Y + radius * Cos(angle);
            return pos;
        }

        //Same as above, except the center point is 0,0 -- good for if you only need relative coordinates 
        public static Vector2 CalculateCircleDeg(float angle, float radius = 1)
        {
            angle = MathHelper.DegreesToRadians(angle);
            Vector2 pos;
            //Calculates the sin for the x position and cos for the y, and then multiplies it by the radius to get it to the correct point
            pos.X = radius * Sin(angle);
            pos.Y = radius * Cos(angle);
            return pos;
        }


        //Finds the direction to the center of the play area -- change the vector value as needed
        public static Vector2 FindDirectionToCenter(Vector2 startingPosition)
        {
            return -startingPosition.Normalized();
        }

        //Uses the above function to find the direction, and then converts it to an angle
        public static float FindAngleToCenterRad(Vector2 startingPosition)
        {
            Vector2 directionToCenter = FindDirectionToCenter(startingPosition);
            return GetSignedAngleRad(directionToCenter, upDir);
        }
        //Uses the above function to find the direction, and then converts it to an angle
        public static float FindAngleToCenterDeg(Vector2 startingPosition)
        {
            Vector2 directionToCenter = FindDirectionToCenter(startingPosition);
            return GetSignedAngleDeg(directionToCenter, upDir);
        }

        //Finds the direction to the player object stored in the GameMaster
        public static Vector2 FindDirectionToPlayer(Vector2 startingPosition)
        {
            return (GameMaster.gameMaster.playerEntity.WorldPosition.Xy - startingPosition).Normalized();
        }

        //Uses the above function to find the angle to the player object stored in the GameMaster
        public static float FindAngleToPlayerRad(Vector2 startingPosition)
        {
            Vector2 directionToPlayer = FindDirectionToPlayer(startingPosition);
            return GetSignedAngleRad(directionToPlayer, upDir);
        }
        //Uses the above function to find the angle to the player object stored in the GameMaster
        public static float FindAngleToPlayerDeg(Vector2 startingPosition)
        {
            Vector2 directionToPlayer = FindDirectionToPlayer(startingPosition);
            return GetSignedAngleDeg(directionToPlayer, upDir);
        }

        //Finds the direction between an object's x and z and the starting position's x and z -- mainly used for background effects and turning sprites towards the camera
        public static Vector3 FindDirectionToObject3D(Vector3 startingPosition, Entity otherObject)
        {
            return (new Vector3(otherObject.WorldPosition.X, 0, otherObject.WorldPosition.Z) - 
                new Vector3(startingPosition.X, 0, startingPosition.Z)).Normalized();
        }

        //Uses the transform of an object to find the direction towards it from the starting position
        public static Vector2 FindDirectionToObject(Vector2 startingPosition, Entity otherObject)
        {
            return (otherObject.WorldPosition.Xy - startingPosition).Normalized();
        }

        //Uses the function above to find the angle towards another object from the starting position
        public static float FindAngleToObjectRad(Vector2 startingPosition, Entity otherObject)
        {
            Vector2 directionToObject = FindDirectionToObject(startingPosition, otherObject);
            return GetSignedAngleRad(directionToObject, upDir);
        }

        //Finds the angle between two positions
        public static float FindAngleRad(Vector2 startPoint, Vector2 endPoint)
        {
            Vector2 direction = endPoint - startPoint;
            return GetSignedAngleRad(direction, upDir);
        }

        //Finds the angle difference between two directions
        public static float FindAngleBetweenRad(Vector2 startPoint, Vector2 endPoint)
        {
            return GetSignedAngleRad(startPoint, endPoint);
        }

        //Finds the angle of the direction, based off of 0,1
        public static float FindAngleRad(Vector2 direction)
        {
            return GetSignedAngleRad(direction,upDir);
        }

        public static Vector3 QuaternionToEuler(Quaternion quat)
        {
            Vector3 euler = new Vector3();
            euler.X = MathF.Atan2(2 * (quat.W * quat.X + quat.Y * quat.Z), 1 - 2 * (quat.X * quat.X + quat.Y * quat.Y));

            float Yval = 2 * (quat.W * quat.Y - quat.Z * quat.X);
            if (Math.Abs(Yval) >= 1)
                euler.Y = MathF.CopySign(MathHelper.PiOver2, Yval);
            else
                euler.Y = MathF.Asin(Yval);

            euler.Z = MathF.Atan2(2 * (quat.W * quat.Z + quat.X * quat.Y), 1 - 2 * (quat.Y * quat.Y + quat.Z * quat.Z));

            return euler;
        }


        //Uses the function above to find the angle towards another object from the starting position
        public static float FindAngleToObjectDeg(Vector2 startingPosition, Entity otherObject)
        {
            Vector2 directionToObject = FindDirectionToObject(startingPosition, otherObject);
            return GetSignedAngleDeg(directionToObject, upDir);
        }

        //Finds the angle between two positions
        public static float FindAngleDeg(Vector2 startPoint, Vector2 endPoint)
        {
            Vector2 direction = endPoint - startPoint;
            return GetSignedAngleDeg(direction, upDir);
        }

        //Finds the angle difference between two directions
        public static float FindAngleBetweenDeg(Vector2 startPoint, Vector2 endPoint)
        {
            return GetSignedAngleDeg(startPoint, endPoint);
        }

        //Finds the angle of the direction, based off of 0,1
        public static float FindAngleDeg(Vector2 direction)
        {
            return GetSignedAngleDeg(direction, upDir);
        }



        public static Entity FindNearestEntity(Vector2 worldPos, float maxDist = float.MaxValue,  Entity thisObject = null)
        {
            int nearest = -1, i, entities = Entity.allEntities.Count;
            float dist, shortestDist = maxDist;
            for (i = 0; i < entities; i++)
            {
                dist = Vector2.Distance(worldPos, Entity.allEntities[i].WorldPosition.Xy);
                if (dist < shortestDist)
                    if (Entity.allEntities[i] != thisObject)
                    {
                        nearest = i;
                        shortestDist = dist;
                    }
            }
            if (nearest != -1)
                return Entity.allEntities[nearest];
            else
                return null;
        }

        //Runs through every enemy position under the enemy container, and finds the nearest to the position within a maximum distance of the max radius
        //And returns the shortest distance as an out
        //This accounts for bosses under the if statements below the main for loop
        /* public static Transform FindNearestEnemy(Vector2 position, float maxRadius, out float shortestDist)
         {
             shortestDist = float.MaxValue;
             float tempDist;
             Transform newTarget = null;
             GameMaster gameMaster = GameMaster.gameMaster;
             Transform enemyContainer = gameMaster.enemyContainer;
             int childCount = enemyContainer.childCount;

             for (int i = 0; i < childCount; i++)
             {
                 tempDist = Vector2.Distance(position, enemyContainer.GetChild(i).position);
                 if (tempDist <= maxRadius && tempDist < shortestDist)
                 {
                     newTarget = enemyContainer.GetChild(i);
                     shortestDist = tempDist;
                 }
             }

             //Finds nearest distance to any midboss while fighting the midboss -- is set up to allow for multiple-character midbosses
             if (gameMaster.stageScript.fightingMidboss)
             {
                 BaseBossScript[] midbosses;
                 midbosses = gameMaster.stageScript.midbossSpawn.GetComponentsInChildren<BaseBossScript>();
                 Transform thisBoss;
                 for (int i = 0; i < midbosses.Length; i++)
                 {
                     thisBoss = midbosses[i].transform;
                     tempDist = Vector2.Distance(thisBoss.position, position);
                     if (tempDist <= maxRadius && tempDist <= shortestDist)
                     {
                         newTarget = thisBoss;
                         shortestDist = tempDist;
                     }
                 }
             }

             //Finds nearest distance to any boss while fighting the boss -- is set up to allow for multiple-character bosses
             if (gameMaster.stageScript.fightingBoss)
             {
                 BaseBossScript[] bosses;
                 bosses = gameMaster.stageScript.bossSpawn.GetComponentsInChildren<BaseBossScript>();
                 Transform thisBoss;
                 for (int i = 0; i < bosses.Length; i++)
                 {
                     thisBoss = bosses[i].transform;
                     tempDist = Vector2.Distance(thisBoss.position, position);
                     if (tempDist <= maxRadius && tempDist <= shortestDist)
                     {
                         newTarget = thisBoss;
                         shortestDist = tempDist;
                     }
                 }
             }

             //Returns the nearest enemy's transform
             return newTarget;
         }
         */
        //Same the above function, and even uses it, but doesn't return the shortest distance
        /*public static Transform FindNearestEnemy(Vector2 position, float maxRadius)
        {
            float outDeletion;
            return FindNearestEnemy(position, maxRadius, out outDeletion);
        }*/


        //Runs through every enemy position under the enemy container, and finds the nearest to the position
        //And returns the shortest distance as an out
        //This accounts for bosses under the if statements below the main for loop
        /*public static Transform FindNearestEnemy(Vector2 position, out float shortestDist)
        {
            shortestDist = float.MaxValue;
            float tempDist;
            Transform newTarget = null;
            GameMaster gameMaster = GameMaster.gameMaster;
            Transform enemyContainer = gameMaster.enemyContainer;
            int childCount = enemyContainer.childCount;

            for (int i = 0; i < childCount; i++)
            {
                tempDist = Vector2.Distance(position, enemyContainer.GetChild(i).position);
                if (tempDist < shortestDist)
                {
                    newTarget = enemyContainer.GetChild(i);
                    shortestDist = tempDist;
                }
            }

            //Finds nearest distance to any midboss while fighting the midboss -- is set up to allow for multiple-character midbosses
            if (gameMaster.stageScript.fightingMidboss)
            {
                BaseBossScript[] midbosses;
                midbosses = gameMaster.stageScript.midbossSpawn.GetComponentsInChildren<BaseBossScript>();
                Transform thisBoss;
                for (int i = 0; i < midbosses.Length; i++)
                {
                    thisBoss = midbosses[i].transform;
                    tempDist = Vector2.Distance(thisBoss.position, position);
                    if (tempDist <= shortestDist)
                    {
                        newTarget = thisBoss;
                        shortestDist = tempDist;
                    }
                }
            }

            //Finds nearest distance to any boss while fighting the boss -- is set up to allow for multiple-character bosses
            if (gameMaster.stageScript.fightingBoss)
            {
                BaseBossScript[] bosses;
                bosses = gameMaster.stageScript.bossSpawn.GetComponentsInChildren<BaseBossScript>();
                Transform thisBoss;
                for (int i = 0; i < bosses.Length; i++)
                {
                    thisBoss = bosses[i].transform;
                    tempDist = Vector2.Distance(thisBoss.position, position);
                    if (tempDist <= shortestDist)
                    {
                        newTarget = thisBoss;
                        shortestDist = tempDist;
                    }
                }
            }

            return newTarget;
        }*/

        //Same the above function, and even uses it, but doesn't return the shortest distance
        /*public static Transform FindNearestEnemy(Vector2 position)
        {
            float outDeletion;
            return FindNearestEnemy(position, out outDeletion);
        }*/
    }
}
