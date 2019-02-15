using System;
using System.Collections.Generic;
using UnityEngine;

using SA.Android.GMS.Games;
using SA.Foundation.Templates;


namespace SA.CrossPlatform.GameServices
{
    /// <summary>
    /// A client to interact with saved games functionality.
    /// </summary>
    internal class UM_AndroidSavedGamesClient : UM_AbstractSavedGamesClient, UM_iSavedGamesClient
    {
        public void FetchSavedGames(Action<UM_SavedGamesMetadataResult> callback) {
            var client = AN_Games.GetSnapshotsClient();
            client.Load((result) => {

            UM_SavedGamesMetadataResult loadResult;

            if (result.IsSucceeded) {
                    loadResult = new UM_SavedGamesMetadataResult();
                    foreach (var meta in result.Snapshots) {
                        var an_meta = new UM_AndroidSavedGameMetadata(meta);
                        loadResult.AddMetadata(an_meta);
                    }
                } else {
                    loadResult = new UM_SavedGamesMetadataResult(result.Error);
                }


                callback.Invoke(loadResult);
            });
        }

        public void Delete(UM_iSavedGameMetadata game, Action<SA_Result> callback) {
            UM_AndroidSavedGameMetadata an_meta = (UM_AndroidSavedGameMetadata)game;

            var client = AN_Games.GetSnapshotsClient();
            client.Delete(an_meta.NativeMeta, (result) => {
                callback.Invoke(result);
            });
        }


        public void SaveGame(string name, UM_SavedGameData save, Action<SA_Result> callback) {
            var client = AN_Games.GetSnapshotsClient();
            bool createIfNotFound = true;

            //This resilution is picked since this is the only we can currently implement for iOS
            //so we pick same one for android, just to be consistent.
            var conflictPolicy = AN_SnapshotsClient.ResolutionPolicy.MOST_RECENTLY_MODIFIED;

            client.Open(name, createIfNotFound, conflictPolicy, (result) => {
                if (result.IsSucceeded) {
                    AN_Snapshot snapshot = result.Data;

                    snapshot.WriteBytes(save.Data);
                    client.CommitAndClose(snapshot, AN_SnapshotMetadataChange.EMPTY_CHANGE, (commitResult) => {
                        ReportGameSave(name, result);
                        callback.Invoke(result);
                    });

                } else {
                    callback.Invoke(result);
                }
            });
        }

        public void LoadGameData(UM_iSavedGameMetadata game, Action<UM_SavedGameDataLoadResult> callback) {
            var client = AN_Games.GetSnapshotsClient();

            bool createIfNotFound = true;
            var conflictPolicy = AN_SnapshotsClient.ResolutionPolicy.LAST_KNOWN_GOOD;

            client.Open(game.Name, createIfNotFound, conflictPolicy, (result) => {

                UM_SavedGameDataLoadResult loadResult;
                if (result.IsSucceeded) {
                    AN_Snapshot snapshot = result.Data;
                    byte[] data = snapshot.ReadFully();


                    client.CommitAndClose(snapshot, AN_SnapshotMetadataChange.EMPTY_CHANGE, (commitResult) => {
                        if (commitResult.IsSucceeded) {
                            UM_SavedGameData savedGame = new UM_SavedGameData(data);
                            loadResult = new UM_SavedGameDataLoadResult(savedGame);
                        } else {
                            loadResult = new UM_SavedGameDataLoadResult(commitResult.Error);
                        }

                        callback.Invoke(loadResult);
                    });

                } else {
                    loadResult = new UM_SavedGameDataLoadResult(result.Error);
                    callback.Invoke(loadResult);
                }
            });
        }

       
    }
}