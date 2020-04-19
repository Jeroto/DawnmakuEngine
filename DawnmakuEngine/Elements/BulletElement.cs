using System;
using System.Collections.Generic;
using System.Text;
using OpenTK;
using DawnmakuEngine.Data;

namespace DawnmakuEngine.Elements
{
    class BulletElement : Element
    {
        static float maxBoundsExit = 16;

        public static byte newKillzoneIndex;
        public byte killzoneDetectIndex;

        static System.Random bulletRandom = new System.Random(0);
        public static Entity playerTransform;

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
        bool destroyingBullet;
        public bool destroy;
        float facingAngleRad,
            renderScale;

        [System.Serializable]
        public class BulletStage
        {
            //Change Condition
            public float framesToLast;
            //Bullet Type
            public BulletType spriteType;
            public BulletColor bulletColor;
            public bool animatedSprite;
            public Texture customSprite;
            public TextureAnimator.AnimationState[] customAnim;
            public float renderScale;
            //Movement
            public Vector2 movementDirection;
            public float startingSpeed, endingSpeed, framesToChangeSpeed;
            public bool rotate, aimAtPlayer, keepOldAngle, modifyAngle;
            //Stage Change
            public bool turnAtStart;
            public float initialMoveDelay;
            public bool turnAfterDelay;
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
                copy.customSprite = customSprite;
                copy.customAnim = customAnim;

                copy.movementDirection = movementDirection;
                copy.startingSpeed = startingSpeed;
                copy.endingSpeed = endingSpeed;
                copy.framesToChangeSpeed = framesToChangeSpeed;
                copy.rotate = rotate;
                copy.aimAtPlayer = aimAtPlayer;
                copy.keepOldAngle = keepOldAngle;

                copy.turnAtStart = turnAtStart;
                copy.initialMoveDelay = initialMoveDelay;
                copy.turnAfterDelay = turnAfterDelay;

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
        [System.Serializable]
        public enum BulletType
        {
            Custom, Round, Wave, SmallShard,
            ThickShard, Crystal, Kunai, Shell, Pellet,
            RoundOutlined, Dagger, Star, BigDrop, SmallDrop,
            Charm, CharmWhite, BeamOnePxWide, BeamOnePx,
            BeamTwoPx, BeamThreePx, BeamFourPx, Heart, Popcorn,
            DarkPopcorn, Coin, Droplet, Arrow, Rest, /*Giant,*/
            Butterfly, Fire, MusicNote,
        };

        // Start is called before the first frame update
        public override void PostCreate()
        {
            killzoneDetectIndex = newKillzoneIndex;
            newKillzoneIndex = (byte)DawnMath.Repeat(newKillzoneIndex + 1, 6);

            gameMaster = GameMaster.gameMaster;

            DetectDestroy();

            currentSpeed = bulletStages[stageIndex].startingSpeed;

            if (bulletStages[stageIndex].turnAtStart)
                /*facingAngleRad = DawnMath.FindAngleRad(bulletStages[stageIndex].movementDirection) - ((EntityAttachedTo.parent != null) ?
                    EntityAttachedTo.parent.WorldRotation.Z + EntityAttachedTo.WorldRotation.Z : EntityAttachedTo.WorldRotation.Z);*/
                entityAttachedTo .LocalRotationRad = new Vector3(0, 0, DawnMath.FindAngleRad(bulletStages[stageIndex].movementDirection) - ((EntityAttachedTo.parent != null) ?
                    EntityAttachedTo.parent.WorldRotation.Z + EntityAttachedTo.WorldRotation.Z : EntityAttachedTo.WorldRotation.Z));


                meshRenderer = EntityAttachedTo.GetElement<MeshRenderer>();
            spriteAnimator = EntityAttachedTo.GetElement<TextureAnimator>();

            startScale = bulletStages[stageIndex].renderScale;
            startColor = meshRenderer.ColorByte;

            UpdateSprite();

            meshRenderer.ColorByte = new Vector4(255, 255, 255, 0);
            renderScale *= spawnEffectStartScale;

            if (bulletStages[0].hasEffect)
                SpawnEffect();

            prevPos = EntityAttachedTo.WorldPosition.Xy;

            base.PostCreate();
        }

