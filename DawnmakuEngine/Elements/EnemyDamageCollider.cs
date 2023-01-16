using System;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Mathematics;

namespace DawnmakuEngine.Elements
{
    class EnemyDamageCollider : BulletColliderBase
    {
        public static List<EnemyDamageCollider> enemyDamageColliders = new List<EnemyDamageCollider>();

        public EnemyDamageCollider() : base() { }
        public EnemyDamageCollider(BulletElement bullet) : base(bullet) { }
        public EnemyDamageCollider(Vector2 size_, Vector2 offset_) : base(size_, offset_) { }
        public EnemyDamageCollider(Vector2 size_, Vector2 offset_, BulletElement bullet) : base(size_, offset_, bullet) { }

        protected override void OnEnableAndCreate()
        {
            enemyDamageColliders.Add(this);
            base.OnEnableAndCreate();
        }

        protected override void OnDisableAndDestroy()
        {
            enemyDamageColliders.Remove(this);
            base.OnDisableAndDestroy();
        }

        public override void Remove()
        {
            enemyDamageColliders.Remove(this);
        }

        public override void AttemptDelete()
        {
            base.AttemptDelete();
            enemyDamageColliders.Remove(this);
        }
    }
}
