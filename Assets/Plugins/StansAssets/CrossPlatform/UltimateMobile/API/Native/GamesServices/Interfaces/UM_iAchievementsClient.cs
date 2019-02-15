using System;
using SA.Foundation.Templates;


namespace SA.CrossPlatform.GameServices
{

    /// <summary>
    /// A client to interact with achievements functionality.
    /// </summary>
    public interface UM_iAchievementsClient
    {

        /// <summary>
        /// Show the native UI with the list of achievements for a game.
        /// </summary>
        void ShowUI();

        /// <summary>
        /// Retrive's the achievements info
        /// </summary>
        /// <param name="callback"></param>
        void Load(Action<UM_AchievementsLoadResult> callback);

        /// <summary>
        /// Asynchronously reveals a hidden achievement to the currently signed in player. 
        /// If the achievement is already visible, this will have no effect.
        /// </summary>
        /// <param name="achievementId">The achievement ID to reveal.</param>
        /// <param name="callback">Result callback</param>
        void Reveal(string achievementId, Action<SA_Result> callback);


        /// <summary>
        /// Asynchronously unlocks an achievement for the currently signed in player. 
        /// If the achievement is hidden this will reveal it to the player.
        /// </summary>
        /// <param name="achievementId">The achievement ID to unlock.</param>
        /// <param name="callback">Result callback</param>
        void Unlock(string achievementId, Action<SA_Result> callback);


        /// <summary>
        /// Increments an achievement by the given number of steps. 
        /// The achievement must be an incremental achievement. 
        /// Once an achievement reaches at least the maximum number of steps, it will be unlocked automatically. 
        /// Any further increments will be ignored.
        /// </summary>
        /// <param name="achievementId">The achievement ID to increment.</param>
        /// <param name="numSteps">The number of steps to increment by. Must be greater than 0.</param>
        void Increment(string achievementId, int numSteps, Action<SA_Result> callback);

    }
}