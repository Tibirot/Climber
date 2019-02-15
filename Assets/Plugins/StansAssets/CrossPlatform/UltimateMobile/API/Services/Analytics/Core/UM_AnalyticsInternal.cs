using UnityEngine;
using System.Collections.Generic;
using System;
using SA.CrossPlatform.GameServices;
using SA.Foundation.Templates;
using SA.CrossPlatform.InApp;

namespace SA.CrossPlatform.Analytics
{
    internal static class UM_AnalyticsInternal
    {

        public static void Init() {
            Application.logMessageReceived += HandleLog;
        }

        //--------------------------------------
        // General
        //--------------------------------------

        private static void HandleLog(string condition, string stackTrace, LogType type) {

            if (!UM_AnalyticsService.Client.IsInitialized) { return; }

            if (type == LogType.Exception) {
                var data = new Dictionary<string, object>();
                data.Add("Error", condition);
                data.Add("Stack", stackTrace);

                UM_AnalyticsService.Client.Event("Exception", data);
            }
        }


        //--------------------------------------
        // Game Service API
        //--------------------------------------


        internal static void OnPlayerUpdated(UM_PlayerInfo info) {
            if (!UM_AnalyticsService.Client.IsInitialized) { return;}
            if (!Settings.PlayerIdTracking) { return; }

            if(info.State == UM_PlayerState.SignedIn) {
                UM_AnalyticsService.Client.SetUserId(info.Player.Id);
            }
        }

        internal static void OnAchievementUpdated(UM_AbstractAchievement achievement) {
            if (!UM_AnalyticsService.Client.IsInitialized) { return; }
            if (!Settings.Achievements) { return; }

            var data = new Dictionary<string, object>();
            data.Add("AchievementId", achievement.Identifier);
            data.Add("State", achievement.State.ToString());

            if(achievement.State == UM_AchievementState.REVEALED) {
                data.Add("CurrentSteps", achievement.CurrentSteps);
            }

            UM_AnalyticsService.Client.Event("Achievement", data);
        }


        internal static void OnGameSave(string name, SA_Result result) {
            if (!UM_AnalyticsService.Client.IsInitialized) { return; }
            if (!Settings.GameSaves) { return; }
            if (result.IsFailed) { return; }

            var data = new Dictionary<string, object>();
            data.Add("SaveName", name);

            UM_AnalyticsService.Client.Event("GameSave", data);
        }

        internal static void OnScoreSubmit(string leaderboardId, long score, SA_Result result) {
            if (!UM_AnalyticsService.Client.IsInitialized) { return; }
            if (!Settings.Scores) { return; }
            if (result.IsFailed) { return; }


            var data = new Dictionary<string, object>();
            data.Add("LeaderboardId", leaderboardId);
            data.Add("score", score);

            UM_AnalyticsService.Client.Event("Score", data);
        }


        //--------------------------------------
        // Vending 
        //--------------------------------------

        internal static void OnTransactionUpdated(UM_iTransaction transaction) {
            if (!UM_AnalyticsService.Client.IsInitialized) { return; }
           

            switch (transaction.State) {
                case UM_TransactionState.Failed:
                    if (!Settings.FailedTransactions) { return; }

                    Dictionary<string, object> data = new Dictionary<string, object>();
                    data.Add("TransactionId", transaction.Id);
                    data.Add("ProductId", transaction.ProductId);
                    data.Add("Error", transaction.Error.FullMessage);


                    UM_AnalyticsService.Client.Event("TransactionFailed", data);
                    break;
                case UM_TransactionState.Purchased:
                    if (!Settings.SuccessfulTransactions) { return; }

                    var product = UM_InAppService.Client.GetProductById(transaction.ProductId);
                    if(product != null) {
                        float price = (float) product.PriceInMicros / 1000000f;
                        UM_AnalyticsService.Client.Transaction(product.Id, price, product.PriceCurrencyCode);
                    }
                    break;

            }
        }

        internal static void OnRestoreTransactions() {
            if (!UM_AnalyticsService.Client.IsInitialized) { return; }
            if (!Settings.RestoreRequests) { return; }

            UM_AnalyticsService.Client.Event("RestoreTransactions");
        }


        //--------------------------------------
        // Private 
        //--------------------------------------


        private static UM_AnalyticsAutomationSettings Settings {
            get {
                return UM_Settings.Instance.Analytics.Automation;
            }
        }
    }
}