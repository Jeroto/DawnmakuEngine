﻿using NAudio.Wave;
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

        public BaseResource() { }
    }
}
