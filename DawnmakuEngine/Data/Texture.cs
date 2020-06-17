using System;
using System.Collections.Generic;
using System.Text;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

using OpenTK.Graphics.ES30;

namespace DawnmakuEngine.Data
{
    public class Texture
    {
        protected int handle, texWidth, texHeight;
        public int Handle { get { return handle; } }
        public int Width { get { return texWidth; } }
        public int Height { get { return texHeight; } }

        public Texture(Image<Rgba32> image, bool generateMipmaps)
        {
            handle = GL.GenTexture();
            Use();

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);

            SetUpTexture(image, generateMipmaps);
        }

        public Texture(string texPath, bool generateMipmaps)
        {
            handle = GL.GenTexture();
            Image<Rgba32> image = Image.Load<Rgba32>(texPath);
            Use();

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);

            SetUpTexture(image, generateMipmaps);
        }

        public void Use(TextureUnit unit = TextureUnit.Texture0)
        {
            GL.ActiveTexture(unit);
            GL.BindTexture(TextureTarget.Texture2D, handle);
        }

        public void SetUpTexture(Image<Rgba32> image, bool generateMipmaps)
        {
            int i, k;
            image.Mutate(x => x.Flip(FlipMode.Vertical));
            Rgba32[] tempPixels = new Rgba32[image.Height * image.Width];
            Rgba32[] superTempPixels;
            int height = image.Height, width = image.Width, pixelsRead = 0, length;
            for (i = 0; i < height; i++)
            {
                superTempPixels = image.GetPixelRowSpan(i).ToArray();
                length = superTempPixels.Length;
                for (k = 0; k < length; k++)
                {
                    tempPixels[pixelsRead + k] = superTempPixels[k];
                }
                pixelsRead += width;
            }

            int pixelCount = tempPixels.Length;
            pixelsRead = 0;
            byte[] pixels = new byte[pixelCount * 4];
            for (i = 0; i < pixelCount; i++)
            {
                pixels[pixelsRead] = tempPixels[i].R;
                pixels[pixelsRead + 1] = tempPixels[i].G;
                pixels[pixelsRead + 2] = tempPixels[i].B;
                pixels[pixelsRead + 3] = tempPixels[i].A;
                pixelsRead += 4;
            }

            GL.TexImage2D(TextureTarget2d.Texture2D, 0, TextureComponentCount.Rgba, image.Width, image.Height,
                0, PixelFormat.Rgba, PixelType.UnsignedByte, pixels);

            if (generateMipmaps)
                GL.GenerateMipmap(TextureTarget.Texture2D);

            texWidth = image.Width;
            texHeight = image.Height;
        }
    }
}
