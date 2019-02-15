using UnityEngine;
using System;
using System.Collections.Generic;

using SA.Foundation.Templates;

namespace SA.CrossPlatform.GameServices
{

    /// <summary>
    /// The result of Snapshots loading
    /// </summary>
    [Serializable]
    public class UM_SavedGamesMetadataResult : SA_Result
    {
        [SerializeField] List<UM_iSavedGameMetadata> m_metadataList = new List<UM_iSavedGameMetadata>();


        public UM_SavedGamesMetadataResult() : base() { }

        public UM_SavedGamesMetadataResult(SA_Error error): base(error) {}

        /// <summary>
        /// The Metadata Snapshots List
        /// </summary>
        public List<UM_iSavedGameMetadata> Snapshots {
            get {
                return m_metadataList;
            }
        }


        public void AddMetadata(UM_iSavedGameMetadata meta) {
            m_metadataList.Add(meta);
        }
    }
}