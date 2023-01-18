using System;
using System.Collections.Generic;
using System.Text;
using DawnmakuEngine.Elements;
using OpenTK;
using OpenTK.Mathematics;

namespace DawnmakuEngine
{
    public static class DawnMath
    { //Note: All angles are calculated with 0 degrees being the vector 0,1,0 -- straight up
        /// <summary>Vector2(0,1)</summary>
        public static Vector2 vec2Up = new Vector2(0, 1);
        /// <summary>Vector2(1,0)</summary>
        public static Vector2 vec2Right = new Vector2(1, 0);
        /// <summary>Vector2(0,-1)</summary>
        public static Vector2 vec2Down = new Vector2(0, -1);
        /// <summary>Vector2(-1,0)</summary>
        public static Vector2 vec2Left = new Vector2(-1,0);

        /// <summary>Vector3(0,1,0)</summary>
        public static Vector3 vec3Up = new Vector3(0, 1, 0);
        /// <summary>Vector3(1,0,0)</summary>
        public static Vector3 vec3Right = new Vector3(1, 0, 0);
        /// <summary>Vector3(0,-1,0)</summary>
        public static Vector3 vec3Down = new Vector3(0, -1, 0);
        /// <summary>Vector3(-1,0,0)</summary>
        public static Vector3 vec3Left = new Vector3(-1, 0, 0);
        /// <summary>Vector3(0,0,-1)</summary>
        public static Vector3 vec3Forward = new Vector3(0, 0, -1);
        /// <summary>Vector3(0,0,1)</summary>
        public static Vector3 vec3Backward = new Vector3(0, 0, 1);

        /// <summary>
        /// Repeats a float value, keeping it at or below the max value 
        /// and at or below the minimum value (default minVal = 0)
        /// </summary>
        public static float Repeat(float curVal, float maxVal, float minVal = 0)
        {
            while (curVal < minVal)
                curVal = Math.Abs(curVal - (minVal - 1)) + maxVal;

            while (curVal > maxVal)
                curVal = Math.Abs(curVal - (maxVal + 1)) + minVal;

            /*while(curVal < minVal)
                curVal += maxVal;
            while (curVal > maxVal)
                curVal -= maxVal;*/
            return curVal;
        }
        /// <summary>
        /// Repeats an integer value, keeping it at or below the max value 
        /// and at or below the minimum value (default minVal = 0)
        /// </summary>
        public static int Repeat(int curVal, int maxVal, int minVal = 0)
        {
            while (curVal < minVal)
                curVal = Math.Abs(curVal - (minVal - 1)) + maxVal;

            while (curVal > maxVal)
                curVal = Math.Abs(curVal - (maxVal + 1)) + minVal;
            return curVal;
        }
        /// <summary>
        /// Repeats a long value, keeping it at or below the max value 
        /// and at or below the minimum value (default minVal = 0)
        /// </summary>
        public static long Repeat(long curVal, long maxVal, long minVal = 0)
        {
            while (curVal < minVal)
                curVal = Math.Abs(curVal - (minVal - 1)) + maxVal;

            while (curVal > maxVal)
                curVal = Math.Abs(curVal - (maxVal + 1)) + minVal;
            /*while (curVal < minVal)
                curVal += maxVal;
            while (curVal > maxVal)
                curVal -= maxVal;*/
            return curVal;
        }
        /// <summary>
        /// Rounds a float to the nearest integer, and returns as an integer
        /// </summary>
        public static int Round(float value)
        {
            return (int)Math.Round(value);
        }
        /// <summary>
        /// Rounds a float down to the nearest integer, and returns as an integer
        /// </summary>
        public static int Floor(float value)
        {
            return (int)Math.Floor(value);
        }
        /// <summary>
        /// Rounds a float up to the nearest integer, and returns as an integer
        /// </summary>
        public static int Ceil(float value)
        {
            return (int)Math.Ceiling(value);
        }

        /// <summary>
        /// returns the sine of an angle
        /// </summary>
        public static float Sin(float angle)
        {
            return MathF.Sin(angle);
        }
        /// <summary>
        /// returns the cosine of an angle
        /// </summary>
        public static float Cos(float angle)
        {
            return MathF.Cos(angle);
        }
        /// <summary>
        /// returns the sine of an angle, rounded to the nearest integer
        /// </summary>
        public static int SinRounded(float angle)
        {
            return Round(Sin(angle));
        }
        /// <summary>
        /// returns the cosine of an angle, rounded to the nearest integer
        /// </summary>
        public static int CosRounded(float angle)
        {
            return Round(Cos(angle));
        }