        // Update is called once per frame
        public override void OnUpdate()
        {
            if (stageIndex + 1 < bulletStages.Length && bulletStages[stageIndex].framesToLast <= 0)
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
                else if (bulletStages[stageIndex].aimAtPlayer)
                {
                    if (bulletStages[stageIndex].modifyAngle)
                        bulletStages[stageIndex].movementDirection = DawnMath.CalculateCircleRad(DawnMath.FindAngleToPlayerRad(EntityAttachedTo.WorldPosition.Xy) +
                            DawnMath.FindAngleRad(bulletStages[stageIndex].movementDirection));
                    else
                        bulletStages[stageIndex].movementDirection = DawnMath.FindDirectionToPlayer(EntityAttachedTo.WorldPosition.Xy);
                }
                else if (bulletStages[stageIndex].modifyAngle)
                    bulletStages[stageIndex].movementDirection = DawnMath.CalculateCircleDeg(DawnMath.FindAngleRad(bulletStages[stageIndex - 1].movementDirection) +
                            DawnMath.FindAngleRad(bulletStages[stageIndex].movementDirection));


                if (bulletStages[stageIndex].turnAtStart)
                    /*facingAngleRad = DawnMath.FindAngleRad(bulletStages[stageIndex].movementDirection) - ((EntityAttachedTo.parent != null) ?
                        EntityAttachedTo.parent.WorldRotation.Z + EntityAttachedTo.WorldRotation.Z : EntityAttachedTo.WorldRotation.Z);*/
                    entityAttachedTo.LocalRotationRad = new Vector3(0, 0 , DawnMath.FindAngleRad(bulletStages[stageIndex].movementDirection) - ((EntityAttachedTo.parent != null) ?
                        EntityAttachedTo.parent.WorldRotation.Z + EntityAttachedTo.WorldRotation.Z : EntityAttachedTo.WorldRotation.Z));

                if (!ShouldSpin(bulletStages[stageIndex].spriteType))
                    EntityAttachedTo.GetElement<RotateElement>().Disable();
                else
                    EntityAttachedTo.GetElement<RotateElement>().Enable();

                if (bulletStages[stageIndex].hasEffect)
                    SpawnEffect();

                timePassed = 0;

                UpdateSprite();
            }
            else
                bulletStages[stageIndex].framesToLast -= gameMaster.timeScale;

            //Trigers destroy animation, if it is destroying, exit out of code immediately
            if (destroyingBullet)
                return;
            else if (destroy)
            {
                destroyingBullet = true;
                /*else
                    StartCoroutine(DestroyAnimation());*/
                return;
            }

