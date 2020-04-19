using System;
using System.Collections.Generic;
using System.Text;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

using OpenTK.Graphics.ES30;

namespace DawnmakuEngine
{
    class Texture
    {
        protected int handle, texWidth, texHeight;
        public int Handle { get { return handle; } }
        public int Width { get { return texWidth; } }
        public int Height { get { return texHeight; } }

        public Texture(string texPath, bool generateMipmaps)
        {
            handle = GL.GenTexture();
            Use();

            Image<Rgba32> image = Image.Load<Rgba32>(texPath);

            image.Mutate(x => x.Flip(FlipMode.Vertical));

            Rgba32[] tempPixels = image.GetPixelSpan().ToArray();

            List<byte> pixels = new List<byte>();
            int pixelCount = tempPixels.Length;
            for (int i = 0; i < pixelCount; i++)
            {
                pixels.Add(tempPixels[i].R);
                pixels.Add(tempPixels[i].G);
                pixels.Add(tempPixels[i].B);
                pixels.Add(tempPixels[i].A);
            }

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);

            GL.TexImage2D(TextureTarget2d.Texture2D, 0, TextureComponentCount.Rgba, image.Width, image.Height,
                0, PixelFormat.Rgba, PixelType.UnsignedByte, pixels.ToArray());

            if (generateMipmaps)
                GL.GenerateMipmap(TextureTarget.Texture2D);

            texWidth = image.Width;
            texHeight = image.Height;
        }

        public void Use(TextureUnit unit = TextureUnit.Texture0)
        {
            GL.ActiveTexture(unit);
            GL.BindTexture(TextureTarget.Texture2D, handle);
        }
    }
}
