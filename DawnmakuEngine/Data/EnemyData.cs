using System;
using System.Collections.Generic;
using System.Text;
using OpenTK;

namespace DawnmakuEngine.Data
{
    class EnemyData
    {
        public float health;
        public float invTime = 30;
        public int deathScoreValue;
        public int hitsToGiveScore;

        public float colliderSize;
        public Vector2 colliderOffset;

        public int movementCurve;
        public int[] animations;
        public Difficulty[] patternsByDifficulty;

        [System.Serializable]
        public class Difficulty
        {
            public int[] patterns;
        }
    }
}
