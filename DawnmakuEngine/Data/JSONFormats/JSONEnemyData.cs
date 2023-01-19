using System;
using System.Collections.Generic;
using System.Text;

namespace DawnmakuEngine.Data.JSONFormats
{
    public class JSONEnemyData
    {
        public string shader;
        public int health;
        public float spawnIFrames;
        public int deathScore;

        public float hurtColliderSize;
        public JSONVec2 hurtColliderOffset;

        public JSONVec2 playerKillcolliderSize;
        public JSONVec2 playerKillColliderOffset;

        public string movementCurve;
        public List<string> animations = new List<string>();
        public List<List<string>> patternsByDifficulty= new List<List<string>>();
    }
}
