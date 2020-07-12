using OpenTK;
using System;
using System.Collections.Generic;
using System.Text;

namespace DawnmakuEngine.Data
{
    public class StageData
    {
        //BGM
        public List<CamVelocities> camVel = new List<CamVelocities>();
        public List<SectionSpawns> secSpawns = new List<SectionSpawns>();
        public List<EnemySpawn> enemySpawns = new List<EnemySpawn>();

        public class CamVelocities
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
