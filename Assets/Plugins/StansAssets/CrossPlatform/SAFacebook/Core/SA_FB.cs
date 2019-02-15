using System;
using System.Collections.Generic;
using UnityEngine;



namespace SA.Facebook
{
	public static class SA_FB
	{

		private static SA_FB_GraphAPI m_graphAPI = new SA_FB_GraphAPI();
		//private static SA_FB_Mobile m_mobileAPI = new SA_FB_Mobile();

		private static bool s_isInitializing = false;
		private static InitDelegate s_initCallback = delegate { };

		public static SA_FB_User CurrentUser { get; private set; }
		/// <summary>
		/// Sets the state of the Facebook SDK, and initializes all platform-specific data structures and behaviors. 
		/// This function can only be called once during the lifetime of the object; later calls lead to undefined behavior.
		/// Relies on properties that are set in the Unity Editor using the Facebook | Edit settings menu option,
		/// 
		/// Also <see cref="FB.ActivateApp()"/> method will be callend automaticly when initialization is completed
		/// </summary>
		/// <param name="onInitComplete">A function that will be called once all data structures in the SDK are initialized; any code that should synchronize with the player's Facebook session should be in onInitComplete().</param>
		/// <param name="onHideUnity">A function that will be called when Facebook tries to display HTML content within the boundaries of the Canvas. When called with its sole argument set to false, your game should pause and prepare to lose focus. If it's called with its argument set to true, your game should prepare to regain focus and resume play. Your game should check whether it is in fullscreen mode when it resumes, and offer the player a chance to go to fullscreen mode if appropriate.</param>
		/// <param name="authResponse">effective in Web Player only, rarely used A Facebook auth_response you have cached to preserve a session, represented in JSON. If an auth_response is provided, FB will initialize itself using the data from that session, with no additional checks.</param>
		public static void Init(InitDelegate onInitComplete = null, HideUnityDelegate onHideUnity = null, string authResponse = null)
		{
			if(s_isInitializing) {
				if(onInitComplete != null) {
					s_initCallback += onInitComplete;
				}
				return;
			}

			s_isInitializing = true;
            SA_FB_Proxy.Init(() => {
                SA_FB_Proxy.ActivateApp();
				s_initCallback += onInitComplete;
				s_initCallback.Invoke();
			}, onHideUnity, authResponse);
		}

		/// <summary>
		/// Sets the state of the Facebook SDK, and initializes all platform-specific data structures and behaviors. 
		/// This function can only be called once during the lifetime of the object; later calls lead to undefined behavior.
		///
		/// Also <see cref="FB.ActivateApp()"/> method will be callend automaticly when initialization is completed
		/// </summary>
		/// <param name="appId">The Facebook application ID of the initializing app. </param>
		/// <param name="clientToken">Client Token</param>
		/// <param name="cookie">Sets a cookie which your server-side code can use to validate a user's Facebook session</param>
		/// <param name="logging">If true, outputs a verbose log to the Javascript console to facilitate debugging. Effective on Web only.</param>
		/// <param name="status">If true, attempts to initialize the Facebook object with valid session data.*</param>
		/// <param name="xfbml">If true, Facebook will immediately parse any XFBML elements on the Facebook Canvas page hosting the app, like the page plugin. Effective on Web only.</param>
		/// <param name="frictionlessRequests">Frictionless Requests</param>
		/// <param name="authResponse">effective in Web Player only, rarely used A Facebook auth_response you have cached to preserve a session, represented in JSON. If an auth_response is provided, FB will initialize itself using the data from that session, with no additional checks.</param>
		/// <param name="javascriptSDKLocale">javascript SDK Locale</param>
		/// <param name="onHideUnity">A function that will be called when Facebook tries to display HTML content within the boundaries of the Canvas. When called with its sole argument set to false, your game should pause and prepare to lose focus. If it's called with its argument set to true, your game should prepare to regain focus and resume play. Your game should check whether it is in fullscreen mode when it resumes, and offer the player a chance to go to fullscreen mode if appropriate.</param>
		/// <param name="onInitComplete">A function that will be called once all data structures in the SDK are initialized; any code that should synchronize with the player's Facebook session should be in onInitComplete().</param>
		public static void Init(string appId, string clientToken = null, bool cookie = true, bool logging = true, bool status = true, bool xfbml = false, bool frictionlessRequests = true, string authResponse = null, string javascriptSDKLocale = "en_US", HideUnityDelegate onHideUnity = null, InitDelegate onInitComplete = null)
		{
            SA_FB_Proxy.Init(appId, clientToken, cookie, logging, status, xfbml, frictionlessRequests, authResponse, javascriptSDKLocale, onHideUnity, () =>
			{
                SA_FB_Proxy.ActivateApp();
				if(onInitComplete != null) {
					onInitComplete.Invoke();
				}
			});
		}


		/// <summary>
		/// Prompts the user to authorize your application using the Login Dialog appropriate to the platform.
		/// If the user is already logged in and has authorized your application, 
		/// checks whether all permissions in the permissions parameter have been granted, and if not, 
		/// prompts the user for any that are newly requested.
		/// Method using scopes that was specified inside Stan's Assets -> SocialPlugin -> Settings Editor
		///
		/// In the Unity Editor, a stub function is called, which will prompt you to provide an access token 
		/// </summary>
		/// <param name="callback">A delegate that will be called with the result of the Login Dialog </param>
		public static void Login(Action<SA_FB_LoginResult> callback)
		{
			Login(SA_FB_Settings.Instance.Scopes, callback);
		}

