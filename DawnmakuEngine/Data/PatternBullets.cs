using System;
using System.Collections.Generic;
using System.Text;
using OpenTK;
using DawnmakuEngine.Elements;
using OpenTK.Mathematics;

namespace DawnmakuEngine.Data
{
    public class PatternBullets : Pattern
    {
        public List<InstanceList> bulletStages = new List<InstanceList>();

        [System.Serializable]
        public class InstanceList
        {
            public List<BulletElement.BulletStage> instances = new List<BulletElement.BulletStage>();
        }

        override public Pattern CopyPattern()
        {
            PatternBullets newCopy = new PatternBullets();
            newCopy.burstCount = burstCount;
            newCopy.bulletsInBurst = bulletsInBurst;
            newCopy.overallDegrees = overallDegrees;
            newCopy.degreeOffset = degreeOffset;
            newCopy.randomDegreeOffset = randomDegreeOffset;
            newCopy.perBulletRandomOffset = perBulletRandomOffset;
            newCopy.aimed = aimed;
            newCopy.burstDelay = burstDelay;
            newCopy.initialDelay = initialDelay;

            int i, s;

            for (i = 0; i < perBulletDelay.Count; i++)
                newCopy.perBulletDelay.Add(perBulletDelay[i]);

            for (i = 0; i < spawnSounds.Count; i++)
                newCopy.spawnSounds.Add(spawnSounds[i]);

            for (i = 0; i < damage.Count; i++)
                newCopy.damage.Add(damage[i]);

            for (i = 0; i < offsets.Count; i++)
            {
                newCopy.offsets.Add(offsets[i]);
                newCopy.turnOffsets.Add(turnOffsets[i]);
            }

            for (s = 0; s < bulletStages.Count; s++)
            {
                newCopy.bulletStages.Add(new InstanceList());
                for (i = 0; i < bulletStages[s].instances.Count; i++)
                {
                    newCopy.bulletStages[s].instances.Add(bulletStages[s].instances[i].CopyValues());
                }
            }

            return newCopy;
        }


        public class BulletPatternVars : PatternVariables
        {
            public int[] instanceIndexes;

            public BulletPatternVars(PatternBullets pattern)
            {
                burstsRemaining = pattern.burstCount;
                instanceIndexes = new int[pattern.bulletStages.Count];
            }
            public BulletPatternVars(int burstCount, int instanceIndexCount)
            {
                burstsRemaining = burstCount;
                instanceIndexes = new int[instanceIndexCount];
            }

            override public PatternVariables Copy()
            {
                BulletPatternVars copy = new BulletPatternVars(burstsRemaining, instanceIndexes.Length);

                copy.perBulletIndex = perBulletIndex;
                copy.delayIndex = delayIndex;
                copy.damageIndex = damageIndex;
                copy.offsetIndex = offsetIndex;
                copy.turnOffsetIndex = turnOffsetIndex;
                copy.fireDelay = fireDelay;
                for (int i = 0; i < instanceIndexes.Length; i++)
                {
                    copy.instanceIndexes[i] = instanceIndexes[i];
                }
                return copy;
            }
        }

