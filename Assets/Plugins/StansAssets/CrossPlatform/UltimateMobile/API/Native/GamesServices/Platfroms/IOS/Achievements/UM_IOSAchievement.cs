using System;
using System.Collections.Generic;
using UnityEngine;

using SA.iOS.GameKit;

namespace SA.CrossPlatform.GameServices
{
    [Serializable]
    internal class UM_IOSAchievement : UM_AbstractAchievement, UM_iAchievement
    {

        public UM_IOSAchievement(ISN_GKAchievement achievement) {
            m_identifier = achievement.Identifier;
            m_name = achievement.Name;
            m_currentSteps = Mathf.RoundToInt(achievement.PercentComplete);
            m_totalSteps = 100;
            m_type = UM_AchievementType.INCREMENTAL;

            if(achievement.Completed) {
                m_state = UM_AchievementState.UNLOCKED;
            } else {
                m_state = UM_AchievementState.REVEALED;
            }
        }
    }
}