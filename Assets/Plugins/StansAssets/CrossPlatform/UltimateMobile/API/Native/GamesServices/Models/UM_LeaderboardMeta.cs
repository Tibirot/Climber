using UnityEngine;
using System;
using System.Collections;


namespace SA.CrossPlatform.GameServices
{
    /// <summary>
    /// Data interface for leaderboar metadata.
    /// </summary>
    [Serializable]
    public class UM_LeaderboardMeta : UM_iLeaderboard
    {
        [SerializeField] string m_identifier = string.Empty;
        [SerializeField] string m_title = string.Empty;


        /// <summary>
        /// Creates new metadata object.
        /// </summary>
        /// <param name="identifier">leaderboard identifier.</param>
        /// <param name="title">leaderboard title.</param>
        public UM_LeaderboardMeta(string identifier, string title) {
            m_identifier = identifier;
            m_title = title;
        }

        /// <summary>
        /// The named leaderboard to retrieve information from.
        /// </summary>
        public string Identifier {
            get {
                return m_identifier;
            }

            set {
                m_identifier = value;
            }
        }


        /// <summary>
        /// The localized title for the leaderboard.
        /// </summary>
        public string Title {
            get {
                return m_title;
            }

            set {
                m_title = value;
            }
        }
    }
}