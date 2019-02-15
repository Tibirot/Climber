using UnityEngine;
using System.Collections;

using SA.iOS.Foundation;

namespace SA.CrossPlatform.App
{

    internal class UM_IOSBuildInfo : UM_AbstractBuildInfo, UM_iBuildInfo
    {

        public override string Version {
            get {
                ISN_NSBuildInfo buildInfo = ISN_NSBundle.BuildInfo;
                return buildInfo.AppVersion;
            }
        }
    }
}