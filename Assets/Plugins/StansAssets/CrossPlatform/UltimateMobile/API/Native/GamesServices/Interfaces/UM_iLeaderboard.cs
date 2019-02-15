using UnityEngine;
using System.Collections;


namespace SA.CrossPlatform.GameServices
{

    /// <summary>
    /// Data interface for leaderboard metadata.
    /// </summary>
    public interface UM_iLeaderboard
    {

        /// <summary>
        /// Retrieves the Identifier of this leaderboard.
        /// </summary>
        string Identifier { get; }

        /// <summary>
        /// The localized title for the leaderboard.
        /// </summary>
        string Title { get; }
    }
}