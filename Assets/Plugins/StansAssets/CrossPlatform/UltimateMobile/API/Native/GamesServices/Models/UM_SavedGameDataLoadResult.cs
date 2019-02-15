using UnityEngine;
using System;
using System.Collections;

using SA.Foundation.Templates;


namespace SA.CrossPlatform.GameServices
{

    /// <summary>
    /// Game load result object.
    /// </summary>
    [Serializable]
    public class UM_SavedGameDataLoadResult : SA_Result
    {
        [SerializeField] UM_SavedGameData m_savedGame;

        public UM_SavedGameDataLoadResult(UM_SavedGameData savedGame) {
            m_savedGame = savedGame;
        }

        public UM_SavedGameDataLoadResult(SA_Error error): base(error) { }



        /// <summary>
        /// Loaded game data.
        /// </summary>
        public UM_SavedGameData SavedGame {
            get {
                return m_savedGame;
            }
        }

    }
}