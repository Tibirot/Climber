
using UnityEngine;
using UnityEngine.SocialPlatforms;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;


#if UNITY_WSA
using UnityEngine.Windows;
#endif
using Combu;

namespace Combu
{
	/// <summary>
	/// Combu Manager class, it follows the Singleton pattern design (accessible through the 'instance' static property),
	/// it means that you shouldn't have more than one instance of this component in your scene.
	/// </summary>
	public class CombuManager : MonoBehaviour
	{
		/// <summary>
		/// Current API version.
		/// </summary>
		public const string COMBU_VERSION = "3.1.1";

        #region Static members

        #region Internal access

        /// <summary>
        /// The singleton instance of CombuManager
        /// </summary>
        protected static CombuManager _instance;

		/// <summary>
		/// The singleton instance of CombuPlatform.
		/// </summary>
		protected static CombuPlatform _platform;

        /// <summary>
        /// The instance of data encryption.
        /// </summary>
        protected static CombuEncryption encryption = new CombuEncryption();

        #endregion

        #region Public access

        /// <summary>
        /// Gets the session token.
        /// </summary>
        /// <value>The session token.</value>
        public static string sessionToken { get { return (encryption == null ? "" : encryption.Token); } }

		/// <summary>
		/// Gets the current singleton instance.
		/// </summary>
		/// <value>The instance.</value>
		public static CombuManager instance { get { return _instance; } }

		/// <summary>
		/// Gets the Combu ISocialPlatform implementation.
		/// </summary>
		/// <value>The platform.</value>
		public static CombuPlatform platform { get { return _platform; } }

		/// <summary>
		/// Gets the local user.
		/// </summary>
		/// <value>The local user.</value>
		public static User localUser { get { return (_platform == null ? null : (User)_platform.localUser); } }

		/// <summary>
		/// Gets a value indicating whether the Singleton instance of <see cref="Combu.CombuManager"/> is initialized.
		/// </summary>
		/// <value><c>true</c> if is initialized; otherwise, <c>false</c>.</value>
        public static bool isInitialized { get { return (_instance != null && _instance.isActiveAndEnabled && _instance._serverInfo != null); } }

        #endregion

        #endregion

        #region Internal access

        protected ISocialPlatform _defaultSocialPlatform;
        protected CombuServerInfo _serverInfo;
        protected bool downloading, cancelling;
        bool initialized;

        #endregion

        #region Public properties

        /// <summary>
        /// Should call DontDestroyOnLoad on the CombuManager gameObject?
        /// Recommended: set to true
        /// </summary>
        public bool dontDestroyOnLoad = true;

		/// <summary>
		/// Should set Combu as the active social platform? The previous platform is accessible from defaultSocialPlatform
		/// </summary>
		public bool setAsDefaultSocialPlatform;

        /// <summary>
        /// Auto-connect to server on Awake.
        /// </summary>
        public bool connectOnAwake = true;

        /// <summary>
        /// Upon failed connection to server, it will  retry to connect after specified seconds (set 0 to disable auto-retry).
        /// </summary>
        public float retryConnectAfterSeconds = 0f;

		/// <summary>
		/// The app identifier.
		/// </summary>
		public string appId;

		/// <summary>
		/// The app secret key.
		/// </summary>
		public string appSecret;

		/// <summary>
		/// The URL root for the production environment.
		/// </summary>
		public string urlRootProduction;

		/// <summary>
		/// The URL root for the stage environment.
		/// </summary>
		public string urlRootStage;

		/// <summary>
		/// If <em>true</em> sets the stage as current environment (default: false for production).
		/// </summary>
		public bool useStage;

		/// <summary>
		/// Print debug info in the console log.
		/// </summary>
		public bool logDebugInfo;

		/// <summary>
		/// Store the user credentials in PlayerPrefs to auto-login on next connection.
		/// </summary>
		public bool rememberCredentials;

		/// <summary>
		/// The ping interval in seconds (set 0 to disable automatic pings).
		/// Ping is currently used to mantain the online state of the local user
		/// and is automatically called only if the local user is authenticated.
		/// </summary>
		public float pingIntervalSeconds = 30f;

