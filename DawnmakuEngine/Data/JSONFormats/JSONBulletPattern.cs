using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Text;
using OpenTK.Mathematics;

namespace DawnmakuEngine.Data.JSONFormats
{
    public class JSONBulletPattern : JSONPatternData
    {
        public List<JSONBulletStage> bulletStages = new List<JSONBulletStage>();

        public class StartEnd { public float start, end; }

        public class JSONBulletStage
        {
            public string bulletType, bulletColor;
            public JSONColor color;
            public float framesTochangeTint = 0;
            public float renderScale = 1;

            public float angle = 0;
            public float startingSpeed = 0, endingSpeed = 0;
            public float framesToChangeSpeed = 0;
            public bool rotate, reAim, keepOldAngle, modifyAngle;
            public bool affectedByTimescale = true;

            public float framesToLast;
            public bool turnAtStart;
            public float initialMoveDelay = 0;
            public bool turnAfterDelay;

            public string stageSound = "NULL";
            public bool hasEffect = false;
            public string effectColor;
            public Vector2 effectSize;
            public StartEnd effectOpacity;
        }
    }
}
