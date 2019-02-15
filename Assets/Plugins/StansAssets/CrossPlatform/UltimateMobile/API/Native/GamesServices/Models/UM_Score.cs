using UnityEngine;
using System;
using System.Collections;
using SA.Foundation.Time;


namespace SA.CrossPlatform.GameServices
{

    /// <summary>
    /// An object containing information for a score that was earned by the player.
    /// </summary>
    [Serializable]
    public class UM_Score : UM_iScore
    {
       
        [SerializeField] long m_value;
        [SerializeField] long m_rank;
        [SerializeField] int m_context;
        [SerializeField] long m_date;


        public UM_Score(long value, long rank, int context, long date) {
            m_value = value;
            m_rank = rank;
            m_context = context;
            m_date = date;
        }



        /// <summary>
        /// The position of the score in the results of a leaderboard search.
        /// </summary>
        public long Rank {
            get {
                return m_rank;
            }
        }


        /// <summary>
        /// The score earned by the player.
        /// </summary>
        public long Value {
            get {
                return m_value;
            }
        }


        /// <summary>
        /// An integer value used by your game.
        /// </summary>
        public int Context {
            get {
                return m_context;
            }
        }

        /// <summary>
        /// The date and time when the score was earned.
        /// </summary>
        public DateTime Date {
            get {
                return SA_Unix_Time.ToDateTime(m_date);
            }
        }


        /// <summary>
        /// The <see cref="Date"/> field value as unix time stamp
        /// </summary>
        public long DateUnix {
            get {
                return m_date;
            }
        }
    }
}