        /// <summary>
        /// Linearly interpolates between a start and end float by t, which should remain at or between 0 and 1
        /// </summary>
        public static float Lerp(float start, float end, float t)
        {
            t = Math.Clamp(t, 0, 1);
            return start * (1 - t) + end * t;
        }
        /// <summary>
        /// Linearly interpolates between a start and end int, rounded, by t, which should remain at or between 0 and 1
        /// </summary>
        public static int LerpInt(int start, int end, float t)
        {
            t = Math.Clamp(t, 0, 1);
            return Round(start * (1 - t) + end * t);
        }
        /// <summary>
        /// Linearly interpolates between a start and end two-component vector by t, which should remain at or between 0 and 1
        /// </summary>
        public static Vector2 Lerp(Vector2 start, Vector2 end, float t)
        {
            t = Math.Clamp(t, 0, 1);
            start.X = start.X * (1 - t) + end.X * t;
            start.Y = start.Y * (1 - t) + end.Y * t;
            return start;
        }
        /// <summary>
        /// Linearly interpolates between a start and end three-component vector by t, which should remain at or between 0 and 1
        /// </summary>
        public static Vector3 Lerp(Vector3 start, Vector3 end, float t)
        {
            t = Math.Clamp(t, 0, 1);
            start.X = start.X * (1 - t) + end.X * t;
            start.Y = start.Y * (1 - t) + end.Y * t;
            start.Z = start.Z * (1 - t) + end.Z * t;
            return start;
        }
        /// <summary>
        /// Linearly interpolates between a start and end four-component vector by t, which should remain at or between 0 and 1
        /// </summary>
        public static Vector4 Lerp(Vector4 start, Vector4 end, float t)
        {
            t = Math.Clamp(t, 0, 1);
            start.X = start.X * (1 - t) + end.X * t;
            start.Y = start.Y * (1 - t) + end.Y * t;
            start.Z = start.Z * (1 - t) + end.Z * t;
            start.W = start.W * (1 - t) + end.W * t;
            return start;
        }

        public static Vector3 ConvertColorToRgb(string format, Vector3 colorVal, bool returnIn255 = true)
        {

            switch (format.ToLower())
            {
                case "rgb":
                    colorVal /= 255;
                    return colorVal;
                case "rgb01":
                    return colorVal;
                case "hsv":
                    return HsvToRgb(colorVal, returnIn255);
                case "hsl":
                    return HslToRgb(colorVal, returnIn255);
                default:
                    GameMaster.LogWarningMessage("Color format \"" + format + "\" not supported. Please use rgb, rgb01, hsv, or hsl instead.",
                        "Defaulting color to rgb 255,255,255");
                    return Vector3.One;
            }
        }
        public static Vector3 HsvToRgb(Vector3 Hsv, bool returnIn255  = true)
        {
            Vector3 outputColor;
            float c, x, m;
            c = Hsv.Z * Hsv.Y;
            x = c * (1 - Math.Abs((Hsv.X / 60) % 2 - 1));
            m = Hsv.Z - c;

            if (Hsv.X <= 60)
                outputColor = new Vector3(c + m, x + m, m);
            else if (Hsv.X <= 120)
                outputColor = new Vector3(x + m, c + m, m);
            else if (Hsv.X <= 180)
                outputColor = new Vector3(m, c + m, x + m);
            else if (Hsv.X <= 240)
                outputColor = new Vector3(m, x + m, c + m);
            else if (Hsv.X <= 300)
                outputColor = new Vector3(x + m, m, c + m);
            else
                outputColor = new Vector3(c + m, m, x + m);
            return outputColor * (returnIn255 ? 255 : 1);
        }
        public static Vector3 HslToRgb(Vector3 Hsl, bool returnIn255 = true)
        {
            Vector3 outputColor;
            float c, x, m;
            c = (1 - Math.Abs(2 * Hsl.Z)) * Hsl.Y;
            x = c * (1 - Math.Abs((Hsl.X / 60) % 2 - 1));
            m = Hsl.Z - c / 2;

            if (Hsl.X <= 60)
                outputColor = new Vector3(c + m, x + m, m);
            else if (Hsl.X <= 120)
                outputColor = new Vector3(x + m, c + m, m);
            else if (Hsl.X <= 180)
                outputColor = new Vector3(m, c + m, x + m);
            else if (Hsl.X <= 240)
                outputColor = new Vector3(m, x + m, c + m);
            else if (Hsl.X <= 300)
                outputColor = new Vector3(x + m, m, c + m);
            else
                outputColor = new Vector3(c + m, m, x + m);
            return outputColor * (returnIn255 ? 255 : 1);
        }

