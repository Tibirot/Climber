
using System;
using UnityEngine;
using SA.Foundation.Templates;

namespace SA.CrossPlatform.GameServices
{

    /// <summary>
    /// A client to interact with leaderboards functionality.
    /// </summary>
    public interface UM_iLeaderboardsClient
    {
        
      
        /// <summary>
        /// Show the native UI with the list of leaderboards for a game.
        /// </summary>
        /// <param name="callback">Operation callback.</param>
        void ShowUI(Action<SA_Result> callback);


        /// <summary>
        /// Show the native UI of the specified leaderboard.
        /// </summary>
        /// <param name="leaderboardId">The Id of the leaderboard to view.</param>
        /// <param name="callback">Operation callback.</param>
        void ShowUI(string leaderboardId, Action<SA_Result> callback);


        /// <summary>
        /// Show the native UI of the specified leaderboard and time span page.
        /// </summary>
        /// <param name="leaderboardId">The Id of the leaderboard to view.</param>
        /// <param name="timeSpan">ime span to retrieve data for.</param>
        /// <param name="callback">Operation callback.</param>
        void ShowUI(string leaderboardId, UM_LeaderboardTimeSpan timeSpan, Action<SA_Result> callback);


        /// <summary>
        /// Submits the score to the game service.
        /// Use this method whenever you need to submit scores.
        /// </summary>
        /// <param name="leaderboardId">The identifier for the leaderboard.</param>
        /// <param name="score">
        /// The score earned by the player.
        /// You can use any algorithm you want to calculate scores in your game. 
        /// Your game must set the value property before reporting a score, otherwise an error is returned.
        /// The value provided by a score object is interpreted by Game service provided only when formatted for display.
        /// </param>
        /// <param name="context">
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
        /// </param>
        /// <param name="callback">Operation callback.</param>
        void SubmitScore(string leaderboardId, long score, int context, Action<SA_Result> callback);


        /// <summary>
        /// Asynchronously loads game registred leaderboards metadata
        /// </summary>
        void LoadLeaderboardsMetadata(Action<UM_LoadLeaderboardsMetaResult> callback);


        /// <summary>
        /// Asynchronously loads <see cref="UM_Score"/> 
        /// that represents the signed-in player's score for the leaderboard specified by leaderboardId. 
        /// </summary>
        /// <param name="leaderboardId">The ID of the leaderboard to view.</param>
        /// <param name="span">Time span to retrieve data for. </param>
        /// <param name="leaderboardCollection">The collection to show by default. </param>
        /// <param name="callback">Request async callback</param>
        void LoadCurrentPlayerScore(string leaderboardId, UM_LeaderboardTimeSpan span, UM_LeaderboardCollection collection, Action<UM_ScoreLoadResult> callback);
    }
}