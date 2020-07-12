using System;
using System.Collections.Generic;
using OpenTK;
using SixLabors.ImageSharp;

namespace DawnmakuEngine.Elements
{
    class PlayerDamageCollider : BulletColliderBase
    {
        public static List<PlayerDamageCollider> playerDamageColliders = new List<PlayerDamageCollider>();

        public PlayerDamageCollider() : base() { }
        public PlayerDamageCollider(BulletElement bullet) : base(bullet) { }
        public PlayerDamageCollider(Vector2 size_, Vector2 offset_) : base(size_, offset_) { }
        public PlayerDamageCollider(Vector2 size_, Vector2 offset_, BulletElement bullet) : base(size_, offset_, bullet) { }

        protected override void OnEnableAndCreate()
        {
            playerDamageColliders.Add(this);
            base.OnEnableAndCreate();
        }

        protected override void OnDisableAndDestroy()
        {
            playerDamageColliders.Remove(this);
            base.OnDisableAndDestroy();
        }

        public override void Remove()
        {
            playerDamageColliders.Remove(this);
        }

        public override void AttemptDelete()
        {
            base.AttemptDelete();
            playerDamageColliders.Remove(this);
        }
    }
}
