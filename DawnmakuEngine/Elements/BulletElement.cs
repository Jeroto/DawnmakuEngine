using System;
using System.Collections.Generic;
using System.Text;
using OpenTK;
using DawnmakuEngine.Data;
using OpenTK.Mathematics;

namespace DawnmakuEngine.Elements
{
    public class BulletElement : Element
    {
        public float maxBoundsExit = 16;
        public bool firedByPlayer;

        public static byte newKillzoneIndex;
        public byte killzoneDetectIndex;

        static System.Random bulletRandom = new System.Random(0);
        public static Entity playerTransform;

        //Grazing
        public bool grazed;
        public int grazeDelay = 0, //Set greater than 0 to allow repeated grazing
            grazeDelayCurrent;

        //Bullet Stages
        public int stageIndex;
        public BulletStage[] bulletStages = new BulletStage[1];
        //Bullet Constants
        public ushort damage;
        public float currentSpeed;

        //Spawn Vars
        public float spawnTime = 15;
        float spawnTimeCurrent;

        //Misc
        float facingAngleRad,
            renderScale;

        //Destruction
        bool destroyingBullet;
        public bool destroy;
        int destroyAnimTime = 10;
        float destroyAnimCurrent = 0;

        [System.Serializable]
        public class BulletStage
        {
            //Change Condition
            public float framesToLast = 60;
            //Bullet Type
            public string spriteType = "round";
            public int bulletColor = 0;
            public bool animatedSprite;
            public float renderScale = 1;
            public byte r = 255, g = 255, b = 255, a = 255;
            public float framesToChangeTint = 0;

            public Vector4 Color {
                set
                {
                    r = (byte)value.X;
                    g = (byte)value.Y;
                    b = (byte)value.Z;
                    a = (byte)value.W;
                }
                get
                {
                    return new Vector4(r, g, b, a);
                }
            }

            //Movement
            public Vector2 movementDirection = DawnMath.vec2Up;
            public float startingSpeed = 200, endingSpeed = 100, framesToChangeSpeed = 60;
            public bool rotate, reAim, keepOldAngle, modifyAngle;
            //Stage Change
            public bool turnAtStart;
            public float initialMoveDelay = 0;
            public bool turnAfterDelay;
            //Stage Effect Sound
            public AudioData stageSound;
            //Spawn Effect Burst
            public bool hasEffect;
            public BulletColor effectColor;
            public float effectDuration = 30;
            public Vector2 effectSize = new Vector2(0.1f, 2f);
            public Vector2 effectOpacity = new Vector2(1, 0);
            public bool affectedByTimescale = true;

            public BulletStage CopyValues()
            {
                BulletStage copy = new BulletStage();
                copy.framesToLast = framesToLast;
                copy.spriteType = spriteType;
                copy.bulletColor = bulletColor;
                copy.animatedSprite = animatedSprite;

                copy.movementDirection = movementDirection;
                copy.startingSpeed = startingSpeed;
                copy.endingSpeed = endingSpeed;
                copy.framesToChangeSpeed = framesToChangeSpeed;
                copy.rotate = rotate;
                copy.reAim = reAim;
                copy.keepOldAngle = keepOldAngle;
                copy.modifyAngle = modifyAngle;

                copy.turnAtStart = turnAtStart;
                copy.initialMoveDelay = initialMoveDelay;
                copy.turnAfterDelay = turnAfterDelay;

                copy.stageSound = stageSound;

                copy.hasEffect = hasEffect;
                copy.effectColor = effectColor;
                copy.effectDuration = effectDuration;
                copy.effectSize = effectSize;
                copy.effectOpacity = effectOpacity;
                copy.affectedByTimescale = affectedByTimescale;

                return copy;
            }
        }

        Vector2 prevPos;
        MeshRenderer meshRenderer;
        TextureAnimator spriteAnimator;

        GameMaster gameMaster;
        float startScale;
        Vector4 startColor;

