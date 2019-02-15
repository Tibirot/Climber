using UnityEngine;
using System;
using System.Collections;

using SA.Android.GMS.Games;

namespace SA.CrossPlatform.GameServices
{

    [Serializable]
    internal class UM_AndroidSavedGameMetadata : UM_iSavedGameMetadata
    {
        private AN_SnapshotMetadata meta;

        public UM_AndroidSavedGameMetadata(AN_SnapshotMetadata an_metadata) {
            meta = an_metadata;
        }

        public string Name {
            get {
                return meta.Title;
            }
        }

        public string DeviceName {
            get {
                return meta.DeviceName;
            }
        }


        public AN_SnapshotMetadata NativeMeta {
            get {
                return meta;
            }
        }

       
    }
}