using System;
using System.Collections.Generic;
using System.Diagnostics;
using DawnmakuEngine.Data;
using OpenTK;
using OpenTK.Graphics;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.Drawing;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using OpenTK.Mathematics;

namespace DawnmakuEngine.Elements
{
    public class TextRenderer : Element
    {
        public Shader currentShader;
        GameMaster gameMaster = GameMaster.gameMaster;
        MeshRenderer refRend;
        public bool uiText;
        Vector4 color;
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
        List<SpriteRenderer> renderers = new List<SpriteRenderer>();

        /// <summary>
        /// Forces text to have this width between characters. Only takes effect when not 0.
        /// </summary>
        float monospaceWidth = 0;

        public Vector4 Color
        {
            get { return color; }
            set
            {
                color = value;
                generated = false;
            }
        }
        public float MonospaceWidth
        {
            get { return monospaceWidth; }
            set
            {
                monospaceWidth = value;
                generated = false;
            }
        }
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
                generated = false;
                //UpdateText();
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

                generated = false;
                //UpdateText();
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
                generated = false;
                //UpdateText();
            }
        }
        public string WriteFontFamilyName
        {
            get { return font.Family.Name; }
            set
            {
                FontFamily family = gameMaster.fonts.Get(value);

                bool foundStyle = false;
                foreach (FontStyle fs in family.GetAvailableStyles())
                {
                    if (fs == style)
                    {
                        font = family.CreateFont(textSize, style);
                        foundStyle = true;
                        break;
                    }
                }

                if (!foundStyle)
                    font = family.CreateFont(textSize);

                /*if (textTex != null)
                    GenerateImage();*/
                generated = false;
                //UpdateText();
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
                    bool foundStyle = false;
                    foreach (FontStyle fs in family.GetAvailableStyles())
                    {
                        if (fs == style)
                        {
                            font = family.CreateFont(textSize, style);
                            foundStyle = true;
                            break;
                        }
                    }

                    if (!foundStyle)
                        font = family.CreateFont(textSize);

                    /*if (textTex != null)
                        GenerateImage();*/
                    generated = false;
                    //UpdateText();
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
                generated = false;
                //UpdateText();
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
                generated = false;
                //UpdateText();
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
                generated = false;
                //UpdateText();
            }
        }
        public HorizontalAlignment HoriAlign
        {
            get { return horiAlign; }
            set
            {
                horiAlign = value;
                generated = false;
            }
        }
        public VerticalAlignment VertAlign
        {
            get { return vertAlign; }
            set
            {
                vertAlign = value;
                generated = false;
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

        /*void GenerateImage()
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
        }*/

        public void UpdateText()
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
                    SpriteRenderer spriteRend;
                    for (i = 0; i < charDif; i++)
                    {
                        newCharObj = new Entity("CharObj" + charObjects.Count, entityAttachedTo);

                        newRenderer = new MeshRenderer(Mesh.CreatePrimitiveMesh(Mesh.Primitives.SqrPlaneWTriangles), 
                            uiText ? OpenTK.Graphics.ES30.BufferUsageHint.StaticDraw : OpenTK.Graphics.ES30.BufferUsageHint.DynamicDraw);
                        newRenderer.LayerName = uiText ? "borderuitext" : "effects";
                        newRenderer.shader = currentShader;
                        //newRenderer.resizeSprite = true;
                        newRenderer.ColorByte = color;

                        spriteRend = new SpriteRenderer(newRenderer);

                        newCharObj.AddElement(spriteRend);
                        newCharObj.AddElement(newRenderer);

                        renderers.Add(spriteRend);
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
                //curCharSprite = gameMaster.playerSprites["reimu"].sprites[0];

                renderers[i].Sprite = curCharSprite;
                /*renderers[i].mesh.SetUVs(curCharSprite.GetUVs());
                renderers[i].tex = curCharSprite.tex;*/


                //renderers[i].modelScale = textSize;
                renderers[i].meshRend.ColorByte = color;
                renderers[i].mesh.SetUp(OpenTK.Graphics.ES30.BufferUsageHint.DynamicDraw);
            }
            return fontCharIsIn;
        }

        void SetCharPositions(byte[] fontCharIsIn)
        {
            TextOptions measureOptions;

            char curChar;
            string textSoFar = "";
            int charObjectLength = charObjects.Count, i;
            Font currentFont = font;
            //GlyphInstance glyph;
            FontRectangle rect;
            float width = 0, prevWidth, difference;
            Vector3 charObjPos;


            measureOptions = new TextOptions(currentFont)
            {
                KerningMode = kerning ? KerningMode.Normal : KerningMode.None,
                WrappingLength = wrapWidth,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
            };

            if (monospaceWidth == 0)
            {
                if(kerning)
                {
                    //write out the first character normally
                    curChar = text[0]; 
                    rect = TextMeasurer.Measure(curChar.ToString(), measureOptions);
                    charObjPos = charObjects[0].LocalPosition;
                    charObjPos.X = rect.Width * 0.5f;
                    charObjects[0].LocalPosition = charObjPos;
                    width += rect.Width;

                    FontRectangle rectOther, rectBoth;
                    for (i = 1; i < charObjectLength; i++)
                    {
                        currentFont = font;

                        if (fontCharIsIn[i] != 255)
                            currentFont = gameMaster.fonts.Get(gameMaster.fontNames[fontCharIsIn[i]]).CreateFont(font.Size);

                        measureOptions.Font = currentFont;

                        curChar = text[i];

                        rectOther = rect;
                        rect = TextMeasurer.Measure(curChar.ToString(), measureOptions);
                        rectBoth = TextMeasurer.Measure(text.Substring(i - 1, 2), measureOptions);

                        difference = (rectBoth.Width - ((rect.Width * 0.6f) + (rectOther.Width * 0.6f)));

                        charObjPos = charObjects[i].LocalPosition;
                        charObjPos.X = charObjects[i - 1].LocalPosition.X + difference;
                        charObjects[i].LocalPosition = charObjPos;
                    }
                    width = charObjects[charObjectLength - 1].LocalPosition.X + rect.Width * 0.5f;
                }
                else //No Kerning
                {
                    for (i = 0; i < charObjectLength; i++)
                    {
                        currentFont = font;

                        if (fontCharIsIn[i] != 255)
                            currentFont = gameMaster.fonts.Get(gameMaster.fontNames[fontCharIsIn[i]]).CreateFont(font.Size);

                        measureOptions.Font = currentFont;

                        curChar = text[i];

                        rect = TextMeasurer.Measure(curChar.ToString(), measureOptions);


                        charObjPos = charObjects[i].LocalPosition;
                        charObjPos.X = width + rect.Width * 0.5f;
                        charObjects[i].LocalPosition = charObjPos;

                        width += rect.Width;
                    }
                }

                switch (horiAlign)
                {
                    case HorizontalAlignment.Center:
                        width *= 0.5f;
                        for (i = 0; i < charObjectLength; i++)
                        {
                            charObjPos = charObjects[i].LocalPosition;
                            charObjPos.X -= width;
                            charObjects[i].LocalPosition = charObjPos;
                        }
                        break;
                    case HorizontalAlignment.Right:
                        for (i = 0; i < charObjectLength; i++)
                        {
                            charObjPos = charObjects[i].LocalPosition;
                            charObjPos.X -= width;
                            charObjects[i].LocalPosition = charObjPos;
                        }
                        break;
                    case HorizontalAlignment.Left:
                        break;
                }

                for (i = 0; i < charObjectLength; i++)
                {
                    prevWidth = width;
                    curChar = text[i];
                    textSoFar += curChar;
                    //glyph = currentFont.GetGlyph(curChar).Instance;
                    rect = TextMeasurer.Measure(curChar.ToString(), measureOptions);

                    /*renderers[i].mesh.SetVertex(0, new Vector3(-rect.Width * 0.5f, rect.Height * 0.5f, 0)); //tl
                    renderers[i].mesh.SetVertex(1, new Vector3(rect.Width * 0.5f, rect.Height * 0.5f, 0)); //tr
                    renderers[i].mesh.SetVertex(2, new Vector3(rect.Width * 0.5f, -rect.Height * 0.5f, 0)); //br
                    renderers[i].mesh.SetVertex(3, new Vector3(-rect.Width * 0.5f, -rect.Height * 0.5f, 0)); //bl*/

                    width += rect.Width;

                    /*if (HoriAlign == HorizontalAlignment.Left)
                    {
                        charObjects[i].LocalPosition = new Vector3(width + rect.Width, 0, 0);
                        width += rect.Width;
                    }
                    else
                        width += rect.Width;*/
                }
            }
            else // monospace
            {
                width = charObjectLength * monospaceWidth;

                float widthPerChar = width / charObjectLength,
                    startPoint = 0;

                switch (horiAlign)
                {
                    case HorizontalAlignment.Center:
                        startPoint = -width / 2;
                        break;
                    case HorizontalAlignment.Right:
                        startPoint = -width;
                        break;
                    case HorizontalAlignment.Left:
                        startPoint = 0;
                        break;
                }


                for (i = 0; i < charObjectLength; i++)
                {
                    charObjPos = charObjects[i].LocalPosition;

                    charObjPos.X = startPoint + widthPerChar * i;

                    charObjects[i].LocalPosition = charObjPos;
                }
            }
        }

        public TextRenderer() : base(false, false, true)
        {

        }



        const string TEXT_REND_NAME = "textrend";
        public static TextRenderer Create(Vector3 position, Vector3 rotation, bool uiText, string font, Shader shader, string name = TEXT_REND_NAME)
        {
            Entity newEntity = new Entity(name, position, rotation, Vector3.One);

            TextRenderer newRend = new TextRenderer();
            newEntity.AddElement(newRend);

            newRend.uiText = uiText;
            newRend.WriteFontFamilyName = font;
            newRend.currentShader = shader;

            return newRend;
        }

        public static TextRenderer Create(Vector3 position, Vector3 rotation, bool uiText, string font, Shader shader,
            Vector4 color, float textSize, HorizontalAlignment horiAlign, VerticalAlignment vertAlign, string text, float monospaceWidth, string name = TEXT_REND_NAME)
        {
            TextRenderer newRend = Create(position, rotation, uiText, font, shader, name);

            newRend.Color = color;
            newRend.TextSize = textSize;
            newRend.MonospaceWidth = monospaceWidth;
            newRend.HoriAlign = horiAlign;
            newRend.VertAlign = vertAlign;

            newRend.Text = text;

            return newRend;
        }
    }
}