		/// <summary>
		/// The max seconds from now to a user's <strong>lastSeen</strong> to consider the online state.
		/// </summary>
		public int onlineSeconds = 120;

		/// <summary>
		/// The max seconds from now to a user's <strong>lastSeen</strong> to consider the playing state.
		/// </summary>
		public int playingSeconds = 120;

		/// <summary>
		/// The language to localize the error messages from web services.
		/// </summary>
		public string language = "en";

		/// <summary>
		/// The achievement user interface object for CombuPlatform.ShowAchievementsUI().
		/// </summary>
		public GameObject achievementUIObject;

		/// <summary>
		/// The achievement user interface function for CombuPlatform.ShowAchievementsUI().
		/// </summary>
		public string achievementUIFunction;

		/// <summary>
		/// The leaderboard user interface object for CombuPlatform.ShowLeaderboardUI().
		/// </summary>
		public GameObject leaderboardUIObject;

		/// <summary>
		/// The leaderboard user interface function for CombuPlatform.ShowLeaderboardUI().
		/// </summary>
		public string leaderboardUIFunction;

        /// <summary>
        /// Gets the default social platform defined (this is set before Combu is set as activate, eventually).
        /// </summary>
        /// <value>The default social platform.</value>
        public ISocialPlatform defaultSocialPlatform { get { return _defaultSocialPlatform; } }

        /// <summary>
        /// Gets a value indicating whether this <see cref="Combu.CombuManager"/> is downloading from a webservice.
        /// </summary>
        /// <value><c>true</c> if is downloading; otherwise, <c>false</c>.</value>
        public bool isDownloading { get { return downloading; } }

        /// <summary>
        /// Gets a value indicating whether this <see cref="Combu.CombuManager"/> is cancelling a webservice request.
        /// </summary>
        /// <value><c>true</c> if is cancelling; otherwise, <c>false</c>.</value>
        public bool isCancelling { get { return cancelling; } }

        /// <summary>
        /// Gets a value indicating whether <see cref="Combu.CombuManager.localUser"/> is authenticated.
        /// </summary>
        /// <value><c>true</c> if is authenticated; otherwise, <c>false</c>.</value>
        public bool isAuthenticated { get { return (localUser != null && localUser.authenticated); } }

        /// <summary>
        /// Gets the server info.
        /// </summary>
        /// <value>The server info.</value>
        public CombuServerInfo serverInfo { get { return _serverInfo; } }

        #endregion

        #region Monobehaviour events

        protected virtual void Awake ()
		{
			// Ensure we have one only instance
			if (_instance != null)
			{
				Destroy(this);
			}
			else
			{
				if (dontDestroyOnLoad)
					DontDestroyOnLoad(gameObject);
				_instance = this;
				downloading = false;
				cancelling = false;
				_defaultSocialPlatform = Social.Active;
				_platform = new CombuPlatform();
				if (setAsDefaultSocialPlatform)
					Social.Active = _platform;
			}
		}

		protected virtual void Start ()
		{
            if (!urlRootProduction.EndsWith("/", StringComparison.InvariantCultureIgnoreCase))
				urlRootProduction += "/";
            if (!urlRootStage.EndsWith("/", StringComparison.InvariantCultureIgnoreCase))
				urlRootStage += "/";

            // Initialize the connection to the server
            if (connectOnAwake)
            {
                Connect();
            }
		}

        protected virtual void OnEnable ()
		{
			if (pingIntervalSeconds > 0)
				InvokeRepeating("AutoPing", 0, pingIntervalSeconds);
		}

        protected virtual void OnDisable ()
		{
			if (pingIntervalSeconds > 0)
				CancelInvoke("AutoPing");
		}

        #endregion

        #region Internal utilities
		
		/// <summary>
		/// This is InvokeRepeated on enable and automatically pings the server. If the user was authenticated and the ping fails, the local user is automatically disconnected.
		/// </summary>
		protected virtual void AutoPing ()
		{
			Ping(true, (bool success) => {
				if (!success)
					SetLocalUser(new User());
			});
		}

