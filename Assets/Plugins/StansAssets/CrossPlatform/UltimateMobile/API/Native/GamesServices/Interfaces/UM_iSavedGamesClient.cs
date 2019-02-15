using System;
using SA.Foundation.Templates;

namespace SA.CrossPlatform.GameServices
{
    /// <summary>
    /// A client to interact with saved games functionality.
    /// </summary>
    public interface UM_iSavedGamesClient 
    {

        /// <summary>
        /// Retrieves all available saved games.
        /// </summary>
        void FetchSavedGames(Action<UM_SavedGamesMetadataResult> callback);


        /// <summary>
        /// Saves game data under the specified name.
        /// If save with such name alresy exists, save game data will be replaced.
        /// </summary>
        void SaveGame(string name, UM_SavedGameData save, Action<SA_Result> callback);


        /// <summary>
        /// Loads specific saved game data.
        /// </summary>
        void LoadGameData(UM_iSavedGameMetadata game, Action<UM_SavedGameDataLoadResult> callback);


        /// <summary>
        /// Deletes a specific saved game.
        /// </summary>
        void Delete(UM_iSavedGameMetadata game, Action<SA_Result> callback);
    }
}