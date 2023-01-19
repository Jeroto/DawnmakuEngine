using System;
using System.Collections.Generic;
using System.Text;

namespace DawnmakuEngine.Data.JSONFormats
{
    public class JSONStageData
    {
        public string stageBGM;
        public string bossBGM;

        public List<AmbientLights> ambientLights = new List<AmbientLights>();
        public List<CamMovements> backgroundCamMovement = new List<CamMovements>();
        public List<BackgroundSegment> backgroundSegments = new List<BackgroundSegment>();
        public List<EnemySpawn> enemySpawns = new List<EnemySpawn>();


        public class AmbientLights
        {
            public string colorForm;
            public JSONColor colorVal;
            public float intensity;
            public uint startTime;
            public float transitionTime;
        }

        public class CamMovements
        {
            public JSONVec3 vel;
            public JSONVec3 randMinPos;
            public JSONVec3 randMaxPos;
            public JSONVec3 randMinRot;
            public JSONVec3 randMaxRot;

            public uint randomizationDelay;
            public uint startTime;
            public float transitionTime;
        }

        public class BackgroundSegment
        {
            public uint time;
            public JSONVec3 spawnDirection;
            public byte segmentCount;
            public List<string> segments = new List<string>();
        }

        public class EnemySpawn
        {
            public string enemy;
            public uint time;
            public JSONVec2 pos;
            public List<string> items = new List<string>();
        }
    }
}