        const float spawnEffectStartScale = 3;
        const int framesBetweenRotate = 2;
        int rotateFramesWaited;
        float timePassed;

        [System.Serializable]
        public enum BulletColor
        {
            White, Red, Black, Green, Teal, Blue, Pink, Orange, Yellow, Purple,
            WhiteInverse, RedInverse, BlackInverse, GreenInverse, TealInverse, BlueInverse, PinkInverse, OrangeInverse, YellowInverse, PurpleInverse,
            WhiteInvert, RedInvert, BlackInvert, GreenInvert, TealInvert, BlueInvert, PinkInvert, OrangeInvert, YellowInvert, PurpleInvert,
            WhiteInvInv, RedInvInv, BlackInvInv, GreenInvInv, TealInvInv, BlueInvInv, PinkInvInv, OrangeInvInv, YellowInvInv, PurpleInvInv,
            Flame, FlameInvert, Special, SpecialInvert
        };

        // Start is called before the first frame update
        public override void PostCreate()
        {
            killzoneDetectIndex = newKillzoneIndex;
            newKillzoneIndex = (byte)DawnMath.Repeat(newKillzoneIndex + 1, 6);

            gameMaster = GameMaster.gameMaster;


            currentSpeed = bulletStages[stageIndex].startingSpeed;

            if (bulletStages[stageIndex].turnAtStart)
                /*facingAngleRad = DawnMath.FindAngleRad(bulletStages[stageIndex].movementDirection) - ((EntityAttachedTo.parent != null) ?
                    EntityAttachedTo.parent.WorldRotation.Z + EntityAttachedTo.WorldRotation.Z : EntityAttachedTo.WorldRotation.Z);*/
                entityAttachedTo.LocalRotationRad = new Vector3(0, 0, DawnMath.FindAngleRad(bulletStages[stageIndex].movementDirection) - ((EntityAttachedTo.Parent != null) ?
                    EntityAttachedTo.Parent.WorldRotation.Z + EntityAttachedTo.WorldRotation.Z : EntityAttachedTo.WorldRotation.Z));

            maxBoundsExit = gameMaster.bulletData[bulletStages[stageIndex].spriteType].boundsExitDist;

            meshRenderer = EntityAttachedTo.GetElement<MeshRenderer>();
            spriteAnimator = EntityAttachedTo.GetElement<TextureAnimator>();

            startScale = bulletStages[stageIndex].renderScale;
            startColor = new Vector4(bulletStages[0].r, bulletStages[0].g, bulletStages[0].b, bulletStages[0].a);

            UpdateSprite();
            UpdateColliders(bulletStages[0].spriteType);

            meshRenderer.ColorByte = new Vector4(startColor.X, startColor.Y, startColor.Z, 0);
            renderScale *= spawnEffectStartScale;

            if (bulletStages[0].hasEffect)
                SpawnEffect();

            prevPos = EntityAttachedTo.WorldPosition.Xy;

            base.PostCreate();

            DetectDestroy();

            if (firedByPlayer && !gameMaster.playerBulletStageSoundPlayed)
            {
                gameMaster.audioManager.PlaySound(bulletStages[stageIndex].stageSound, AudioController.AudioCategory.PlayerBullet,
                    gameMaster.PlayerBulletVol);
                gameMaster.playerBulletStageSoundPlayed = true;
            }
            else if (!firedByPlayer && !gameMaster.enemyBulletStageSoundPlayed)
            {
                gameMaster.audioManager.PlaySound(bulletStages[stageIndex].stageSound, AudioController.AudioCategory.Bullet,
                    gameMaster.BulletStageVol);
                gameMaster.enemyBulletStageSoundPlayed = true;
            }
        }

