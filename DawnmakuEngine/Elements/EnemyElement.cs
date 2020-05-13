using System;
using System.Collections.Generic;
using System.Text;
using DawnmakuEngine.Data;
using OpenTK;

namespace DawnmakuEngine.Elements
{
    class EnemyElement : BaseEnemyElement
    {

        public EnemyElement(EnemyData enemyToSpawn) : base(enemyToSpawn)
        {

        }

        public static Entity SpawnEnemy(EnemyData enemy, Vector3 position)
        {
            Entity newSpawn = new Entity(enemy.enemyName, position);
            MeshRenderer renderer = new MeshRenderer(Mesh.CreatePrimitiveMesh(Mesh.Primitives.SqrPlaneWTriangles), OpenTK.Graphics.ES30.BufferUsageHint.DynamicDraw,
                GameMaster.gameMaster.spriteShader, enemy.animations[0].animFrames[0].sprite.tex);
            newSpawn.AddElement(renderer);
            newSpawn.AddElement(new EnemyElement(enemy));
            newSpawn.AddElement(new TextureAnimator(enemy.animations, renderer, true));

            return newSpawn;
        }
    }
}
