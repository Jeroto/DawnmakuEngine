using System;
using System.Collections.Generic;
using System.Text;

namespace DawnmakuEngine.Data.JSONFormats
{
    internal class JSONResource
    {
        public string name;
        public string type = "Int";
        public string value = "NULL";
        public float min = 0, max = 999_999_999;
    }
}
