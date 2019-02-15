using UnityEngine;
using System.Collections;
using SA.Foundation.Templates;


namespace SA.CrossPlatform.GameServices
{

    /// <summary>
    /// Object that holds score load result
    /// </summary>
    public class UM_ScoreLoadResult : SA_Result
    {
        [SerializeField] UM_iScore m_score;


        public UM_ScoreLoadResult(UM_iScore score) {
            m_score = score;
        }

        public UM_ScoreLoadResult(SA_Error error) : base(error) { }

        /// <summary>
        /// Loaded score
        /// </summary>
        public UM_iScore Score {
            get {
                return m_score;
            }
        }
    }
}


