using System;
using System.Collections.Generic;
using System.Text;

namespace DawnmakuEngine.Data.JSONFormats
{
    public class JSONItemData
    {
        public string shader;
        public bool canBeAutoCollected;
        public bool autoDraw;
        public List<string> animStates = new List<string>();

        public bool overrideDefaultValues;
        public JSONMinMax randXVelRange;
        public JSONMinMax randYVelRange;
        public float maxFallSpeed;
        public float gravAccel;
        public float xDecel;
        public float magnetDist;
        public float magnetSpeed;
        public float drawSpeed;
        public float collectDist;
    }
}
