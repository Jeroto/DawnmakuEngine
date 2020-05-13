using System;
using System.Collections.Generic;
using System.Text;
using DawnmakuEngine.Elements;
using OpenTK;

namespace DawnmakuEngine.Data
{
    class PlayerOrbData
    {
        public bool[] activePowerLevels = null;
        public Vector2 unfocusPosition = Vector2.Zero, 
            focusPosition = Vector2.Zero;
        public float framesToMove = 10,
            rotateDegreesPerSecond = 10;
        public TextureAnimator.AnimationState[] animStates = null;
        public int startAnimFrame = 0;
        public bool leaveBehindWhileFocused = false,
            leaveBehindWhileUnfocused = false,
            followPrevious = false;
        public float followDist, followDistSq;

        public Pattern[] unfocusedPatterns = null;
        public Pattern[] focusedPatterns = null;
    }
}
