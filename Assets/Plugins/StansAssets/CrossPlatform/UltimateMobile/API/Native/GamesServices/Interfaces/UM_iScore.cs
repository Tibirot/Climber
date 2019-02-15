using System;


namespace SA.CrossPlatform.GameServices
{
    /// <summary>
    /// An object containing information for a score that was earned by the player.
    /// </summary>
    public interface UM_iScore
    {
        /// <summary>
        /// The position of the score in the results of a leaderboard search.
        /// </summary>
        long Rank { get; }


        /// <summary>
        /// The score earned by the player.
        /// </summary>
        long Value { get; }


        /// <summary>
        /// An integer value used by your game.
        /// </summary>
        int Context { get; }

        /// <summary>
        /// The date and time when the score was earned.
        /// </summary>
        DateTime Date { get; }


        /// <summary>
        /// The <see cref="Date"/> field value as unix time stamp
        /// </summary>
        long DateUnix { get; }
    }
}