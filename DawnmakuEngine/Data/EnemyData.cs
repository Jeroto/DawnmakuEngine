using System;
using System.Collections.Generic;
using System.Text;
using DawnmakuEngine.Elements;
using OpenTK;

namespace DawnmakuEngine.Data
{
    public class EnemyData
    {
        public string enemyName;
        public int health;
        public float invTime = 30;
        public int deathScoreValue;

        public float colliderSize;
        public Vector2 colliderOffset;

        public Bezier movementCurve;
        public TextureAnimator.AnimationState[] animations;
        public Difficulty[] patternsByDifficulty;

        [System.Serializable]
        public class Difficulty
        {
            public Pattern[] patterns;
        }
    }
}
