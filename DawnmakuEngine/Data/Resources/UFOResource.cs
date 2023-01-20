﻿using Newtonsoft.Json.Linq;
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
                ufos[i] = ColorToInt(colors[i]);
            }
        }
        public int ColorToInt(string col)
        {
            switch (col)
            {
                default:
                case "None":
                    return 0;
                case "Red":
                    return 1;
                case "Green":
                    return 2;
                case "Blue":
                    return 3;
            }
        }
        public string IntToColor(int col)
        {
            switch (col) 
            {
                default:
                case 0:
                    return "None";
                case 1:
                    return "Red";
                case 2:
                    return "Green";
                case 3:
                    return "Blue";
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
        public override string OutputString()
        {
            return IntToColor(ufos[0]) + ", " + IntToColor(ufos[1]) + ", " + IntToColor(ufos[2]);
        }

        public UFOResource(string resourceName) : base(resourceName)
        {
            resourceType = typeof(List<int>);
        }
    }
}
