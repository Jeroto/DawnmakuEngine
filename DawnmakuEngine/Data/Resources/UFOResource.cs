using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace DawnmakuEngine.Data.Resources
{
    public class UFOResource : BaseResource
    {
        int[] ufos = new int[3];
        public override void InitValue(string stringValue)
        {
            string[] colors = stringValue.Split(',');

            for (int i = 0; i < colors.Length; i++)
            {
                switch(colors[i])
                {
                    case "none":
                        ufos[i] = 0;
                        break;
                    case "red":
                        ufos[i] = 1;
                        break;
                    case "green":
                        ufos[i] = 2;
                        break;
                    case "blue":
                        ufos[i] = 3;
                        break;
                }
            }
        }

        public override object GetValue()
        {
            return ufos;
        }

        public override void ModifyValue(params object[] values)
        {
            int index = Convert.ToInt32(values[0]);
            if(index > 0 && index < ufos.Length)
            {
                ufos[index] = Convert.ToInt32(values[1]);
            }
        }

        public override float OutputFloat()
        {
            return ufos[0];
        }

        public UFOResource(string resourceName) : base(resourceName)
        {
            resourceType = typeof(List<int>);
        }
    }
}
