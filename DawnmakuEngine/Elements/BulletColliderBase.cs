using System;
using System.Collections.Generic;
using OpenTK;
using SixLabors.ImageSharp;
using OpenTK.Mathematics;

namespace DawnmakuEngine.Elements
{
    class BulletColliderBase : Element
    {
        public BulletColliderBase() : base(true, false, false, false, true, true) { CalculateAll(); }
        public BulletColliderBase(BulletElement bullet) : base(true, false, false, false, true, true)
        {
            bulletAttached = bullet;
            EntityAttachedTo = bullet.EntityAttachedTo;
            CalculateAll();
        }

        public BulletColliderBase(Vector2 size_, Vector2 offset_) : base(true, false, false, false, true, true)
        {
            offset = offset_;
            width = size_.X;
            height = size_.Y;
            CalculateAll();
        }
        public BulletColliderBase(Vector2 size_, Vector2 offset_, BulletElement bullet) : base(true, false, false, false, true, true)
        {
            offset = offset_;
            width = size_.X;
            height = size_.Y;
            bulletAttached = bullet;
            EntityAttachedTo = bullet.EntityAttachedTo;
            CalculateAll();
        }

        public BulletElement bulletAttached;
        public float width, height,
            scaledWidth, scaledHeight,
            rotRad;
        public Vector2 offset,
            rotatedOffset;

        private Matrix4 rotMat = Matrix4.Identity, scaleMat = Matrix4.Identity;

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

        public override void PostCreate()
        {
            CalculateAll();
            base.PostCreate();
        }

        public override void OnRotate()
        {
            CalculateRotation();
        }
        public override void OnScale()
        {
            CalculateScale();
        }

        public Vector2 GetRotatedCollider() { return rotatedOffset + entityAttachedTo.WorldPosition.Xy; }

        public void CalculateAll()
        {
            if (entityAttachedTo == null)
                return;
            CalculateScale();
            CalculateRotation();
        }

        public void CalculateRotation()
        {
            rotMat = Matrix4.Identity //Identity base matrix
                    * Matrix4.CreateRotationX(entityAttachedTo.WorldRotationRad.X)    //Rotates position based on parent's rotation
                    * Matrix4.CreateRotationY(entityAttachedTo.WorldRotationRad.Y)    // /\
                    * Matrix4.CreateRotationZ(entityAttachedTo.WorldRotationRad.Z);    // /\
            Matrix4 modifyMatrix = Matrix4.Identity * scaleMat * rotMat;
            rotatedOffset = (new Vector4(offset.X, offset.Y, 0, 1) * modifyMatrix).Xy;

            rotRad = entityAttachedTo.WorldRotationRad.Z;
        }

        public void CalculateScale()
        {
            scaleMat = Matrix4.Identity //Identity base matrix
                    * Matrix4.CreateScale(entityAttachedTo.WorldScale);             //Adds a scale of the parent's local scale to scale units

            Matrix4 modifyMatrix = Matrix4.Identity * scaleMat * rotMat;
            rotatedOffset = (new Vector4(offset.X, offset.Y, 0, 1) * modifyMatrix).Xy;

            scaledWidth = width * entityAttachedTo.WorldScale.X;
            scaledHeight = height * entityAttachedTo.WorldScale.Y;
        }
    }
}
