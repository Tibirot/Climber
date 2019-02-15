using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SA.CrossPlatform.GameServices
{
    public interface UM_iAchievement
    {

        /// <summary>
        /// The achievement ID.
        /// </summary>
        string Identifier { get; }

        /// <summary>
        /// Retrieves the name of this achievement.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Retrieves the number of steps this user has gone toward unlocking this achievement; 
        /// only applicable for <see cref="UM_AchievementType.INCREMENTAL"/> achievement types.
        /// </summary>
        int CurrentSteps { get; }

        /// <summary>
        /// Retrieves the total number of steps necessary to unlock this achievement; 
        /// only applicable for <see cref="UM_AchievementType.INCREMENTAL"/> achievement types.
        /// </summary>
        int TotalSteps { get; }

        /// <summary>
        /// Returns the Type of this achievement.
        /// </summary>
        UM_AchievementType Type { get; }

        /// <summary>
        /// Returns the State of the achievement.
        /// </summary>
        UM_AchievementState State { get; }
    }
}