using System;
using System.Collections.Generic;
using UnityEngine;

#if (UNITY_ANDROID || UNITY_IOS) && UM_GOOGLE_ADMOB
using GoogleMobileAds.Api;
#endif

/*

namespace SA.CrossPlatform.Advertisement {
	
	public class UM_GoogleAdsClient : UM_iAdsClient {

		private static event Action s_onConnect = delegate {};
		
		public static event EventHandler<EventArgs> OnBannerAdLoaded = delegate {};
		public static event EventHandler<AdFailedToLoadEventArgs> OnBannerAdFailedToLoad = delegate {};
		public static event EventHandler<EventArgs> OnBannerAdOpening = delegate {};
		public static event EventHandler<EventArgs> OnBannerAdClosed = delegate {};
		public static event EventHandler<EventArgs> OnBannerAdLeavingApplication = delegate {};
	    
		public static event EventHandler<EventArgs> OnInterstitialAdLoaded = delegate {};
		public static event EventHandler<AdFailedToLoadEventArgs> OnInterstitialAdFailedToLoad = delegate {};
		public static event EventHandler<EventArgs> OnInterstitialAdOpening = delegate {};
		public static event EventHandler<EventArgs> OnInterstitialAdClosed = delegate {};
		public static event EventHandler<EventArgs> OnInterstitialAdLeavingApplication = delegate {};
		
		public static event EventHandler<EventArgs> OnRewardBasedVideoAdLoaded = delegate {};
		public static event EventHandler<AdFailedToLoadEventArgs> OnRewardBasedVideoAdFailedToLoad = delegate {};
		public static event EventHandler<EventArgs> OnRewardBasedVideoAdOpening = delegate {};
		public static event EventHandler<EventArgs> OnRewardBasedVideoAdStarted = delegate {};
		public static event EventHandler<EventArgs> OnRewardBasedVideoAdClosed = delegate {};
		public static event EventHandler<Reward> OnRewardBasedVideoAdRewarded = delegate {};
		public static event EventHandler<EventArgs> OnRewardBasedVideoAdLeavingApplication = delegate {};
		
		private static bool m_isConnectionInProgress = false;
		private static bool m_isInited = false;
		
		//#if (UNITY_ANDROID || UNITY_IOS) && AN_GOOGLE_ADMOB
		private BannerView Banner { get; set; }
		private InterstitialAd Interstitial { get; set; }
		private RewardBasedVideoAd RewardBasedVideo { get; set; }
		//#endif
		
		/// <summary>
		/// Initializing Google AdMob client
		/// </summary>
		/// <param name="callback"></param>
		public void Init(Action callback) {
			//#if (UNITY_ANDROID || UNITY_IOS) && AN_GOOGLE_ADMOB
			if (m_isInited) {
				callback.Invoke();
				return;
			}

			s_onConnect += callback;
			if (m_isConnectionInProgress) {
				return;
			}
			
			m_isConnectionInProgress = true;
			
			#if (UNITY_IOS && AN_GOOGLE_ADMOB)
				MobileAds.SetiOSAppPauseOnBackground(true);
			#endif
			
			// Initialize the Google Mobile Ads SDK.
			MobileAds.Initialize("ca-app-pub-3940256099942544~3347511713");
			InitRewardedBasedVideoAd();

			m_isInited = true;
			m_isConnectionInProgress = false;
			s_onConnect.Invoke();
			s_onConnect = delegate {};
			//#endif
		}

		/// <summary>
		/// Create banner
		/// </summary>
		public void CreateBanner() {
			InitBanner();
		}

		/// <summary>
		/// Destroys banner
		/// </summary>
		public void DestroyBanner() {
			//#if (UNITY_ANDROID || UNITY_IOS) && AN_GOOGLE_ADMOB
			if (Banner != null) {
				Banner.Destroy();
			}
			//#endif
		}
	    
		/// <summary>
		/// Shows banner
		/// </summary>
		public void ShowBanner() {
			//#if (UNITY_ANDROID || UNITY_IOS) && AN_GOOGLE_ADMOB
			if (Banner != null) {
				Banner.Show();
			}
			//#endif
		}
	    
		/// <summary>
		/// Hides banner
		/// </summary>
		public void HideBanner() {
			//#if (UNITY_ANDROID || UNITY_IOS) && AN_GOOGLE_ADMOB
			if (Banner != null) {
				Banner.Hide();
			}
			//#endif
		}
		
		/// <summary>
		/// Initializing banner with specified settings(AdRequest)
		/// </summary>
		private void InitBanner() {
			//#if (UNITY_ANDROID || UNITY_IOS) && AN_GOOGLE_ADMOB
			AdSize size = UM_GoogleAdMobSettings.Size;
			AdPosition position = UM_GoogleAdMobSettings.Position;
			    
			// Clean up banner ad before creating a new one.
			DestroyBanner();

			Banner = new BannerView(BannerId, size, position);

			Banner.OnAdLoaded += HandleBannerAdLoaded;
			Banner.OnAdFailedToLoad += HandleBannerAdFailedToLoad;
			Banner.OnAdOpening += HandleBannerAdOpened;
			Banner.OnAdClosed += HandleBannerAdClosed;
			Banner.OnAdLeavingApplication += HandleBannerAdLeftApplication;

			// Load a banner ad.
			Banner.LoadAd(CreateAdRequest());
			//#endif
		}

		//#if (UNITY_ANDROID || UNITY_IOS) && AN_GOOGLE_ADMOB
		/// <summary>
		/// Create Ads request with specified settings
		/// </summary>
		private AdRequest CreateAdRequest() {
			return new AdRequest.Builder()
				.AddTestDevice(UM_GoogleAdMobSettings.TEST_DEVICE_ID)
				.AddKeyword(UM_GoogleAdMobSettings.Keywords[0])
				.SetGender(UM_GoogleAdMobSettings.Gender)
				.SetBirthday(UM_GoogleAdMobSettings.BirthdayTime)
				.TagForChildDirectedTreatment(UM_GoogleAdMobSettings.TagForChildDirectedTreatment)
				.Build();
		}
		//#endif
		
		#region Banner callback handlers

		private static void HandleBannerAdLoaded(object sender, EventArgs args) {
			OnBannerAdLoaded(sender, args);
		}

		//#if (UNITY_ANDROID || UNITY_IOS) && AN_GOOGLE_ADMOB
		private static void HandleBannerAdFailedToLoad(object sender, AdFailedToLoadEventArgs args) {
			OnBannerAdFailedToLoad(sender, args);
		}
		//#endif
		
		private static void HandleBannerAdOpened(object sender, EventArgs args) {
			OnBannerAdOpening(sender, args);
		}

		private static void HandleBannerAdClosed(object sender, EventArgs args) {
			OnBannerAdClosed(sender, args);
		}

		private static void HandleBannerAdLeftApplication(object sender, EventArgs args) {
			OnBannerAdLeavingApplication(sender, args);
		}

		#endregion
		
		/// <summary>
		/// Creates Interstitial Ad
		/// </summary>
		public void CreateInterstitial() {
			InitInterstitial();
		}
		
		/// <summary>
		/// Destroys Interstitial Ad
		/// </summary>
		public void DestroyInterstitial() {
			//#if (UNITY_ANDROID || UNITY_IOS) && AN_GOOGLE_ADMOB
			if (Interstitial != null) {
				Interstitial.Destroy();
			}
			//#endif
		}
		
		/// <summary>
		/// Shows Interstitial Ad
		/// </summary>
		public void ShowInterstitial() {
			//#if (UNITY_ANDROID || UNITY_IOS) && AN_GOOGLE_ADMOB
			if (IsInterstitialReady()) {
				Interstitial.Show();
			}
			else {
				Debug.Log("Interstitial is not ready yet");
			}
			//#endif
		}
		
		/// <summary>
		/// Returns result is Interstitial Ad is ready
		/// </summary>
		public bool IsInterstitialReady() {
			//#if (UNITY_ANDROID || UNITY_IOS) && AN_GOOGLE_ADMOB
			return Interstitial.IsLoaded();
			//#else
			return false;
			//#endif
		}
	    
		/// <summary>
		/// Initialized Interstitial Ad with specified settings(AdRequest)
		/// </summary>
		private void InitInterstitial() {
			//#if (UNITY_ANDROID || UNITY_IOS) && AN_GOOGLE_ADMOB
			// Clean up interstitial ad before creating a new one.
			DestroyInterstitial();

			// Create an interstitial.
			Interstitial = new InterstitialAd(InterstitialId);
			
			// Register for ad events.
			Interstitial.OnAdLoaded += HandleInterstitialLoaded;
			Interstitial.OnAdFailedToLoad += HandleInterstitialFailedToLoad;
			Interstitial.OnAdOpening += HandleInterstitialOpened;
			Interstitial.OnAdClosed += HandleInterstitialClosed;
			Interstitial.OnAdLeavingApplication += HandleInterstitialLeftApplication;
		    
			// Load an interstitial ad.
			Interstitial.LoadAd(CreateAdRequest());
			//#endif
		}

		#region Interstitial callback handlers

		private static void HandleInterstitialLoaded(object sender, EventArgs args) {
			OnInterstitialAdLoaded(sender, args);
		}

		//#if (UNITY_ANDROID || UNITY_IOS) && AN_GOOGLE_ADMOB
		private static void HandleInterstitialFailedToLoad(object sender, AdFailedToLoadEventArgs args) {
			OnInterstitialAdFailedToLoad(sender, args);
		}
		//#endif

		private static void HandleInterstitialOpened(object sender, EventArgs args) {
			OnInterstitialAdOpening(sender, args);
		}

		private static void HandleInterstitialClosed(object sender, EventArgs args) {
			OnInterstitialAdClosed(sender, args);
		}

		private static void HandleInterstitialLeftApplication(object sender, EventArgs args) {
			OnInterstitialAdLeavingApplication(sender, args);
		}

		#endregion
	
		/// <summary>
		/// Creates Rewarded based video ad
		/// </summary>
		public void CreateRewardedBasedVideo() {
			//#if (UNITY_ANDROID || UNITY_IOS) && AN_GOOGLE_ADMOB
			RewardBasedVideo.LoadAd(CreateAdRequest(), RewardedVideoId);
			//#endif
		}
	    
		/// <summary>
		/// Shows Rewarded based video ad
		/// </summary>
		public void ShowRewardedBasedVideoAdVideo() {
			//#if (UNITY_ANDROID || UNITY_IOS) && AN_GOOGLE_ADMOB
			if (IsRewardedBasedVideoAdReady()) {
				RewardBasedVideo.Show();
			}
			else {
				Debug.Log("Reward based video ad is not ready yet");
			}
			//#endif
		}
		
		/// <summary>
		/// Returns result is Rewarded based video Ad is ready
		/// </summary>
		public bool IsRewardedBasedVideoAdReady() {
			//#if (UNITY_ANDROID || UNITY_IOS) && AN_GOOGLE_ADMOB
			return RewardBasedVideo.IsLoaded();
			//#endif
		}
		
		/// <summary>
		/// Get singleton reward based video ad reference.
		/// RewardBasedVideoAd is a singleton, so handlers should only be registered once.
		/// </summary>
		private void InitRewardedBasedVideoAd() {
			//#if (UNITY_ANDROID || UNITY_IOS) && AN_GOOGLE_ADMOB
				RewardBasedVideo = RewardBasedVideoAd.Instance;
				RegisterRewardedBasedVideoEvents();
			//#endif
		}
		
		/// <summary>
		/// RewardBasedVideoAd is a singleton, so handlers should only be registered once.
		/// </summary>
		private void RegisterRewardedBasedVideoEvents() {
			//#if (UNITY_ANDROID || UNITY_IOS) && AN_GOOGLE_ADMOB
			RewardBasedVideo.OnAdLoaded += HandleRewardBasedVideoLoaded;
			RewardBasedVideo.OnAdFailedToLoad += HandleRewardBasedVideoFailedToLoad;
			RewardBasedVideo.OnAdOpening += HandleRewardBasedVideoOpened;
			RewardBasedVideo.OnAdStarted += HandleRewardBasedVideoStarted;
			RewardBasedVideo.OnAdRewarded += HandleRewardBasedVideoRewarded;
			RewardBasedVideo.OnAdClosed += HandleRewardBasedVideoClosed;
			RewardBasedVideo.OnAdLeavingApplication += HandleRewardBasedVideoLeftApplication;
			//#endif
		}
		
		#region RewardBasedVideo callback handlers 

		private void HandleRewardBasedVideoLoaded(object sender, EventArgs args) {
			OnRewardBasedVideoAdLoaded(sender, args);
		}

		//#if (UNITY_ANDROID || UNITY_IOS) && AN_GOOGLE_ADMOB
		private void HandleRewardBasedVideoFailedToLoad(object sender, AdFailedToLoadEventArgs args) {
			OnRewardBasedVideoAdFailedToLoad(sender, args);
		}
		//#endif

		private void HandleRewardBasedVideoOpened(object sender, EventArgs args) {
			OnRewardBasedVideoAdOpening(sender, args);
		}

		private void HandleRewardBasedVideoStarted(object sender, EventArgs args) {
			OnRewardBasedVideoAdStarted(sender, args);
		}

		private void HandleRewardBasedVideoClosed(object sender, EventArgs args) {
			OnRewardBasedVideoAdClosed(sender, args);
		}

		private void HandleRewardBasedVideoRewarded(object sender, Reward args) {
			OnRewardBasedVideoAdRewarded(sender, args);
		}

		private void HandleRewardBasedVideoLeftApplication(object sender, EventArgs args) {
			OnRewardBasedVideoAdLeavingApplication(sender, args);
		}

		#endregion
		
		public string AppId {
			get {
				#if UNITY_EDITOR
					return "unexpected_value";
				#elif UNITY_ANDROID
					return UM_GoogleAdMobSettings.ANDROID_ADMOB_APP_ID;
				#elif UNITY_IPHONE
					return UM_GoogleAdMobSettings.IOS_ADMOB_APP_ID;
				#else
					return "unexpected_value";
				#endif
			}
		}
		
		public string BannerId {
			get {
				#if UNITY_EDITOR
					return "unexpected_value";
				#elif UNITY_ANDROID
					return UM_GoogleAdMobSettings.ANDROID_BANNERS_UNIT_ID;
				#elif UNITY_IPHONE
					return UM_GoogleAdMobSettings.IOS_BANNERS_UNIT_ID;
				#else
					return "unexpected_value";
				#endif
			}
		}

		public string InterstitialId {
			get {
				#if UNITY_EDITOR
					return "unexpected_value";
				#elif UNITY_ANDROID
					return UM_GoogleAdMobSettings.ANDROID_INTERSTITIAL_UNIT_ID;
				#elif UNITY_IPHONE
					return UM_GoogleAdMobSettings.IOS_INTERSTITIAL_UNIT_ID;
				#else
					return "unexpected_value";
				#endif
			}
		}

		public string RewardedVideoId {
			get {
				#if UNITY_EDITOR
					return "unexpected_value";
				#elif UNITY_ANDROID
					return UM_GoogleAdMobSettings.ANDROID_REWARDED_UNIT_ID;
				#elif UNITY_IPHONE
					return UM_GoogleAdMobSettings.IOS_REWARDED_UNIT_ID;
				#else
					return "unexpected_value";
				#endif
			}
		}
	}
}

    */