using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.Intrinsics;
using System.Text;

namespace DawnmakuEngine.Data
{
    public class SpriteSet
    {
        public List<Sprite> sprites = new List<Sprite>();

        public SpriteSet() { }
        public SpriteSet(List<Sprite> newSprites) : this()
        {
            sprites = newSprites;
        }
        public SpriteSet(List<Sprite> newSprites, Texture newTex) : this(newSprites)
        {
            ReplaceTexture(newTex);
        }

        public void ReplaceTexture(Texture newTex)
        {
            int i, spriteCount = sprites.Count;
            for (i = 0; i < spriteCount; i++)
            {
                sprites[i].tex = newTex;
            }
        }
        
        public class Sprite
        {
            public float left, top, right, bottom;
            public Texture tex;

            public Sprite() { }
            /// <summary>
            /// Set up Sprite with a texture
            /// </summary>
            /// <param name="left_"></param>
            /// <param name="top_"></param>
            /// <param name="right_"></param>
            /// <param name="bottom_"></param>
            /// <param name="imageHeight"></param>
            /// <param name="imageWidth"></param>
            /// <param name="pixelCoords"></param>
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
            /// <summary>
            /// Set up Sprite without a texture, but with an image size
            /// </summary>
            /// <param name="left_"></param>
            /// <param name="top_"></param>
            /// <param name="right_"></param>
            /// <param name="bottom_"></param>
            /// <param name="imageHeight"></param>
            /// <param name="imageWidth"></param>
            /// <param name="pixelCoords"></param>
            public Sprite(float left_, float top_, float right_, float bottom_, int imageHeight, int imageWidth, bool pixelCoords)
            {
                if (pixelCoords)
                {
                    left = left_ / imageWidth;
                    top = 1 - top_ / imageHeight;
                    right = right_ / imageWidth;
                    bottom = 1 - bottom_ / imageHeight;
                }
                else
                {
                    left = left_;
                    top = top_;
                    right = right_;
                    bottom = bottom_;
                }
            }
        }
    }
}
