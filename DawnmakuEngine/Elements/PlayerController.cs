using System;
using System.Collections.Generic;
using System.Text;

using OpenTK;
using OpenTK.Input;

namespace DawnmakuEngine.Elements
{
    class PlayerController : Element
    {
        static Random playerRandom;

        public float hitboxSize;
        protected float moveSpeed;
        protected float focusModifier;
        float shootDelay = 2, shootDelayCurrent = 0;
        int count = 0;

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
            set { focusModifier = value / 100f; }
        }

        private Vector2 movementVelocity = Vector2.Zero;


        public PlayerController() : base()
        {
            gameMaster = GameMaster.gameMaster;
        }

        public override void PostCreate()
        {
            anim = EntityAttachedTo.GetElement<TextureAnimator>();
            base.PostCreate();
        }

        public override void OnUpdate()
        {
            movementVelocity.X = InputScript.DirectionalInput.X;
            movementVelocity.Y = InputScript.DirectionalInput.Y;
            if (movementVelocity.LengthSquared > 0)
                movementVelocity.Normalize();

            movementVelocity *= MoveSpeed;
            if (InputScript.Focus)
                movementVelocity *= focusModifier;

            entityAttachedTo.LocalPosition += new Vector3(movementVelocity.X, movementVelocity.Y, 0);

            if (movementVelocity.X > 0)
                anim.UpdateStateIndex = 1;
            else if (movementVelocity.X < 0)
                anim.UpdateStateIndex = 2;
            else
                anim.UpdateStateIndex = 0;

            if(InputScript.Shoot)
            {
                if (shootDelayCurrent <= 0)
                {
                    for (int i = 0; i < 2; i++)
                    {

                        BulletElement.BulletStage[] stages = new BulletElement.BulletStage[1];
                        stages[0] = new BulletElement.BulletStage();
                        stages[0].spriteType = (BulletElement.BulletType)BulletElement.Random(1, Enum.GetValues(typeof(BulletElement.BulletType)).Length);
                        stages[0].bulletColor = (BulletElement.BulletColor)BulletElement.Random(0, Enum.GetValues(typeof(BulletElement.BulletColor)).Length - 2);
                        stages[0].startingSpeed = 200;
                        stages[0].endingSpeed = 75;
                        stages[0].framesToChangeSpeed = 30;
                        stages[0].movementDirection = DawnMath.CalculateCircleDeg(count * (i == 0 ? 1 : -1));
                        //stages[0].movementDirection = DawnMath.RandomCircle();
                        stages[0].rotate = BulletElement.ShouldTurn(stages[0].spriteType);
                        //stages[0].rotate = false;
                        BulletElement.SpawnBullet(stages, EntityAttachedTo.LocalPosition, BulletElement.ShouldSpin(stages[0].spriteType));
                    }
                    shootDelayCurrent = shootDelay;
                    count += 13;
                }
            }
            shootDelayCurrent -= gameMaster.timeScale;
        }
    }
}
