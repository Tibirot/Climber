using UnityEngine;
using System;
using System.Collections.Generic;

using SA.Android.App;
using SA.Android.GMS.Games;
using SA.Foundation.Templates;

namespace SA.CrossPlatform.GameServices
{
    internal class UM_AndroidLeaderboardsClient : UM_AbstractLeaderboardsClient, UM_iLeaderboardsClient
    {

        public void ShowUI(Action<SA_Result> callback) {
            var client = AN_Games.GetLeaderboardsClient();
            client.GetAllLeaderboardsIntent((result) => {
                if (result.IsSucceeded) {
                    var intent = result.Intent;
                    AN_ProxyActivity proxy = new AN_ProxyActivity();
                    proxy.StartActivityForResult(intent, (intentResult) => {
                        proxy.Finish();
                        callback.Invoke(intentResult); 
                    });

                } else {
                    callback.Invoke(result);  
                }
            });
        }

        public void ShowUI(string leaderboardId, Action<SA_Result> callback) {
            ShowUI(leaderboardId, UM_LeaderboardTimeSpan.AllTime, callback);
        }

        public void ShowUI(string leaderboardId, UM_LeaderboardTimeSpan timeSpan, Action<SA_Result> callback) {

            AN_Leaderboard.TimeSpan span = AN_Leaderboard.TimeSpan.AllTime;

            switch (timeSpan) {
                case UM_LeaderboardTimeSpan.AllTime:
                    span = AN_Leaderboard.TimeSpan.AllTime;
                    break;
                case UM_LeaderboardTimeSpan.Daily:
                    span = AN_Leaderboard.TimeSpan.Daily;
                    break;
                case UM_LeaderboardTimeSpan.Weekly:
                    span = AN_Leaderboard.TimeSpan.Weekly;
                    break;
            }

            var client = AN_Games.GetLeaderboardsClient();
            client.GetLeaderboardIntent(leaderboardId, span, (result) => {
                if (result.IsSucceeded) {
                    var intent = result.Intent;
                    AN_ProxyActivity proxy = new AN_ProxyActivity();
                    proxy.StartActivityForResult(intent, (intentResult) => {
                        proxy.Finish();
                        callback.Invoke(intentResult);
                    });

                } else {
                    callback.Invoke(result);
                }
            });
        }


        public void SubmitScore(string leaderboardId, long score, int context, Action<SA_Result> callback) {
            var client = AN_Games.GetLeaderboardsClient();
            client.SubmitScoreImmediate(leaderboardId, score, context.ToString(), (result) => {
                ReportScoreSubmited(leaderboardId, score, result);
                callback.Invoke(result);
            });
        }

        public void LoadLeaderboardsMetadata(Action<UM_LoadLeaderboardsMetaResult> callback) {
            var leaderboards = AN_Games.GetLeaderboardsClient();
            leaderboards.LoadLeaderboardMetadata(false, (result) => {
                UM_LoadLeaderboardsMetaResult um_result;
                if (result.IsSucceeded) {
                    List<UM_iLeaderboard> um_leaderboards = new List<UM_iLeaderboard>();
                    foreach (var leaderboard in result.Leaderboards) {
                        var um_leaderboardMetda = new UM_LeaderboardMeta(leaderboard.LeaderboardId, leaderboard.DisplayName);
                        um_leaderboards.Add(um_leaderboardMetda);
                    }

                    um_result = new UM_LoadLeaderboardsMetaResult(um_leaderboards);
                } else {
                    um_result = new UM_LoadLeaderboardsMetaResult(result.Error);
                }

                callback.Invoke(um_result);
            });
        }

        public void LoadCurrentPlayerScore(string leaderboardId, UM_LeaderboardTimeSpan span, UM_LeaderboardCollection collection, Action<UM_ScoreLoadResult> callback) {
            var leaderboards = AN_Games.GetLeaderboardsClient();


            AN_Leaderboard.TimeSpan an_timeSpan = AN_Leaderboard.TimeSpan.AllTime;
            switch (span) {
                case UM_LeaderboardTimeSpan.AllTime:
                    an_timeSpan = AN_Leaderboard.TimeSpan.AllTime;
                    break;
                case UM_LeaderboardTimeSpan.Weekly:
                    an_timeSpan = AN_Leaderboard.TimeSpan.Weekly;
                    break;
                case UM_LeaderboardTimeSpan.Daily:
                    an_timeSpan = AN_Leaderboard.TimeSpan.Daily;
                    break;
            }

            AN_Leaderboard.Collection an_collection = AN_Leaderboard.Collection.Public;
            switch (collection) {
                case UM_LeaderboardCollection.Public:
                    an_collection = AN_Leaderboard.Collection.Public;
                    break;
                case UM_LeaderboardCollection.Social:
                    an_collection = AN_Leaderboard.Collection.Social;
                    break;
            }



            leaderboards.LoadCurrentPlayerLeaderboardScore(leaderboardId, an_timeSpan, an_collection, (res) => {

                UM_ScoreLoadResult um_result;
                if (res.IsSucceeded) {
                    AN_LeaderboardScore an_score = res.Data;

                    int context;
                    try {
                        context = Convert.ToInt32(an_score.ScoreTag);
                    } catch(Exception ex) {
                        Debug.LogWarning("Failed to convert anroid score tag to int. leaderboardId: " + leaderboardId + " error: " + ex.Message);
                        context = 0;
                    }

                    UM_Score score = new UM_Score(an_score.RawScore,
                        an_score.Rank,
                        context,
                        an_score.TimestampMillis);

                    um_result = new UM_ScoreLoadResult(score);
                } else {
                    um_result = new UM_ScoreLoadResult(res.Error);
                }

                callback.Invoke(um_result);
            });
        }

    }
}