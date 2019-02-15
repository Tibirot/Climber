using UnityEngine;
using System;
using SA.iOS;
using SA.Foundation.Editor;
using SA.Android;

namespace SA.CrossPlatform
{
    public abstract class UM_NativeServiceLayoutBasedSetting : UM_NativeServiceSettings
    {

        public abstract SA_ServiceLayout Layout { get; }
       

        public override string ServiceName { get { return Layout.Title; } }
        public override Type ServiceUIType { get { return Layout.GetType(); } }


        public override bool IsEnabled {
            get {
                 return false;
            }
        }

     

    }
}