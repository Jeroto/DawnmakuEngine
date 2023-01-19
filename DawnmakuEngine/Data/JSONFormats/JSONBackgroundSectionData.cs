using System;
using System.Collections.Generic;
using System.Text;

namespace DawnmakuEngine.Data.JSONFormats
{
    public class JSONBackgroundSectionData
    {
        public float sectionLength;
        public JSONVec3 offset;

        public List<ObjectSetup> objectSetup = new List<ObjectSetup>();

        public class ObjectSetup
        {
            public List<string> objectOptions = new List<string>();
            public int parentIndex;
            public JSONVec3 pos;
            public JSONVec3 rot;
            public JSONVec3 scale;

            public List<ElementSetup> elements = new List<ElementSetup>();
        }

        public class ElementSetup
        {
            public string capsSensitiveName;
            public List<string> constructorValues;
        }
    }
}
