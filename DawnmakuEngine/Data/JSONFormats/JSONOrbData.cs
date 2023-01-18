using System;
using System.Collections.Generic;
using System.Text;

namespace DawnmakuEngine.Data.JSONFormats
{
    public class JSONOrbData
    {
        public string shader;
        public List<bool> activePowerLevels = new List<bool>();
        public JSONVec2 unfocusPos;
        public JSONVec2 focusPos;

        public int framesToMove;
        public float rotateSpeed;
        public List<string> animStates = new List<string>();
        public int startAnimFrame = 0;
        public string leaveBehind = "never";
        public bool followPrevious = false;
        public float followDist = 0;

        public List<string> unfocusPatternsByPower = new List<string>();
        public List<string> focusPatternsByPower = new List<string>();
    }
}