        /// <summary>
        /// Downloads the content of an URL with the specified form.
        /// </summary>
        /// <returns>The URL.</returns>
        /// <param name="url">URL.</param>
        /// <param name="form">Form.</param>
        /// <param name="onComplete">On complete.</param>
        protected IEnumerator DownloadUrl (string url, WWWForm form, Action<string, string> onComplete)
        {
            // Allow only one call at once
            while (downloading) {
                yield return new WaitForSeconds (.3f);
            }
            // Set the flag to know that it's downloading
            downloading = true;
            if (logDebugInfo)
                Debug.Log("Sending: " + url + "?" + System.Text.ASCIIEncoding.ASCII.GetString(form.data));
            // Call the webservice
            WWW www = new WWW(url, form);
            bool cancelled = false;
            while (!www.isDone)
            {
                if (cancelling)
                {
                    cancelled = true;
                    break;
                }
                yield return null;
            }
            string error = (cancelled ? "Request cancelled" : www.error);
            string text = (string.IsNullOrEmpty(error) ? www.text : "");
            // Decrypt the server response
            text = encryption.DecryptResponse (text);
            if (logDebugInfo)
                Debug.Log("TEXT: " + text + "\nERROR: " + error);
            // Execute the callback
            if (onComplete != null)
                onComplete(text, error);
            yield return new WaitForSeconds (.5f);
            downloading = false;
            cancelling = false;
        }

        /// <summary>
        /// Compares the camera depth (sort the cameras in CaptureScreenshot).
        /// </summary>
        /// <returns>The camera depth.</returns>
        /// <param name="a">The alpha component.</param>
        /// <param name="b">The blue component.</param>
        static int CompareCameraDepth (Camera a, Camera b)
        {
            return a.depth.CompareTo(b.depth);
        }

        #endregion

        #region Public utilities

        /// <summary>
        /// Connect to server if not already initialized.
        /// </summary>
        public void Connect()
        {
            if (!isInitialized && !isDownloading)
            {
                InitializeServer((bool success) =>
                {
                    if (!success && retryConnectAfterSeconds > 0f)
                    {
                        Invoke("Connect", retryConnectAfterSeconds);
                    }
                });
            }
        }

		/// <summary>
		/// Calls a webservice.
		/// </summary>
		/// <param name='url'>
		/// URL.
		/// </param>
		/// <param name='form'>
		/// Form.
		/// </param>
		/// <param name='onComplete'>
		/// On complete callback.
		/// </param>
		public void CallWebservice (string url, WWWForm form, System.Action<string, string> onComplete)
		{
			StartCoroutine(DownloadUrl(url, form, onComplete));
		}
		
		/// <summary>
		/// Cancels the current request (next frame).
		/// </summary>
		public void CancelRequest ()
		{
			if (downloading && !cancelling)
				cancelling = true;
		}
		
		/// <summary>
		/// Creates a new form to be passed to a webservice.
		/// </summary>
		/// <returns>
		/// The form.
		/// </returns>
		public CombuForm CreateForm()
		{
			return new CombuForm();
		}
		
		/// <summary>
		/// Gets the absolute URL from a relative.
		/// </summary>
		/// <returns>
		/// The URL.
		/// </returns>
		/// <param name='relativeUrl'>
		/// Relative URL.
		/// </param>
		public string GetUrl (string relativeUrl)
		{
			Uri uri = new Uri( new Uri(useStage ? urlRootStage : urlRootProduction) , relativeUrl);
			return uri.ToString();
		}

		/// <summary>
		/// Captures the screen shot.
		/// </summary>
		/// <returns>The screen shot.</returns>
		/// <param name="thumbnailHeight">Thumbnail height.</param>
		/// <param name="excludeCams">Exclude cams.</param>
		public static byte[] CaptureScreenshot(int thumbnailHeight = 720, List<Camera> excludeCams = null)
		{
			if (excludeCams == null)
				excludeCams = new List<Camera>();
			
			float ratio = (float)Screen.width / (float)Screen.height;
			int thumbnailWidth = Mathf.RoundToInt((float)thumbnailHeight * ratio);
			
			byte[] result;
			
			RenderTexture rt = new RenderTexture(thumbnailWidth, thumbnailHeight, 24);
			Texture2D screenShot = new Texture2D(rt.width, rt.height, TextureFormat.RGB24, false);
			
			List<Camera> allCameras = new List<Camera>();
			allCameras.AddRange(FindObjectsOfType<Camera>());
			allCameras.RemoveAll( cam => !cam.enabled );
			allCameras.Sort(CompareCameraDepth);
			
			for (int i = 0; i < allCameras.Count; ++i)
			{
				Camera cam = allCameras[i];
				if (excludeCams.Contains(cam))
					continue;
				
				cam.targetTexture = rt;
				cam.Render();
				RenderTexture.active = rt;
				screenShot.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0, false);
				screenShot.Apply();
				
				cam.targetTexture = null;
				RenderTexture.active = null;
			}
			