            SpawnAnim();
            Movement();
            AdditionalFunctions();
            if (bulletStages[stageIndex].rotate)
                Rotate();
            DetectDestroy();
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
                meshRenderer.ColorByte = DawnMath.Lerp(new Vector4(startColor.X, startColor.Y, startColor.Z, 0),
                    startColor, spawnTimeCurrent / (spawnTime * 2.5f));

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
                    new Vector3(bulletStages[stageIndex].movementDirection * currentSpeed * gameMaster.frameTime * gameMaster.timeScale);

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
                            /*facingAngleRad = DawnMath.FindAngleRad(new Vector2(EntityAttachedTo.WorldPosition.X - prevPos.X, 
                                EntityAttachedTo.WorldPosition.Y - prevPos.Y));*/
                            entityAttachedTo.SetQuaternion(new Vector3(0, 0, 1), -DawnMath.FindAngleRad(new Vector2(EntityAttachedTo.WorldPosition.X - prevPos.X,
                            EntityAttachedTo.WorldPosition.Y - prevPos.Y).Normalized()));
                        rotateFramesWaited = 0;
                    }
            }
            else
                rotateFramesWaited++;
        }

        //Destroy detection
        public void DetectDestroy()
        {
            if (killzoneDetectIndex == gameMaster.killzoneDetectIndex)
            {
                CheckOutOfBounds();
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

        public void CheckOutOfBounds()
        {
            Vector2 outOfBoundsDist = gameMaster.CheckDistOutsideCamBounds(EntityAttachedTo.WorldPosition.Xy);
            if (outOfBoundsDist.X > maxBoundsExit || outOfBoundsDist.Y > maxBoundsExit)
                EntityAttachedTo.Disable();
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

        //Check if should use anim or sprite
        public bool CheckForAnimatedType(bool custom)
        {
            if (bulletStages[stageIndex].spriteType == BulletType.Custom)
                return custom;

            return (bulletStages[stageIndex].spriteType > BulletType.Rest);
        }

        //Updates sprite by checking if it's animated, and then changing either the Sprite or RuntimeAnimatorController
        public void UpdateSprite()
        {
            spriteAnimator.animationStates = GetBulletAnim(bulletStages[stageIndex].spriteType, bulletStages[stageIndex].bulletColor, gameMaster.bulletSheet);
            /*if (!CheckForAnimatedType(bulletStages[stageIndex].animatedSprite))
            {
                meshRenderer.mesh.SetUVs(GetBulletSprite(bulletStages[stageIndex].spriteType, bulletStages[stageIndex].bulletColor, gameMaster.bulletSheet));
            }
            else
            {
                spriteAnimator.animationStates = GetBulletAnim(bulletStages[stageIndex].spriteType, bulletStages[stageIndex].bulletColor, gameMaster.bulletSheet);
            }*/
        }

        //Gets a bullet sprite by getting the index of the type by the list of the color -- or uses a custom sprite that is set
        public static float[] GetBulletSprite(BulletType type, BulletColor color, Texture tex, int spriteNum = 0)
        {
            GameMaster gameMaster = GameMaster.gameMaster;
            float left = gameMaster.bulletColumns[(int)color], 
                right = gameMaster.bulletColumns[(int)color + 1],
                top = 1, 
                bottom = 0;

            switch (type)
            {
                case BulletType.Round:
                    top = gameMaster.roundBulletSprites[0];
                    bottom = gameMaster.roundBulletSprites[1];
                    break;
                case BulletType.Wave:
                    top = gameMaster.waveBulletSprites[0];
                    bottom = gameMaster.waveBulletSprites[1];
                    break;
                case BulletType.SmallShard:
                    top = gameMaster.smallShardSprites[0];
                    bottom = gameMaster.smallShardSprites[1];
                    break;
                case BulletType.ThickShard:
                    top = gameMaster.thickShardSprites[0];
                    bottom = gameMaster.thickShardSprites[1];
                    break;
                case BulletType.Crystal:
                    top = gameMaster.crystalBulletSprites[0];
                    bottom = gameMaster.crystalBulletSprites[1];
                    break;
                case BulletType.Kunai:
                    top = gameMaster.kunaiBulletSprites[0];
                    bottom = gameMaster.kunaiBulletSprites[1];
                    break;
                case BulletType.Shell:
                    top = gameMaster.shellBulletSprites[0];
                    bottom = gameMaster.shellBulletSprites[1];
                    break;
                case BulletType.Pellet:
                    top = gameMaster.pelletBulletSprites[0];
                    bottom = gameMaster.pelletBulletSprites[1];
                    break;
                case BulletType.RoundOutlined:
                    top = gameMaster.roundOultineSprites[0];
                    bottom = gameMaster.roundOultineSprites[1];
                    break;
                case BulletType.Dagger:
                    top = gameMaster.daggerBulletSprites[0];
                    bottom = gameMaster.daggerBulletSprites[1];
                    break;
                case BulletType.Star:
                    top = gameMaster.starBulletSprites[0];
                    bottom = gameMaster.starBulletSprites[1];
                    break;
                case BulletType.Fire:
                    switch(spriteNum)
                    {
                        case 0:
                            top = gameMaster.fireBulletSprites1[0];
                            bottom = gameMaster.fireBulletSprites1[1];
                            break;
                        case 1:
                            top = gameMaster.fireBulletSprites2[0];
                            bottom = gameMaster.fireBulletSprites2[1];
                            break;
                        case 2:
                            top = gameMaster.fireBulletSprites3[0];
                            bottom = gameMaster.fireBulletSprites3[1];
                            break;
                        case 3:
                            top = gameMaster.fireBulletSprites4[0];
                            bottom = gameMaster.fireBulletSprites4[1];
                            break;
                    }
                    break;
                case BulletType.BigDrop:
                    if(Random(0,2) == 0)
                    {
                        if(Random(0,2) == 0)
                        {
                            top = gameMaster.bigDropBulletSprites3[0];
                            bottom = gameMaster.bigDropBulletSprites3[1];
                        }
                        else
                        {
                            top = gameMaster.bigDropBulletSprites1[0];
                            bottom = gameMaster.bigDropBulletSprites1[1];
                        }
                    }
                    else
                    {
                        top = gameMaster.bigDropBulletSprites2[0];
                        bottom = gameMaster.bigDropBulletSprites2[1];
                    }
                    break;
                case BulletType.SmallDrop:
                    if (Random(0, 2) == 0)
                    {
                        if (Random(0, 2) == 0)
                        {
                            top = gameMaster.smallDropBulletSprites3[0];
                            bottom = gameMaster.smallDropBulletSprites3[1];
                        }
                        else
                        {
                            top = gameMaster.smallDropBulletSprites1[0];
                            bottom = gameMaster.smallDropBulletSprites1[1];
                        }
                    }
                    else
                    {
                        top = gameMaster.smallDropBulletSprites2[0];
                        bottom = gameMaster.smallDropBulletSprites2[1];
                    }
                    break;
                case BulletType.Charm:
                    top = gameMaster.charmBulletSprites[0];
                    bottom = gameMaster.charmBulletSprites[1];
                    break;
                case BulletType.CharmWhite:
                    top = gameMaster.charmWhiteBulletSprites[0];
                    bottom = gameMaster.charmWhiteBulletSprites[1];
                    break;
                case BulletType.BeamOnePxWide:
                    top = gameMaster.beamWideSprites[0];
                    bottom = gameMaster.beamWideSprites[1];
                    break;
                case BulletType.BeamTwoPx:
                    top = gameMaster.beam2pxSprites[0];
                    bottom = gameMaster.beam2pxSprites[1];
                    break;
                case BulletType.BeamThreePx:
                    top = gameMaster.beam3pxSprites[0];
                    bottom = gameMaster.beam3pxSprites[1];
                    break;
                case BulletType.BeamFourPx:
                    top = gameMaster.beam4pxSprites[0];
                    bottom = gameMaster.beam4pxSprites[1];
                    break;
                case BulletType.BeamOnePx:
                    top = gameMaster.beam1pxSprites[0];
                    bottom = gameMaster.beam1pxSprites[1];
                    break;
                case BulletType.Heart:
                    top = gameMaster.heartBulletSprites[0];
                    bottom = gameMaster.heartBulletSprites[1];
                    break;
                case BulletType.Butterfly:
                    switch (spriteNum)
                    {
                        case 0:
                            top = gameMaster.butterflyBulletSprites1[0];
                            bottom = gameMaster.butterflyBulletSprites1[1];
                            break;
                        case 1:
                            top = gameMaster.butterflyBulletSprites2[0];
                            bottom = gameMaster.butterflyBulletSprites2[1];
                            break;
                        case 2:
                            top = gameMaster.butterflyBulletSprites3[0];
                            bottom = gameMaster.butterflyBulletSprites3[1];
                            break;
                        case 3:
                            top = gameMaster.butterflyBulletSprites4[0];
                            bottom = gameMaster.butterflyBulletSprites4[1];
                            break;
                    }
                    break;
                case BulletType.Popcorn:
                    top = gameMaster.popcornBulletSprites[0];
                    bottom = gameMaster.popcornBulletSprites[1];
                    break;
                case BulletType.DarkPopcorn:
                    top = gameMaster.popcornDarkBulletSprites[0];
                    bottom = gameMaster.popcornDarkBulletSprites[1];
                    break;
                case BulletType.Coin:
                    top = gameMaster.coinBulletSprites[0];
                    bottom = gameMaster.coinBulletSprites[1];
                    break;
                case BulletType.Droplet:
                    top = gameMaster.dropletBulletSprites[0];
                    bottom = gameMaster.dropletBulletSprites[1];
                    break;
                case BulletType.Arrow:
                    top = gameMaster.arrowBulletSprites[0];
                    bottom = gameMaster.arrowBulletSprites[1];
                    break;
                case BulletType.Rest:
                    top = gameMaster.restBulletSprites[0];
                    bottom = gameMaster.restBulletSprites[1];
                    break;
                case BulletType.MusicNote:
                    switch (spriteNum)
                    {
                        case 0:
                            top = gameMaster.noteBulletSprites1[0];
                            bottom = gameMaster.noteBulletSprites1[1];
                            break;
                        case 1:
                            top = gameMaster.noteBulletSprites2[0];
                            bottom = gameMaster.noteBulletSprites2[1];
                            break;
                        case 2:
                            top = gameMaster.noteBulletSprites3[0];
                            bottom = gameMaster.noteBulletSprites3[1];
                            break;
                    }
                    break;
            }

            return new float[] { left, top,  right, top,  right, bottom,   left, top,  right, bottom,  left, bottom };
        }

        //Gets a RuntimeAnimatorController by getting the index of the type by the list of the color -- or uses a custom animator that is set
        public static TextureAnimator.AnimationState[] GetBulletAnim(BulletType type, BulletColor color, Texture tex, TextureAnimator.AnimationState[] customAnimController = null)
        {
            if (type == BulletType.Custom)
                return customAnimController;
            else
            {
                TextureAnimator.AnimationState[] state = new TextureAnimator.AnimationState[] { new TextureAnimator.AnimationState() };
                state[0].animFrames = new TextureAnimator.AnimationFrame[1];
                switch (type)
                {
                    case BulletType.Round:
                    case BulletType.Wave:
                    case BulletType.SmallShard:
                    case BulletType.ThickShard:
                    case BulletType.Crystal:
                    case BulletType.Kunai:
                    case BulletType.Shell:
                    case BulletType.Pellet:
                    case BulletType.RoundOutlined:
                    case BulletType.Dagger:
                    case BulletType.Star:
                    case BulletType.BigDrop:
                    case BulletType.SmallDrop:
                    case BulletType.Charm:
                    case BulletType.CharmWhite:
                    case BulletType.BeamOnePxWide:
                    case BulletType.BeamTwoPx:
                    case BulletType.BeamThreePx:
                    case BulletType.BeamFourPx:
                    case BulletType.BeamOnePx:
                    case BulletType.Heart:
                    case BulletType.Popcorn:
                    case BulletType.DarkPopcorn:
                    case BulletType.Coin:
                    case BulletType.Droplet:
                    case BulletType.Arrow:
                    case BulletType.Rest:
                        state[0].animFrames = new TextureAnimator.AnimationFrame[1];
                        state[0].animFrames[0] = new TextureAnimator.AnimationFrame();
                        state[0].animFrames[0].tex = tex; state[0].animFrames[0].frameDuration = 120;
                        state[0].animFrames[0].textureRects = GetBulletSprite(type, color, tex, 0);
                        break;
                    case BulletType.Fire:
                        state[0].animFrames = new TextureAnimator.AnimationFrame[4];
                        state[0].animFrames[0] = new TextureAnimator.AnimationFrame();
                        state[0].animFrames[1] = new TextureAnimator.AnimationFrame();
                        state[0].animFrames[2] = new TextureAnimator.AnimationFrame();
                        state[0].animFrames[3] = new TextureAnimator.AnimationFrame();
                        state[0].animFrames[0].tex = tex; state[0].animFrames[1].tex = tex; state[0].animFrames[2].tex = tex; state[0].animFrames[3].tex = tex;
                        state[0].animFrames[0].frameDuration = 8; state[0].animFrames[1].frameDuration = 8; state[0].animFrames[2].frameDuration = 8; state[0].animFrames[3].frameDuration = 8;

                        state[0].animFrames[0].textureRects = GetBulletSprite(type, color, tex, 0);
                        state[0].animFrames[1].textureRects = GetBulletSprite(type, color, tex, 1);
                        state[0].animFrames[2].textureRects = GetBulletSprite(type, color, tex, 2);
                        state[0].animFrames[3].textureRects = GetBulletSprite(type, color, tex, 3);
                        break;
                    case BulletType.Butterfly:
                        state[0].animFrames = new TextureAnimator.AnimationFrame[4];
                        state[0].animFrames[0] = new TextureAnimator.AnimationFrame();
                        state[0].animFrames[1] = new TextureAnimator.AnimationFrame();
                        state[0].animFrames[2] = new TextureAnimator.AnimationFrame();
                        state[0].animFrames[3] = new TextureAnimator.AnimationFrame();
                        state[0].animFrames[0].tex = tex; state[0].animFrames[1].tex = tex; state[0].animFrames[2].tex = tex; state[0].animFrames[3].tex = tex;
                        state[0].animFrames[0].frameDuration = 8; state[0].animFrames[1].frameDuration = 8; state[0].animFrames[2].frameDuration = 8; state[0].animFrames[3].frameDuration = 8;

                        state[0].animFrames[0].textureRects = GetBulletSprite(type, color, tex, 0);
                        state[0].animFrames[1].textureRects = GetBulletSprite(type, color, tex, 1);
                        state[0].animFrames[2].textureRects = GetBulletSprite(type, color, tex, 2);
                        state[0].animFrames[3].textureRects = GetBulletSprite(type, color, tex, 3);
                        break;
                    case BulletType.MusicNote:
                        state[0].animFrames = new TextureAnimator.AnimationFrame[4];
                        state[0].animFrames[0] = new TextureAnimator.AnimationFrame();
                        state[0].animFrames[1] = new TextureAnimator.AnimationFrame();
                        state[0].animFrames[2] = new TextureAnimator.AnimationFrame();
                        state[0].animFrames[3] = new TextureAnimator.AnimationFrame();
                        state[0].animFrames[0].tex = tex; state[0].animFrames[1].tex = tex; state[0].animFrames[2].tex = tex; state[0].animFrames[3].tex = tex;
                        state[0].animFrames[0].frameDuration = 30; state[0].animFrames[1].frameDuration = 30; state[0].animFrames[2].frameDuration = 30; state[0].animFrames[3].frameDuration = 30;

                        state[0].animFrames[0].textureRects = GetBulletSprite(type, color, tex, 0);
                        state[0].animFrames[1].textureRects = GetBulletSprite(type, color, tex, 1);
                        state[0].animFrames[2].textureRects = GetBulletSprite(type, color, tex, 0);
                        state[0].animFrames[3].textureRects = GetBulletSprite(type, color, tex, 2);
                        break;
                }
                return state;
            }
        }

        public static bool ShouldSpin (BulletType type)
        {
            switch(type)
            {


                case BulletType.Round:
                case BulletType.Wave:
                case BulletType.SmallShard:
                case BulletType.ThickShard:
                case BulletType.Crystal:
                case BulletType.Kunai:
                case BulletType.Shell:
                case BulletType.Pellet:
                case BulletType.RoundOutlined:
                case BulletType.Dagger:
                case BulletType.Fire:
                case BulletType.BigDrop:
                case BulletType.SmallDrop:
                case BulletType.Charm:
                case BulletType.CharmWhite:
                case BulletType.BeamOnePxWide:
                case BulletType.BeamTwoPx:
                case BulletType.BeamThreePx:
                case BulletType.BeamFourPx:
                case BulletType.BeamOnePx:
                case BulletType.Heart:
                case BulletType.Butterfly:
                case BulletType.Droplet:
                case BulletType.Arrow:
                case BulletType.Rest:
                case BulletType.MusicNote:
                default:
                    return false;
                case BulletType.Star:
                case BulletType.Popcorn:
                case BulletType.DarkPopcorn:
                case BulletType.Coin:
                    return true;
            }
        }

        public static bool ShouldTurn (BulletType type)
        {
            switch(type)
            {
                case BulletType.Round:
                case BulletType.Pellet:
                case BulletType.RoundOutlined:
                case BulletType.Star:
                case BulletType.Popcorn:
                case BulletType.DarkPopcorn:
                case BulletType.Coin:
                case BulletType.MusicNote:
                    return false;

                case BulletType.Wave:
                case BulletType.SmallShard:
                case BulletType.ThickShard:
                case BulletType.Crystal:
                case BulletType.Kunai:
                case BulletType.Shell:
                case BulletType.Dagger:
                case BulletType.Fire:
                case BulletType.BigDrop:
                case BulletType.SmallDrop:
                case BulletType.Charm:
                case BulletType.CharmWhite:
                case BulletType.BeamOnePxWide:
                case BulletType.BeamTwoPx:
                case BulletType.BeamThreePx:
                case BulletType.BeamFourPx:
                case BulletType.BeamOnePx:
                case BulletType.Heart:
                case BulletType.Butterfly:
                case BulletType.Droplet:
                case BulletType.Arrow:
                case BulletType.Rest:
                default:
                    return true;
            }
        }

        public BulletElement() : base()
        {

        }


        public static Entity SpawnBullet(BulletStage[] stages, Vector3 position, bool shouldSpin = false, ushort damage = 1, int stage = 0)
        {
            Entity newBullet = new Entity(stages[0].bulletColor.ToString() + " "+ stages[0].spriteType.ToString());
            newBullet.LocalPosition = position;
            MeshRenderer renderer = new MeshRenderer();
            renderer.tex = GameMaster.gameMaster.bulletSheet;
            renderer.shader = GameMaster.gameMaster.spriteShader;
            renderer.mesh = new Mesh(Mesh.Primitives.SqrPlane);
            renderer.mesh.SetUp(OpenTK.Graphics.ES30.BufferUsageHint.DynamicDraw);

            newBullet.AddElement(renderer);
            newBullet.AddElement(new TextureAnimator(GetBulletAnim(stages[stage].spriteType, stages[stage].bulletColor, renderer.tex), renderer, true));

            newBullet.AddElement(new RotateElement(180, true, true));
            if(!shouldSpin)
                newBullet.GetElement<RotateElement>().Disable();

            BulletElement bullet = new BulletElement();
            bullet.bulletStages = new BulletStage[stages.Length];
            for (int i = 0; i < stages.Length; i++)
            {
                bullet.bulletStages[i] = stages[i].CopyValues();
            }
            bullet.spriteAnimator = newBullet.GetElement<TextureAnimator>();
            bullet.meshRenderer = renderer;
            newBullet.AddElement(bullet);
            Console.WriteLine("New bullet: {0}", newBullet.Name);


            return newBullet;
        }

        //Updates bullet random seed
        public static void UpdateSeed(int newSeed)
        {
            bulletRandom = new System.Random(newSeed);
            Console.WriteLine("Bullets' Random is set to " + newSeed);
        }
        //Gets random int from bullet Random
        public static int Random(int lower, int upper)
        { return bulletRandom.Next(lower, upper); }
        //Gets random float from bullet Random
        public static float Random(float lower, float upper)
        { return (((float)bulletRandom.NextDouble() * Math.Abs(lower - upper)) + lower); }
    }
}
