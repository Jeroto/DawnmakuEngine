using System;
using System.Collections.Generic;
using System.Text;
using OpenTK;
using DawnmakuEngine.Elements;

namespace DawnmakuEngine
{
    class EnemyPattern
    {
        public int burstCount = int.MaxValue;
        public int bulletsInBurst;
        public float overallDegrees;
        public float degreeOffset;
        public float randomDegreeOffset;
        public bool perBulletRandomOffset;
        public bool aimed;

        public float burstDelay;
        public float initialDelay;
        public List<float> perBulletDelay = new List<float>();

        //Old single-stage bullet code, keeping it around for a while as a just-in-case
        /*public List<BaseBulletScript.BulletType> bulletType = new List<BaseBulletScript.BulletType>();
        public List<BaseBulletScript.BulletType> customSpriteType = new List<BaseBulletScript.BulletType>();
        public List<GameObject> customBullet = new List<GameObject>();
        public List<bool> customHasScript = new List<bool>();
        public List<bool> customHasSprite = new List<bool>();
        public List<BaseBulletScript.BulletColor> bulletColors = new List<BaseBulletScript.BulletColor>();
        public List<float> bulletEndSpeed = new List<float>();
        public List<float> bulletStartSpeed = new List<float>();
        public List<float> bulletTimeToSpeedUp = new List<float>();
        public List<bool> turn = new List<bool>();*/

        public List<int> customBullet = new List<int>();
        public List<bool> customHasScript = new List<bool>();
        public List<ushort> damage = new List<ushort>();


        public List<Vector2> offsets = new List<Vector2>();
        public List<bool> turnOffsets = new List<bool>();

        public List<InstanceList> bulletStages = new List<InstanceList>();

        [System.Serializable]
        public class InstanceList
        {
            public List<BulletElement.BulletStage> instances = new List<BulletElement.BulletStage>();
        }

        public EnemyPattern CopyPattern()
        {
            EnemyPattern newCopy = new EnemyPattern();
            newCopy.burstCount = burstCount;
            newCopy.bulletsInBurst = bulletsInBurst;
            newCopy.overallDegrees = overallDegrees;
            newCopy.degreeOffset = degreeOffset;
            newCopy.randomDegreeOffset = randomDegreeOffset;
            newCopy.perBulletRandomOffset = perBulletRandomOffset;
            newCopy.aimed = aimed;
            newCopy.burstDelay = burstDelay;
            newCopy.initialDelay = initialDelay;
            for (int i = 0; i < perBulletDelay.Count; i++)
                newCopy.perBulletDelay.Add(perBulletDelay[i]);
            /* for (int i = 0; i < bulletType.Count; i++)
             {
                 newCopy.bulletType.Add(bulletType[i]);
                 newCopy.customSpriteType.Add(customSpriteType[i]);
                 newCopy.customBullet.Add(customBullet[i]);
                 newCopy.customHasScript.Add(customHasScript[i]);
                 newCopy.customHasSprite.Add(customHasSprite[i]);
             }
             for (int i = 0; i < bulletColors.Count; i++)
                 newCopy.bulletColors.Add(bulletColors[i]);
             for (int i = 0; i < bulletEndSpeed.Count; i++)
             {
                 newCopy.bulletEndSpeed.Add(bulletEndSpeed[i]);
                 newCopy.bulletStartSpeed.Add(bulletStartSpeed[i]);
                 newCopy.bulletTimeToSpeedUp.Add(bulletTimeToSpeedUp[i]);
                 newCopy.turn.Add(turn[i]);
             }*/
            for (int i = 0; i < offsets.Count; i++)
            {
                newCopy.offsets.Add(offsets[i]);
                newCopy.turnOffsets.Add(turnOffsets[i]);
            }
            return newCopy;
        }
    }
}
