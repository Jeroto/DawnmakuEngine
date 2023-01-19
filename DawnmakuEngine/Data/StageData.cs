using OpenTK;
using System;
using System.Collections.Generic;
using System.Text;
using OpenTK.Mathematics;

namespace DawnmakuEngine.Data
{
    public class StageData
    {
        public string stageTrackFile, bossTrackFile;
        public List<AmbientLights> ambientLights = new List<AmbientLights>();
        public List<CamMovements> camMovements = new List<CamMovements>();
        public List<SectionSpawns> secSpawns = new List<SectionSpawns>();
        public List<EnemySpawn> enemySpawns = new List<EnemySpawn>();

        public class AmbientLights
        {
            public float r, g, b, intensity;
            public uint time;
            public float transitionTime;

        }
        public class CamMovements
        {
            public Vector3 vel,
                randMinPos, randMaxPos,
                randMinRot, randMaxRot;
            public uint randDelay;
            public uint time;
            public float transitionTime;
        }
        public class SectionSpawns
        {
            public Vector3 spawnDirection = DawnMath.vec3Forward;
            public byte segmentCount;
            public List<BackgroundSection> section = new List<BackgroundSection>();
            public uint time;
        }
        public class EnemySpawn
        {
            public EnemyData enemy;
            public uint time;
            public Vector3 pos;
            public ItemData[] itemSpawns = new ItemData[0];
        }
    }
}
