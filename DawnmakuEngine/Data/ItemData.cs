using System;
using System.Collections.Generic;
using System.Text;
using DawnmakuEngine.Elements;
using OpenTK;

namespace DawnmakuEngine.Data
{
    public class ItemData
    {
        public Shader shader;
        public TextureAnimator.AnimationState[] animations;
        public Vector2 randXRange, randYRange;
        public float maxFallSpeed, gravAccel, xDecel,
            magnetDist, magnetDistSqr, drawSpeed, magnetSpeed,
            collectDist, collectDistSqr;

        public bool canBePOC = true,
            autoDraw = false;
    }
}
