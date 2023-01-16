using System;
using System.Collections.Generic;
using System.Text;
using OpenTK;
using OpenTK.Mathematics;

namespace DawnmakuEngine.Elements
{
    class RotateElement : Element
    {
        public Vector3 axis = new Vector3(0, 0, 1);
        float rotationSpeed; 
        bool affectedByTimescale;
        public float RotSpeedDeg { get { return MathHelper.RadiansToDegrees(rotationSpeed); } set { rotationSpeed = MathHelper.DegreesToRadians(value); } }
        public float RotSpeedRad { get { return rotationSpeed; } set { rotationSpeed = value; } }

        public RotateElement(float rotationSpeed_, bool isDegrees_, bool affectedByTimescale_) : base()
        {
            affectedByTimescale = affectedByTimescale_;
            if (isDegrees_)
                RotSpeedDeg = rotationSpeed_;
            else
                RotSpeedRad = rotationSpeed_;
        }
        public RotateElement(float rotationSpeed_, bool isDegrees_, bool affectedByTimescale_, Vector3 rotateAround) 
            : this(rotationSpeed_, isDegrees_, affectedByTimescale_)
        {
            axis = rotateAround;
        }
        public RotateElement(float rotationSpeed_, bool isDegrees_, bool affectedByTimescale_, float rotateAroundX, float rotateAroundY, float rotateAroundZ)
            : this(rotationSpeed_, isDegrees_, affectedByTimescale_, new Vector3(rotateAroundX, rotateAroundY, rotateAroundZ))
        {
        }

        public override void OnUpdate()
        {
            if (rotationSpeed.CompareTo(0) != 0) 
                EntityAttachedTo.RotateQuaternion(axis, 
                    rotationSpeed * GameMaster.gameMaster.frameTime * (affectedByTimescale ? GameMaster.gameMaster.timeScale : 1));
            base.OnUpdate();
        }

        public override Element Clone()
        {
            return new RotateElement(rotationSpeed, false, affectedByTimescale, axis);
        }
    }
}
