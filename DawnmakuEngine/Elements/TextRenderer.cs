using System;
using System.Collections.Generic;
using System.Diagnostics;
using DawnmakuEngine.Data;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.ES10;
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
        public Shader currentShader;
        GameMaster gameMaster = GameMaster.gameMaster;
        MeshRenderer refRend;
        public bool uiText;
        float textSize;
        float wrapWidth;
        Font font;
        string text = "";
        bool generated = false;
        bool kerning = true;
        bool antialias = true;
        HorizontalAlignment horiAlign;
        VerticalAlignment vertAlign;
        FontStyle style;

        Texture textTex;
        Image<Rgba32> textImage;
        List<Entity> charObjects = new List<Entity>();
        List<MeshRenderer> renderers = new List<MeshRenderer>();

        public float TextSize
        {
            get { return textSize; }
            set
            {
                textSize = value;
                if (font != null)
                    WriteFont = new Font(font, textSize);
                /*if (textTex != null)
                    GenerateImage();*/
                    UpdateText();
            }
        }
        public float WrapWidth
        {
            get { return wrapWidth; }
            set
            {
                wrapWidth = value;

                /*if (textTex != null)
                    GenerateImage();*/
                UpdateText();
            }
        }
        public Font WriteFont
        {
            get { return font; }
            set
            {
                font = value;
                /*if (textTex != null)
                    GenerateImage();*/
                UpdateText();
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

                /*if (textTex != null)
                    GenerateImage();*/
                UpdateText();
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

                    /*if (textTex != null)
                        GenerateImage();*/
                    UpdateText();
                }
            }
        }
        public bool Kerning
        {
            get { return kerning; }
            set
            {
                kerning = value;

                /*if (textTex != null)
                    GenerateImage();*/
                UpdateText();
            }
        }
        public bool AntiAlias
        {
            get { return antialias; }
            set
            {
                antialias = value;

                /*if (textTex != null)
                    GenerateImage();*/
                UpdateText();
            }
        }
        public string Text
        {
            get { return text; }
            set
            {
                text = value;
                /*if (textTex != null)
                    GenerateImage();*/
                UpdateText();
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
            //if (textTex == null && font != null)
            if(!generated && font != null)
            {
                //refRend = entityAttachedTo.GetElement<MeshRenderer>();
                //if(refRend != null)
                    //GenerateImage();
                    UpdateText();
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

        void UpdateText()
        {
            generated = true;
            GenerateObjects();
            SetCharPositions(SetCharTexture());
        }

        void GenerateObjects()
        {
            if(charObjects.Count != text.Length)
            {
                int i;
                short charDif = (short)(text.Length - charObjects.Count);

                if(charDif < 0)
                    for (i = 0; i < -charDif; i++)
                    {
                        renderers.RemoveAt(renderers.Count - 1);
                        charObjects[charObjects.Count - 1].AttemptDelete();
                        charObjects.RemoveAt(charObjects.Count - 1);
                    }
                else
                {
                    Entity newCharObj;
                    MeshRenderer newRenderer;
                    for (i = 0; i < charDif; i++)
                    {
                        newCharObj = new Entity("CharObj" + charObjects.Count, entityAttachedTo);

                        newRenderer = new MeshRenderer(Mesh.CreatePrimitiveMesh(Mesh.Primitives.SqrPlaneWTriangles), 
                            uiText ? OpenTK.Graphics.ES30.BufferUsageHint.StaticDraw : OpenTK.Graphics.ES30.BufferUsageHint.DynamicDraw);
                        newRenderer.LayerName = uiText ? "borderui" : "effects";
                        newRenderer.shader = currentShader;
                        newRenderer.resizeSprite = true;
                        newRenderer.ColorByte = new Vector4(255, 255, 255, 255);

                        newCharObj.AddElement(newRenderer);

                        renderers.Add(newRenderer);
                        charObjects.Add(newCharObj);
                    }
                }
            }
        }

        byte[] SetCharTexture()
        {
            char curChar;
            SpriteSet.Sprite curCharSprite;
            int rendererLength = renderers.Count, fontNames = gameMaster.fontNames.Count,
                i, f;
            byte[] fontCharIsIn = new byte[rendererLength];
            for (i = 0; i < rendererLength; i++)
            {
                curChar = text[i];
                if(gameMaster.fontCharList[font.Name].charListHash.Contains(curChar))
                {
                    fontCharIsIn[i] = (byte)255;
                    curCharSprite = gameMaster.fontGlyphSprites[font.Name].sprites[(ushort)curChar];
                }
                else
                {
                    for (f = 0; f < fontNames; f++)
                        if (gameMaster.fontCharList[gameMaster.fontNames[f]].charListHash.Contains(curChar))
                            break;
                    if (f >= fontNames)
                    {
                        GameMaster.LogError("Char \" " + curChar + " \" not in font");
                        continue;
                    }
                    fontCharIsIn[i] = (byte)f;
                    curCharSprite = gameMaster.fontGlyphSprites[gameMaster.fontNames[f]].sprites[gameMaster.fontCharList[gameMaster.fontNames[f]].indexes[(ushort)curChar]];
                }
                curCharSprite = gameMaster.playerSprites["reimu"].sprites[0];
                renderers[i].mesh.SetUVs(curCharSprite.GetUVs());
                renderers[i].tex = curCharSprite.tex;
            }
            return fontCharIsIn;
        }

        void SetCharPositions(byte[] fontCharIsIn)
        {
            RendererOptions measureOptions;

            char curChar;
            string textSoFar = "";
            int charObjectLength = charObjects.Count, i;
            Font currentFont;
            GlyphInstance glyph;
            FontRectangle rect;
            float width = 0, prevWidth, difference;

            for (i = 0; i < charObjectLength; i++)
            {
                prevWidth = width;
                curChar = text[i];
                textSoFar += curChar;
                if (fontCharIsIn[i] == 255)
                {
                    currentFont = font;
                }
                else
                {
                    currentFont = gameMaster.fonts.Find(gameMaster.fontNames[fontCharIsIn[i]]).CreateFont(font.Size);
                }
                measureOptions = new RendererOptions(currentFont)
                {
                    ApplyKerning = kerning,
                    WrappingWidth = wrapWidth,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                };
                //glyph = currentFont.GetGlyph(curChar).Instance;
                rect = TextMeasurer.Measure(curChar.ToString(), measureOptions);

                width += rect.Width;

                /*if (HoriAlign == HorizontalAlignment.Left)
                {
                    charObjects[i].LocalPosition = new Vector3(width + rect.Width, 0, 0);
                    width += rect.Width;
                }
                else
                    width += rect.Width;*/
            }


            float widthPerChar = width / charObjectLength,
                startPoint = 0;
            Vector3 charObjPos;

            switch (horiAlign)
            {
                case HorizontalAlignment.Center:
                    startPoint = -width / 2;
                    break;
                case HorizontalAlignment.Right:
                    startPoint = -width;
                    break;
                case HorizontalAlignment.Left:
                    startPoint = 0 + widthPerChar / 2;
                    break;
            }


            for (i = 0; i < charObjectLength; i++)
            {
                charObjPos = charObjects[i].LocalPosition;

                charObjPos.X = startPoint + widthPerChar * i;

                charObjects[i].LocalPosition = charObjPos;
            }
        }

        public TextRenderer() : base(false, false, true)
        {

        }
    }
}
