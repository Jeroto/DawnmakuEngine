using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Transactions;
using DawnmakuEngine.Data;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace DawnmakuEngine.Elements
{
    class PlayerController : Element
    {
        static Random playerRandom;

        public PlayerCharData playerData;

        public PlayerOrbData[] orbData;
        public Entity[] spawnedOrbs = new Entity[0];
        public MeshRenderer[] orbRenderers = new MeshRenderer[0];
        public float[] orbMoveFrames;
        public Vector2[] orbTransitionPositions;
        public bool[] orbInPlace;

        int shotSwitchDelay = 10, shotSwitchTime;
        public float colliderSize;
        public Vector2 colliderOffset;
        protected float moveSpeed;
        protected float focusModifier;
        Pattern.PatternVariables[] patternVariables;

        TextureAnimator anim;
        GameMaster gameMaster;

        public float MoveSpeed
        {
            get { return moveSpeed; }
            set { moveSpeed = value; }
        }
        public float FocusSpeedPercent
        {
            get { return focusModifier; }
            set {
                if (value < 3)
                    focusModifier = value;
                else
                    focusModifier = value / 100f; 
            }
        }

        private Vector2 movementVelocity = Vector2.Zero;


        public PlayerController() : base(true)
        {
            gameMaster = GameMaster.gameMaster;
        }

        public override void PostCreate()
        {
            anim = EntityAttachedTo.GetElement<TextureAnimator>();
            AssignCharValues();
            AssignShotValues(gameMaster.playerTypeIndex, gameMaster.playerShotIndex);
            base.PostCreate();
        }

        public override void OnUpdate()
        {
            movementVelocity.X = InputScript.DirectionalInput.X;
            movementVelocity.Y = InputScript.DirectionalInput.Y;
            if (movementVelocity.LengthSquared > 0)
                movementVelocity.Normalize();

            movementVelocity *= MoveSpeed * gameMaster.timeScale;
            if (InputScript.Focus)
                movementVelocity *= focusModifier;

            if (InputScript.focusDown)
                shotSwitchTime = shotSwitchDelay;
            if(InputScript.shootDown && shotSwitchTime > 0)
            {
                shotSwitchTime = 0;
                SwitchShot();
            }

            shotSwitchTime--;

            entityAttachedTo.LocalPosition += new Vector3(movementVelocity.X, movementVelocity.Y, 0);

            if (movementVelocity.X > 0)
                anim.UpdateStateIndex = 1;
            else if (movementVelocity.X < 0)
                anim.UpdateStateIndex = 2;
            else
                anim.UpdateStateIndex = 0;

            Orbs();

            if (InputScript.focusDown || InputScript.focusUp || gameMaster.PowerLevelChange)
                SetPatternVars();

            Pattern pattern;
            if (InputScript.Shoot)
            {
                for (int i = 0; i < orbData.Length; i++)
                {
                    pattern = InputScript.Focus ? orbData[i].focusedPatterns[gameMaster.currentPowerLevel] : orbData[i].unfocusedPatterns[gameMaster.currentPowerLevel];
                    if (patternVariables[i].burstsRemaining == 0 && InputScript.shootDown)
                        patternVariables[i].fireDelay = pattern.initialDelay;
                    patternVariables[i].burstsRemaining = pattern.burstCount;
                }
            }

            for (int i = 0; i < orbData.Length; i++)
            {
                if (!orbData[i].activePowerLevels[gameMaster.currentPowerLevel] || patternVariables[i].burstsRemaining == 0)
                    continue;

                pattern = InputScript.Focus ? orbData[i].focusedPatterns[gameMaster.currentPowerLevel] : orbData[i].unfocusedPatterns[gameMaster.currentPowerLevel];
                pattern.ProcessPattern(spawnedOrbs[i], true, ref patternVariables[i]);
            }
        }

        public void Orbs()
        {
            float lastOrbMoveTime;
            float t = 0;
            if (orbData.Length != 0)
                lastOrbMoveTime = orbData[orbData.Length - 1].framesToMove;
            else
                lastOrbMoveTime = 10;

            for (int i = 0; i < spawnedOrbs.Length; i++)
            {

                if(i < orbData.Length)
                {
                    if (!orbData[i].activePowerLevels[gameMaster.currentPowerLevel])
                    {
                        orbMoveFrames[i] = Math.Clamp(orbMoveFrames[i] + gameMaster.timeScale, 0, orbData[i].framesToMove);
                        t = orbMoveFrames[i] / orbData[i].framesToMove;
                        spawnedOrbs[i].LocalPosition = new Vector3(DawnMath.Lerp(orbTransitionPositions[i], Vector2.Zero, t));
                        orbRenderers[i].colorA = (byte)DawnMath.Round(DawnMath.Lerp(255, 0, t));
                        orbInPlace[i] = false;
                    }
                    else if (!orbInPlace[i])
                    {
                        orbMoveFrames[i] = Math.Clamp(orbMoveFrames[i] + gameMaster.timeScale, 0, orbData[i].framesToMove);
                        t = orbMoveFrames[i] / orbData[i].framesToMove;
                        spawnedOrbs[i].LocalPosition = new Vector3(DawnMath.Lerp(orbTransitionPositions[i], orbData[i].focusPosition, t));
                        orbRenderers[i].colorA = (byte)DawnMath.Round(DawnMath.Lerp(0, 255, t));
                        orbInPlace[i] = t >= 1;

                        if(orbInPlace[i])
                            if(orbData[i].followPrevious || (orbData[i].leaveBehindWhileFocused && InputScript.Focus) || (orbData[i].leaveBehindWhileUnfocused && !InputScript.Focus))
                                spawnedOrbs[i].Parent = null;
                    }
                    else if (orbData[i].leaveBehindWhileFocused && InputScript.focusDown)
                        spawnedOrbs[i].Parent = null;
                    else if (!orbData[i].followPrevious && orbData[i].leaveBehindWhileFocused && InputScript.focusUp)
                    {
                        spawnedOrbs[i].Parent = EntityAttachedTo;
                        orbInPlace[i] = false;
                        orbMoveFrames[i] = 0;
                    }
                    else if (orbData[i].leaveBehindWhileUnfocused && InputScript.focusUp)
                        spawnedOrbs[i].Parent = null;
                    else if (!orbData[i].followPrevious && orbData[i].leaveBehindWhileUnfocused && InputScript.focusDown)
                    {
                        spawnedOrbs[i].Parent = EntityAttachedTo;
                        orbInPlace[i] = false;
                        orbMoveFrames[i] = 0;
                    }
                    else if (i > 0 && orbData[i].followPrevious)
                    {
                        Vector2 worldpos1 = spawnedOrbs[i].WorldPosition.Xy, worldpos2 = spawnedOrbs[i - 1].WorldPosition.Xy;
                        float dist = Vector2.DistanceSquared(worldpos1, worldpos2);
                        if (dist > orbData[i].followDistSq)
                        {
                            Vector3 dir = new Vector3(worldpos1 - worldpos2);
                            dir.NormalizeFast();
                            spawnedOrbs[i].LocalPosition = new Vector3(worldpos2) + (dir * orbData[i].followDist);
                        }
                    }
                    else if (spawnedOrbs[i].Parent != null)
                    {
                        if (InputScript.Focus)
                            orbMoveFrames[i] = Math.Clamp(orbMoveFrames[i] + gameMaster.timeScale, 0, orbData[i].framesToMove);
                        else
                            orbMoveFrames[i] = Math.Clamp(orbMoveFrames[i] - gameMaster.timeScale, 0, orbData[i].framesToMove);

                        t = orbMoveFrames[i] / orbData[i].framesToMove;
                        spawnedOrbs[i].LocalPosition = new Vector3(DawnMath.Lerp(orbData[i].unfocusPosition, orbData[i].focusPosition, t));
                    }
                }
                else
                {
                    if (!orbInPlace[i])
                    {
                        orbMoveFrames[i] = Math.Clamp(orbMoveFrames[i] + gameMaster.timeScale, 0, lastOrbMoveTime);
                        t = orbMoveFrames[i] / lastOrbMoveTime;
                        spawnedOrbs[i].LocalPosition = new Vector3(DawnMath.Lerp(orbTransitionPositions[i], Vector2.Zero, t));
                        orbRenderers[i].colorA = (byte)DawnMath.Round(DawnMath.Lerp(255, 0, t));
                        orbInPlace[i] = t >= 1;
                    }
                }
            }
        }

        public void SpawnOrbs(int type, int shot)
        {
            int i = 0;
            PlayerShotData thisShot = playerData.types[type].shotData[shot];
            orbData = thisShot.orbData;
            TextureAnimator orbAnim;

            for (i = 0; i < spawnedOrbs.Length; i++)
            {
                spawnedOrbs[i].Parent = EntityAttachedTo;
            }

            if(thisShot.orbData.Length > spawnedOrbs.Length)
            {
                Entity[] tempOrbs = (Entity[])spawnedOrbs.Clone();
                MeshRenderer[] tempRenderers = (MeshRenderer[])orbRenderers.Clone();
                spawnedOrbs = new Entity[thisShot.orbData.Length];
                orbRenderers = new MeshRenderer[thisShot.orbData.Length];
                for (i = 0; i < spawnedOrbs.Length; i++)
                {
                    if(tempOrbs.Length > i)
                    {
                        spawnedOrbs[i] = tempOrbs[i];
                        orbRenderers[i] = tempRenderers[i];
                        orbAnim = spawnedOrbs[i].GetElement<TextureAnimator>();
                        orbAnim.animationStates = orbData[i].animStates;
                        orbAnim.UpdateStateIndex = 0;
                        orbAnim.FrameIndex = 0;
                        spawnedOrbs[i].GetElement<RotateElement>().RotSpeedDeg = orbData[i].rotateDegreesPerSecond;
                    }
                    else
                    {
                        spawnedOrbs[i] = new Entity("PlayerOrb" + i.ToString(), EntityAttachedTo, Vector3.Zero, Vector3.Zero, Vector3.One);
                        orbRenderers[i] = new MeshRenderer(Mesh.CreatePrimitiveMesh(Mesh.Primitives.SqrPlaneWTriangles), "playerorbs",
                            OpenTK.Graphics.ES30.BufferUsageHint.DynamicDraw, gameMaster.spriteShader, orbData[i].animStates[0].animFrames[0].sprite.tex);
                        orbRenderers[i].colorA = 0;
                        spawnedOrbs[i].AddElement(orbRenderers[i]);
                        
                        orbAnim = new TextureAnimator(orbData[i].animStates, orbRenderers[i], true);
                        orbAnim.FrameIndex = orbData[i].startAnimFrame;
                        spawnedOrbs[i].AddElement(orbAnim);
                        spawnedOrbs[i].AddElement(new RotateElement(orbData[i].rotateDegreesPerSecond, true, true));
                    }
                }
            }
            else
            {
                for (i = 0; i < orbData.Length; i++)
                {
                    orbAnim = spawnedOrbs[i].GetElement<TextureAnimator>();
                    orbAnim.animationStates = orbData[i].animStates;
                    orbAnim.UpdateStateIndex = 0;
                    orbAnim.FrameIndex = 0;
                    spawnedOrbs[i].GetElement<RotateElement>().RotSpeedDeg = orbData[i].rotateDegreesPerSecond;
                }
            }

            orbTransitionPositions = new Vector2[spawnedOrbs.Length];
            orbMoveFrames = new float[spawnedOrbs.Length];
            orbInPlace = new bool[spawnedOrbs.Length];
            for (i = 0; i < orbTransitionPositions.Length; i++)
            {
                orbTransitionPositions[i] = spawnedOrbs[i].LocalPosition.Xy;
                orbMoveFrames[i] = 0;
                orbInPlace[i] = false;
            }
        }

        public void SwitchShot()
        {
            int prevIndex = gameMaster.playerShotIndex;
            Console.WriteLine(gameMaster.playerShotIndex);
            gameMaster.playerShotIndex = DawnMath.Repeat(gameMaster.playerShotIndex + 1, playerData.types[gameMaster.playerTypeIndex].shotData.Length - 1);
            Console.WriteLine(gameMaster.playerShotIndex);
            if (prevIndex == gameMaster.playerShotIndex)
                return;
            AssignShotValues(gameMaster.playerTypeIndex, gameMaster.playerShotIndex);
        }
        
        public void AssignShotValues(int type, int shot)
        {
            PlayerShotData thisShot = playerData.types[type].shotData[shot];
            MoveSpeed = thisShot.moveSpeed;
            FocusSpeedPercent = thisShot.focusModifier;

            if (thisShot.colliderSize <= 0)
            {
                colliderSize = playerData.colliderSize;
                colliderOffset = playerData.colliderOffset;
            }
            else
            {
                colliderSize = thisShot.colliderSize;
                colliderOffset = thisShot.colliderOffset;
            }

            SpawnOrbs(type, shot);
            SetPatternVars();
        }

        public void SetPatternVars()
        {
            Pattern pattern;
            patternVariables = new Pattern.PatternVariables[orbData.Length];
            for (int i = 0; i < orbData.Length; i++)
            {
                pattern = InputScript.Focus ? orbData[i].focusedPatterns[gameMaster.currentPowerLevel] : orbData[i].unfocusedPatterns[gameMaster.currentPowerLevel];
                switch (pattern.GetType().Name)
                {
                    case "PatternBullets":
                        patternVariables[i] = new PatternBullets.BulletPatternVars((PatternBullets)pattern);
                        break;
                }
                patternVariables[i].burstsRemaining = 0;
                patternVariables[i].fireDelay = pattern.initialDelay;
            }
        }

        public void AssignCharValues()
        {
            MoveSpeed = playerData.moveSpeed;
            FocusSpeedPercent = playerData.focusModifier;
            anim.animationStates = playerData.animStates;
        }
    } //Class end
}