        public static float EaseIn(float start, float end, float t, float inAmount = 0.25f)
        {
            float final;
            final = Lerp(start, end, t);
            if (t <= inAmount)
                final = Lerp(start, final, t / inAmount);
            return final;
        }
        public static Vector2 EaseIn(Vector2 start, Vector2 end, float t, float inAmount = 0.25f)
        {
            Vector2 final;
            final = Lerp(start, end, t);
            if (t <= inAmount)
                final = Lerp(start, final, t / inAmount);
            return final;
        }
        public static Vector3 EaseIn(Vector3 start, Vector3 end, float t, float inAmount = 0.25f)
        {
            Vector3 final;
            final = Lerp(start, end, t);
            if (t <= inAmount)
                final = Lerp(start, final, t / inAmount);
            return final;
        }
        public static Vector4 EaseIn(Vector4 start, Vector4 end, float t, float inAmount = 0.25f)
        {
            Vector4 final;
            final = Lerp(start, end, t);
            if (t <= inAmount)
                final = Lerp(start, final, t / inAmount);
            return final;
        }
        public static float EaseOut(float start, float end, float t, float outAmount = 0.75f)
        {
            float final;
            final = Lerp(start, end, t);
            if (t >= outAmount)
                final = Lerp(final, end, (t - outAmount) / (1 - outAmount));
            return final;
        }
        public static Vector2 EaseOut(Vector2 start, Vector2 end, float t, float outAmount = 0.75f)
        {
            Vector2 final;
            final = Lerp(start, end, t);
            if (t >= outAmount)
                final = Lerp(final, end, (t - outAmount) / (1 - outAmount));
            return final;
        }
        public static Vector3 EaseOut(Vector3 start, Vector3 end, float t, float outAmount = 0.75f)
        {
            Vector3 final;
            final = Lerp(start, end, t);
            if (t >= outAmount)
                final = Lerp(final, end, (t - outAmount) / (1 - outAmount));
            return final;
        }
        public static Vector4 EaseOut(Vector4 start, Vector4 end, float t, float outAmount = 0.75f)
        {
            Vector4 final;
            final = Lerp(start, end, t);
            if (t >= outAmount)
                final = Lerp(final, end, (t - outAmount) / (1 - outAmount));
            return final;
        }
        public static float EaseInOut(float start, float end, float t)
        {
            /*float final;
            final = Lerp(start, end, t);
            if (t <= inAmount)
                final = Lerp(start, final, t / inAmount);
            else if (t >= outAmount)
                final = Lerp(final, end, (t - outAmount) / (1 - outAmount));
            return final;*/
            float amount = -(MathF.Cos(MathHelper.Pi * t) - 1) / 2;
            return Lerp(start, end, amount);
        }
        public static Vector2 EaseInOut(Vector2 start, Vector2 end, float t)
        {
            /*Vector2 final;
            final = Lerp(start, end, t);
            if (t <= inAmount)
                final = Lerp(start, final, inAmount);
            else if (t >= outAmount)
                final = Lerp(final, end, (t - outAmount) / (1 - outAmount));
            return final;*/
            float amount = -(MathF.Cos(MathHelper.Pi * t) - 1) / 2;
            return Lerp(start, end, amount);
        }
        public static Vector3 EaseInOut(Vector3 start, Vector3 end, float t)
        {
            /*Vector3 final;
            final = Lerp(start, end, t);
            if (t <= inAmount)
                final = Lerp(start, final, t / inAmount);
            else if (t >= outAmount)
                final = Lerp(final, end, (t - outAmount) / (1 - outAmount));
            return final;*/
            float amount = -(MathF.Cos(MathHelper.Pi * t) - 1) / 2;
            return Lerp(start, end, amount);
        }
        public static Vector4 EaseInOut(Vector4 start, Vector4 end, float t)
        {
            /*Vector4 final;
            final = Lerp(start, end, t);
            if (t <= inAmount)
                final = Lerp(start, final, t / inAmount);
            else if (t >= outAmount)
                final = Lerp(final, end, (t - outAmount) / (1 - outAmount));
            return final;*/
            float amount = -(MathF.Cos(MathHelper.Pi * t) - 1) / 2;
            return Lerp(start, end, amount);
        }


