using System;
using System.Collections.Generic;
using UnityEngine;

using SA.Android.App;
using SA.Android.GMS.Games;

using SA.Foundation.Templates;

namespace SA.CrossPlatform.GameServices
{
    internal class UM_AndroidAchievementsClient : UM_AbstractAchievementsClient, UM_iAchievementsClient
    {

        public void ShowUI() {
            var client = AN_Games.GetAchievementsClient();
            client.GetAchievementsIntent((result) => {
                if (result.IsSucceeded) {
                    var intent = result.Intent;
                    AN_ProxyActivity proxy = new AN_ProxyActivity();
                    proxy.StartActivityForResult(intent, (intentResult) => {
                        proxy.Finish();
                    });
                } else {
                    Debug.LogError("Failed to Get Achievements Intent " + result.Error.FullMessage);
                }
            });
        }

        public void Load(Action<UM_AchievementsLoadResult> callback) {
            var client = AN_Games.GetAchievementsClient();
            client.Load(false, (result) => {

                UM_AchievementsLoadResult loadResult;
                if (result.IsSucceeded) {

                    var achievements = new List<UM_iAchievement>();
                    foreach (var achievement in result.Achievements) {
                        var androidAchievement = new UM_AndroidAchievement(achievement);
                        achievements.Add(androidAchievement);
                        SetAchievementCahce(androidAchievement);
                    }

                    loadResult = new UM_AchievementsLoadResult(achievements);
                } else {
                    loadResult = new UM_AchievementsLoadResult(result.Error);
                }

                callback.Invoke(loadResult);
            });
        }

        public void Reveal(string achievementId, Action<SA_Result> callback) {
            var client = AN_Games.GetAchievementsClient();
            client.RevealImmediate(achievementId, (result) => {
                if(result.IsSucceeded) {
                    RevealLocally(achievementId);
                }
                callback.Invoke(result);
            });
        }

        public void Unlock(string achievementId, Action<SA_Result> callback) {
            var client = AN_Games.GetAchievementsClient();
            client.UnlockImmediate(achievementId, (result) => {
                if (result.IsSucceeded) {
                    UnlockLocally(achievementId);
                }
                callback.Invoke(result);
            });
        }

        public void Increment(string achievementId, int numSteps, Action<SA_Result> callback) {
            var client = AN_Games.GetAchievementsClient();
            client.IncrementImmediate(achievementId, numSteps, (result) => {
                if(result.IsSucceeded) {
                    IncrementLocally(achievementId, numSteps);
                    callback.Invoke(new SA_Result());
                } else {
                    callback.Invoke(new SA_Result(result.Error));
                }
            });
        }

    }
}