using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.Intrinsics;
using System.Text;

namespace DawnmakuEngine.Data
{
    class SpriteSet
    {
        public List<Sprite> sprites = new List<Sprite>();
        public class Sprite
        {
            public float left, top, right, bottom;
            public Texture tex;

            public Sprite() { }
            public Sprite(float left_, float top_, float right_, float bottom_, Texture tex_, bool pixelCoords)
            {
                if(pixelCoords)
                {
                    left = left_ / tex_.Width;
                    top = 1 - top_ / tex_.Height;
                    right = right_ / tex_.Width;
                    bottom = 1 - bottom_ / tex_.Height;
                }
                else
                {
                    left = left_;
                    top = top_;
                    right = right_;
                    bottom = bottom_;
                }
                tex = tex_;
            }
        }
    }
}
