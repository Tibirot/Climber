
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if SA_FB_INSTALLED
using FB_Plugin = Facebook.Unity;
#endif

namespace SA.Facebook
{

    public delegate void InitDelegate();
    public delegate void HideUnityDelegate(bool isUnityShown);
    public delegate void FacebookDelegate<T>(T result) where T : IResult;


    public enum HttpMethod
    {
        GET = 0,
        POST = 1,
        DELETE = 2
    }




    internal static class SA_FB_Proxy {

        public static void Init(InitDelegate onInitComplete = null, HideUnityDelegate onHideUnity = null, string authResponse = null) {
#if SA_FB_INSTALLED

            
            FB_Plugin.FB.Init(() => {
                if (onInitComplete != null) {
                    onInitComplete.Invoke();
                }
            }, (isUnityShown) => {
                if (onHideUnity != null) {
                    onHideUnity.Invoke(isUnityShown);
                }
            },
            authResponse);
#endif
        }

        public static void SetAppId(string appId) {

#if SA_FB_INSTALLED
            List<string> appIds = new List<string>() { appId };
            FB_Plugin.Settings.FacebookSettings.AppIds = appIds;
#endif
        }

        public static void API(string query, HttpMethod method, FacebookDelegate<IGraphResult> callback = null, IDictionary<string, string> formData = null) {

#if SA_FB_INSTALLED
            FB_Plugin.FB.API(query, (FB_Plugin.HttpMethod)method, (result) => {
                if(callback != null) {
                    SA_IGraphResult_Proxy res = new SA_IGraphResult_Proxy(result);
                    callback.Invoke(res);
                }
            },
            formData
            );
#endif
        }



        public static void Init(string appId, string clientToken = null, bool cookie = true, bool logging = true, bool status = true, bool xfbml = false, bool frictionlessRequests = true, string authResponse = null, string javascriptSDKLocale = "en_US", HideUnityDelegate onHideUnity = null, InitDelegate onInitComplete = null) {

#if SA_FB_INSTALLED
            FB_Plugin.FB.Init(appId,
                clientToken,
                cookie,
                logging,
                status,
                xfbml,
                frictionlessRequests,
                authResponse,
                javascriptSDKLocale,
                (isUnityShown) => {
                    if (onHideUnity != null) {
                        onHideUnity.Invoke(isUnityShown);
                    }
                },
                () => {
                    if (onInitComplete != null) {
                        onInitComplete.Invoke();
                    }
                });
#endif
        }


        public static void LogInWithReadPermissions(IEnumerable<string> permissions = null, FacebookDelegate<ILoginResult> callback = null) {
#if SA_FB_INSTALLED
            FB_Plugin.FB.LogInWithReadPermissions(permissions, (result) => {
                if(callback != null) {
                    SA_ILoginResult_Proxy res = new SA_ILoginResult_Proxy(result);
                    callback.Invoke(res);
                }
            });
#endif
        }

        public static void LogInWithPublishPermissions(IEnumerable<string> permissions = null, FacebookDelegate<ILoginResult> callback = null) {
#if SA_FB_INSTALLED
            FB_Plugin.FB.LogInWithPublishPermissions(permissions, (result) => {
                if (callback != null) {
                    SA_ILoginResult_Proxy res = new SA_ILoginResult_Proxy(result);
                    callback.Invoke(res);
                }
            });
#endif
        }


        public static void LogOut() {
#if SA_FB_INSTALLED
            FB_Plugin.FB.LogOut();
#endif
        }






        public static void ActivateApp() {
#if SA_FB_INSTALLED
            FB_Plugin.FB.ActivateApp();
#endif
        }


        public static void LogAppEvent(string logEvent, float? valueToSum = null, Dictionary<string, object> parameters = null) {
#if SA_FB_INSTALLED

            FB_Plugin.FB.LogAppEvent(logEvent, valueToSum, parameters);
#endif
        }

        public static void LogPurchase(float logPurchase, string currency = null, Dictionary<string, object> parameters = null) {
#if SA_FB_INSTALLED
            FB_Plugin.FB.LogPurchase(logPurchase, currency, parameters);
#endif
        }


       
        public static bool IsInitialized {
            get {
#if SA_FB_INSTALLED
                return FB_Plugin.FB.IsInitialized;
#else
                return false;
#endif
            }
        }

        public static bool IsLoggedIn {
            get {
#if SA_FB_INSTALLED
               
                return FB_Plugin.FB.IsLoggedIn;
#else
                return false;
#endif
            }
        }


        public static string GraphApiVersion {
            get {
#if SA_FB_INSTALLED
                return FB_Plugin.FB.GraphApiVersion;
#else
                return  string.Empty;
#endif
            }
        }


        public static string ClientToken {
            get {
#if SA_FB_INSTALLED
                return FB_Plugin.FB.ClientToken;
#else
                return  string.Empty;
#endif
            }
        }

        public static string AppId {
            get {
#if SA_FB_INSTALLED
                return FB_Plugin.FB.AppId;
#else
                return  string.Empty;
#endif
            }
        }

       
    }
}