        /// <summary>
        /// Returns the dot product of 2 two-component vectors
        /// </summary>
        public static float Dot(Vector2 first, Vector2 second)
        {
            return first.X * second.X + first.Y * second.Y;
        }

        /// <summary>
        /// Calculates the normal based on three clockwise points
        /// </summary>
        /// <param name="vertOne">First vertex position</param>
        /// <param name="vertTwo">Second vertex position</param>
        /// <param name="vertThree">Third vertex position</param>
        /// <returns></returns>
        public static Vector3 CalculateNormal(Vector3 vertOne, Vector3 vertTwo, Vector3 vertThree)
        {
            Vector3 a, b;
            a = vertTwo - vertOne;
            b = vertThree - vertOne;
            return Vector3.Cross(a, b).Normalized();
        }

        /// <summary>
        /// Gets the unsigned (only positive) angle difference between 2 two-component vectors in radians
        /// </summary>
        public static float GetUnsignedAngleRad(Vector2 start, Vector2 end)
        {
            float dem = (float)Math.Sqrt(start.LengthFast * end.LengthFast);
            if (dem.CompareTo(0) == 0)
                return 0;
            float dot = Math.Clamp(Dot(start, end) / dem, -1f, 1f);

            return MathF.Acos(dot);
        }
        /// <summary>
        /// Gets the signed (positive or negative) angle difference between 2 two-component vectors in radians
        /// </summary>
        public static float GetSignedAngleRad(Vector2 start, Vector2 end)
        {
            return GetUnsignedAngleRad(start, end) * Math.Sign(start.X * end.Y - start.Y * end.X);
        }

        /// <summary>
        /// Gets the unsigned (only positive) angle difference between 2 two-component vectors in degrees
        /// </summary>
        public static float GetUnsignedAngleDeg(Vector2 start, Vector2 end)
        {
            float dem = (float)Math.Sqrt(start.LengthFast * end.LengthFast);
            if (dem.CompareTo(0) == 0)
                return 0;
            float dot = Math.Clamp(Dot(start, end) / dem, -1f, 1f);

            return MathHelper.RadiansToDegrees(MathF.Acos(dot));
        }
        /// <summary>
        /// Gets the signed (positive or negative) angle difference between 2 two-component vectors in degrees
        /// </summary>
        public static float GetSignedAngleDeg(Vector2 start, Vector2 end)
        {
            return MathHelper.RadiansToDegrees(GetUnsignedAngleRad(start, end) * Math.Sign(start.X * end.Y - start.Y * end.X));
        }


        /// <summary>
        /// Calculates a position in a circle by generating a random angle value with a set radius and centerpoint
        /// Uses the direction 0,1 as 0 degrees.
        /// </summary>
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

        /// <summary>
        /// Calculates a position in a circle by generating a random angle value with a set radius and a centerpoint of 0,0.
        /// Uses the direction 0,1 as 0 degrees.
        /// Good for if you only need relative coordinates
        /// </summary>
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

        /// <summary>
        /// Calculates a position in a circle by generating a random angle value with a radius of 1 and a centerpoint of 0,0.
        /// Uses the direction 0,1 as 0 degrees.
        /// Good for if you only need the direction
        /// </summary>
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

        /// <summary>
        /// Calculates a position in a circle by taking an exact angle in radians and uses a centerpoint and radius (default = 1).
        /// Uses the direction 0,1 as 0 degrees
        /// </summary>
        public static Vector2 CalculateCircleRad(Vector2 center, float angle, float radius = 1)
        {
            Vector2 pos;
            //Calculates the sin for the x position and cos for the y, multiplies it by the radius, and then adds the center x and y to get it to the correct point
            pos.X = center.X + radius * Sin(angle);
            pos.Y = center.Y + radius * Cos(angle);
            return pos;
        }