        // Update is called once per frame
        public override void OnUpdate()
        {
            //Trigers destroy animation, if it is destroying, exit out of code immediately
            if (destroy)
            {
                if(!destroyingBullet)
                {
                    destroyingBullet = true;
                    RemoveColliders();
                    grazed = true;
                    startScale = bulletStages[stageIndex].renderScale;
                    startColor = bulletStages[stageIndex].Color;
                }
                destroyAnimCurrent += gameMaster.timeScale;

                renderScale = startScale * DawnMath.Lerp(1, spawnEffectStartScale, destroyAnimCurrent / destroyAnimTime);
                meshRenderer.colorA = (byte)Math.Clamp(DawnMath.Round(DawnMath.Lerp(startColor.W, 0, destroyAnimCurrent / destroyAnimTime)),0, 255);

                meshRenderer.modelScale = renderScale;

                if(destroyAnimCurrent >= destroyAnimTime)
                {
                    entityAttachedTo.AttemptDelete();
                }
                return;
            }

            if (stageIndex + 1 < bulletStages.Length && bulletStages[stageIndex].framesToLast <= 0)
            {
                ChangeStage();
            }
            else
                bulletStages[stageIndex].framesToLast -= gameMaster.timeScale;


            if(grazeDelayCurrent < grazeDelay && grazed)
            {
                grazeDelayCurrent++;
                if (grazeDelayCurrent >= grazeDelay)
                    grazed = false;
            }

            SpawnAnim();
            Movement();
            AdditionalFunctions();
            if (bulletStages[stageIndex].rotate)
                Rotate();
            DetectDestroy();

            if (spawnTimeCurrent >= spawnTime)
                if (bulletStages[stageIndex].framesToChangeTint < timePassed)
                {
                    meshRenderer.ColorByte = DawnMath.Lerp(startColor, bulletStages[stageIndex].Color, timePassed / bulletStages[stageIndex].framesToChangeTint);
                }

            prevPos = EntityAttachedTo.WorldPosition.Xy;
        }

        public void SpawnEffect()
        {
            /*BasicExplosionEffect newEffect = Instantiate(gameMaster.destroyEffectSpawn, EntityAttachedTo.WorldPosition,
                new Quaternion(), gameMaster.effectContainer).GetComponent<BasicExplosionEffect>();

            newEffect.duration = bulletStages[stageIndex].effectDuration;
            newEffect.size = bulletStages[stageIndex].effectSize;
            newEffect.opacity = bulletStages[stageIndex].effectOpacity;
            newEffect.effectedByTimescale = bulletStages[stageIndex].affectedByTimescale;
            newEffect.GetComponent<SpriteRenderer>().sprite = gameMaster.destroyEffects[(int)bulletStages[stageIndex].effectColor];*/
        }

        public virtual void AdditionalFunctions()
        {
            /*To be overwritten using:
             public override void AdditionalFunctions()
             {
             }
            This is used to create unique bullet functionality in child scripts*/
        }

        public void SpawnAnim()
        {
            if (spawnTimeCurrent < spawnTime)
            {
                spawnTimeCurrent += gameMaster.timeScale;

                renderScale = startScale * DawnMath.Lerp(spawnEffectStartScale, 1, spawnTimeCurrent / spawnTime);
                meshRenderer.colorA = (byte)Math.Clamp(DawnMath.Floor(DawnMath.Lerp(0, startColor.W, spawnTimeCurrent / (spawnTime/* * 2.5f*/))), 0, 255);

                meshRenderer.modelScale = renderScale;

                if (spawnTimeCurrent >= spawnTime)
                {
                    meshRenderer.ColorByte = startColor;
                    renderScale = startScale;
                }
            }
        }

        //Moves bullet
        public void Movement()
        {
            if (bulletStages[stageIndex].initialMoveDelay <= 0)
            {
                EntityAttachedTo.LocalPosition += 
                    new Vector3(bulletStages[stageIndex].movementDirection * (currentSpeed * gameMaster.frameTime * gameMaster.timeScale));

                if (bulletStages[stageIndex].framesToChangeSpeed > 0)
                {
                    timePassed += gameMaster.timeScale;

                    currentSpeed = DawnMath.Lerp(bulletStages[stageIndex].startingSpeed,
                        bulletStages[stageIndex].endingSpeed, timePassed / bulletStages[stageIndex].framesToChangeSpeed);
                }

            }
            else
                bulletStages[stageIndex].initialMoveDelay -= gameMaster.timeScale;
        }

