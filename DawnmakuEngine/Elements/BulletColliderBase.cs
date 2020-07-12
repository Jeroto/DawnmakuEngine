using System;
using System.Collections.Generic;
using OpenTK;
using SixLabors.ImageSharp;

namespace DawnmakuEngine.Elements
{
    class BulletColliderBase : Element
    {
        public BulletColliderBase() : base() { RotateOffset(); }
        public BulletColliderBase(BulletElement bullet) : base()
        {
            bulletAttached = bullet;
            entityAttachedTo = bullet.EntityAttachedTo; 
            RotateOffset();
        }

        public BulletColliderBase(Vector2 size_, Vector2 offset_) : base()
        {
            offset = offset_;
            width = size_.X;
            height = size_.Y;
            RotateOffset();
        }
        public BulletColliderBase(Vector2 size_, Vector2 offset_, BulletElement bullet) : base()
        {
            offset = offset_;
            width = size_.X;
            height = size_.Y;
            bulletAttached = bullet;
            entityAttachedTo = bullet.EntityAttachedTo;
            RotateOffset();
        }

        public BulletElement bulletAttached;

        public float width, height,
            scaledWidth, scaledHeight,
            rotRad;
        public Vector2 offset,
            rotatedOffset,
            posWithOffset;

        public float SmallestDimension
        {
            get
            {
                if (width <= height)
                    return width;
                return height;
            }
        }
        public float LargestDimension
        {
            get
            {
                if (width >= height)
                    return width;
                return height;
            }
        }
        public float SmallestScaledDimension
        {
            get
            {
                if (scaledWidth <= scaledHeight)
                    return scaledWidth;
                return scaledHeight;
            }
        }
        public float LargestScaledDimension
        {
            get
            {
                if (scaledWidth >= scaledHeight)
                    return scaledWidth;
                return scaledHeight;
            }
        }

        public override void OnUpdate()
        {
            RotateOffset();
        }

        public void RotateOffset()
        {
            if (entityAttachedTo == null)
                return;
            Matrix4 modifyMatrix = Matrix4.Identity //Identity base matrix
                    * Matrix4.CreateScale(entityAttachedTo.WorldScale)             //Adds a scale of the parent's local scale to scale units
                    * Matrix4.CreateRotationX(entityAttachedTo.WorldRotationRad.X)    //Rotates position based on parent's rotation
                    * Matrix4.CreateRotationY(entityAttachedTo.WorldRotationRad.Y)    // /\
                    * Matrix4.CreateRotationZ(entityAttachedTo.WorldRotationRad.Z);    // /\
            rotatedOffset *= (new Vector4(offset.X, offset.Y, 0, 1) * modifyMatrix).Xy;
            rotatedOffset += entityAttachedTo.WorldPosition.Xy;
            posWithOffset = entityAttachedTo.WorldPosition.Xy + rotatedOffset;
            scaledWidth = width * entityAttachedTo.WorldScale.X;
            scaledHeight = height * entityAttachedTo.WorldScale.Y;
            rotRad = entityAttachedTo.WorldRotationRad.Z;
        }
    }
}
