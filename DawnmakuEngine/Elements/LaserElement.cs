using System;
using System.Collections.Generic;
using System.Text;

namespace DawnmakuEngine.Elements
{
    class LaserElement : Element
    {
        public bool grazed;
        public int grazeDelay = 0, //Set greater than 0 to allow repeated grazing
            grazeDelayCurrent;

        //Bullet Constants
        public ushort damage;
        public float currentSpeed;
        public LaserElement() : base(true)
        {

        }
    }
}
