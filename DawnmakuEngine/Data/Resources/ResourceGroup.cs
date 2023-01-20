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
        public float GetFloatCalc(CalculationType calc, object[] args = null)
        {
            switch(calc)
            {
                default:
                case CalculationType.Add:
                    return AddAllFloat();
                case CalculationType.Average:
                    return AverageAllFloat();
                case CalculationType.Maximum:
                    return MaximumFloat();
                case CalculationType.Minimum:
                    return MinimumFloat();
                case CalculationType.Specific:
                    return SpecificValueFloat((string)args[0]);
                case CalculationType.Custom:
                    return CustomFloat(args);
            }
        }
        public int GetIntCalc(CalculationType calc, object[] args = null)
        {
            switch (calc)
            {
                default:
                case CalculationType.Add:
                    return AddAllInt();
                case CalculationType.Average:
                    return AverageAllInt();
                case CalculationType.Maximum:
                    return MaximumInt();
                case CalculationType.Minimum:
                    return MinimumInt();
                case CalculationType.Specific:
                    return SpecificValueInt((string)args[0]);
                case CalculationType.Custom:
                    return CustomInt(args);
            }
        }
        public object GetObjCalc(CalculationType calc, object[] args = null)
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
        #endregion

        #region Int Calcs
        protected int AddAllInt()
        {
            int result = 0;
            for (int i = 0; i < resources.Count; i++)
                result += resources[i].OutputInt();
            return result;
        }
        protected int AverageAllInt()
        {
            return (int)(AddAllInt() / resources.Count);
        }
        protected int MaximumInt()
        {
            int result = 0;
            int test = 0;
            for (int i = 0; i < resources.Count; i++)
            {
                test = resources[i].OutputInt();
                if (result < test)
                    result = test;
            }
            return result;
        }
        protected int MinimumInt()
        {
            int result = int.MaxValue;
            int test = 0;
            for (int i = 0; i < resources.Count; i++)
            {
                test = resources[i].OutputInt();
                if (result > test)
                    result = test;
            }
            return result;
        }
        protected int SpecificValueInt(string resourceName)
        {
            for (int i = 0; i < resources.Count; i++)
            {
                if (resources[i].name == resourceName)
                    return resources[i].OutputInt();
            }
            return -1;
        }
        protected virtual int CustomInt(object[] args) { return 0; }
        #endregion

        #region Float Calcs
        protected float AddAllFloat()
        {
            float result = 0;
            for (int i = 0; i < resources.Count; i++)
                result += resources[i].OutputFloat();
            return result;
        }
        protected float AverageAllFloat()
        {
            return AddAllFloat() / resources.Count;
        }
        protected float MaximumFloat()
        {
            float result = 0;
            float test = 0;
            for (int i = 0; i < resources.Count; i++)
            {
                test = resources[i].OutputFloat();
                if (result < test)
                    result = test;
            }
            return result;
        }
        protected float MinimumFloat()
        {
            float result = float.MaxValue;
            float test = 0;
            for (int i = 0; i < resources.Count; i++)
            {
                test = resources[i].OutputFloat();
                if (result > test)
                    result = test;
            }
            return result;
        }
        protected float SpecificValueFloat(string resourceName)
        {
            for (int i = 0; i < resources.Count; i++)
            {
                if (resources[i].name == resourceName)
                    return resources[i].OutputFloat();
            }
            return -1;
        }
        protected virtual float CustomFloat(object[] args) { return 0; }
        #endregion

        #region Custom Obj OutputCalcs
        protected virtual object AddAllObj(object[] args) { return null; }
        protected virtual object AverageAllObj(object[] args) { return null; }
        protected virtual object MaximumObj(object[] args) { return null; }
        protected virtual object MinimumObj(object[] args) { return null; }
        protected virtual object SpecificValueObj(object[] args) { return null; }
        protected virtual object CustomObj(object[] args) { return null; }
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