        /// <summary>
        /// Calculates a position in a circle by taking an exact angle in raidians and uses a radius (default = 1) and a center of 0,0.
        /// Uses the direction 0,1 as 0 degrees.
        /// Good for if you only need relative coordinates
        /// </summary>
        public static Vector2 CalculateCircleRad(float angle, float radius = 1)
        {
            Vector2 pos;
            //Calculates the sin for the x position and cos for the y, and then multiplies it by the radius to get it to the correct point
            pos.X = radius * Sin(angle);
            pos.Y = radius * Cos(angle);
            return pos;
        }

        /// <summary>
        /// Calculates a position in a circle by taking an exact angle in raidians and uses a centerpoint and radius (default = 1).
        /// Uses the direction 0,1 as 0 degrees
        /// </summary>
        public static Vector2 CalculateCircleDeg(Vector2 center, float angle, float radius = 1)
        {
            angle = MathHelper.DegreesToRadians(angle);
            Vector2 pos;
            //Calculates the sin for the x position and cos for the y, multiplies it by the radius, and then adds the center x and y to get it to the correct point
            pos.X = center.X + radius * Sin(angle);
            pos.Y = center.Y + radius * Cos(angle);
            return pos;
        }

        /// <summary>
        /// Calculates a position in a circle by taking an exact angle in raidians and uses a radius (default = 1) and a center of 0,0.
        /// Uses the direction 0,1 as 0 degrees.
        /// Good for if you only need relative coordinates
        /// </summary>
        public static Vector2 CalculateCircleDeg(float angle, float radius = 1)
        {
            angle = MathHelper.DegreesToRadians(angle);
            Vector2 pos;
            //Calculates the sin for the x position and cos for the y, and then multiplies it by the radius to get it to the correct point
            pos.X = radius * Sin(angle);
            pos.Y = radius * Cos(angle);
            return pos;
        }

        /// <summary>
        /// Finds the direction to the center of the play area.
        /// [not yet implemented]
        /// </summary>
        public static Vector2 FindDirectionToCenter(Vector2 startingPosition)
        {
            return -startingPosition.Normalized();
        }

        /// <summary>
        /// Finds the angle in radians to the center of the play area.
        /// Uses FindDirectionToCenter() and converts result to an angle in radians, using the direction (0,1) as 0.
        /// [not yet implemented]
        /// </summary>
        public static float FindAngleToCenterRad(Vector2 startingPosition)
        {
            Vector2 directionToCenter = FindDirectionToCenter(startingPosition);
            return GetSignedAngleRad(directionToCenter, vec2Up);
        }
        /// <summary>
        /// Finds the angle in degrees to the center of the play area.
        /// Uses FindDirectionToCenter() and converts result to an angle in degrees, using the direction (0,1) as 0.
        /// [not yet implemented]
        /// </summary>
        public static float FindAngleToCenterDeg(Vector2 startingPosition)
        {
            Vector2 directionToCenter = FindDirectionToCenter(startingPosition);
            return GetSignedAngleDeg(directionToCenter, vec2Up);
        }

        /// <summary>
        /// Finds the direction to the player object stored in the GameMaster
        /// </summary>
        public static Vector2 FindDirectionToPlayer(Vector2 startingPosition)
        {
            return (GameMaster.gameMaster.playerWorldPos.Xy - startingPosition).Normalized();
        }

        /// <summary>
        /// Finds the angle in radians to the player object stored in the GameMaster, using the direction (0,1) as 0. 
        /// Uses FindDirectionToPlayer() and converts result to an angle in radians
        /// </summary>
        public static float FindAngleToPlayerRad(Vector2 startingPosition)
        {
            Vector2 directionToPlayer = FindDirectionToPlayer(startingPosition);
            return GetSignedAngleRad(directionToPlayer, vec2Up);
        }
        /// <summary>
        /// Finds the angle in degrees to the player object stored in the GameMaster, using the direction (0,1) as 0. 
        /// Uses FindDirectionToPlayer() and converts result to an angle in degrees
        /// </summary>
        public static float FindAngleToPlayerDeg(Vector2 startingPosition)
        {
            Vector2 directionToPlayer = FindDirectionToPlayer(startingPosition);
            return GetSignedAngleDeg(directionToPlayer, vec2Up);
        }

