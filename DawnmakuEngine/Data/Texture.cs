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
        public Texture(Image<A8> image, bool generateMipmaps)
        {
            handle = GL.GenTexture();
            Use();

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);

            SetUpTexture(image, generateMipmaps);
        }
        public Texture(System.Drawing.Bitmap image, bool generateMipmaps)
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
        public void SetUpTexture(Image<A8> image, bool generateMipmaps)
        {
            int i, k;
            image.Mutate(x => x.Flip(FlipMode.Vertical));
            A8[] tempPixels = new A8[image.Height * image.Width];
            A8[] superTempPixels;
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
            byte[] pixels = new byte[pixelCount];
            for (i = 0; i < pixelCount; i++)
            {
                pixels[i] = tempPixels[i].PackedValue;
            }

            GL.TexImage2D(TextureTarget2d.Texture2D, 0, TextureComponentCount.Alpha8Ext, image.Width, image.Height,
                0, PixelFormat.Alpha, PixelType.UnsignedByte, pixels);

            if (generateMipmaps)
                GL.GenerateMipmap(TextureTarget.Texture2D);

            texWidth = image.Width;
            texHeight = image.Height;
        }
        public void SetUpTexture(System.Drawing.Bitmap image, bool generateMipmaps)
        {
            image = image.Clone(new System.Drawing.Rectangle(new System.Drawing.Point(0,0), new System.Drawing.Size(image.Width, image.Height)),
                System.Drawing.Imaging.PixelFormat.Format16bppArgb1555);
            
            int i, k;
            image.RotateFlip(System.Drawing.RotateFlipType.RotateNoneFlipY);
            
            int height = image.Height, width = image.Width, pixelsRead = 0, pixelColor = 0;
            
            byte[] pixels = new byte[height * width * 4];
            System.Diagnostics.Stopwatch timer = System.Diagnostics.Stopwatch.StartNew();
            /*
            for (i = 0; i < height; i++)
            {
                pixelColor = 0;
                for (k = 0; k < width; k++)
                {
                    pixels[pixelsRead + pixelColor] = image.GetPixel(k, i).R;
                    pixels[pixelsRead + pixelColor + 1] = image.GetPixel(k, i).R;
                    pixels[pixelsRead + pixelColor + 2] = image.GetPixel(k, i).R;
                    pixels[pixelsRead + pixelColor + 3] = image.GetPixel(k, i).R;
                    pixelColor += 4;
                }
                pixelsRead += width;
            }*/
            try
            {
                using (System.IO.MemoryStream stream = new System.IO.MemoryStream())
                {
                    image.Save(stream, System.Drawing.Imaging.ImageFormat.Bmp);
                    image.Save("C:\\Users\\Damio\\Desktop\\FontImage.bpm", System.Drawing.Imaging.ImageFormat.Bmp);
                    pixels = stream.ToArray();
                }
            }
            catch (Exception e)
            {
                GameMaster.LogError(e.Message);
            }
            GameMaster.LogNegativeNotice("Time to cycle pixels: " + timer.ElapsedMilliseconds);

            GL.TexImage2D(TextureTarget2d.Texture2D, 0, TextureComponentCount.Rgba, image.Width, image.Height,
                0, PixelFormat.Rgba, PixelType.UnsignedByte, pixels);

            if (generateMipmaps)
                GL.GenerateMipmap(TextureTarget.Texture2D);

            texWidth = image.Width;
            texHeight = image.Height;
        }
    }
}
