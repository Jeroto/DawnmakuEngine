using System;
using System.Collections.Generic;
using System.Text;

namespace DawnmakuEngine.Data.JSONFormats
{
    public class JSONShotData
    {
        public List<string> bombName = new List<string>();

        public float moveSpeed = -1;
        public float focusSpeedPercent = -1;
        public float colliderSize = -1;
        public float colliderOffsetX = 0;
        public float colliderOffsetY = 0;

        public List<string> orbs = new List<string>();
    }
}
