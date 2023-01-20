using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Text;

namespace DawnmakuEngine.Data.Resources
{
    public class FloatResource : BaseResource
    {
        float value = 0;

        public override void InitValue(string stringValue)
        {
            if (stringValue == "NULL")
                return;
            value = float.Parse(stringValue);
        }

        public override object GetValue()
        {
            return value;
        }

        public override void ModifyValue(params object[] values)
        {
            try
            {
                value += Convert.ToSingle(values[0]);

                value = MathHelper.Clamp(value, min, max);
            }
            catch (Exception e)
            {
                GameMaster.LogErrorMessage("There was an error modifying a float resource!", e.Message);
            }
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
