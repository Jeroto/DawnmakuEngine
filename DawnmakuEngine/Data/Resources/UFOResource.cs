using System;
using System.Collections.Generic;
using System.Text;

namespace DawnmakuEngine.Data.Resources
{
    public class UFOResource : BaseResource
    {
        List<int> ufos = new List<int>();

        public override object GetValue()
        {
            return ufos;
        }

        public override void ModifyValue(params object[] values)
        {
            int index = Convert.ToInt32(values[0]);
            if(index > 0 && index < ufos.Count)
            {
                ufos[index] = Convert.ToInt32(values[1]);
            }
        }

        public override float OutputFloat()
        {
            return ufos[0];
        }

        public UFOResource(int ufoCount = 3) : base()
        {
            resourceType = typeof(List<int>);

            for (int i = 0; i < ufoCount; i++)
                ufos.Add(0);
        }
    }
}
