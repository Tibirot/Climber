using UnityEngine;
using System.Collections;


namespace SA.CrossPlatform.App
{

    internal abstract class UM_AbstractBuildInfo 
    {

        public virtual string Identifier {
            get {
                return Application.identifier;
            }
        }

        public virtual string Version {
            get {
                return Application.version;
            }
        }

    }
}