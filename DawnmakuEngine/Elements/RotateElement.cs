using System;
using System.Collections.Generic;
using System.Text;
using OpenTK;

namespace DawnmakuEngine.Elements
{
    class RotateElement : Element
    {
        float rotationSpeed; 
        bool affectedByTimescale;
        public float RotSpeedDeg { get { return MathHelper.RadiansToDegrees(rotationSpeed); } set { rotationSpeed = MathHelper.DegreesToRadians(value); } }
        public float RotSpeedRad { get { return rotationSpeed; } set { rotationSpeed = value; } }

        public RotateElement(float rotationSpeed_, bool isDegrees, bool affectedByTimescale_) : base()
        {
            affectedByTimescale = affectedByTimescale_;
            if (isDegrees)
                RotSpeedDeg = rotationSpeed_;
            else
                RotSpeedRad = rotationSpeed_;
        }

        public override void OnUpdate()
        {
            EntityAttachedTo.RotateQuaternion(new Vector3(0,0,1), rotationSpeed * GameMaster.gameMaster.frameTime * (affectedByTimescale ? GameMaster.gameMaster.timeScale : 1));
            base.OnUpdate();
        }
    }
}