        /// <summary>
        /// Finds the direction between an object's x and z and the starting position's x and z.
        /// Mainly used for background effects and turning sprites towards the camera
        /// [unused]
        /// </summary>
        public static Vector3 FindDirectionToObject3D(Vector3 startingPosition, Entity otherObject)
        {
            return (new Vector3(otherObject.WorldPosition.X, 0, otherObject.WorldPosition.Z) - 
                new Vector3(startingPosition.X, 0, startingPosition.Z)).Normalized();
        }

        /// <summary>
        /// Uses the position of an object to find the direction towards it from the starting position
        /// </summary>
        public static Vector2 FindDirectionToObject(Vector2 startingPosition, Entity otherObject)
        {
            if (otherObject == null)
            {
                startingPosition.NormalizeFast();
                return startingPosition;
            }
            return (otherObject.WorldPosition.Xy - startingPosition).Normalized();
        }
        /// <summary>
        /// Uses the position of an object to find the direction towards it from the starting object
        /// </summary>
        public static Vector2 FindDirectionToObject(Entity startingObject, Entity otherObject)
        {
            return (otherObject.WorldPosition.Xy - startingObject.WorldPosition.Xy).Normalized();
        }
        /// <summary>
        /// Uses the position of an object to find the angle in radians towards it from the starting position, using the direction (0,1) as 0.
        /// Uses FindDirectionToObject() and converts result to an angle in radians
        /// </summary>
        public static float FindAngleToObjectRad(Vector2 startingPosition, Entity otherObject)
        {
            Vector2 directionToObject = FindDirectionToObject(startingPosition, otherObject);
            return GetSignedAngleRad(directionToObject, vec2Up);
        }
        /// <summary>
        /// Uses the position of an object to find the angle in degrees towards it from the starting position, using the direction (0,1) as 0.
        /// Uses FindDirectionToObject() and converts result to an angle in degrees
        /// </summary>
        public static float FindAngleToObjectDeg(Vector2 startingPosition, Entity otherObject)
        {
            Vector2 directionToObject = FindDirectionToObject(startingPosition, otherObject);
            return GetSignedAngleDeg(directionToObject, vec2Up);
        }

        /// <summary>
        /// Uses the position of an object to find the angle in radians towards it from the starting object, using the direction (0,1) as 0.
        /// Uses FindDirectionToObject() and converts result to an angle in radians
        /// </summary>
        public static float FindAngleToObjectRad(Entity startingObject, Entity otherObject)
        {
            Vector2 directionToObject = FindDirectionToObject(startingObject, otherObject);
            return GetSignedAngleRad(directionToObject, vec2Up);
        }
        /// <summary>
        /// Uses the position of an object to find the angle in degrees towards it from the starting object, using the direction (0,1) as 0.
        /// Uses FindDirectionToObject() and converts result to an angle in degrees
        /// </summary>
        public static float FindAngleToObjectDeg(Entity startingObject, Entity otherObject)
        {
            Vector2 directionToObject = FindDirectionToObject(startingObject, otherObject);
            return GetSignedAngleDeg(directionToObject, vec2Up);
        }


        /// <summary>
        /// Finds the angle in radians between two positions, using the direction (0,1) as 0 
        /// </summary>
        public static float FindAngleRad(Vector2 startPoint, Vector2 endPoint)
        {
            Vector2 direction = endPoint - startPoint;
            return GetSignedAngleRad(direction, vec2Up);
        }
        /// <summary>
        /// Finds the angle in degrees between two positions, using the direction (0,1) as 0 
        /// </summary>
        public static float FindAngleDeg(Vector2 startPoint, Vector2 endPoint)
        {
            Vector2 direction = endPoint - startPoint;
            return GetSignedAngleDeg(direction, vec2Up);
        }

        /// <summary>
        /// Finds the angle difference in radians between two directions
        /// </summary>
        public static float FindAngleBetweenRad(Vector2 startPoint, Vector2 endPoint)
        {
            return GetSignedAngleRad(startPoint, endPoint);
        }
        /// <summary>
        /// Finds the angle difference in degrees between two directions
        /// </summary>
        public static float FindAngleBetweenDeg(Vector2 startPoint, Vector2 endPoint)
        {
            return GetSignedAngleDeg(startPoint, endPoint);
        }

