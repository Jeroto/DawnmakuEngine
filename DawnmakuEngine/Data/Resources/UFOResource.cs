using Newtonsoft.Json.Linq;
using OpenTK.Mathematics;
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
            if(stringValue == "NULL")
                return;

            string[] colors = stringValue.Split(',');
            int ufoCount;

            if(colors.Length == 1 && int.TryParse(colors[0], out ufoCount))
            {
                ufos = new int[ufoCount];
            }
            else
            {
                ufos = new int[colors.Length];
                for (int i = 0; i < colors.Length; i++)
                {
                    ufos[i] = ColorToInt(colors[i].ToLower());
                }
            }
        }
        public int ColorToInt(string col)
        {
            switch (col)
            {
                default:
                case "null":
                case "none":
                    return 0;
                case "red":
                    return 1;
                case "green":
                    return 2;
                case "blue":
                    return 3;
            }
        }
        public string IntToColor(int col)
        {
            switch (col) 
            {
                default:
                case 0:
                    return "none";
                case 1:
                    return "red";
                case 2:
                    return "green";
                case 3:
                    return "blue";
            }
        }

        public override object GetValue(params object[] values)
        {
            return ufos;
        }

        public override void ModifyValue(params object[] values)
        {
            try
            {
                int index = Convert.ToInt32(values[0]);
                int ufoType = Convert.ToInt32(values[1]);
                //Set an exact index if we get one
                if (index >= 0 && index < ufos.Length)
                {
                    ufos[index] = ufoType;
                }
                else 
                {
                    int i;

                    //try to fill any slot that isn't the last one
                    for (i = 0; i < ufos.Length - 1; i++)
                    {
                        if (ufos[i] == 0)
                        {
                            ufos[i] = ufoType;
                            break;
                        }
                    }

                    //If i is greater than the count minus one, we've set a ufo, otherwise we begin checking for equality
                    if(i >= ufos.Length - 1)
                    {
                        bool match = true;
                        for (i = 0; i < ufos.Length - 1; i++)
                        {
                            if (ufos[i] != ufoType)
                            {
                                match = false;
                                break;
                            }
                        }

                        //If one didn't match the new ufo, we shift all ufos over one and replace the first one
                        //However, we continue leaving the last slot empty
                        if(!match)
                        {
                            for (i = ufos.Length - 2; i >= 1; i--)
                            {
                                ufos[i] = ufos[i - 1];
                            }
                            ufos[0] = ufoType;
                        }
                        else //If the types are all the same, we add on the last one
                            ufos[ufos.Length - 1] = ufoType;
                    }
                }
            }
            catch (Exception e)
            {
                GameMaster.LogErrorMessage("There was an error modifying a UFO resource!", e.Message);
            }
        }

        public override float OutputFloat(params object[] values)
        {
            return ufos[0];
        }

        public override int OutputInt(params object[] values)
        {
            try
            {
                return ufos[Convert.ToInt32(values[0])];
            }
            catch (Exception e)
            {
                GameMaster.LogErrorMessage("There was an error outputting an int for a UFO resource!", e.Message);
            }
            return ufos[0];
        }

        public override string OutputString(params object[] values)
        {
            return IntToColor(ufos[0]) + ", " + IntToColor(ufos[1]) + ", " + IntToColor(ufos[2]);
        }

        public UFOResource(string resourceName) : base(resourceName)
        {
            resourceType = typeof(List<int>);
        }
    }
}
