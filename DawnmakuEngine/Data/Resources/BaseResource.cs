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
        /// <param name="values">A list of any data type used to modify the value of the resource. Within the function, each entry needs to be converted to the type you want to use.</param>
        public virtual void ModifyValue(params object[] values)
        {

        }



        /// <summary>
        /// Returns an 'object' containing the value, which will need to be converted to the intended type. Check for the value being null to avoid errors.
        /// </summary>
        /// <param name="values">A list of any data type, used to modify the output in some way. Within the function, each entry needs to be converted to the type you want to use.</param>
        ///<returns>A generic 'object' which contains the value. Will need to be converted to the type you want to use wherever it's outputted.</returns>
        public virtual object GetValue(params object[] values)
        {
            return null;
        }

        /// <summary>
        /// Used for meters and UI. Outputs the value modified into a float. Usually a range between 0 and 1 is desired.
        /// </summary>
        /// <param name="values">A list of any data type, used to modify the output float in some way. Within the function, each entry needs to be converted to the type you want to use.</param>
        public virtual float OutputFloat(params object[] values)
        {
            return 0;
        }

        /// <summary>
        /// Used for UI. Outputs the value modified into an int. Usually used as an index of some sort.
        /// </summary>
        /// <param name="values">A list of any data type, used to modify the output int in some way. Within the function, each entry needs to be converted to the type you want to use.</param>
        public virtual int OutputInt(params object[] values)
        {
            return 0;
        }

        /// <summary>
        /// Used mainly for UI. Outputs the value modified into a readable string.
        /// </summary>
        /// <param name="values">A list of any data type, used to modify the output string in some way.</param>
        public virtual string OutputString(params object[] values)
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
