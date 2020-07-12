using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using DawnmakuEngine.Data;
using OpenTK;

namespace DawnmakuEngine.Elements
{
    /// <summary>
    /// Not designed to be used on its own.
    /// Use EnemyElement for general cases -- derive from this to create a heavily customized enemy.
    /// </summary>
    class BaseEnemyElement : Element
    {
        public static List<BaseEnemyElement> allEnemies = new List<BaseEnemyElement>();

        public static Random enemyRandom;
        public static bool playedDeathSound;
        public static int newKillzoneIndex;

        public EnemyData enemyData;

        protected int killzoneDetectIndex;

        protected Vector2 startPosition;

        public int health;
        public int lowHealth;
        public float invFrames = 30;
        public int scoreOnDeath;

        public int segmentIndex = 0;
        protected float moveTimePassed, waitTime;

        protected Pattern.PatternVariables[] patternVars;

        protected GameMaster gameMaster;
        protected TextureAnimator anim;

        Vector2 prevPos, prevMovement;

        //Item Drops
        public ItemData[] itemSpawns;

        Vector2 destroyExtentsX = new Vector2(-600, 600);
        Vector2 destroyExtentsY = new Vector2(-600, 600);

        public BaseEnemyElement(EnemyData enemyToSpawn) : this(enemyToSpawn, new ItemData[0]) { }
        public BaseEnemyElement(EnemyData enemyToSpawn, ItemData[] itemSpawns_) : base(true, true, true)
        {
            enemyData = enemyToSpawn;
            itemSpawns = itemSpawns_;

            health = enemyData.health;
        }

        public override void PostCreate()
        {
            startPosition = EntityAttachedTo.LocalPosition.Xy;

            killzoneDetectIndex = newKillzoneIndex;
            newKillzoneIndex = DawnMath.Repeat(newKillzoneIndex + 1, 6);

            gameMaster = GameMaster.gameMaster;

            anim = EntityAttachedTo.GetElement<TextureAnimator>();
            anim.animationStates = enemyData.animations;

            waitTime = enemyData.movementCurve.StartWaitTime(segmentIndex);

            entityAttachedTo.AddElement(new EnemyDamageCollider(enemyData.killColliderSize, enemyData.killColliderOffset));

            base.PostCreate();
        }

        public override void OnUpdate()
        {
            prevPos = entityAttachedTo.LocalPosition.Xy;

            if (invFrames > 0)
                invFrames -= gameMaster.timeScale;

            Movement();

            SpawnBullets();

            BulletCollisions();

            if (killzoneDetectIndex == gameMaster.killzoneDetectIndex)
                DetectKillzone();

            if (anim != null)
                UpdateAnim();
        }

        public override void PreRender()
        {
            playedDeathSound = false;
        }

        public virtual void Movement()
        {
            if (waitTime <= 0)
            {
                if (segmentIndex >= enemyData.movementCurve.NumSegments)
                    EntityAttachedTo.LocalPosition += new Vector3(prevMovement);
                else
                {
                    moveTimePassed += gameMaster.timeScale;
                    if (moveTimePassed >= enemyData.movementCurve.EndTimeOfSegment(segmentIndex))
                    {
                        segmentIndex++;
                        prevMovement = EntityAttachedTo.LocalPosition.Xy - prevPos;
                        waitTime = enemyData.movementCurve.StartWaitTime(segmentIndex);

                        BulletElement.BulletStage[] stages = new BulletElement.BulletStage[1];
                        stages[0] = new BulletElement.BulletStage();
                        stages[0].spriteType = gameMaster.bulletTypes[BulletElement.Random(1, gameMaster.bulletTypes.Count)];
                        stages[0].bulletColor = BulletElement.Random(0, gameMaster.bulletSprites[stages[0].spriteType].sprites.Count - 2);
                        stages[0].startingSpeed = 200;
                        stages[0].endingSpeed = 75;
                        stages[0].framesToChangeSpeed = 30;
                        stages[0].movementDirection = DawnMath.RandomCircle();
                        //stages[0].movementDirection = DawnMath.RandomCircle();
                        stages[0].rotate = BulletElement.ShouldTurn(stages[0].spriteType);
                        //stages[0].rotate = false;
                        BulletElement.SpawnBullet(stages, EntityAttachedTo.LocalPosition, BulletElement.ShouldSpin(stages[0].spriteType));
                    }

                    if (waitTime <= 0)
                    {
                        if (segmentIndex >= enemyData.movementCurve.NumSegments)
                            EntityAttachedTo.LocalPosition += new Vector3(prevMovement);
                        else
                            EntityAttachedTo.LocalPosition = new Vector3(startPosition +
                                enemyData.movementCurve.PositionOnSegment(segmentIndex, moveTimePassed, true));
                    }
                }
            }
            else
                waitTime -= gameMaster.timeScale;
        }

