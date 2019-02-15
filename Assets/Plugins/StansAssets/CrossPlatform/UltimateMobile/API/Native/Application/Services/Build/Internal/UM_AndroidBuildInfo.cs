using UnityEngine;
using System.Collections;

using SA.Android.App;
using SA.Android.Content.Pm;

namespace SA.CrossPlatform.App
{

    internal class UM_AndroidBuildInfo : UM_AbstractBuildInfo, UM_iBuildInfo
    {

        public override string Version {
            get {
                var pm = AN_MainActivity.Instance.GetPackageManager();
                AN_PackageInfo packageInfo = pm.GetPackageInfo(Identifier, 0);

                return packageInfo.VersionName;
            }
        }
    }
}