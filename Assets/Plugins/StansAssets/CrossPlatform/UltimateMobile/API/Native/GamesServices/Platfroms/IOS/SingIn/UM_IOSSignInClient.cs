using System;
using UnityEngine;

using SA.Foundation.Templates;

using SA.iOS.GameKit;

namespace SA.CrossPlatform.GameServices
{
    internal class UM_IOSSignInClient : UM_AbstractSignInClient, UM_iSignInClient
    {


        protected override void StartSingInFlow(Action<SA_Result> callback) {
            ISN_GKLocalPlayer.Authenticate((SA_Result result) => {
                if (result.IsSucceeded) {
                    ISN_GKLocalPlayer player = ISN_GKLocalPlayer.LocalPlayer;
                    UpdatePlayerInfo(player);
                } 

                callback.Invoke(result);
            });
        }

        public void SingOut(Action<SA_Result> callback) {
            //We will jus do nothingt for iOS
        }



        private void UpdatePlayerInfo(ISN_GKLocalPlayer player) {
            var localPlayer = new UM_IOSPlayer(player);
            var playerInfo = new UM_PlayerInfo(UM_PlayerState.SignedIn, localPlayer);
            UpdateSignedPlater(playerInfo);
        }
    }
}