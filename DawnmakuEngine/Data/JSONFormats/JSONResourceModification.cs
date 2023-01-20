using System;
using System.Collections.Generic;
using System.Text;

namespace DawnmakuEngine.Data.JSONFormats
{
    public class JSONResourceModification
    {
        public string name;
        public List<ModValue> values = new List<ModValue>();

        public class ModValue
        {
            public string valueType;
            public string value;
        }
    }
}
