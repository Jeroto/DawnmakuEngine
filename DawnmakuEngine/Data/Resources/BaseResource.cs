using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Text;

namespace DawnmakuEngine.Data.Resources
{
    /// <summary>
    ///  A generic resource without any held value. Override GetValue and ModifyValue to manipulate it.
    /// </summary>
    public class BaseResource
    {
        public string name;
        public Type resourceType = typeof(BaseResource);

        public float min = 0, max = 999999999;

        /// <summary>
        /// Called when initially loading the resource with json. Recieves the direct value string if given, and "NULL" otherwise.
        /// </summary>
        /// <param name="stringValue">Either the string imported from JSON or "NULL" if no value was given.</param>
        public virtual void InitValue(string stringValue)
        {
        }

        /// <summary>
        /// Modifies the value(s) contained in the resource. Any number and types of values can be passed in to modify the resource.
        /// </summary>
        /// <param name="values">The list of values. Within the function, each entry needs to be converted to the type you want to use.</param>
        public virtual void ModifyValue(params object[] values)
        {

        }

        

        /// <summary>
        /// Returns an 'object' containing the value, which will need to be converted to the intended type. Check for the value being null to avoid errors.
        /// </summary>
        /// <returns></returns>
        public virtual object GetValue()
        {
            return null;
        }
        
        /// <summary>
        /// Used for meters and UI. Outputs the value modified into a float. Usually a range between 0 and 1 is desired.
        /// </summary>
        /// <returns></returns>
        public virtual float OutputFloat()
        {
            return 0;
        }

        /// <summary>
        /// Used for UI. Outputs the value modified into an int. Usually used as an index of some sort.
        /// </summary>
        /// <returns></returns>
        public virtual int OutputInt()
        {
            return 0;
        }

        /// <summary>
        /// Used mainly for UI. Outputs the value modified into a readable string.
        /// </summary>
        /// <returns></returns>
        public virtual string OutputString()
        {
            return "";
        }

        public override string ToString()
        {
            return name + ": " + resourceType.ToString() + ", " + OutputString();
        }

        public BaseResource(string resourceName) 
        {
            name = resourceName;
            GameMaster.gameMaster.resources.Add(name, this);
        }
    }
}