        public override void ProcessPattern(Entity caller, bool callerIsPlayer, ref PatternVariables vars)
        {
            GameMaster gameMaster = GameMaster.gameMaster;
            BulletPatternVars bulletVars = (BulletPatternVars)vars;

            bulletVars.fireDelay -= gameMaster.timeScale;
            if (vars.fireDelay > 0)
                return;

            Vector2 position = caller.WorldPosition.Xy;

            float random = 0;

            do
            {
                while (bulletVars.fireDelay > 0)
                {
                    bulletVars.fireDelay -= gameMaster.timeScale;
                    return;
                }

                if (bulletVars.perBulletIndex == 0)
                    random = !perBulletRandomOffset ? GameMaster.Random(-randomDegreeOffset, randomDegreeOffset) : 0;
                float startingAngle = degreeOffset + 
                    (aimed ? (callerIsPlayer ? DawnMath.FindAngleToObjectDeg(position, DawnMath.FindNearestEnemy(position)) : DawnMath.FindAngleToPlayerDeg(position)) : 0)
                    + random;
                float currentAngle;
                Vector2 offset;
                for (int i = bulletVars.perBulletIndex; i < bulletsInBurst; i++)
                {
                    currentAngle = startingAngle + (((-overallDegrees / bulletsInBurst) * (bulletsInBurst / 2)) +
                            ((overallDegrees / bulletsInBurst) * i)) + ((perBulletRandomOffset) ?
                            GameMaster.Random(-randomDegreeOffset, randomDegreeOffset) : 0);

                    offset = !turnOffsets[bulletVars.turnOffsetIndex] ? offsets[bulletVars.offsetIndex] :
                        DawnMath.CalculateCircleDeg(offsets[bulletVars.offsetIndex].LengthFast, DawnMath.FindAngleDeg(offsets[bulletVars.offsetIndex]) + currentAngle);


                    BulletElement.BulletStage[] currentStages = new BulletElement.BulletStage[bulletVars.instanceIndexes.Length];
                    for (int s = 0; s < bulletVars.instanceIndexes.Length; s++)
                    {
                        currentStages[s] = bulletStages[s].instances[bulletVars.instanceIndexes[s]].CopyValues();
                    }

                    currentStages[0].movementDirection = DawnMath.CalculateCircleDeg(currentAngle);


                    BulletElement.SpawnBullet(currentStages, new Vector3(position + offset), BulletElement.ShouldSpin(currentStages[0].spriteType), 
                        damage[bulletVars.damageIndex], 0, callerIsPlayer);


                    if(callerIsPlayer && !gameMaster.playerBulletSpawnSoundPlayed && spawnSounds.Count > 0)
                    {
                        gameMaster.audioManager.PlaySound(spawnSounds[bulletVars.soundIndex], AudioController.AudioCategory.PlayerBulletSpawn,
                            gameMaster.PlayerShootVol);
                        bulletVars.soundIndex = DawnMath.Repeat(bulletVars.soundIndex + 1, spawnSounds.Count - 1);
                        gameMaster.playerBulletSpawnSoundPlayed = true;
                    }
                    else if(!callerIsPlayer && !gameMaster.enemyBulletSpawnSoundPlayed && spawnSounds.Count > 0)
                    {
                        gameMaster.audioManager.PlaySound(spawnSounds[bulletVars.soundIndex], AudioController.AudioCategory.BulletSpawn,
                            gameMaster.BulletSpawnVol);
                        bulletVars.soundIndex = DawnMath.Repeat(bulletVars.soundIndex + 1, spawnSounds.Count - 1);
                        gameMaster.enemyBulletSpawnSoundPlayed = true;
                    }

                    bulletVars.turnOffsetIndex = DawnMath.Repeat(bulletVars.turnOffsetIndex + 1, turnOffsets.Count - 1);
                    bulletVars.offsetIndex = DawnMath.Repeat(bulletVars.offsetIndex + 1, offsets.Count - 1);
                    bulletVars.damageIndex = DawnMath.Repeat(bulletVars.damageIndex + 1, damage.Count - 1);

                    for (int s = 0; s < bulletVars.instanceIndexes.Length; s++)
                    {
                        if (bulletVars.instanceIndexes[s] + 1 >= bulletStages[s].instances.Count - 1)
                            bulletVars.instanceIndexes[s] = 0;
                        else
                            bulletVars.instanceIndexes[s]++;
                    }


                    bulletVars.perBulletIndex++;
                    if (bulletVars.perBulletIndex >= bulletsInBurst)
                    {
                        if (bulletsInBurst < 2)
                            bulletVars.fireDelay += perBulletDelay[bulletVars.delayIndex];
                        bulletVars.fireDelay += burstDelay[bulletVars.burstDelayIndex];
                        bulletVars.perBulletIndex = 0;
                        bulletVars.burstsRemaining--;
                        bulletVars.burstDelayIndex = DawnMath.Repeat(bulletVars.burstDelayIndex + 1, burstDelay.Count - 1);
                    }
                    else if (perBulletDelay[bulletVars.delayIndex] > 0)
                    {
                        //Console.WriteLine(perBulletDelay[delayIndex]);
                        //fireDelay -= bulletSpawnWait;
                        bulletVars.fireDelay += perBulletDelay[bulletVars.delayIndex];
                        bulletVars.delayIndex = DawnMath.Repeat(bulletVars.delayIndex + 1, perBulletDelay.Count - 1);
                        break;
                    }
                    bulletVars.delayIndex = DawnMath.Repeat(bulletVars.delayIndex + 1, perBulletDelay.Count - 1);
                    if (bulletVars.fireDelay > 0)
                        return;
                }
            } while (bulletVars.burstsRemaining != 0);
        }
    }
}
