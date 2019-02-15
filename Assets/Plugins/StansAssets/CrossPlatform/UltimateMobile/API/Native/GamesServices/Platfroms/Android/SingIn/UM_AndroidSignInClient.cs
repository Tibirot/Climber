using System;
using System.Collections.Generic;
using UnityEngine;

using SA.Foundation.Events;
using SA.Foundation.Templates;
using SA.Foundation.Async;

using SA.Android.App;
using SA.Android.App.View;
using SA.Android.GMS.Auth;
using SA.Android.GMS.Games;
using SA.Android.GMS.Drive;
using SA.Android.GMS.Common;
using SA.Android.Utilities;



namespace SA.CrossPlatform.GameServices
{
    internal class UM_AndroidSignInClient : UM_AbstractSignInClient, UM_iSignInClient
    {

        private List<int> m_resolvedErrors = new List<int>();

        public UM_AndroidSignInClient() {
            SA_MonoEvents.OnApplicationPause.AddSafeListener(this, (paused) => {
                if(!paused) {

                    //We do not want to do Silent SignIn on resume in case player not yet signed.
                    if (PlayerInfo.State == UM_PlayerState.SignedOut) {

                        // In case it's not null, this means we are missng something, so we will do  Silent SignIn
                        // The case may happend because we sending fail event on propxy Activity Destory event.
                        // But propxy Activity Destory not always means that player is failed to log in.
                        // We have to send fail evennt on propxy Activity Destory, since if we not, in cases where google and our proxy
                        // activity both are destoryed, we will not get any event.
                        if (AN_GoogleSignIn.GetLastSignedInAccount() == null) {
                            return;
                        }
                    }

                    //We need to perform Silent SignIn every time we back from pause
                    SignInClient.SilentSignIn((silentSignInResult) => {
                        if (silentSignInResult.IsSucceeded) {
                            RetrivePlayer((result) => { });
                        } else {
                            //looks Like player singed out
                            UpdatePlayerInfo(null);                      
                        }
                    });
                }
            });
        }


        protected override void StartSingInFlow(Action<SA_Result> callback) {
            m_resolvedErrors.Clear();
            StartSingInFlowternal(callback);
        }


        private void StartSingInFlowternal(Action<SA_Result> callback) {

            AN_Logger.Log("UM_AndroidSignInClient, starting siglent sing-in");
            SignInClient.SilentSignIn((silentSignInResult) => {
                if(silentSignInResult.IsSucceeded) {
                    AN_Logger.Log("UM_AndroidSignInClient, siglent sing-in Succeeded");
                    RetrivePlayer(callback);
                } else {
                    AN_Logger.Log("UM_AndroidSignInClient, siglent sing-in Failed");
                    AN_Logger.Log("UM_AndroidSignInClient, starting interactive sing-in");
                    SignInClient.SignIn((interactiveSignInResult) => {

                        AN_Logger.Log("UM_AndroidSignInClient, interactive sing-in completed");
                        if (interactiveSignInResult.IsSucceeded) {
                            AN_Logger.Log("UM_AndroidSignInClient, interactive sing-in succeeded");
                            RetrivePlayer(callback);
                        } else {
                            AN_Logger.Log("UM_AndroidSignInClient, interactive sing-in failed");
                            int errorCode = interactiveSignInResult.Error.Code;
                            switch (errorCode) {
                                //Retry may solve the issue
                                case (int)AN_CommonStatusCodes.NETWORK_ERROR:

                                //in some cases it may cause a loop
                                //case (int)AN_CommonStatusCodes.INTERNAL_ERROR:
                                case (int)AN_CommonStatusCodes.FAILED_ACTIVITY_ERROR:
                                    //Let's see if we tried to do it before
                                    if(m_resolvedErrors.Contains(errorCode)) {
                                        AN_Logger.Log("UM_AndroidSignInClient, sending fail result");
                                        callback.Invoke(new SA_Result(interactiveSignInResult.Error));
                                    } else {
                                        //Nope, this is new one, let's try to resolve it
                                        AN_Logger.Log("Trying to resolved failed sigin-in result with code: " + errorCode);
                                        StartSingInFlowternal(callback);
                                    } 
                                    break;
                                default:
                                    AN_Logger.Log("UM_AndroidSignInClient, sending fail result");
                                    callback.Invoke(new SA_Result(interactiveSignInResult.Error));
                                    break;

                            }
                        }
                    });
                }
            });
        }

        public void SingOut(Action<SA_Result> callback) {
            SignInClient.RevokeAccess(() => {
                UpdatePlayerInfo(null);
                callback.Invoke(new SA_Result());
            });
        }




        //--------------------------------------
        //  Private Methods
        //--------------------------------------


        private void UpdatePlayerInfo(AN_Player player) {
            UM_PlayerInfo playerInfo;
            if(player != null) {
                playerInfo = new UM_PlayerInfo(UM_PlayerState.SignedIn, new UM_AndroidPlayer(player));
            } else {
                playerInfo = new UM_PlayerInfo(UM_PlayerState.SignedOut, null);
            }

            UpdateSignedPlater(playerInfo);
        }


        private void RetrivePlayer(Action<SA_Result> callback) {

            AN_Logger.Log("UM_AndroidSignInClient, cleint sigined-in, getting the player info");

            //When Sign in is finished with successes
            var gamesClient = AN_Games.GetGamesClient();
            gamesClient.SetViewForPopups(AN_MainActivity.Instance);

            //optionally
            gamesClient.SetGravityForPopups(AN_Gravity.TOP | AN_Gravity.CENTER_HORIZONTAL);

            AN_PlayersClient client = AN_Games.GetPlayersClient();
            SA_Result apiResult;
            client.GetCurrentPlayer((result) => {
                if (result.IsSucceeded) {
                    apiResult = new SA_Result();

                    AN_Logger.Log("UM_AndroidSignInClient, player info retrived, OnPlayerChnaged event will be sent");
                    UpdatePlayerInfo(result.Data);
                } else {
                    apiResult = new SA_Result(result.Error);
                }


                AN_Logger.Log("UM_AndroidSignInClient, sending sing in result");
                callback.Invoke(apiResult);
            });
        }


        private AN_GoogleSignInClient SignInClient {
            get {
                AN_GoogleSignInOptions.Builder builder = new AN_GoogleSignInOptions.Builder(AN_GoogleSignInOptions.DEFAULT_SIGN_IN);
                builder.RequestId();
                builder.RequestScope(new AN_Scope(AN_Scopes.GAMES_LITE));

                if (UM_Settings.Instance.AndroidRequestEmail) {
                    builder.RequestEmail();
                }

                if (UM_Settings.Instance.AndroidRequestProfile) {
                    builder.RequestProfile();
                }

                if(UM_Settings.Instance.AndroidSavedGamesEnabled) {
                    builder.RequestScope(AN_Drive.SCOPE_APPFOLDER);
                }

                if(UM_Settings.Instance.AndroidRequestServerAuthCode) {
                    builder.RequestServerAuthCode(UM_Settings.Instance.AndroidGMSServerId, false);
                }
              
                AN_GoogleSignInOptions gso = builder.Build();
                return AN_GoogleSignIn.GetClient(gso);
            }
        }






    }
}