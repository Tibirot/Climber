using System;
using System.Collections.Generic;
using UnityEngine;

using SA.Android.GMS.Games;

namespace SA.CrossPlatform.GameServices
{
    [Serializable]
    internal class UM_AndroidAchievement : UM_AbstractAchievement, UM_iAchievement
    {

        public UM_AndroidAchievement(AN_Achievement achievement) {
            m_identifier = achievement.AchievementId;
            m_name = achievement.Name;
            m_currentSteps = achievement.CurrentSteps;
            m_totalSteps = achievement.TotalSteps;

            m_type = (UM_AchievementType) achievement.Type;
            m_state = (UM_AchievementState) achievement.State;
        }
    }
}