			result = screenShot.EncodeToPNG();
			
			DestroyImmediate(rt);
			DestroyImmediate(screenShot);
			
			return result;
		}

		/// <summary>
		/// Encrypts a string in MD5.
		/// </summary>
		/// <returns>The encrypted string.</returns>
		/// <param name="inputString">Input string.</param>
		public static string EncryptMD5 (string inputString)
		{
			return encryption.EncryptMD5 (inputString);
		}

		/// <summary>
		/// Encrypts a string in SHA1.
		/// </summary>
		/// <returns>The encrypted string.</returns>
		/// <param name="inputString">Input string.</param>
		public static string EncryptSHA1 (string inputString)
		{
			return encryption.EncryptSHA1 (inputString);
		}

		/// <summary>
		/// Encrypts a string in AES.
		/// </summary>
		/// <returns>The encrypted string.</returns>
		/// <param name="inputString">Input string.</param>
		public static string EncryptAES (string inputString)
		{
			return encryption.EncryptAES (inputString);
		}

		/// <summary>
		/// Decrypts a string in AES.
		/// </summary>
		/// <returns>The decrypted string.</returns>
		/// <param name="inputString">Input string.</param>
		public static string DecryptAES (string inputString)
		{
			return encryption.DecryptAES (inputString);
		}

		/// <summary>
		/// Sets the local user. For internal use only (e.g. User.Authenticate), it's not recommended to call this method directly.
		/// </summary>
		/// <param name="user">User.</param>
		public virtual void SetLocalUser (User user)
		{
			_platform.SetLocalUser(user);
		}

        #endregion

        #region Client/Server initialization and utilities

