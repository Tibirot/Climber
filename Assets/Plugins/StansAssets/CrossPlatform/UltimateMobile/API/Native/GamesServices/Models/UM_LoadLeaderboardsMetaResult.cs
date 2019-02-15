
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SA.Foundation.Templates;


namespace SA.CrossPlatform.GameServices
{
    /// <summary>
    /// Object that represent leaderboards loading result
    /// </summary>
    public class UM_LoadLeaderboardsMetaResult : SA_Result
    {

        [SerializeField] List<UM_iLeaderboard> m_leaderboards = new List<UM_iLeaderboard>();

        public UM_LoadLeaderboardsMetaResult(SA_Error erorr) : base(erorr) {

        }

        public UM_LoadLeaderboardsMetaResult(List<UM_iLeaderboard> leaderboards) {
            m_leaderboards = leaderboards;
        }

        /// <summary>
        /// Loaded Leaderboards meta.
        /// </summary>
        public List<UM_iLeaderboard> Leaderboards {
            get {
                return m_leaderboards;
            }
        }
    }
}