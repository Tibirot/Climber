using UnityEngine;


using SA.CrossPlatform.GameServices;

public class UM_GameServiceSavedGamesExample : MonoBehaviour
{
  

   public void Test() {

        var client = UM_GameService.SavedGamesClient;
        client.FetchSavedGames((result) => {
            if(result.IsSucceeded) {
                foreach(var snapshot in result.Snapshots) {
                    Debug.Log("snapshot.Name: " + snapshot.Name);
                    Debug.Log("snapshot.DeviceName: " + snapshot.DeviceName);
                }
            } else {
                Debug.LogError("FetchSavedGames failed: " + result.Error.FullMessage);
            }
        });
   }


    public void LoadGame(UM_iSavedGameMetadata game) {
        var client = UM_GameService.SavedGamesClient;
        client.LoadGameData(game, (result) => {
            if (result.IsSucceeded) {
                UM_SavedGameData data = result.SavedGame;
                Debug.Log("Data size (bytes): " + data.Data.Length);
                //Restore your game progress here
            } else {
                Debug.LogError("Failed to load saved game data: " + result.Error.FullMessage);
            }
        });
    }

    public void SaveGame(byte[] gameData) {
        var client = UM_GameService.SavedGamesClient;
        UM_SavedGameData data = new UM_SavedGameData(gameData);
        client.SaveGame("MySave", data, (result) => {
            if(result.IsSucceeded) {
                Debug.Log("Game saved");
            } else {
                Debug.Log("Failed to save: " + result.Error.FullMessage);
            }
        });
    }


    public void DeleteGameSave(UM_iSavedGameMetadata game) {
        var client = UM_GameService.SavedGamesClient;
        client.Delete(game, (result) => {
            if (result.IsSucceeded) {
                Debug.Log("Game Deleted");
            } else {
                Debug.Log("Failed to delete: " + result.Error.FullMessage);
            }
        });
    }


}