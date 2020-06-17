using System;
using System.Collections.Generic;
using System.Text;
using DawnmakuEngine.Elements;
using OpenTK;

namespace DawnmakuEngine.Data
{
    class ItemData
    {
        public TextureAnimator.AnimationState[] animations;
        public Vector2 randXRange, randYRange;
        public float maxFallSpeed, gravAccel, xDecel,
            magnetDist, magnetDistSqr, magnetSpeed,
            collectDist, collectDistSqr;

        public bool canBePOC = true,
            autoMagnetDraw = false;
    }
}
