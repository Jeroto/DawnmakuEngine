using DawnmakuEngine.Elements;
using OpenTK;

namespace DawnmakuEngine.Data
{
    public class BulletData
    {
        public Shader shader;
        public bool isAnimated, shouldSpin, shouldTurn, randomizeSprite;
        public float boundsExitDist = -1;
        public Vector2[] colliderSize, colliderOffset;
        public ushort spriteColors;

        public TextureAnimator.AnimationState[][] animStates = new TextureAnimator.AnimationState[0][];
    }
}