        /// <summary>
        /// Initializes the connection to Combu server: retrieves the RSA Public Key generated from the server, then sends the AES Key and IV generated from the client (encrypted by RSA).
        /// </summary>
        void InitializeServer (Action<bool> callback)
		{
			if (string.IsNullOrEmpty (appId) || string.IsNullOrEmpty (appSecret)) {
				Debug.LogError ("Combu initialization failed: missing App Id or Secret Key");
				return;
			}
			Debug.Log ("Connecting to Combu server...");
			WWWForm form = new WWWForm();
            form.AddField("action", "token");
            form.AddField("version", COMBU_VERSION);
			CallWebservice(GetUrl("server.php"), form, (string text, string error) => {
				string token = string.Empty, xml = string.Empty, modulus = string.Empty, exponent = string.Empty;
				if (string.IsNullOrEmpty(error) && !string.IsNullOrEmpty(text))
				{
					Hashtable data = text.hashtableFromJson();
					if (data != null)
					{
						if (data.ContainsKey("token") && !string.IsNullOrEmpty("" + data["token"])) {
							token = data["token"].ToString().Trim();
						}
						if (data.ContainsKey("xml") && !string.IsNullOrEmpty("" + data["xml"])) {
							xml = data["xml"].ToString().Trim();
						}
						if (data.ContainsKey("modulus") && !string.IsNullOrEmpty("" + data["modulus"])) {
							modulus = data["modulus"].ToString().Trim();
						}
						if (data.ContainsKey("exponent") && !string.IsNullOrEmpty("" + data["exponent"])) {
							exponent = data["exponent"].ToString().Trim();
						}
					}
				}
				if (!string.IsNullOrEmpty(token) && (!string.IsNullOrEmpty(xml) || (!string.IsNullOrEmpty(modulus) && !string.IsNullOrEmpty(exponent))))
				{
					if (!string.IsNullOrEmpty(xml)) {
						encryption.LoadRSA (token, xml);
					} else {
						encryption.LoadRSA (token, modulus, exponent);
					}
					form = new WWWForm();
					form.AddField("action", "authorize");
					form.AddField("token", token);
					form.AddField("app", encryption.EncryptRSA(appId));
					form.AddField("secret", encryption.EncryptRSA(appSecret));
					form.AddField("key", encryption.EncryptRSA(encryption.Key));
					form.AddField("iv", encryption.EncryptRSA(encryption.IV));
					CallWebservice(GetUrl("server.php"), form, (string textAuthorize, string errorAuthorize) => {
						bool successAuthorize = false;
						if (string.IsNullOrEmpty(errorAuthorize) && !string.IsNullOrEmpty(textAuthorize)) {
							if (string.IsNullOrEmpty(errorAuthorize) && !string.IsNullOrEmpty(textAuthorize))
							{
								var result = textAuthorize.hashtableFromJson();
								if (result != null) {
									if (result.ContainsKey("success")) {
										if (!bool.TryParse(result["success"].ToString(), out successAuthorize))
											successAuthorize = false;
									}
									if (!successAuthorize && result.ContainsKey("message"))
										errorAuthorize = result["message"] + "";
								}
							}
						}
						if (!successAuthorize) {
							Debug.LogError("Failed authorization from Combu server" + (string.IsNullOrEmpty(errorAuthorize) ? "" : ": " + errorAuthorize));
                            if (callback != null)
                                callback(false);
						} else {
							// Get the server info
							GetServerInfo((bool success, CombuServerInfo loadedInfo) => {
								if (success)
								{
									_serverInfo = loadedInfo;
									Debug.Log(_serverInfo.ToString());
								}
								else
								{
									_serverInfo = new CombuServerInfo();
									Debug.LogError("Failed to get Combu server info");
								}
                                if (callback != null)
                                    callback(success);
                            });
						}
					});
				}
				else
				{
                    Debug.LogError("Failed connection to Combu server" + (string.IsNullOrEmpty(error) ? "" : ": " + error));
                    if (callback != null)
                        callback(false);
				}
			});
        }

        /// <summary>
        /// Gets the server info.
        /// </summary>
        /// <param name="callback">Callback.</param>
        public virtual void GetServerInfo(Action<bool, CombuServerInfo> callback)
        {
            var form = CreateForm();
            form.AddField("action", "info");
            CallWebservice(GetUrl("server.php"), form, (string text, string error) => {
                bool success = false;
                Hashtable data = new Hashtable();
                if (string.IsNullOrEmpty(error) && !string.IsNullOrEmpty(text))
                {
                    data = text.hashtableFromJson();
                    if (data != null)
                        success = true;
                }
                CombuServerInfo info = null;
                if (success)
                    info = new CombuServerInfo(data);
                if (callback != null)
                    callback(success, info);
            });
        }

        /// <summary>
        /// Ping the server.
        /// </summary>
        /// <param name="onlyIfAuthenticated">If set to <c>true</c> then it runs only if local user is authenticated.</param>
        /// <param name="callback">Callback.</param>
        public virtual void Ping(bool onlyIfAuthenticated = true, Action<bool> callback = null)
        {
            // Skip if CombuManager is not initialized
            if (!isInitialized)
                return;
            // If requested, do pings only if local user is authenticated
            if (onlyIfAuthenticated && !isAuthenticated)
            {
                if (callback != null)
                    callback(false);
                return;
            }
            var form = CreateForm();
            form.AddField("action", "ping");
            CallWebservice(GetUrl("server.php"), form, (string text, string error) => {
                bool success = false;
                Hashtable data = new Hashtable();
                if (string.IsNullOrEmpty(error) && !string.IsNullOrEmpty(text))
                {
                    data = text.hashtableFromJson();
                    if (data != null)
                    {
                        success = bool.Parse(data["success"].ToString());
                        if (localUser.authenticated)
                        {
                            if (success)
                            {
                                User user = new User(data["message"].ToString());
                                if (user.idLong > 0)
                                {
                                    (_platform.localUser as User).FromUser(user);
                                    encryption.SetToken(user.sessionToken);
                                }
                            }
                        }
                    }
                }
                if (callback != null)
                    callback(success);
            });
        }

        #endregion
	}
}
