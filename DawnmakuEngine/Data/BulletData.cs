using DawnmakuEngine.Elements;
using System;
using System.Collections.Generic;
using System.Text;

namespace DawnmakuEngine.Data
{
    class BulletData
    {
        public bool isAnimated, shouldSpin, shouldTurn, randomizeSprite;
        public float colliderSize, colliderOffsetX, colliderOffsetY;
        public ushort spriteColors;

        public TextureAnimator.AnimationState[][] animStates = new TextureAnimator.AnimationState[0][];
    }
}