        /// <summary>
        /// Finds the angle of the direction in radians, using the direction (0,1) as 0
        /// </summary>
        public static float FindAngleRad(Vector2 direction)
        {
            return GetSignedAngleRad(direction, vec2Up);
        }
        /// <summary>
        /// Finds the angle of the direction in degrees, using the direction (0,1) as 0
        /// </summary>
        public static float FindAngleDeg(Vector2 direction)
        {
            return GetSignedAngleDeg(direction, vec2Up);
        }

        /// <summary>
        /// Converts a quaternion rotation to a euler rotation
        /// </summary>
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


        /// <summary>
        /// Finds the nearest entity to the given position, within a max radius, ignoring the calling object if it is passed
        /// </summary>
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

        /// <summary>
        /// Runs through each enemy, and finds the nearest position within the maximum radius, and returns the shortest distance
        /// </summary>
        public static Entity FindNearestEnemy(Vector2 position, float maxRadius, out float shortestDist)
         {
             shortestDist = float.MaxValue;
             float tempDist;
             Entity newTarget = null;
             GameMaster gameMaster = GameMaster.gameMaster;
             int childCount = BaseEnemyElement.allEnemies.Count;

             for (int i = 0; i < childCount; i++)
             {
                 tempDist = Vector2.Distance(position, BaseEnemyElement.allEnemies[i].EntityAttachedTo.WorldPosition.Xy);
                 if (tempDist <= maxRadius && tempDist < shortestDist)
                 {
                     newTarget = BaseEnemyElement.allEnemies[i].EntityAttachedTo;
                     shortestDist = tempDist;
                 }
             }
            /*
             //Finds nearest distance to any midboss while fighting the midboss -- is set up to allow for multiple-character midbosses
             if (gameMaster.stageScript.fightingMidboss)
             {
                 BaseBossScript[] midbosses;
                 midbosses = gameMaster.stageScript.midbossSpawn.GetComponentsInChildren<BaseBossScript>();
                 Entity thisBoss;
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
                 Entity thisBoss;
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
             }*/

             //Returns the nearest enemy's transform
             return newTarget;
         }

        /// <summary>
        /// Runs through each enemy, and finds the nearest position within the maximum radius, but does not return the shortest distance
        /// Calls most similar FindNearestEnemy() with an out float
        /// </summary>
        public static Entity FindNearestEnemy(Vector2 position, float maxRadius)
        {
            float outDeletion;
            return FindNearestEnemy(position, maxRadius, out outDeletion);
        }


        /// <summary>
        /// Runs through each enemy, and finds the nearest position, and returns the shortest distance
        /// </summary>
        public static Entity FindNearestEnemy(Vector2 position, out float shortestDist)
        {
            shortestDist = float.MaxValue;
            float tempDist;
            Entity newTarget = null;
            GameMaster gameMaster = GameMaster.gameMaster;
            int childCount = BaseEnemyElement.allEnemies.Count;

            for (int i = 0; i < childCount; i++)
            {
                tempDist = Vector2.Distance(position, BaseEnemyElement.allEnemies[i].EntityAttachedTo.WorldPosition.Xy);
                if (tempDist < shortestDist)
                {
                    newTarget = BaseEnemyElement.allEnemies[i].EntityAttachedTo;
                    shortestDist = tempDist;
                }
            }

            /*//Finds nearest distance to any midboss while fighting the midboss -- is set up to allow for multiple-character midbosses
            if (gameMaster.stageScript.fightingMidboss)
            {
                BaseBossScript[] midbosses;
                midbosses = gameMaster.stageScript.midbossSpawn.GetComponentsInChildren<BaseBossScript>();
                Entity thisBoss;
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
                Entity thisBoss;
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
            }*/

            return newTarget;
        }

        /// <summary>
        /// Runs through each enemy, and finds the nearest position, but does not return the shortest distance.
        /// Calls most similar FindNearestEnemy() with an out float
        /// </summary>
        public static Entity FindNearestEnemy(Vector2 position)
        {
            float outDeletion;
            return FindNearestEnemy(position, out outDeletion);
        }
    }
}
