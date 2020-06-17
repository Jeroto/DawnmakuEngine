using System;
using System.Collections.Generic;
using System.Text;
using DawnmakuEngine.Data;
using OpenTK;

namespace DawnmakuEngine.Elements
{
    class EnemyElement : BaseEnemyElement
    {
        public override void SpawnBullets()
        {
            Pattern[] patternSet = enemyData.patternsByDifficulty[gameMaster.difficulty].patterns;
            int patternCount = patternSet.Length;
            for (int i = 0; i < patternCount; i++)
            {
                if(patternVars[i].burstsRemaining > 0)
                    patternSet[i].ProcessPattern(entityAttachedTo, false, ref patternVars[i]);
            }
        }

        public EnemyElement(EnemyData enemyToSpawn) : base(enemyToSpawn)
        {
            int patternCount = enemyData.patternsByDifficulty[GameMaster.gameMaster.difficulty].patterns.Length;
            patternVars = new Pattern.PatternVariables[patternCount];
            Pattern pattern;
            for (int i = 0; i < patternCount; i++)
            {
                pattern = enemyData.patternsByDifficulty[GameMaster.gameMaster.difficulty].patterns[i];
                switch (pattern.GetType().Name)
                {
                    case "PatternBullets":
                        patternVars[i] = new PatternBullets.BulletPatternVars((PatternBullets)pattern);
                        patternVars[i].fireDelay = pattern.initialDelay;
                        break;
                }
            }
        }

        public static Entity SpawnEnemy(EnemyData enemy, Vector3 position)
        {
            Entity newSpawn = new Entity(enemy.enemyName, position);
            MeshRenderer renderer = new MeshRenderer(Mesh.CreatePrimitiveMesh(Mesh.Primitives.SqrPlaneWTriangles), "enemies",
                OpenTK.Graphics.ES30.BufferUsageHint.DynamicDraw, GameMaster.gameMaster.spriteShader, enemy.animations[0].animFrames[0].sprite.tex, true);
            newSpawn.AddElement(renderer);
            newSpawn.AddElement(new EnemyElement(enemy));
            newSpawn.AddElement(new TextureAnimator(enemy.animations, renderer));

            return newSpawn;
        }
    }
}
