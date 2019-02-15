using System;
using System.Collections.Generic;
using UnityEngine;
using SA.iOS.GameKit;
using SA.Foundation.Templates;

namespace SA.CrossPlatform.GameServices
{ 

    internal class UM_IOSAchievementsClient : UM_AbstractAchievementsClient, UM_iAchievementsClient
    {

        public void ShowUI() {
            ISN_GKGameCenterViewController viewController = new ISN_GKGameCenterViewController();
            viewController.ViewState = ISN_GKGameCenterViewControllerState.Achievements;
            viewController.Show();
        }


        public void Load(Action<UM_AchievementsLoadResult> callback) {

            Dictionary<string, ISN_GKAchievement> achievementsDict = new Dictionary<string, ISN_GKAchievement>();

            foreach(var achievement in ISN_GKAchievement.GetGameAchievements()) {
                achievementsDict.Add(achievement.Identifier, achievement);
            }


            ISN_GKAchievement.LoadAchievements((result) => {
                UM_AchievementsLoadResult loadResult;
                if (result.IsSucceeded) {

                    foreach (var achievement in result.Achievements) {
                        if (achievementsDict.ContainsKey(achievement.Identifier)) {
                            string name = achievementsDict[achievement.Identifier].Name;
                            achievement.Name = name;
                            achievementsDict[achievement.Identifier] = achievement;
                        } else {
                            achievementsDict.Add(achievement.Identifier, achievement);
                        }
                    }

                    var achievements = new List<UM_iAchievement>();
                    foreach(var pair in achievementsDict) {
                        var achievement = pair.Value;
                        var iosAchievement = new UM_IOSAchievement(achievement);
                        SetAchievementCahce(iosAchievement);
                        achievements.Add(iosAchievement);
                    }

                   
                    loadResult = new UM_AchievementsLoadResult(achievements);
                } else {
                    loadResult = new UM_AchievementsLoadResult(result.Error);
                }
                callback.Invoke(loadResult);
            });
        }


        public void Reveal(string achievementId, Action<SA_Result> callback) {
           //Always revealed on iOS
        }

        public void Unlock(string achievementId, Action<SA_Result> callback) {

            ISN_GKAchievement achievement = new ISN_GKAchievement(achievementId);
            achievement.PercentComplete = 100.0f;
            achievement.Report((result) => {
                if (result.IsSucceeded) {
                    UnlockLocally(achievementId);
                }

                callback.Invoke(result);
            });
        }

        public void Increment(string achievementId, int numSteps, Action<SA_Result> callback) {

            //We need to caclulate PercentComplete. 
            //Let's see if we have an achivement info already
            UM_iAchievement achievement = GetAchievementById(achievementId);
            if (achievement != null) {
                Increment(achievement, numSteps, callback);
            } else {
                Load((result) => {
                    if(result.IsSucceeded) {
                        //It should be cached at this point already
                        achievement = GetAchievementById(achievementId);
                        if(achievement == null) {

                            ISN_GKAchievement iSN_achievement = new ISN_GKAchievement(achievementId);
                            var iosAchievement = new UM_IOSAchievement(iSN_achievement);
                            SetAchievementCahce(iosAchievement);

                            achievement = iosAchievement;
                        }

                        Increment(achievement, numSteps, callback);
                    } else {
                        var error = new SA_Error(100, "Wasn't able to load achievement with id: " + achievementId);
                        callback.Invoke(new SA_Result(error));
                    }
                });
            }

        }

        private void Increment(UM_iAchievement achievement, int numSteps, Action<SA_Result> callback) {
            ISN_GKAchievement iosAchievement = new ISN_GKAchievement(achievement.Identifier);
            int progress = achievement.CurrentSteps + numSteps;
            iosAchievement.PercentComplete = progress;

            iosAchievement.Report((result) => {
                if(result.IsSucceeded) {
                    IncrementLocally(achievement.Identifier, numSteps);
                }
                callback.Invoke(result);
            });
        }

    }
}