        //Rotates bullet
        public void Rotate()
        {
            if (rotateFramesWaited >= framesBetweenRotate)
            {
                if (bulletStages[stageIndex].rotate)
                    if (!bulletStages[stageIndex].turnAfterDelay || bulletStages[stageIndex].initialMoveDelay <= 0)
                    {
                        if (Vector2.Distance(EntityAttachedTo.WorldPosition.Xy, prevPos) > 0.01f)
                        {
                            float angle = -DawnMath.FindAngleRad(new Vector2(EntityAttachedTo.WorldPosition.X - prevPos.X,
                                EntityAttachedTo.WorldPosition.Y - prevPos.Y).Normalized());
                            if(MathF.Abs(entityAttachedTo.LocalRotationRad.Z - angle) > 0.01f)
                            entityAttachedTo.SetQuaternion(new Vector3(0, 0, 1), angle);
                        }

                        /*facingAngleRad = DawnMath.FindAngleRad(new Vector2(EntityAttachedTo.WorldPosition.X - prevPos.X, 
                            EntityAttachedTo.WorldPosition.Y - prevPos.Y));*/
                        rotateFramesWaited = 0;
                    }
            }
            else
                rotateFramesWaited++;
        }

        public void ChangeStage()
        {
            stageIndex++;

            if (bulletStages[stageIndex].keepOldAngle)
            {
                if (bulletStages[stageIndex].modifyAngle)
                    bulletStages[stageIndex].movementDirection = DawnMath.CalculateCircleDeg(DawnMath.FindAngleRad(bulletStages[stageIndex - 1].movementDirection) +
                        DawnMath.FindAngleRad(bulletStages[stageIndex].movementDirection));
                else
                    bulletStages[stageIndex].movementDirection = bulletStages[stageIndex - 1].movementDirection;
            }
            else if (bulletStages[stageIndex].reAim)
            {
                Entity aimTarget;
                if (!firedByPlayer)
                    aimTarget = gameMaster.playerEntity;
                else
                    aimTarget = DawnMath.FindNearestEnemy(entityAttachedTo.WorldPosition.Xy);

                if (bulletStages[stageIndex].modifyAngle)
                    bulletStages[stageIndex].movementDirection = DawnMath.CalculateCircleRad(DawnMath.FindAngleToObjectRad(EntityAttachedTo.WorldPosition.Xy, aimTarget) +
                        DawnMath.FindAngleRad(bulletStages[stageIndex].movementDirection));
                else
                    bulletStages[stageIndex].movementDirection = DawnMath.FindDirectionToObject(EntityAttachedTo.WorldPosition.Xy, aimTarget);
            }
            else if (bulletStages[stageIndex].modifyAngle)
                bulletStages[stageIndex].movementDirection = DawnMath.CalculateCircleDeg(DawnMath.FindAngleRad(bulletStages[stageIndex - 1].movementDirection) +
                        DawnMath.FindAngleRad(bulletStages[stageIndex].movementDirection));


            if (bulletStages[stageIndex].turnAtStart)
                entityAttachedTo.LocalRotationRad = new Vector3(0, 0, DawnMath.FindAngleRad(bulletStages[stageIndex].movementDirection) - ((EntityAttachedTo.Parent != null) ?
                    EntityAttachedTo.Parent.WorldRotation.Z + EntityAttachedTo.WorldRotation.Z : EntityAttachedTo.WorldRotation.Z));

            if (!ShouldSpin(bulletStages[stageIndex].spriteType))
                EntityAttachedTo.GetElement<RotateElement>().Disable();
            else
            {
                EntityAttachedTo.GetElement<RotateElement>().RotSpeedDeg = GetSpinSpeed(bulletStages[stageIndex].spriteType);
                EntityAttachedTo.GetElement<RotateElement>().Enable();
            }

            if (bulletStages[stageIndex].hasEffect)
                SpawnEffect();

            startColor = meshRenderer.ColorByte;
            meshRenderer.shader = gameMaster.bulletData[bulletStages[stageIndex].spriteType].shader;

            maxBoundsExit = gameMaster.bulletData[bulletStages[stageIndex].spriteType].boundsExitDist;

            timePassed = 0;

            if (firedByPlayer && !gameMaster.playerBulletStageSoundPlayed)
            {
                gameMaster.audioManager.PlaySound(bulletStages[stageIndex].stageSound, AudioController.AudioCategory.PlayerBullet,
                    gameMaster.PlayerBulletVol);
                gameMaster.playerBulletStageSoundPlayed = true;
            }
            else if (!firedByPlayer && !gameMaster.enemyBulletStageSoundPlayed)
            {
                gameMaster.audioManager.PlaySound(bulletStages[stageIndex].stageSound, AudioController.AudioCategory.Bullet,
                    gameMaster.BulletStageVol);
                gameMaster.enemyBulletStageSoundPlayed = true;
            }

            UpdateSprite();
            if (bulletStages[stageIndex - 1].spriteType != bulletStages[stageIndex].spriteType)
                UpdateColliders(bulletStages[stageIndex].spriteType);
        }

