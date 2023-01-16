using System;
using System.Collections.Generic;
using System.Text;

namespace DawnmakuEngine.Data.Resources
{
    public class FloatResource : BaseResource
    {
        float value;

        public override object GetValue()
        {
            return value;
        }

        public override void ModifyValue(params object[] values)
        {
            value += Convert.ToSingle(values[0]);
        }

        public override float OutputFloat()
        {
            return value;
        }

        public FloatResource() : base()
        {
            resourceType = typeof(float);
        }
    }
}
