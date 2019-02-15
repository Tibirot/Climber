using UnityEngine;
using System.Collections;


namespace SA.CrossPlatform.GameServices
{

    /// <summary>
    /// The period of time to which a player’s best score is restricted.
    /// </summary>
    public enum UM_LeaderboardTimeSpan
    {
        /// <summary>
        /// Scores are reset every day. The reset occurs at 11:59PM PST.
        /// </summary>
        Daily = 0,

        /// <summary>
        /// Scores are reset once per week. The reset occurs at 11:59PM PST on Sunday.
        /// </summary>
        Weekly = 1,

        /// <summary>
        /// Scores are never reset.
        /// </summary>
        AllTime = 2
    }
}