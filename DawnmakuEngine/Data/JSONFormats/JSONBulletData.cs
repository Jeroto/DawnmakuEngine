using System;
using System.Collections.Generic;
using System.Text;

namespace DawnmakuEngine.Data.JSONFormats
{
    public class JSONBulletData
    {
        public string shader;
        public bool isAnimated;
        public float spinSpeed;
        public bool shouldTurn;
        public int spriteColorCount;
        public bool randomizeSprite;
        public float boundsExitDist = -1;

        public List<Collider> colliders = new List<Collider>();
        public List<List<string>> animStates = new List<List<string>>();

        public class Collider
        {
            public float sizeX, sizeY;
            public float offsetX, offsetY;
        }
    }
}
