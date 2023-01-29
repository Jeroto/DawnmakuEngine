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

        public override object GetValue(params object[] values)
        {
            return value;
        }

        public override void ModifyValue(params object[] values)
        {
            try
            {
                value += Convert.ToInt32(values[0]);

                value = MathHelper.Clamp(value, (int)min, (int)max);
            }
            catch (Exception e)
            {
                GameMaster.LogErrorMessage("There was an error modifying an int resource!", e.Message);
            }
        }

        public override float OutputFloat(params object[] values)
        {
            return (value - min) / (max - min);
        }

        public override int OutputInt(params object[] values)
        {
            try
            {
                return value + Convert.ToInt32(values[0]);
            }
            catch (Exception e)
            {
                GameMaster.LogErrorMessage("There was an error outputting an int for an int resource!", e.Message);
            }

            return value;
        }

        public override string OutputString(params object[] values)
        {
            return value.ToString();
        }

        public IntResource(string resourceName) : base(resourceName)
        {
            resourceType = typeof(int);
        }
    }
}
