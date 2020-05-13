using System;
using System.Collections.Generic;
using System.Text;
using DawnmakuEngine.Elements;
using OpenTK;

namespace DawnmakuEngine.Data
{
    class PlayerShotData
    {
        public string[] bombName;

        public float moveSpeed = -1;
        public float focusModifier = -1;
        public float colliderSize = -1;
        public Vector2 colliderOffset = Vector2.Zero;

        public PlayerOrbData[] orbData = null;
        public TextureAnimator.AnimationState[] animStates = null;
    }
}
