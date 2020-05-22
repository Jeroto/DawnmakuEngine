using System;
using System.Collections.Generic;
using System.Text;
using OpenTK.Graphics.ES30;
using DawnmakuEngine.Data;

namespace DawnmakuEngine.Elements
{
    /// <summary>
    /// Used to animate images on the MeshRenderer
    /// Object MUST include MeshRenderer
    /// </summary>
    class TextureAnimator : Element //Object MUST include MeshRenderer
    {
        public MeshRenderer refRenderer;
        protected int frameIndex = 0, stateIndex = 0, updateStateIndex = 0;
        protected float animFramesRemaining;
        public bool resizePlane;

        public new Entity EntityAttachedTo
        {
            get { return entityAttachedTo; }
            set
            {
                entityAttachedTo = value;
                if (refRenderer == null)
                    refRenderer = entityAttachedTo.GetElement<MeshRenderer>();

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
                if (stateIndex < 0)
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
                if (updateStateIndex < 0)
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
            if (refRenderer == null)
            {
                refRenderer = entityAttachedTo.GetElement<MeshRenderer>();
                if (refRenderer == null)
                {
                    Disable();
                    return;
                }
            }
            UpdateAnim(false);
            base.PostCreate();
        }
        public override void OnUpdate()
        {
            if (stateIndex != UpdateStateIndex && animationStates[UpdateStateIndex].autoTransition != stateIndex)
                UpdateState();

            if (animFramesRemaining <= 0)
                UpdateAnim(true);
            else
                animFramesRemaining--;
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

            refRenderer.tex = animationStates[StateIndex].animFrames[FrameIndex].sprite.tex;

            if (resizePlane)
            {
                if(refRenderer.mesh.triangleData != null)
                {
                    for (int i = 0; i < refRenderer.mesh.vertices.Length; i+=5)
                    {
                        refRenderer.mesh.vertices[i] = refRenderer.tex.Width / 2 *
                            Math.Abs(animationStates[StateIndex].animFrames[FrameIndex].sprite.right - animationStates[StateIndex].animFrames[FrameIndex].sprite.left)
                            * (i / 5 == 0 || i / 5 == 3 ? -1 : 1);
                        refRenderer.mesh.vertices[i + 1] = refRenderer.tex.Height / 2 *
                            Math.Abs(animationStates[StateIndex].animFrames[FrameIndex].sprite.bottom - animationStates[StateIndex].animFrames[FrameIndex].sprite.top)
                            * (i / 5 == 2 || i / 5 == 3 ? -1 : 1);
                        refRenderer.mesh.vertices[i + 2] = 0;
                    }
                }
                else
                {
                    for (int i = 0; i < refRenderer.mesh.vertices.Length; i += 5)
                    {
                        refRenderer.mesh.vertices[i] = refRenderer.tex.Width / 2 *
                            Math.Abs(animationStates[StateIndex].animFrames[FrameIndex].sprite.right - animationStates[StateIndex].animFrames[FrameIndex].sprite.left)
                            * (i / 5 == 0 || i / 5 == 3 || i / 5 == 5 ? -1 : 1);
                        refRenderer.mesh.vertices[i + 1] = refRenderer.tex.Height / 2 *
                            Math.Abs(animationStates[StateIndex].animFrames[FrameIndex].sprite.bottom - animationStates[StateIndex].animFrames[FrameIndex].sprite.top)
                            * (i / 5 == 2 || i / 5 == 4 || i / 5 == 5 ? -1 : 1);
                        refRenderer.mesh.vertices[i + 2] = 0;
                        /*Console.Write(refRenderer.mesh.vertices[i] + "," + refRenderer.mesh.vertices[i + 1] + ","
                            + refRenderer.mesh.vertices[i + 2] + ",");*/
                    }
                }
            }
            //Console.Write("\n");

            if(animationStates[StateIndex].animFrames[FrameIndex].sprite != null)
            {
                if(refRenderer.mesh.triangleData != null)
                {
                    for (int i = 0; i < refRenderer.mesh.vertices.Length; i += 5)
                    {
                        if (i / 5 == 0 || i / 5 == 3)
                            refRenderer.mesh.vertices[i + 3] = animationStates[StateIndex].animFrames[FrameIndex].sprite.left;
                        else
                            refRenderer.mesh.vertices[i + 3] = animationStates[StateIndex].animFrames[FrameIndex].sprite.right;

                        if (i / 5 == 2 || i / 5 == 3)
                            refRenderer.mesh.vertices[i + 4] = animationStates[StateIndex].animFrames[FrameIndex].sprite.bottom;
                        else
                            refRenderer.mesh.vertices[i + 4] = animationStates[StateIndex].animFrames[FrameIndex].sprite.top;
                    }
                }
                else
                {
                    for (int i = 0; (i / 2) * 5 < refRenderer.mesh.vertices.Length; i += 2)
                    {
                        if (i / 5 == 0 || i / 5 == 3 || i / 5 == 5)
                            refRenderer.mesh.vertices[(i / 2) * 5 + 3] = animationStates[StateIndex].animFrames[FrameIndex].sprite.left;
                        else
                            refRenderer.mesh.vertices[(i / 2) * 5 + 3] = animationStates[StateIndex].animFrames[FrameIndex].sprite.right;

                        if (i / 5 == 2 || i / 5 == 4 || i / 5 == 5)
                            refRenderer.mesh.vertices[(i / 2) * 5 + 4] = animationStates[StateIndex].animFrames[FrameIndex].sprite.bottom;
                        else
                            refRenderer.mesh.vertices[(i / 2) * 5 + 4] = animationStates[StateIndex].animFrames[FrameIndex].sprite.top;
                    }
                }
            }
            animFramesRemaining += animationStates[StateIndex].animFrames[FrameIndex].frameDuration;
            refRenderer.mesh.SetUp(BufferUsageHint.DynamicDraw);
        }

        void UpdateState()
        {
            stateIndex = UpdateStateIndex;
            frameIndex = 0;
            UpdateAnim(false);
        }

        /*public static AnimationFrame[] CreateAnimFrames(int count, Texture image, float[] durations, float[] rects)
        {
            AnimationFrame[] tempFrames = new AnimationFrame[count];
            for (int i = 0; i < count; i++)
            {
                tempFrames[i] = new AnimationFrame();
                tempFrames[i].tex = image;
                tempFrames[i].frameDuration = durations[OpenTK.MathHelper.Clamp(i, 0, durations.Length - 1)];
                tempFrames[i].textureRects = new float[12] { rects[i * 12], rects[i * 12 + 1], rects[i * 12 + 2],
                    rects[i * 12 + 3],rects[i * 12 + 4],rects[i * 12 + 5],
                    rects[i * 12 + 6],rects[i * 12 + 7],rects[i * 12 + 8],
                    rects[i * 12 + 9],rects[i * 12 + 10],rects[i * 12 + 11]};
            }
            return tempFrames;
        }*/

        public static AnimationFrame[] CreateAnimFrames(int count, float[] durations, SpriteSet.Sprite[] sprites)
        {
            AnimationFrame[] tempFrames = new AnimationFrame[count];
            for (int i = 0; i < count; i++)
            {
                tempFrames[i] = new AnimationFrame();
                tempFrames[i].frameDuration = durations[OpenTK.MathHelper.Clamp(i, 0, durations.Length - 1)];
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

        public TextureAnimator() : base(true)
        { }

        public TextureAnimator(AnimationState[] animStates_) : this()
        {
            animationStates = animStates_;
        }
        public TextureAnimator(AnimationState[] animStates_, bool resizePlane_) : this(animStates_)
        {
            resizePlane = resizePlane_;
        }
        public TextureAnimator(MeshRenderer renderer) : this()
        {
            refRenderer = renderer;
        }
        public TextureAnimator(MeshRenderer renderer, bool resizePlane_) : this(renderer)
        {
            resizePlane = resizePlane_;
        }
        public TextureAnimator(AnimationState[] animStates_, MeshRenderer renderer) : this(animStates_)
        {
            refRenderer = renderer;
        }
        public TextureAnimator(AnimationState[] animStates_, MeshRenderer renderer, bool resizePlane_) : this(animStates_, renderer)
        {
            resizePlane = resizePlane_;
        }
    }
}