        //Destroy detection
        public void DetectDestroy()
        {
            if (killzoneDetectIndex == gameMaster.killzoneDetectIndex)
            {
                if(CheckOutOfBounds())
                    EntityAttachedTo.AttemptDelete();

                /*if (regularKillzone && !regularDestroyExtents)
                {
                    DetectKillzone(destroyXExtents, destroyYExtents);
                }
                else if (regularKillzone)
                {
                    DetectKillzone();
                }*/
            }
        }

        public override void AttemptDelete()
        {
            base.AttemptDelete();
        }

        public bool CheckOutOfBounds()
        {
            Vector2 outOfBoundsDist = Vector2.Zero, pos = entityAttachedTo.WorldPosition.Xy;

            if (pos.X < gameMaster.bulletBoundsX.X)
                outOfBoundsDist.X = Math.Abs(pos.X - gameMaster.bulletBoundsX.X);
            else if (pos.X > gameMaster.bulletBoundsX.Y)
                outOfBoundsDist.X = Math.Abs(pos.X - gameMaster.bulletBoundsX.Y);

            if (pos.Y < gameMaster.bulletBoundsY.X)
                outOfBoundsDist.Y = Math.Abs(pos.Y - gameMaster.bulletBoundsY.X);
            else if (pos.Y > gameMaster.bulletBoundsY.Y)
                outOfBoundsDist.Y = Math.Abs(pos.Y - gameMaster.bulletBoundsY.Y);

            return outOfBoundsDist.X > maxBoundsExit || outOfBoundsDist.Y > maxBoundsExit;
        }

        //Detects when bullets are destroyed
        public void DetectKillzone()
        {
            /*if (EntityAttachedTo.WorldPosition.x < regularDestroyExtentsX.x || EntityAttachedTo.WorldPosition.x > regularDestroyExtentsX.y || EntityAttachedTo.WorldPosition.y < regularDestroyExtentsY.x || EntityAttachedTo.WorldPosition.y > regularDestroyExtentsY.y)
            {
                //Destroy(gameObject);
                gameObject.SetActive(false);
            }
            else if (!destroyImmune)
            {
                DetectTrigger();
            }*/
        }

