using System;
using System.Collections.Generic;
using UnityEngine;

using SA.iOS.GameKit;

using SA.Foundation.Time;
using SA.Foundation.Templates;


namespace SA.CrossPlatform.GameServices
{
    /// <summary>
    /// A client to interact with saved games functionality.
    /// </summary>
    internal class UM_IOSSavedGamesClient : UM_AbstractSavedGamesClient, UM_iSavedGamesClient
    {


        //Automatic conflict resolution
        public UM_IOSSavedGamesClient() {
            ISN_GKLocalPlayerListener.HasConflictingSavedGames.AddListener(HasConflictingSavedGames);
        }


        private void HasConflictingSavedGames(ISN_GKSavedGameFetchResult result) {

            ISN_GKSavedGame resultGame = null;
            List<string> conflictedSavedGamesIds = new List<string>();
            foreach (ISN_GKSavedGame game in result.SavedGames) {
                conflictedSavedGamesIds.Add(game.Id);

                if(resultGame == null) {
                    resultGame = game;
                } else {

                    long gameUnixTime = SA_Unix_Time.ToUnixTime(game.ModificationDate);
                    long currentResultTime = SA_Unix_Time.ToUnixTime(resultGame.ModificationDate);
                    if(gameUnixTime > currentResultTime) {
                        resultGame = game; 
                    }
                }
            }

            ISN_GKLocalPlayer.LoadGameData(resultGame, (dataLoadResult) =>  {
                if(dataLoadResult.IsSucceeded) {
                    ISN_GKLocalPlayer.ResolveConflictingSavedGames(conflictedSavedGamesIds, dataLoadResult.BytesArrayData, (resResult) => {

                    });
                }
            });

        }

        public void FetchSavedGames(Action<UM_SavedGamesMetadataResult> callback) {
            ISN_GKLocalPlayer.FetchSavedGames((ISN_GKSavedGameFetchResult result) => {
                UM_SavedGamesMetadataResult loadResult;

                if (result.IsSucceeded) {
                    loadResult = new UM_SavedGamesMetadataResult();
                    foreach (ISN_GKSavedGame game in result.SavedGames) {
                        var isn_meta = new UM_IOSSavedGameMetadata(game);
                        loadResult.AddMetadata(isn_meta);
                    }
                } else {
                    loadResult = new UM_SavedGamesMetadataResult(result.Error);
                }

                callback.Invoke(loadResult);
            });
        }


        public void SaveGame(string name, UM_SavedGameData save, Action<SA_Result> callback) {
            ISN_GKLocalPlayer.SavedGame(name, save.Data, (ISN_GKSavedGameSaveResult result) => {
                ReportGameSave(name, result);
                callback.Invoke(result);
            });
        }

        public void LoadGameData(UM_iSavedGameMetadata game, Action<UM_SavedGameDataLoadResult> callback) {
            UM_IOSSavedGameMetadata isn_meta = (UM_IOSSavedGameMetadata)game;
            ISN_GKLocalPlayer.LoadGameData(isn_meta.NativeMeta, (ISN_GKSavedGameLoadResult result) => {

                UM_SavedGameDataLoadResult loadResult;
                if(result.IsSucceeded) {
                    UM_SavedGameData savedGame = new UM_SavedGameData(result.BytesArrayData);
                    loadResult = new UM_SavedGameDataLoadResult(savedGame);
                } else  {
                    loadResult = new UM_SavedGameDataLoadResult(result.Error);
                }

                callback.Invoke(loadResult);
            });
        }


       
        public void Delete(UM_iSavedGameMetadata game, Action<SA_Result> callback) {
            UM_IOSSavedGameMetadata isn_meta = (UM_IOSSavedGameMetadata) game;
            ISN_GKLocalPlayer.DeleteSavedGame(isn_meta.NativeMeta, callback);
        }

    }
}