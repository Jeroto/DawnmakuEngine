using System;
using System.Collections.Generic;
using System.Text;

namespace DawnmakuEngine.Data.Resources
{
    public class IntResource : BaseResource
    {
        int value;
        public override void InitValue(string stringValue)
        {
            value = int.Parse(stringValue);
        }

        public override object GetValue()
        {
            return value;
        }

        public override void ModifyValue(params object[] values)
        {
            value += Convert.ToInt32(values[0]);
        }

        public override float OutputFloat()
        {
            return value;
        }

        public IntResource(string resourceName) : base(resourceName)
        {
            resourceType = typeof(int);
        }
    }
}
