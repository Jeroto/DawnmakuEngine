using System;
using System.Collections.Generic;
using System.Text;
using OpenTK.Graphics.ES30;
using DawnmakuEngine.Data;
using OpenTK;
using OpenTK.Mathematics;

namespace DawnmakuEngine.Elements
{
    /// <summary>
    /// Used to animate sprites.
    /// Object MUST include SpriteRenderer.
    /// </summary>
    public class TextureAnimator : Element //Object MUST include MeshRenderer
    {
        public SpriteRenderer refRenderer;
        protected int frameIndex = 0, stateIndex = 0, updateStateIndex = 0;
        protected float animFramesRemaining;
        protected bool initialized;

        public new Entity EntityAttachedTo
        {
            get { return entityAttachedTo; }
            set
            {
                RemoveEntitySubscriptions();
                entityAttachedTo = value;
                AddEntitySubscriptions();
                refRenderer = entityAttachedTo.GetElement<SpriteRenderer>();

                if (refRenderer == null)
                    Disable();
                else
                    UpdateAnim(false);
            }
        }
        public int StateIndex
        {
            get
            {
                if (stateIndex <= 0)
                    return 0;
                else if (stateIndex >= animationStates.Length)
                    return animationStates.Length - 1;
                else
                    return stateIndex;
            }
        }
        public int UpdateStateIndex
        {
            get
            {
                if (updateStateIndex <= 0)
                    return 0;
                else if (updateStateIndex >= animationStates.Length)
                    return animationStates.Length - 1;
                else
                    return updateStateIndex;
            }
            set
            {
                if (value <= 0)
                    updateStateIndex = 0;
                else if (value >= animationStates.Length)
                    updateStateIndex = animationStates.Length - 1;
                else
                    updateStateIndex = value;
            }
        }
        public int FrameIndex
        {
            get
            {
                if (frameIndex < 0)
                    return 0;
                else if (frameIndex >= animationStates[stateIndex].animFrames.Length + 1)
                    return animationStates[stateIndex].animFrames.Length;
                else
                    return frameIndex;
            }
            set
            {
                if (value <= 0)
                    frameIndex = 0;
                else if (value >= animationStates[stateIndex].animFrames.Length + 1)
                    frameIndex = animationStates[stateIndex].animFrames.Length;
                else
                    frameIndex = value;
            }
        }

        public AnimationState[] animationStates;

        public override void PostCreate()
        {
            Initialize();
        }
        public override void PreRender()
        {
            Initialize();
        }
        public override void OnUpdate()
        {
            if (stateIndex != UpdateStateIndex && animationStates[UpdateStateIndex].autoTransition != stateIndex)
                UpdateState();

            if (animFramesRemaining <= 0)
                UpdateAnim(true);
            else
                animFramesRemaining -= GameMaster.gameMaster.timeScale;
        }

        void Initialize()
        {
            if (animationStates == null)
                return;
            initialized = true;
            if (refRenderer == null)
            {
                refRenderer = entityAttachedTo.GetElement<SpriteRenderer>();
                if (refRenderer == null)
                {
                    Disable();
                    return;
                }
            }
            UpdateAnim(false);
            GameMaster.gameMaster.PostCreate -= PostCreate;
            GameMaster.gameMaster.PreRender -= PreRender;
            GameMaster.gameMaster.OnUpdate += OnUpdate;
            requiredSubscriptions = UPDATE_SUB;
        }

        public void UpdateAnim(bool updateIndex)
        {
            if (animationStates[StateIndex].autoTransition > -1 && frameIndex <= animationStates[StateIndex].animFrames.Length - 1 &&
               animationStates[UpdateStateIndex].autoTransition != stateIndex)
                UpdateStateIndex = animationStates[StateIndex].autoTransition;


            if (updateIndex)
            {
                FrameIndex++;
                if (FrameIndex >= animationStates[StateIndex].animFrames.Length)
                {
                    if (animationStates[StateIndex].loop)
                        FrameIndex = 0;
                    else
                        FrameIndex = animationStates[StateIndex].animFrames.Length - 1;
                }
            }

            refRenderer.Sprite = animationStates[StateIndex].animFrames[FrameIndex].sprite;

            animFramesRemaining += animationStates[StateIndex].animFrames[FrameIndex].frameDuration;
        }

        void UpdateState()
        {
            stateIndex = UpdateStateIndex;
            frameIndex = 0;
            UpdateAnim(false);
        }

        public static AnimationFrame[] CreateAnimFrames(int count, float[] durations, SpriteSet.Sprite[] sprites)
        {
            AnimationFrame[] tempFrames = new AnimationFrame[count];
            for (int i = 0; i < count; i++)
            {
                tempFrames[i] = new AnimationFrame();
                tempFrames[i].frameDuration = durations[MathHelper.Clamp(i, 0, durations.Length - 1)];
                tempFrames[i].sprite = sprites[i];
            }
            return tempFrames;
        }

        [System.Serializable]
        public class AnimationFrame
        {
            public float frameDuration;
            public SpriteSet.Sprite sprite;
        }

        [System.Serializable]
        public class AnimationState
        {
            public AnimationFrame[] animFrames;
            public int autoTransition = -1;
            public bool loop = true;
        }

        public TextureAnimator() : base(true, false, true)
        { }

        public TextureAnimator(AnimationState[] animStates_) : this()
        {
            animationStates = animStates_;
        }
        public TextureAnimator(SpriteRenderer renderer) : this()
        {
            refRenderer = renderer;
        }
        public TextureAnimator(AnimationState[] animStates_, SpriteRenderer renderer) : this(animStates_)
        {
            refRenderer = renderer;
        }
    }
}
