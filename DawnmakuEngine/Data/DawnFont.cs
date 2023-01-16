using System;
using System.Collections.Generic;
using System.Reflection.PortableExecutable;
using System.Text;

namespace DawnmakuEngine.Data
{
    public class DawnFont
    {
        public string supportedChars = "";
        public SpriteSet sprites;
        public float defaultSize = 1;
        public float defaultAdvance = 0;
        public float defaultHeight = 0;

        public Dictionary<char, CharSpecs> charSpecs = new Dictionary<char, CharSpecs>();

        public class CharSpecs
        {
            public float advance;
            public float heightOffset;
            public Dictionary<char, float> kerning = new Dictionary<char, float>();
        }
    }
}
