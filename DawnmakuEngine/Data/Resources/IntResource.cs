using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Text;

namespace DawnmakuEngine.Data.Resources
{
    public class IntResource : BaseResource
    {
        int value = 0;

        //int max = 1, min = 0;
        public override void InitValue(string stringValue)
        {
            if (stringValue == "NULL")
                return;
            value = int.Parse(stringValue);
        }

        public override object GetValue()
        {
            return value;
        }

        public override void ModifyValue(params object[] values)
        {
            value += Convert.ToInt32(values[0]);

            value = MathHelper.Clamp(value, (int)min, (int)max);
        }

        public override float OutputFloat()
        {
            return value;
        }

        public override int OutputInt()
        {
            return value;
        }

        public override string OutputString()
        {
            return value.ToString();
        }

        public IntResource(string resourceName) : base(resourceName)
        {
            resourceType = typeof(int);
        }
    }
}
