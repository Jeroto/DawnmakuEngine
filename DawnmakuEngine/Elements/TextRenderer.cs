using System;
using System.Collections.Generic;
using System.Text;
using OpenTK;
using OpenTK.Graphics;
using System.Drawing;

namespace DawnmakuEngine.Elements
{
    class TextRenderer : Element
    {
        /*public static List<TextRenderer> textRenderers = new List<TextRenderer>();

        static QFont defaultFont = new QFont("Fonts/WitchingHour.ttf", 72, new QFontBuilderConfiguration());
        public static QFont DefaultFont { get { return defaultFont; } }
        QFontDrawing drawing = new QFontDrawing();
        QFontDrawingPrimitive prim;
        public string textToWrite;
        public QFont font;
        public Color textColor;
        public FontStyle textStyle;
        public QFontAlignment textAlignment;
        public bool dropShadow, wordWrap;
        public QFontMonospacing monospace;
        
        public override void PreRender()
        {
            drawing.Print(font, textToWrite, EntityAttachedTo.WorldPosition, textAlignment);
            drawing.RefreshBuffers();
        }

        public void SetUp()
        {
            drawing.DrawingPrimitives.Clear();
            QFontRenderOptions textOptions = new QFontRenderOptions()
            { Colour = textColor, DropShadowActive = dropShadow, Monospacing = monospace, WordWrap = wordWrap };
            drawing.DrawingPrimitives.Add(new QFontDrawingPrimitive(font, textOptions));
        }

        public void Draw(Matrix4 projection)
        {
            drawing.ProjectionMatrix = projection;
            drawing.Draw();
        }

        public void SetDrawingColor(byte R, byte G, byte B, byte A)
        {
            textColor = Color.FromArgb(A, R, G, B);
        }

        public TextRenderer() : base()
        {

        }

        protected override void OnEnableAndCreate()
        {
            textRenderers.Add(this);
        }

        protected override void OnDisableAndDestroy()
        {
            textRenderers.Remove(this);
        }

        public override void Remove()
        {
            textRenderers.Remove(this);
        }*/
    }
}
