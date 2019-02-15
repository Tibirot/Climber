using UnityEngine;
using System;
using System.Collections;

using SA.iOS.GameKit;


namespace SA.CrossPlatform.GameServices
{

    [Serializable]
    internal class UM_IOSSavedGameMetadata : UM_iSavedGameMetadata
    {
        private ISN_GKSavedGame meta;

        public UM_IOSSavedGameMetadata(ISN_GKSavedGame isn_metadata) {
            meta = isn_metadata;
        }

        public string Name {
            get {
                return meta.Name;
            }
        }

        public string DeviceName {
            get {
                return meta.DeviceName;
            }
        }

        public ISN_GKSavedGame NativeMeta {
            get {
                return meta;
            }
        }


    }
}