        public virtual void SpawnBullets()
        {
            //Method to be overwritten
        }

        protected virtual void UpdateAnim()
        {
            float direction = entityAttachedTo.LocalPosition.X - prevPos.X;
            int stateIndex = anim.StateIndex;
            if (Math.Abs(direction) < 0.25)
            {
                if (stateIndex != 0)
                {
                    if (stateIndex == 3)
                        anim.UpdateStateIndex = 1;
                    else if (stateIndex == 5)
                        anim.UpdateStateIndex = 2;
                    else if(stateIndex != 1 && stateIndex != 2)
                        anim.UpdateStateIndex = 0;
                }
            }
            else if (direction > 0)
            {
                if (stateIndex != 3 && anim.StateIndex != 4)
                {
                    anim.UpdateStateIndex = 4;
                }
            }
            else if (direction < 0)
            {
                if (stateIndex != 5 && anim.StateIndex != 6)
                {
                    anim.UpdateStateIndex = 6;
                }
            }
        }

        protected void DetectKillzone()
        {
            Vector3 pos = EntityAttachedTo.WorldPosition;
            if (pos.X < destroyExtentsX.X || pos.X > destroyExtentsX.Y || pos.Y < destroyExtentsY.X || pos.Y > destroyExtentsY.Y)
                EntityAttachedTo.AttemptDelete();
        }

        protected virtual void BulletCollisions()
        {
            PlayerDamageCollider thisCol;
            float curHitboxSize, hitboxCombined, sin, cos,
                XCosYSin, XSinYCos;
            Vector2 position = entityAttachedTo.WorldPosition.Xy + enemyData.colliderOffset,
                posDif = new Vector2();
            curHitboxSize = enemyData.colliderSize * entityAttachedTo.WorldScale.X;
            for (int i = 0; i < PlayerDamageCollider.playerDamageColliders.Count; i++)
            {
                thisCol = PlayerDamageCollider.playerDamageColliders[i];
                posDif = thisCol.rotatedOffset - position;
                hitboxCombined = curHitboxSize + thisCol.LargestScaledDimension / 2;

                if (posDif.X * posDif.X + posDif.Y * posDif.Y <= hitboxCombined * hitboxCombined)
                {
                    posDif = new Vector2(Math.Clamp(posDif.X, -curHitboxSize, curHitboxSize),
                        Math.Clamp(posDif.Y, -curHitboxSize, curHitboxSize));
                    sin = MathF.Sin(thisCol.rotRad);
                    cos = MathF.Cos(thisCol.rotRad);
                    XCosYSin = posDif.X * cos - posDif.Y * sin;
                    XCosYSin *= XCosYSin;
                    XSinYCos = posDif.X * sin + posDif.Y * cos;
                    XSinYCos *= XSinYCos;

                    if (XCosYSin / (thisCol.scaledWidth * thisCol.scaledWidth)
                        + XSinYCos / (thisCol.scaledHeight * thisCol.scaledHeight) <= 1)
                    {
                        Damage(thisCol.bulletAttached);
                    }
                }
            }
        }

        protected virtual void Damage(BulletElement bullet)
        {
            if (bullet != null)
            {
                bullet.destroy = true;
                health -= bullet.damage;
            }

            if (health <= 0)
                Death();
        }

        protected virtual void Death()
        {
            for (int i = 0; i < itemSpawns.Length; i++)
            {
                ItemElement.SpawnItem(itemSpawns[i], entityAttachedTo.WorldPosition);
            }
            entityAttachedTo.DeleteAllElementsOfType<EnemyDamageCollider>();
            entityAttachedTo.AttemptDelete();
        }

        protected override void OnEnableAndCreate()
        {
            allEnemies.Add(this);
            base.OnEnableAndCreate();
        }
        protected override void OnDisableAndDestroy()
        {
            allEnemies.Remove(this);
            base.OnDisableAndDestroy();
        }
    }
} //Namespace end
