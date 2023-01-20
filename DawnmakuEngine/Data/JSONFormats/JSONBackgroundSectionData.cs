using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Text;

namespace DawnmakuEngine.Data.JSONFormats
{
    public class JSONBackgroundSectionData
    {
        public float sectionLength;
        public Vector3 offset;

        public List<ObjectSetup> objectSetup = new List<ObjectSetup>();

        public class ObjectSetup
        {
            public List<string> objectOptions = new List<string>();
            public int parentIndex;
            public Vector3 pos;
            public Vector3 rot;
            public Vector3 scale;

            public List<ElementSetup> elements = new List<ElementSetup>();
        }

        public class ElementSetup
        {
            public string capsSensitiveName;
            public List<string> constructorValues;
        }
    }
}
