using UnityEngine;
using System;
using System.Collections;


namespace SA.CrossPlatform.GameServices
{
    public interface UM_iSavedGameMetadata 
    {

        /// <summary>
        /// The name of the saved game.
        /// You can allow users to name their own saved games, or you can create a saved game name automatically.
        /// </summary>
        string Name { get; }


        /// <summary>
        /// Retrieves the name of the device that wrote this snapshot, if known.
        /// </summary>
        string DeviceName { get; }
    }
}