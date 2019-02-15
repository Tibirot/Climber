using UnityEngine;
using System;
using System.Collections.Generic;

using SA.Foundation.Async;
using SA.Foundation.Templates;
using SA.Foundation.Time;


namespace SA.CrossPlatform.GameServices
{
    internal class UM_EditorLeaderboardsClient : UM_AbstractLeaderboardsClient, UM_iLeaderboardsClient
    {
       
        public void ShowUI(Action<SA_Result> callback) {
            callback.Invoke(new SA_Result());
        }

        public void ShowUI(string leaderboardId, Action<SA_Result> callback) {
            callback.Invoke(new SA_Result());
        }

        public void ShowUI(string leaderboardId, UM_LeaderboardTimeSpan timeSpan, Action<SA_Result> callback) {
            callback.Invoke(new SA_Result());
        }


        public void SubmitScore(string leaderboardId, long score, int context, Action<SA_Result> callback) {

            UM_EditorAPIEmulator.WaitForNetwork(() => {
                UM_Score um_score = new UM_Score(score, 10, context, SA_Unix_Time.ToUnixTime(DateTime.Now));
                UM_EditorAPIEmulator.SetString(leaderboardId, JsonUtility.ToJson(um_score));
                callback.Invoke(new SA_Result());
            });
        }


        public void LoadLeaderboardsMetadata(Action<UM_LoadLeaderboardsMetaResult> callback) {
            UM_EditorAPIEmulator.WaitForNetwork(() => {

                List<UM_iLeaderboard> um_leaderboards = new List<UM_iLeaderboard>();
                foreach(var um_leaderboard in UM_Settings.Instance.GSLeaderboards) {
                    um_leaderboards.Add(um_leaderboard);
                }
                var um_result = new UM_LoadLeaderboardsMetaResult(um_leaderboards);
                callback.Invoke(um_result);
            });
        }

        public void LoadCurrentPlayerScore(string leaderboardId, UM_LeaderboardTimeSpan span, UM_LeaderboardCollection collection, Action<UM_ScoreLoadResult> callback) {

            UM_EditorAPIEmulator.WaitForNetwork(() => {
                if(UM_EditorAPIEmulator.HasKey(leaderboardId)) {
                    string json = UM_EditorAPIEmulator.GetString(leaderboardId); 
                    UM_Score um_score = JsonUtility.FromJson<UM_Score>(json);
                    callback.Invoke(new UM_ScoreLoadResult(um_score));
                } else {
                    var error = new SA_Error(100, "Leaderboard with id: " + leaderboardId + " does not have any scores yet.");
                    callback.Invoke(new UM_ScoreLoadResult(error));
                }
               
            });
        }


    }
}