using DawnmakuEngine.Elements;
using System;
using System.Collections.Generic;
using System.Text;
using OpenTK;

namespace DawnmakuEngine.Data
{
    public class PlayerCharData
    {
        public Shader charShader, hitboxShader, focusEffectShader;
        public string name;
        public string jpName;
        public float moveSpeed;
        public float focusModifier;
        public float colliderSize;
        public Vector2 colliderOffset;
        public AudioData hitSound, focusSound, grazeSound;
        public PlayerTypeData[] types;
        public TextureAnimator.AnimationState[] animStates;
        public TextureAnimator.AnimationState hitboxAnim, focusEffectAnim;
        public float hitboxInsetAmount = 0;
        public float focusEffectRotSpeed;
    }
}
