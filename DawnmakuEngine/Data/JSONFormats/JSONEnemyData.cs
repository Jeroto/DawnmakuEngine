using OpenTK.Mathematics;
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
        public Vector2 hurtColliderOffset;

        public Vector2 playerKillcolliderSize;
        public Vector2 playerKillColliderOffset;

        public string movementCurve;
        public List<string> animations = new List<string>();
        public List<List<string>> patternsByDifficulty= new List<List<string>>();
    }
}
