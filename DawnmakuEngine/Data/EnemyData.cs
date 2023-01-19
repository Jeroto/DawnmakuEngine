using System;
using System.Collections.Generic;
using System.Text;
using DawnmakuEngine.Elements;
using OpenTK.Mathematics;

namespace DawnmakuEngine.Data
{
    public class EnemyData
    {
        public Shader shader;
        public string enemyName;
        public int health;
        public float spawnIFrames = 30;
        public int deathScoreValue;

        public float hurtColliderSize;
        public Vector2 hurtColliderOffset;

        public Vector2 killColliderSize,
            killColliderOffset;

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
