using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Text;

namespace DawnmakuEngine.Data.Resources
{
    public class FloatResource : BaseResource
    {
        float value;

        float max = 1, min = 0;

        public override void InitValue(string stringValue)
        {
            string[] split = stringValue.Replace(" ", "").Split(',');

            value = float.Parse(split[0]);

            if(split.Length > 2)
            {
                min= float.Parse(split[1]);
                max= float.Parse(split[2]);
            }
            else if(split.Length > 1)
            {
                max = float.Parse(split[1]);
            }
        }

        public override object GetValue()
        {
            return value;
        }

        public override void ModifyValue(params object[] values)
        {
            value += Convert.ToSingle(values[0]);

            value = MathHelper.Clamp(value, min, max);
        }

        public override float OutputFloat()
        {
            return (value - min) / (max - min);
        }

        public override string OutputString()
        {
            return value.ToString();
        }

        public FloatResource(string resourceName) : base(resourceName)
        {
            resourceType = typeof(float);
        }
    }
}
