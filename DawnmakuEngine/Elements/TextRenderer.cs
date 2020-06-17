using System;
using System.Collections.Generic;
using System.Diagnostics;
using DawnmakuEngine.Data;
using OpenTK;
using OpenTK.Graphics;
using SharpFont;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.Drawing;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace DawnmakuEngine.Elements
{
    class TextRenderer : Element
    {
        GameMaster gameMaster = GameMaster.gameMaster;
        MeshRenderer refRend;
        float textSize;
        float wrapWidth;
        Font font;
        string text;
        bool kerning = true;
        bool antialias = true;
        HorizontalAlignment horiAlign;
        VerticalAlignment vertAlign;
        FontStyle style;

        Texture textTex;
        Image<Rgba32> textImage;

        public float TextSize
        {
            get { return textSize; }
            set
            {
                textSize = value;
                if (font != null)
                    WriteFont = new Font(font, textSize);
                if (textTex != null)
                    GenerateImage();
            }
        }
        public float WrapWidth
        {
            get { return wrapWidth; }
            set
            {
                wrapWidth = value;

                if (textTex != null)
                    GenerateImage();
            }
        }
        public Font WriteFont
        {
            get { return font; }
            set
            {
                font = value;
                if (textTex != null)
                    GenerateImage();
            }
        }
        public string WriteFontFamilyName
        {
            get { return font.Family.Name; }
            set
            {
                FontFamily family = gameMaster.fonts.Find(value);
                if(family.IsStyleAvailable(style))
                    font = family.CreateFont(textSize, style);
                else
                    font = family.CreateFont(textSize);

                if (textTex != null)
                    GenerateImage();
            }
        }
        public FontStyle Style
        {
            get { return style; }
            set
            {
                style = value;
                if(font != null)
                {
                    FontFamily family = font.Family;
                    if (family.IsStyleAvailable(style))
                        font = family.CreateFont(textSize, style);
                    else
                        font = family.CreateFont(textSize);

                    if (textTex != null)
                        GenerateImage();
                }
            }
        }
        public bool Kerning
        {
            get { return kerning; }
            set
            {
                kerning = value;

                if (textTex != null)
                    GenerateImage();
            }
        }
        public bool AntiAlias
        {
            get { return antialias; }
            set
            {
                antialias = value;

                if (textTex != null)
                    GenerateImage();
            }
        }
        public string Text
        {
            get { return text; }
            set
            {
                text = value;
                if (textTex != null)
                    GenerateImage();
            }
        }
        public HorizontalAlignment HoriAlign
        {
            get { return horiAlign; }
            set
            {
                horiAlign = value;
            }
        }
        public VerticalAlignment VertAlign
        {
            get { return vertAlign; }
            set
            {
                vertAlign = value;
            }
        }

        public override void PreRender()
        {
            if (textTex == null && font != null)
            {
                refRend = entityAttachedTo.GetElement<MeshRenderer>();
                if(refRend != null)
                    GenerateImage();
            }
        }

        void GenerateImage()
        {
            TextGraphicsOptions finalRendOpt = new TextGraphicsOptions()
            {
                TextOptions = new TextOptions()
                {
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    ApplyKerning = kerning,
                    WrapTextWidth = wrapWidth
                },
                GraphicsOptions = new GraphicsOptions()
                { 
                    Antialias = antialias 
                }
            };

            RendererOptions textRendOptions = new RendererOptions(font)
            {
                ApplyKerning = kerning,
                WrappingWidth = wrapWidth,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
            };

            Stopwatch watch = Stopwatch.StartNew();

            FontRectangle rect = TextMeasurer.Measure(text, textRendOptions);
            if (textImage == null)
            {
                textImage = new Image<Rgba32>(DawnMath.Ceil(rect.Width), DawnMath.Ceil(rect.Height));
            }
            else
            {
                textImage.Mutate(x => x.Clear(Color.Transparent).Resize(new Size(DawnMath.Ceil(rect.Width), DawnMath.Ceil(rect.Height))));
            }
            
            textImage.Mutate(x => x.DrawText(finalRendOpt, text, font, Color.White, new PointF(textImage.Width / 2f, textImage.Height / 2f)));
            watch.Stop();
            if (textTex == null)
                textTex = new Texture(textImage, false);
            else
                textTex.SetUpTexture(textImage, false);

            refRend.tex = textTex;

            Console.WriteLine("Text rewrite time: {0}", watch.ElapsedMilliseconds);
        }

        public TextRenderer() : base(false, false, true)
        {

        }
    }
}
