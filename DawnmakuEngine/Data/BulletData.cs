using DawnmakuEngine.Elements;
using OpenTK;
using OpenTK.Mathematics;

namespace DawnmakuEngine.Data
{
    public class BulletData
    {
        public Shader shader;
        public bool isAnimated, shouldTurn, randomizeSprite;
        public float spinSpeed = 0;
        public float boundsExitDist = -1;
        public Vector2[] colliderSize, colliderOffset;
        public ushort spriteColors;

        public TextureAnimator.AnimationState[][] animStates = new TextureAnimator.AnimationState[0][];
    }
}
