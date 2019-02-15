using UnityEngine;
using System.Collections;

namespace SA.CrossPlatform.GameServices
{

    /// <summary>
    /// The scope of players to be searched for scores.
    /// </summary>
    public enum UM_LeaderboardCollection
    {
        /// <summary>
        /// All players on Game Center should be considered when generating the list of scores.
        /// </summary>
        Public = 0,


        /// <summary>
        /// Only friends of the local player should be considered when generating the list of scores.
        /// </summary>
        Social = 1
    }
}

