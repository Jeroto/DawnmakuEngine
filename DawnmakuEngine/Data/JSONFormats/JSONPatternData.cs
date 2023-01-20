using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Text;

namespace DawnmakuEngine.Data.JSONFormats
{
    public class JSONPatternData
    {
        public int burstCount, bulletsInBurst;
        public float overallDegrees, degreeOffset, randomDegreeOffset;
        public bool aimed;
        public float initialDelay;

        public List<float> burstDelay = new List<float>();
        public List<float> perBulletDelay = new List<float>();
        public List<string> spawnSounds = new List<string>();
        public List<ushort> damage = new List<ushort>();

        public List<Vector2> offsets = new List<Vector2>();
        public List<bool> turnOffsets = new List<bool>();
    }
}
