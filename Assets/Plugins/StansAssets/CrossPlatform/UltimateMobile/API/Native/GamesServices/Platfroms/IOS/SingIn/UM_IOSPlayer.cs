using System;
using UnityEngine;

using SA.iOS.GameKit;

namespace SA.CrossPlatform.GameServices
{
    [Serializable]
    internal class UM_IOSPlayer : UM_AbstractPlayer, UM_iPlayer
    {

        [SerializeField] ISN_GKPlayer m_player;

        public UM_IOSPlayer(ISN_GKPlayer player) {
            m_id = player.PlayerID;
            m_alias = player.Alias;
            m_displayName = player.DisplayName;

            m_player = player;
        }

        public void GetAvatar(Action<Texture2D> callback) {
            m_player.LoadPhoto(GKPhotoSize.Normal, (result) => {
                if(result.IsSucceeded) {
                    callback.Invoke(result.Image);
                } else {
                    callback.Invoke(null);
                }
            });
        }
    }
}