        public void DetectKillzone(Vector2 xLimits, Vector2 yLimits)
        {
            Vector2 pos = EntityAttachedTo.WorldPosition.Xy;
            if (pos.X < xLimits.X || pos.X > xLimits.Y || pos.Y < yLimits.X || pos.Y > yLimits.Y)
            {
                EntityAttachedTo.Disable();
            }
            else/* if (!destroyImmune)*/
            {
                DetectTrigger();
            }
        }

        //Detects areas that force bullet destruction
        public void DetectTrigger()
        {

        }

        //Destroy animation
        /*public IEnumerator DestroyAnimation()
        {
            float destroyTime = 0.15f;

            if (bulletStages[stageIndex].animatedSprite)
                spriteAnimator.enabled = false;

            meshRenderer.sprite = gameMaster.destroyEffects[(int)bulletStages[stageIndex].bulletColor];

            DisableAllColliders();

            Vector2 startingScale = EntityAttachedTo.localScale;
            Vector2 endingScale = startingScale * 1.5f;
            float timePassed = 0;
            Color startingColor = meshRenderer.color;
            while (destroyTime > timePassed)
            {
                timePassed += gameMaster.timeScale * gameMaster.frameTime;
                EntityAttachedTo.localScale = Vector2.Lerp(startingScale, endingScale, timePassed / destroyTime);
                meshRenderer.color = new Color(startingColor.r, startingColor.g, startingColor.b, Mathf.Lerp(startingColor.a, 0, timePassed / destroyTime));
                yield return null;
            }

            EntityAttachedTo.Disable();
        }*/

        //Disables colliders on bullet
        /* public void DisableAllColliders()
         {
             colliderContainer.gameObject.SetActive(false);
         }

         //Returns prefab of bullet
         public static GameObject GetBulletType(BulletType type)
         {
             return GameMaster.gameMaster.bulletSpawns[(int)type - 1];
         }*/

        /// <summary>
        /// Updates sprite by setting an animaton for it -- creating a new one for a non-animated sprite
        /// </summary>
        public void UpdateSprite()
        {
            spriteAnimator.animationStates = GetBulletAnim(bulletStages[stageIndex].spriteType, bulletStages[stageIndex].bulletColor);
            spriteAnimator.UpdateAnim(false);
        }

        public void UpdateColliders(string type)
        {
            BulletData data = gameMaster.bulletData[type];
            RemoveColliders();
            if (firedByPlayer)
                for (int i = 0; i < data.colliderSize.Length; i++)
                    entityAttachedTo.AddElement(new PlayerDamageCollider(data.colliderSize[i], data.colliderOffset[i], this));
            else
                for (int i = 0; i < data.colliderSize.Length; i++)
                    entityAttachedTo.AddElement(new EnemyDamageCollider(data.colliderSize[i], data.colliderOffset[i], this));
        }

        public void RemoveColliders()
        {
            if (firedByPlayer)
                entityAttachedTo.DeleteAllElementsOfType<PlayerDamageCollider>();
            else
                entityAttachedTo.DeleteAllElementsOfType<EnemyDamageCollider>();
        }

        /// <summary>
        /// Gets a bullet sprite by getting the index of the type by the list of the color -- or uses a custom sprite that is set
        /// </summary>
        /// <param name="type"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        public static SpriteSet.Sprite GetBulletSprite(string type, int color)
        {
            GameMaster gameMaster = GameMaster.gameMaster;
            byte randomizeNum = 0;
            if (gameMaster.bulletData[type].randomizeSprite)
            {
                randomizeNum = (byte)Random(0, MathF.Floor(gameMaster.bulletSprites[type].sprites.Count / gameMaster.bulletData[type].spriteColors));
            }

            return gameMaster.bulletSprites[type].sprites[Math.Clamp(color + gameMaster.bulletData[type].spriteColors * randomizeNum, 0, gameMaster.bulletSprites[type].sprites.Count - 1)];
        }
        /// <summary>
        /// Gets a RuntimeAnimatorController by getting the index of the type by the list of the color -- or uses a custom animator that is set
        /// </summary>
        public static TextureAnimator.AnimationState[] GetBulletAnim(string type, int color)
        {
            TextureAnimator.AnimationState[] state = new TextureAnimator.AnimationState[] { new TextureAnimator.AnimationState() };
            state[0].animFrames = new TextureAnimator.AnimationFrame[1];


            if (!GameMaster.gameMaster.bulletData[type].isAnimated)
            {
                state[0].animFrames[0] = new TextureAnimator.AnimationFrame();
                state[0].animFrames[0].frameDuration = 120;
                state[0].animFrames[0].sprite = GetBulletSprite(type, color);
            }
            else
            {
                color = Math.Clamp(color, 0, GameMaster.gameMaster.bulletData[type].animStates.Length - 1);
                state = GameMaster.gameMaster.bulletData[type].animStates[color];
            }

            if (state[0] == null)
                GameMaster.LogError(type + " " + color.ToString() + "'s state returned null");
            return state;
        }

