using System;
using System.Collections.Generic;
using UnityEngine;


namespace SA.Android.GMS.Games
{
    /// <summary>
    /// Data interface for a specific variant of a leaderboard; 
    /// a variant is defined by the combination of the leaderboard's collection (public or social) 
    /// and time span (daily, weekly, or all-time).
    /// </summary>
    [Serializable]
    public class AN_LeaderboardVariant
    {



        [SerializeField] int m_collection;
        [SerializeField] String m_displayPlayerRank;
        [SerializeField] String m_displayPlayerScore;
        [SerializeField] long m_numScores;
        [SerializeField] long m_playerRank;
        [SerializeField] String m_playerScoreTag;
        [SerializeField] long m_rawPlayerScore;
        [SerializeField] int m_timeSpan;
        [SerializeField] bool m_hasPlayerInfo;


        /// <summary>
        /// Retrieves the collection of scores contained by this variant.
        /// </summary>
        public AN_Leaderboard.Collection Collection {
            get {
                return (AN_Leaderboard.Collection)m_collection;
            }
        }

        /// <summary>
        /// Retrieves the viewing player's formatted rank for this variant, if any.
        /// </summary>
        public string DisplayPlayerRank {
            get {
                return m_displayPlayerRank;
            }
        }

        /// <summary>
        /// Retrieves the viewing player's score for this variant, if any.
        /// </summary>
        public string DisplayPlayerScore {
            get {
                return m_displayPlayerScore;
            }
        }

        /// <summary>
        /// Retrieves the total number of scores for this variant.
        /// </summary>
        public long NumScores {
            get {
                return m_numScores;
            }
        }

        /// <summary>
        /// Retrieves the viewing player's rank for this variant, if any.
        /// </summary>
        public long PlayerRank {
            get {
                return m_playerRank;
            }
        }

        /// <summary>
        /// Retrieves the viewing player's score tag for this variant, if any.
        /// </summary>
        public string PlayerScoreTag {
            get {
                return m_playerScoreTag;
            }
        }

        /// <summary>
        /// Retrieves the viewing player's score for this variant, if any.
        /// </summary>
        public long RawPlayerScore {
            get {
                return m_rawPlayerScore;
            }
        }


        /// <summary>
        /// Retrieves the time span that the scores for this variant are drawn from.
        /// </summary>
        public AN_Leaderboard.TimeSpan TimeSpan {
            get {
                return (AN_Leaderboard.TimeSpan)m_timeSpan;
            }
        }

        /// <summary>
        /// Get whether or not this variant contains score information for the viewing player or not.
        /// </summary>
        public bool HasPlayerInfo {
            get {
                return m_hasPlayerInfo;
            }
        }
    }
}