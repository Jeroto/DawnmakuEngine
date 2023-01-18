using System;
using System.Collections.Generic;
using System.Text;

namespace DawnmakuEngine.Data.JSONFormats
{
    public class JSONShaderData
    {
        public string vert, frag;
        public List<Constant> constants = new List<Constant>();

        public class Constant
        {
            public string name;
            public string type;
            public string value;
        }
    }
}
