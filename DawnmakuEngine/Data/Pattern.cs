using System;
using System.Collections.Generic;
using System.Text;
using OpenTK;

namespace DawnmakuEngine.Data
{
    public class Pattern
    {
        public int burstCount = int.MaxValue;
        public int bulletsInBurst = 0;
        public float overallDegrees = 360;
        public float degreeOffset = 0;
        public float randomDegreeOffset = 0;
        public bool perBulletRandomOffset = false;
        public bool aimed = false;

        public List<float> burstDelay = new List<float>();
        public float initialDelay = 1;
        public List<float> perBulletDelay = new List<float>();

        public List<AudioData> spawnSounds = new List<AudioData>();
        public List<ushort> damage = new List<ushort>();

        public List<Vector2> offsets = new List<Vector2>();
        public List<bool> turnOffsets = new List<bool>();

        virtual public Pattern CopyPattern()
        {
            Pattern newCopy = new Pattern();
            newCopy.burstCount = burstCount;
            newCopy.bulletsInBurst = bulletsInBurst;
            newCopy.overallDegrees = overallDegrees;
            newCopy.degreeOffset = degreeOffset;
            newCopy.randomDegreeOffset = randomDegreeOffset;
            newCopy.perBulletRandomOffset = perBulletRandomOffset;
            newCopy.aimed = aimed;
            newCopy.burstDelay = burstDelay;
            newCopy.initialDelay = initialDelay;

            int i;

            for (i = 0; i < perBulletDelay.Count; i++)
                newCopy.perBulletDelay.Add(perBulletDelay[i]);

            for (i = 0; i < damage.Count; i++)
                newCopy.damage.Add(damage[i]);

            for (i = 0; i < spawnSounds.Count; i++)
                newCopy.spawnSounds.Add(spawnSounds[i]);

            for (i = 0; i < offsets.Count; i++)
            {
                newCopy.offsets.Add(offsets[i]);
                newCopy.turnOffsets.Add(turnOffsets[i]);
            }
            return newCopy;
        }

        public class PatternVariables
        {
            public int burstsRemaining = 999, burstDelayIndex = 0, perBulletIndex = 0, delayIndex = 0, damageIndex = 0, offsetIndex = 0, turnOffsetIndex = 0, soundIndex = 0;
            public float fireDelay;
            virtual public PatternVariables Copy()
            {
                PatternVariables copy = new PatternVariables();
                copy.burstsRemaining = burstsRemaining;
                copy.perBulletIndex = perBulletIndex;
                copy.delayIndex = delayIndex;
                copy.damageIndex = damageIndex;
                copy.offsetIndex = offsetIndex;
                copy.turnOffsetIndex = turnOffsetIndex;
                copy.fireDelay = fireDelay;
                copy.soundIndex = soundIndex;
                return copy;
            }
        }

        virtual public void ProcessPattern(Entity caller, bool callerIsPlayer, ref PatternVariables vars)
        {
            //To be overwritten
        }
    }
}
