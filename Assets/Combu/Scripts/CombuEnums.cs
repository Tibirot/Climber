using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * All enum types used in Combu namespace are defined here
 **/
namespace Combu
{
    /// <summary>
    /// Contact type.
    /// </summary>
    public enum eContactType : int
    {
        PendingRequest = -1, // requests sent by localUser
        Friend = 0, // friends of localUser
        Request, // requests sent to localUser
        Ignore, // ignored by localUser

        MinValue = Friend, // Min valid value as eContactType of users
        MaxValue = Ignore // Max valid value as eContactType of users
    }

    /// <summary>
    /// Mail list.
    /// </summary>
    public enum eMailList : int
    {
        Received = 0,
        Sent,
        Both,
        Unread
    }

    /// <summary>
    /// Custom search operator.
    /// </summary>
    public enum eSearchOperator
    {
        Equals,
        Disequals,
        Like,
        Greater,
        GreaterOrEquals,
        Lower,
        LowerOrEquals
    }

    /// <summary>
    /// Leaderboard interval.
    /// </summary>
    public enum eLeaderboardInterval : int
    {
        Total = 0,
        Month,
        Week,
        Today
    }

    /// <summary>
    /// Leaderboard time scope.
    /// </summary>
    public enum eLeaderboardTimeScope : int
    {
        None,
        Month
    }
}
