using System;
using System.Collections.Generic;
using System.Text;

namespace DawnmakuEngine.Data.Resources
{
    /// <summary>
    /// A class to get values from several resources at once, in the case of multiple needing to interact for one value.
    /// </summary>
    public class ResourceGroup
    {
        public List<BaseResource> resources = new List<BaseResource>();

        public enum CalculationType
        {
            Add, Average, Maximum, Minimum, Specific, Custom
        }

        #region Call Calcs
        public float GetFloatCalc(CalculationType calc, object[][] args = null)
        {
            switch(calc)
            {
                default:
                case CalculationType.Add:
                    return AddAllFloat(args);
                case CalculationType.Average:
                    return AverageAllFloat(args);
                case CalculationType.Maximum:
                    return MaximumFloat(args);
                case CalculationType.Minimum:
                    return MinimumFloat(args);
                case CalculationType.Specific:
                    return SpecificValueFloat(args);
                case CalculationType.Custom:
                    return CustomFloat(args);
            }
        }
        public int GetIntCalc(CalculationType calc, object[][] args = null)
        {
            switch (calc)
            {
                default:
                case CalculationType.Add:
                    return AddAllInt(args);
                case CalculationType.Average:
                    return AverageAllInt(args);
                case CalculationType.Maximum:
                    return MaximumInt(args);
                case CalculationType.Minimum:
                    return MinimumInt(args);
                case CalculationType.Specific:
                    return SpecificValueInt(args);
                case CalculationType.Custom:
                    return CustomInt(args);
            }
        }
        public object GetObjCalc(CalculationType calc, object[][] args = null)
        {
            try
            {
                switch (calc)
                {
                    default:
                    case CalculationType.Add:
                        return AddAllObj(args);
                    case CalculationType.Average:
                        return AverageAllObj(args);
                    case CalculationType.Maximum:
                        return MaximumObj(args);
                    case CalculationType.Minimum:
                        return MinimumObj(args);
                    case CalculationType.Specific:
                        return SpecificValueObj(args);
                    case CalculationType.Custom:
                        return CustomObj(args);
                }
            }
            catch (Exception e)
            {
                GameMaster.LogErrorMessage("There was an error calculating an Obj value in a resource group!", e.Message);
            }

            return null;
        }
        #endregion

        #region Int Calcs
        protected int AddAllInt(object[][] args)
        {
            int result = 0;
            for (int i = 0; i < resources.Count; i++)
            {
                if (args == null || args.Length < i)
                    result += resources[i].OutputInt();
                else
                    result += resources[i].OutputInt(args[i]);
            }
            return result;
        }
        protected int AverageAllInt(object[][] args)
        {
            return AddAllInt(args) / resources.Count;
        }
        protected int MaximumInt(object[][] args)
        {
            int result = 0;
            int test = 0;
            for (int i = 0; i < resources.Count; i++)
            {
                if (args == null || args.Length < i)
                    test = resources[i].OutputInt();
                else
                    test = resources[i].OutputInt(args[i]);

                if (result < test)
                    result = test;
            }
            return result;
        }
        protected int MinimumInt(object[][] args)
        {
            int result = int.MaxValue;
            int test = 0;
            for (int i = 0; i < resources.Count; i++)
            {
                if (args == null || args.Length < i)
                    test = resources[i].OutputInt();
                else
                    test = resources[i].OutputInt(args[i]);

                if (result > test)
                    result = test;
            }
            return result;
        }
        protected int SpecificValueInt(object[][] args)
        {
            try
            {
                for (int i = 0; i < resources.Count; i++)
                {
                    if (resources[i].name == (string)args[0][0])
                        return resources[i].OutputInt(args[1]);
                }
            }
            catch (Exception e)
            {
                GameMaster.LogErrorMessage("There was an error outputting a specific int value in a resource group!", e.Message);
            }
            
            return -1;
        }
        protected virtual int CustomInt(object[][] args) { return 0; }
        #endregion

        #region Float Calcs
        protected float AddAllFloat(object[][] args)
        {
            float result = 0;
            for (int i = 0; i < resources.Count; i++)
            {
                if (args == null || args.Length < i)
                    result += resources[i].OutputFloat();
                else
                    result += resources[i].OutputFloat(args[i]);
            }
            return result;
        }
        protected float AverageAllFloat(object[][] args)
        {
            return AddAllFloat(args) / resources.Count;
        }
        protected float MaximumFloat(object[][] args)
        {
            float result = 0;
            float test = 0;
            for (int i = 0; i < resources.Count; i++)
            {
                if (args == null || args.Length < i)
                    test = resources[i].OutputFloat();
                else
                    test = resources[i].OutputFloat(args[i]);
                if (result < test)
                    result = test;
            }
            return result;
        }
        protected float MinimumFloat(object[][] args)
        {
            float result = float.MaxValue;
            float test = 0;
            for (int i = 0; i < resources.Count; i++)
            {
                if (args == null || args.Length < i)
                    test = resources[i].OutputFloat();
                else
                    test = resources[i].OutputFloat(args[i]);
                if (result > test)
                    result = test;
            }
            return result;
        }
        protected float SpecificValueFloat(object[][] args)
        {
            try
            {
                for (int i = 0; i < resources.Count; i++)
                {
                    if (resources[i].name == (string)args[0][0])
                        return resources[i].OutputFloat(args[1]);
                }
            }
            catch (Exception e)
            {
                GameMaster.LogErrorMessage("There was an error outputting a specific float value in a resource group!", e.Message);
            }
            
            return -1;
        }
        protected virtual float CustomFloat(object[] args) { return 0; }
        #endregion

        #region Custom Obj OutputCalcs
        protected virtual object AddAllObj(object[][] args) { return null; }
        protected virtual object AverageAllObj(object[][] args) { return null; }
        protected virtual object MaximumObj(object[][] args) { return null; }
        protected virtual object MinimumObj(object[][] args) { return null; }
        protected virtual object SpecificValueObj(object[][] args) { return null; }
        protected virtual object CustomObj(object[][] args) { return null; }
        #endregion


        public ResourceGroup(List<BaseResource> newResources) : this()
        {
            resources = newResources;
        }
        public ResourceGroup(params BaseResource[] newResources) : this()
        {
            for (int i = 0; i < newResources.Length; i++)
                resources.Add(newResources[i]);
        }

        public ResourceGroup() { }
    }
}
