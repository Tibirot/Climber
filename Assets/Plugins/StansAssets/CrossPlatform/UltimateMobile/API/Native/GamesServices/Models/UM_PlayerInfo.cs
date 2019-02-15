using System;
using System.Collections.Generic;
using UnityEngine;


namespace SA.CrossPlatform.GameServices
{
    /// <summary>
    /// Data interface for player metadata.
    /// </summary>
    [Serializable]
    public class UM_PlayerInfo 
    {
        [SerializeField] UM_PlayerState m_state;
        [SerializeField] UM_iPlayer m_player;


        public UM_PlayerInfo(UM_PlayerState state, UM_iPlayer player) {
            m_state = state;
            m_player = player;
        }


        /// <summary>
        /// Current player state.
        /// </summary>
        public UM_PlayerState State {
            get {
                return m_state;
            }

        }


        /// <summary>
        /// Player object.
        /// </summary>
        public UM_iPlayer Player {
            get {
                return m_player;
            }
        }
    }
}