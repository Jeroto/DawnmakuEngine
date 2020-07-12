using DawnmakuEngine.Elements;
using System;
using System.Collections.Generic;
using System.Text;
using OpenTK;

namespace DawnmakuEngine.Data
{
    class PlayerCharData
    {
        public Shader charShader, hitboxShader, focusEffectShader;
        public string name;
        public string jpName;
        public float moveSpeed;
        public float focusModifier;
        public float colliderSize;
        public Vector2 colliderOffset;
        public PlayerTypeData[] types;
        public TextureAnimator.AnimationState[] animStates;
        public TextureAnimator.AnimationState hitboxAnim, focusEffectAnim;
        public float hitboxInsetAmount = 0;
        public float focusEffectRotSpeed;
    }
}
