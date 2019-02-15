using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using SA.CrossPlatform.GameServices;

public class UM_GameServiceLeaderboardsExample : MonoBehaviour
{



    [SerializeField] Button m_nativeUI;
    [SerializeField] Button m_load;


    [SerializeField] Button m_submit;



    private void Start() {


        m_nativeUI.onClick.AddListener(() => {

            var client = UM_GameService.LeaderboardsClient;
            var leaderboardId = "YOUR_LEADERBOARD_ID_HERE";
            client.ShowUI(leaderboardId, UM_LeaderboardTimeSpan.Weekly, (result) => {
                if(result.IsSucceeded) {
                    Debug.Log("User closed Leaderboards native view");
                } else {
                    Debug.Log("Failed to start Leaderboards native view: " + result.Error.FullMessage);
                }
            });
        });


        m_submit.onClick.AddListener(() => {

            var client = UM_GameService.LeaderboardsClient;

            //The identifier for the leaderboard.
            var leaderboardId = "YOUR_LEADERBOARD_ID_HERE";

            /// The score earned by the player.
            /// You can use any algorithm you want to calculate scores in your game. 
            /// Your game must set the value property before reporting a score, otherwise an error is returned.
            /// The value provided by a score object is interpreted by Game service provided only when formatted for display.
            int score = 250;

            /// An integer value used by your game.
            /// 
            /// The context property is stored and returned to your game, 
            /// but is otherwise ignored by Game Center. 
            /// It allows your game to associate an arbitrary 64-bit unsigned integer value 
            /// with the score data reported to Game Center. 
            /// You decide how this integer value is interpreted by your game. 
            /// For example, you might use the context property 
            /// to store flags that provide game-specific details about a player’s score, 
            /// or you might use the context as a key to other data stored on the device or on your own server. 
            /// The context is most useful when your game displays a custom leaderboard user interface.
            int context = 0;

            client.SubmitScore(leaderboardId, score, context, (result) => {
                if(result.IsSucceeded) {
                    Debug.Log("Score submitted successfully");
                } else {
                    Debug.Log("Failed to submit score: " + result.Error.FullMessage);
                }
            });
        });


      


    }

    private void LoadMeta() {
        var client = UM_GameService.LeaderboardsClient;

        client.LoadLeaderboardsMetadata((result) => {
            if(result.IsSucceeded) {
                foreach(var leaderboard in result.Leaderboards) {
                    Debug.Log("leaderboard.Identifier: " + leaderboard.Identifier);
                    Debug.Log("leaderboard.Title: " + leaderboard.Title);
                }
            } else {
                Debug.Log("Failed to load leaderboards metadata " + result.Error.FullMessage);
            }
        });
    }

    private void LoadPlayerScore() {

        var client = UM_GameService.LeaderboardsClient;
        //The identifier for the leaderboard.
        var leaderboardId = "YOUR_LEADERBOARD_ID_HERE";

        //The period of time to which a player’s best score is restricted.
        var span = UM_LeaderboardTimeSpan.AllTime;

        //The scope of players to be searched for scores.
        var collection = UM_LeaderboardCollection.Public;

        client.LoadCurrentPlayerScore(leaderboardId, span, collection, (result) => {
            if (result.IsSucceeded) {
                UM_iScore score = result.Score;
                Debug.Log("score.Value: " + score.Value);
                Debug.Log("score.Rank: " + score.Rank);
                Debug.Log("score.Context: " + score.Context);
                Debug.Log("score.Date: " + score.Date); 
            } else {
                Debug.Log("Failed to load player score " + result.Error.FullMessage);
            }
        });
    }
}
