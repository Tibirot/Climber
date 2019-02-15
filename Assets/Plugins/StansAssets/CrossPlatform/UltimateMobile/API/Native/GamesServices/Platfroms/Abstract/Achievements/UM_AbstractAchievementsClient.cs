using UnityEngine;
using System.Collections.Generic;


using SA.CrossPlatform.Analytics;

namespace SA.CrossPlatform.GameServices
{
    internal class UM_AbstractAchievementsClient 
    {

        [SerializeField] Dictionary<string, UM_AbstractAchievement> m_achievementsCahce = new Dictionary<string, UM_AbstractAchievement>();


        protected void SetAchievementCahce(UM_AbstractAchievement achievement) {
           if(m_achievementsCahce.ContainsKey(achievement.Identifier)) {
                m_achievementsCahce[achievement.Identifier] = achievement;
            } else {
                m_achievementsCahce.Add(achievement.Identifier, achievement);
            }
        }

        protected void UnlockLocally(string id) {
            var achievement = GetAchievementById(id);
            if(achievement != null) {
                achievement.SetSate(UM_AchievementState.UNLOCKED);
                UM_AnalyticsInternal.OnAchievementUpdated(achievement);
            }
        }

        protected void RevealLocally(string id) {
            var achievement = GetAchievementById(id);
            if (achievement != null) {
                achievement.SetSate(UM_AchievementState.REVEALED);
                UM_AnalyticsInternal.OnAchievementUpdated(achievement);
            }
        }

        protected void IncrementLocally(string id, int numSteps) {
           
            var achievement = GetAchievementById(id);
            if (achievement != null) {
                int progress = achievement.CurrentSteps + numSteps;
                achievement.SetCurrentSteps(progress);
                if(achievement.CurrentSteps >= achievement.TotalSteps) {
                    achievement.SetSate(UM_AchievementState.UNLOCKED);
                }

                UM_AnalyticsInternal.OnAchievementUpdated(achievement);
            }
        }

 
        protected UM_AbstractAchievement GetAchievementById(string id) {
            if(m_achievementsCahce.ContainsKey(id)) {
                return m_achievementsCahce[id];
            } else {
                return null;
            }
        }

    }
}