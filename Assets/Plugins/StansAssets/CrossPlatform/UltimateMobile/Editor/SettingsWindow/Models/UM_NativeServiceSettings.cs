using UnityEngine;
using System;
using SA.Foundation.Editor;


namespace SA.CrossPlatform
{
    public abstract class UM_NativeServiceSettings 
    {

        public abstract string ServiceName { get; }
        public abstract Type ServiceUIType { get; }
        public abstract bool IsEnabled { get; }

    }
}