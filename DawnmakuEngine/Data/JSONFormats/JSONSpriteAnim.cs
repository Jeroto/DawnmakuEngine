using System;
using System.Collections.Generic;
using System.Text;

namespace DawnmakuEngine.Data.JSONFormats
{
    public class JSONSpriteAnim
    {
        public string name;
        public string autotransition;
        public string loop;
        public List<Frame> frames = new List<Frame>();
        public class Frame
        {
            public string duration;
            public string spriteset;
            public string sprite;
        }
    }
}
