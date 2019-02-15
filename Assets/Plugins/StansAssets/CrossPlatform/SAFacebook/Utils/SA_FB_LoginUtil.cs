using System;
using System.Collections.Generic;
using UnityEngine;


namespace SA.Facebook
{
    public static class SA_FB_LoginUtil
    {
        
        private static List<Action<SA_FB_LoginUtilResult>> m_callbacks = new List<Action<SA_FB_LoginUtilResult>>();
        private static bool m_waitingLoginResult = false; 

        public static void ConfirmLoginStatus(Action<SA_FB_LoginUtilResult> callback) {

            m_callbacks.Add(callback);

            if (SA_FB.IsLoggedIn) {
                DispatchLoginSucceeded();
                return;
            }

            if(m_waitingLoginResult) {
                return;
            }


            m_waitingLoginResult = true;

            if(SA_FB.IsInitialized) {
                OnInitCompleted();
            } else {
                SA_FB.Init(() => {
                    if(SA_FB.IsInitialized) {
                        OnInitCompleted();
                    } else {
                        DispatchLoginFailed();
                    }
                });
            }
        }



        private static void OnInitCompleted() {
            if (SA_FB.IsLoggedIn) {
                DispatchLoginSucceeded();
            } else {
                SA_FB.Login((result) => {
                    if(result.IsSucceeded) {
                        DispatchLoginSucceeded();
                    } else {
                        DispatchLoginFailed();
                    }
                });
            }
        }



        private static void DispatchLoginFailed() {
            DispatchLoginStatus(false);
        }

        private static void DispatchLoginSucceeded() {
            DispatchLoginStatus(true);
        }


        private static void DispatchLoginStatus(bool status) {
            List<Action<SA_FB_LoginUtilResult>> callbacks = new List<Action<SA_FB_LoginUtilResult>>(m_callbacks);
            foreach (var callback in callbacks) {
                callback.Invoke(new SA_FB_LoginUtilResult(status));
            }

            m_callbacks.Clear();
            m_waitingLoginResult = false;
        }


    }
}
