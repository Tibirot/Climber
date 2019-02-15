using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA.CrossPlatform.GameServices
{

    /// <summary>
    /// Main entry point for the Game Services APIs. 
    /// This class provides APIs and interfaces to access the game services functionality.
    /// </summary>
    public static class UM_GameService 
    {
        
        private static UM_iSignInClient m_signInClient = null;
        private static UM_iAchievementsClient m_achievements = null;
        private static UM_iLeaderboardsClient m_leaderboards = null;
        private static UM_iSavedGamesClient m_savedGames = null;




        /// <summary>
        /// Returns a new instance of <see cref="UM_iSignInClient"/>
        /// </summary>
        public static UM_iSignInClient SignInClient {
            get {

                if (m_signInClient == null) {
                    switch (Application.platform) {
                        case RuntimePlatform.Android:
                            m_signInClient = new UM_AndroidSignInClient();
                            break;
                        case RuntimePlatform.IPhonePlayer:
                            m_signInClient = new UM_IOSSignInClient();
                            break;
                        default:
                            m_signInClient = new UM_EditorSignInClient();
                            break;
                    }
                }

                return m_signInClient;
            }
        }




        /// <summary>
        /// Returns a new instance of <see cref="UM_iSignInClient"/>
        /// </summary>
        public static UM_iAchievementsClient AchievementsClient {
            get {

                if (m_achievements == null) {
                    switch (Application.platform) {
                        case RuntimePlatform.Android:
                            m_achievements = new UM_AndroidAchievementsClient();
                            break;
                        case RuntimePlatform.IPhonePlayer:
                            m_achievements = new UM_IOSAchievementsClient();
                            break;
                        default:
                            m_achievements = new UM_EditorAchievementsClient();
                            break;
                    }
                }

                return m_achievements;
            }
        }


        /// <summary>
        /// Returns a new instance of <see cref="UM_iSignInClient"/>
        /// </summary>
        public static UM_iLeaderboardsClient LeaderboardsClient {
            get {

                if (m_leaderboards == null) {
                    switch (Application.platform) {
                        case RuntimePlatform.Android:
                            m_leaderboards = new UM_AndroidLeaderboardsClient();
                            break;
                        case RuntimePlatform.IPhonePlayer:
                            m_leaderboards = new UM_IOSLeaderboardsClient();
                            break;
                        default:
                            m_leaderboards = new UM_EditorLeaderboardsClient();
                            break;
                    }
                }

                return m_leaderboards;
            }
        }


        /// <summary>
        /// Returns a new instance of <see cref="UM_iSavedGamesClient"/>
        /// </summary>
        public static UM_iSavedGamesClient SavedGamesClient {
            get {
                if (m_savedGames == null) {
                    switch (Application.platform) {
                        case RuntimePlatform.Android:
                            m_savedGames = new UM_AndroidSavedGamesClient();
                            break;
                        case RuntimePlatform.IPhonePlayer:
                            m_savedGames = new UM_IOSSavedGamesClient();
                            break;
                        default:
                            m_savedGames = new UM_EditorSavedGamesClient();
                            break;
                    }
                }

                return m_savedGames;
            }
        }

    }
}