        public static bool ShouldSpin (string type)
        {
            return GetSpinSpeed(type) > 0;
        }
        public static float GetSpinSpeed(string type)
        {
            return GameMaster.gameMaster.bulletData[type].spinSpeed;
        }

        public static bool ShouldTurn (string type)
        {
            return GameMaster.gameMaster.bulletData[type].shouldTurn;
        }

        public BulletElement() : base(true)
        {

        }


        public static Entity SpawnBullet(BulletStage[] stages, Vector3 position, bool shouldSpin = false, ushort damage = 1, int stage = 0, bool player = false)
        {
            int i;
            Entity newBullet = new Entity(stages[0].bulletColor.ToString() + " "+ stages[0].spriteType.ToString());
            newBullet.LocalPosition = position;
            if (GameMaster.window.KeyboardState.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.LeftControl))
                newBullet.LocalScale = Vector3.One * 2;
            MeshRenderer renderer = new MeshRenderer();
            renderer.tex = GameMaster.gameMaster.bulletSprites[stages[0].spriteType].sprites[stages[0].bulletColor].tex;
            renderer.shader = GameMaster.gameMaster.bulletData[stages[0].spriteType].shader;
            renderer.mesh = Mesh.CreatePrimitiveMesh(Mesh.Primitives.SqrPlaneWTriangles);
            renderer.mesh.SetUp(OpenTK.Graphics.ES30.BufferUsageHint.DynamicDraw);
            renderer.LayerName = "bullets";
            renderer.resizeSprite = true;

            newBullet.AddElement(renderer);
            newBullet.AddElement(new TextureAnimator(GetBulletAnim(stages[stage].spriteType, stages[stage].bulletColor), renderer));

            newBullet.AddElement(new RotateElement(180, true, true));
            if(!shouldSpin)
                newBullet.GetElement<RotateElement>().Disable();

            BulletElement bullet = new BulletElement();
            bullet.bulletStages = new BulletStage[stages.Length];
            for (i = 0; i < stages.Length; i++)
            {
                bullet.bulletStages[i] = stages[i].CopyValues();
            }
            bullet.spriteAnimator = newBullet.GetElement<TextureAnimator>();
            bullet.meshRenderer = renderer;
            bullet.firedByPlayer = player;
            bullet.damage = damage;
            newBullet.AddElement(bullet);

            //Console.WriteLine("New bullet: {0}", newBullet.Name);


            return newBullet;
        }

        //Updates bullet random seed
        public static void UpdateSeed(int newSeed)
        {
            bulletRandom = new System.Random(newSeed);
            GameMaster.Log("Bullets' Random is set to " + newSeed);
        }
        //Gets random int from bullet Random
        public static int Random(int lower, int upper)
        { return bulletRandom.Next(lower, upper); }
        //Gets random float from bullet Random
        public static float Random(float lower, float upper)
        { return (((float)bulletRandom.NextDouble() * Math.Abs(lower - upper)) + lower); }
    }
}