		/// <summary>
		/// Prompts the user to authorize your application using the Login Dialog appropriate to the platform.
		/// If the user is already logged in and has authorized your application, 
		/// checks whether all permissions in the permissions parameter have been granted, and if not, 
		/// prompts the user for any that are newly requested.
		///
		/// In the Unity Editor, a stub function is called, which will prompt you to provide an access token 
		/// </summary>
		/// <param name="callback">A delegate that will be called with the result of the Login Dialog </param>
		/// <param name="scopes">A list of Facebook permissions requested from the user </param>
		private static void Login(List<string> scopes, Action<SA_FB_LoginResult> callback)
		{

			if(!scopes.Contains(SA_FB_Permissions.publish_actions.ToString())) {
                SA_FB_Proxy.LogInWithReadPermissions(scopes, (loginResult) => {
					var result = new SA_FB_LoginResult(loginResult);
					callback.Invoke(result);
				});

			} else {
                SA_FB_Proxy.LogInWithPublishPermissions(scopes, (loginResult) => {
					var result = new SA_FB_LoginResult(loginResult);
					callback.Invoke(result);
				});
			}
		}

		public static void GetLoggedInUserInfo(Action<SA_FB_GetUserResult> callback) {
			GraphAPI.GetLoggedInUserInfo(result => {
				CurrentUser = result.User;
				callback(result);
			});
		}

		/// <summary>
		/// On Web, this method will log the user out of both your site and Facebook.
		/// On iOS and Android, it will log the user out of your Application.
		/// On all the platforms it will also invalidate any access token that you have for the user that was issued before the logout.
		/// 
		/// On Web, you almost certainly should not use this function, which is provided primarily for completeness. 
		/// Having a logout control inside a game that executes a Facebook-wide logout will violate users' expectations. 
		/// Instead, allow users to control their logged-in status on Facebook itself.
		/// </summary>
		public static void LogOut()
		{
			CurrentUser = null;
            SA_FB_Proxy.LogOut();
		}


		/// <summary>
		/// Makes a call to the Graph API to get data, or take action on a user's behalf.
		/// This will always be used once a user is logged in, and an access token has been granted; 
		/// the permissions encoded by the access token determine which Graph API calls will be available.
		/// 
		/// This method will automatically check user active session, and if user isn't logged in,
		/// SDK Init & User Login attempt would be made using the editor SDK settings
		/// </summary>
		/// <param name="query">The Graph API endpoint to call. e.g. /me/scores </param>
		/// <param name="method">The HTTP method to use in the call </param>
		/// <param name="callback">A delegate which will receive the result of the call </param>
		/// <param name="formData">The key/value pairs to be passed to the endpoint as arguments. </param>
		public static void API(string query, HttpMethod method, FacebookDelegate<IGraphResult> callback = null, IDictionary<string, string> formData = null)
		{

			SA_FB_LoginUtil.ConfirmLoginStatus((statusResult) =>
			{
				if(statusResult.IsSucceeded)
				{
                    SA_FB_Proxy.API(query, method, callback, formData);
				}
				else
				{
					if(callback != null)
					{
						callback.Invoke(statusResult);
					}
				}
			});

		}


		/// <summary>
		/// Check whether a user is currently logged in and has authorized your app
		/// </summary>
		public static bool IsLoggedIn
		{
			get
			{
				return SA_FB_Proxy.IsLoggedIn;
			}
		}

		/// <summary>
		/// Check whether the Unity SDK has been initialized. false if the SDK has not been initalized. 
		/// </summary>
		public static bool IsInitialized
		{
			get
			{
				return SA_FB_Proxy.IsInitialized;
			}
		}


		/// <summary>
		/// Object that contain methods to comunicate with facebook graph API 
		/// </summary>
		public static SA_FB_GraphAPI GraphAPI
		{
			get
			{
				return m_graphAPI;
			}
		}


        /*
		/// <summary>
		/// Facebook Mobile API hub 
		/// </summary>
		public static SA_FB_Mobile Mobile
		{
			get
			{
				return m_mobileAPI;
			}
		}*/

		/// <summary>
		/// Saved user AccessToken from last login result
		/// Can be null if there wa no success during current session
		/// </summary>
		public static SA_AccessToken AccessToken
		{
			get
			{
				return SA_AccessToken.CurrentAccessToken;
			}
		}

		/// <summary>
		/// Saved user AccessToken string from last login result
		/// <see cref="string.Empty"/> if there wa no success during current session
		/// </summary>
		public static string AccessTokenString {
			get {
				if(SA_AccessToken.CurrentAccessToken != null) {
					return SA_AccessToken.CurrentAccessToken.TokenString;
				} else {
					return string.Empty;
				}
			}
		}

		/// <summary>
		/// Gets the app client token. ClientToken might be different from FBSettings.ClientToken
		/// if using the programmatic version of SA_FB.Init().
		/// </summary>
		public static string ClientToken
		{
			get
			{
                return SA_FB_Proxy.ClientToken;
			}
		}

	}
}
