using System;
using System.Collections.Generic;
using UnityEngine;
using SA.Android.GMS.Games;
using SA.Android.GMS.Common.Images;

using SA.Android.Utilities;

namespace SA.CrossPlatform.GameServices
{
    [Serializable]
    internal class UM_AndroidPlayer : UM_AbstractPlayer, UM_iPlayer
    {
        [SerializeField] AN_Player m_anPlayer;
        private Texture2D m_avatar = null;

        public UM_AndroidPlayer(AN_Player player) {
            m_id = player.Id;
            m_alias = player.Title;
            m_displayName = player.DisplayName;

            m_anPlayer = player;
        }

        public void GetAvatar(Action<Texture2D> callback) {

            if(m_avatar != null) {
                callback.Invoke(m_avatar);
                return;
            }

            if(!m_anPlayer.HasHiResImage) {
                callback.Invoke(null);
                return;
            }

            var url = m_anPlayer.HiResImageUri;
            AN_ImageManager manager = new AN_ImageManager();
            AN_Logger.Log("TrYING TO LOAD AN IMAGE");
            manager.LoadImage(url, (result) => {
                if(result.IsSucceeded) {
                    callback.Invoke(result.Image);
                } else {
                    callback.Invoke(null);
                }
            });
        }
    }
}