using UnityEngine;

namespace Yodo1.MAS
{
    public class Yodo1U3dMas
    {
        public static readonly string TAG = "[YodoMas] ";

        public delegate void InitializeDelegate(bool success, Yodo1U3dAdError error);
        [System.Obsolete("Please use `Yodo1U3dMasCallback.OnSdkInitializedEvent` instead.\n" +
            "Yodo1U3dMasCallback.OnSdkInitializedEvent += (success, error) => { };")]
        public static void SetInitializeDelegate(InitializeDelegate initializeDelegate)
        {
            Yodo1U3dMasCallback.SetInitializeDelegate(initializeDelegate);
        }

        #region Ad Delegates

        //InterstitialAd of delegate
        public delegate void InterstitialAdDelegate(Yodo1U3dAdEvent adEvent, Yodo1U3dAdError adError);
        [System.Obsolete("Please use `Yodo1U3dMasCallback.Interstitial` instead.\n" +
            "Yodo1U3dMasCallback.Interstitial.OnAdOpenedEvent += OnInterstitialAdOpenedEvent;\n" +
            "Yodo1U3dMasCallback.Interstitial.OnAdClosedEvent += OnInterstitialAdClosedEvent;\n" +
            "Yodo1U3dMasCallback.Interstitial.OnAdErrorEvent += OnInterstitialAdErorEvent;")]
        public static void SetInterstitialAdDelegate(InterstitialAdDelegate interstitialAdDelegate)
        {
            Yodo1U3dMasCallback.SetInterstitialAdDelegate(interstitialAdDelegate);
        }

        //BannerAd of delegate
        public delegate void BannerdAdDelegate(Yodo1U3dAdEvent adEvent, Yodo1U3dAdError error);
        [System.Obsolete("Please use ad event in `Yodo1U3dBannerAdView` instead. You can get details from here https://developers.yodo1.com/knowledge-base/unity-integration/", false)]
        public static void SetBannerAdDelegate(BannerdAdDelegate bannerAdDelegate)
        {
            Yodo1U3dMasCallback.SetBannerAdDelegate(bannerAdDelegate);
        }

        //RewardVideo of delegate
        public delegate void RewardedAdDelegate(Yodo1U3dAdEvent adEvent, Yodo1U3dAdError error);
        [System.Obsolete("Please use `Yodo1U3dMasCallback.Rewarded` instead.\n" +
            "Yodo1U3dMasCallback.Rewarded.OnAdOpenedEvent += OnRewardedAdOpenedEvent;\n" +
            "Yodo1U3dMasCallback.Rewarded.OnAdClosedEvent += OnRewardedAdClosedEvent;\n" +
            "Yodo1U3dMasCallback.Rewarded.OnAdErrorEvent += OnRewardedAdErorEvent;\n" +
            "Yodo1U3dMasCallback.Rewarded.OnAdReceivedRewardEvent += OnAdReceivedRewardEvent;")]
        public static void SetRewardedAdDelegate(RewardedAdDelegate rewardedAdDelegate)
        {
            Yodo1U3dMasCallback.SetRewardedAdDelegate(rewardedAdDelegate);
        }

        #endregion

        /// <summary>
        /// Initialize the default instance of Yodo1 MAS SDK.
        /// </summary>
        public static void InitializeSdk()
        {
            if (Yodo1U3dMasCallback.isInitialized())
            {
                Debug.LogWarning(Yodo1U3dMas.TAG + "The SDK has been initialized, please do not initialize the SDK repeatedly.");
#if UNITY_EDITOR
                Yodo1EditorAds.InitializeAds();
                var typeEditor = typeof(Yodo1U3dMasCallback);
                if (!GameObject.FindObjectOfType<Yodo1U3dMasCallback>())
                {
                    var sdkObjEditor = new GameObject("Yodo1U3dMasCallback", typeEditor).GetComponent<Yodo1U3dMasCallback>(); // Its Awake() method sets Instance.
                    if (Yodo1U3dMasCallback.Instance != sdkObjEditor)
                    {
                        Debug.LogError(Yodo1U3dMas.TAG + "It looks like you have the " + typeEditor.Name + " on a GameObject in your scene. Please remove the script from your scene.");
                        return;
                    }
                }
                Yodo1U3dMasCallback.ForwardEvent("onSdkInitializedEvent");
#endif
                return;
            }

            var type = typeof(Yodo1U3dMasCallback);
            var sdkObj = new GameObject("Yodo1U3dMasCallback", type).GetComponent<Yodo1U3dMasCallback>(); // Its Awake() method sets Instance.
            if (Yodo1U3dMasCallback.Instance != sdkObj)
            {
                Debug.LogError(Yodo1U3dMas.TAG + "It looks like you have the " + type.Name + " on a GameObject in your scene. Please remove the script from your scene.");
                return;
            }

            Yodo1AdSettings settings = Resources.Load("Yodo1/Yodo1AdSettings", typeof(Yodo1AdSettings)) as Yodo1AdSettings;
            if (settings == null)
            {
                Debug.LogError(Yodo1U3dMas.TAG + "The SDK has not been initialized yet. The Yodo1AdSettings is missing.");
                return;
            }

            string appKey = string.Empty;
#if UNITY_ANDROID
            appKey = settings.androidSettings.AppKey.Trim();
#elif UNITY_IOS
            appKey = settings.iOSSettings.AppKey.Trim();
#endif
            Debug.Log(Yodo1U3dMas.TAG + "The SDK is initializing, the app key is " + appKey);
            Yodo1U3dMas.InitWithAppKey(appKey);

#if UNITY_EDITOR
            Yodo1U3dMas.InitializeDelegate initializeDelegate = Yodo1U3dMasCallback.GetInitializeDelegate();
            if (initializeDelegate != null)
            {
                initializeDelegate(true, null);
            }
#endif
            Yodo1U3dMasCallback.PrintAutoGameInfo();
        }

        /// <summary>
        /// Initialize with app key.
        /// </summary>
        /// <param name="appKey">The app key obtained from MAS Developer Platform.</param>
        static void InitWithAppKey(string appKey)
        {
            if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
#if UNITY_IPHONE
                Yodo1U3dAdsIOS.InitWithAppKey(appKey);
#endif
            }
            else if (Application.platform == RuntimePlatform.Android)
            {
#if UNITY_ANDROID
                Yodo1U3dAdsAndroid.InitWithAppKey(appKey);
#endif
            }
        }

        #region Privacy Methods
        /// <summary>
        /// MAS SDK requires that publishers set a flag indicating whether a user located in the European Economic Area (i.e., EEA/GDPR data subject) has provided opt-in consent for the collection and use of personal data.
        /// If the user has consented, please set the flag to true. If the user has not consented, please set the flag to false.
        ///
        /// This method must be called before InitializeSdk method.
        /// </summary>
        /// <param name="consent"></param>
        public static void SetGDPR(bool consent)
        {
            if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
#if UNITY_IPHONE
                Yodo1U3dAdsIOS.SetUserConsent(consent);
#endif
            }
            else if (Application.platform == RuntimePlatform.Android)
            {
#if UNITY_ANDROID
                Yodo1U3dAdsAndroid.SetUserConsent(consent);
#endif
            }
        }

        public static bool IsGDPRUserConsent()
        {
            bool ret = false;
            if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
#if UNITY_IPHONE
                ret = Yodo1U3dAdsIOS.IsGDPRUserConsent();
#endif
            }
            else if (Application.platform == RuntimePlatform.Android)
            {
#if UNITY_ANDROID
                ret = Yodo1U3dAdsAndroid.IsGDPRUserConsent();
#endif
            }
            return ret;
        }

        /// <summary>
        /// To ensure COPPA, GDPR, and Google Play policy compliance, you should indicate whether a user is a child.
        /// If the user is known to be in an age-restricted category (i.e., under the age of 13) please set the flag to true.
        /// If the user is known to not be in an age-restricted category (i.e., age 13 or older) please set the flag to false.
        ///
        /// This method must be called before InitializeSdk method.
        /// </summary>
        /// <param name="underAgeOfConsent"></param>
        public static void SetCOPPA(bool underAgeOfConsent)
        {
            if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
#if UNITY_IPHONE
                Yodo1U3dAdsIOS.SetTagForUnderAgeOfConsent(underAgeOfConsent);
#endif
            }
            else if (Application.platform == RuntimePlatform.Android)
            {
#if UNITY_ANDROID
                Yodo1U3dAdsAndroid.SetTagForUnderAgeOfConsent(underAgeOfConsent);
#endif
            }
        }

        public static bool IsCOPPAAgeRestricted()
        {
            bool ret = false;
            if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
#if UNITY_IPHONE
                ret = Yodo1U3dAdsIOS.IsCOPPAAgeRestricted();
#endif
            }
            else if (Application.platform == RuntimePlatform.Android)
            {
#if UNITY_ANDROID
                ret = Yodo1U3dAdsAndroid.IsCOPPAAgeRestricted();
#endif
            }
            return ret;
        }

        /// <summary>
        /// Publishers may choose to display a "Do Not Sell My Personal Information" link.
        /// Such publishers may choose to set a flag indicating whether a user located in California, USA has opted to not have their personal data sold.
        ///
        /// This method must be called before InitializeSdk method.
        /// </summary>
        /// <param name="doNotSell"></param>
        public static void SetCCPA(bool doNotSell)
        {
            if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
#if UNITY_IPHONE
                Yodo1U3dAdsIOS.SetDoNotSell(doNotSell);
#endif
            }
            else if (Application.platform == RuntimePlatform.Android)
            {
#if UNITY_ANDROID
                Yodo1U3dAdsAndroid.SetDoNotSell(doNotSell);
#endif
            }
        }

        public static bool IsCCPADoNotSell()
        {
            bool ret = false;
            if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
#if UNITY_IPHONE
                ret = Yodo1U3dAdsIOS.IsCCPADoNotSell();
#endif
            }
            else if (Application.platform == RuntimePlatform.Android)
            {
#if UNITY_ANDROID
                ret = Yodo1U3dAdsAndroid.IsCCPADoNotSell();
#endif
            }
            return ret;
        }
        #endregion

        #region AdBuildConfig
        public static void SetAdBuildConfig(Yodo1AdBuildConfig yodo1AdBuildConfig)
        {
            if (yodo1AdBuildConfig == null)
            {
                return;
            }
            string configStr = yodo1AdBuildConfig.toJson();

            if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
#if UNITY_IPHONE
                Yodo1U3dAdsIOS.SetAdBuildConfig(configStr);
#endif
            }
            else if (Application.platform == RuntimePlatform.Android)
            {
#if UNITY_ANDROID
                Yodo1U3dAdsAndroid.SetAdBuildConfig(configStr);
#endif
            }
        }
        #endregion

        /// <summary>
        /// Pausing game when interstitial or reward video ads are playing.
        ///
        /// Note: This method is work for Android and Unity editor, the default value is True.
        /// The pausing of audios in game is only possible if the developers have used Unity's default audio source.
        /// If the developers have handled audio separately, pausing of audios will not happen, and the developers will have to handle game pause according to their code.
        /// Pausing of physics, animations etc will function normally.
        /// </summary>
        /// <param name="pauseGame"><c>true</c>, the game will be paused when the interstitial or reward video ads are playing, <c>false</c> otherwise.</param>
        public static void SetAutoPauseGame(bool autoPauseGame)
        {
            Yodo1U3dMasCallback.SetAutoPauseGame(autoPauseGame);
        }

        #region Banner Ad Methods
        /// <summary>
        /// Whether the banner ads have been loaded.
        /// </summary>
        /// <returns><c>true</c>, if the banner ads have been loaded, <c>false</c> otherwise.</returns>
        //[System.Obsolete("After the ShowBanner method is called, the banner ad will be displayed automatically when it is loaded successfully", true)]
        [System.Obsolete("Please use `Yodo1U3dBannerAdView` instead. You can get details from here https://developers.yodo1.com/knowledge-base/unity-integration/", true)]
        public static bool IsBannerAdLoaded()
        {
            if (!Yodo1U3dMasCallback.isInitialized())
            {
                Debug.LogError(Yodo1U3dMas.TAG + "The SDK has not been initialized yet. Please initialize the SDK first.");
                return false;
            }
            if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
#if UNITY_IPHONE
                return Yodo1U3dAdsIOS.IsBannerAdLoaded();
#endif
            }
            else if (Application.platform == RuntimePlatform.Android)
            {
#if UNITY_ANDROID
                return Yodo1U3dAdsAndroid.IsBannerAdLoaded();
#endif
            }
            return false;
        }

        /// <summary>
        /// Shows the banner ad.
        /// The banner ad will be displayed automatically after loaded.
        /// It is recommended that this method be called after the MAS SDK initialization successful.
        /// </summary>
        [System.Obsolete("Please use `Yodo1U3dBannerAdView` instead. You can get details from here https://developers.yodo1.com/knowledge-base/unity-integration/", false)]
        public static void ShowBannerAd()
        {
#if UNITY_EDITOR
            Yodo1EditorAds.ShowStamdardBannerAdsInEditor("EditorVersion1");
#endif
#if !UNITY_EDITOR
            if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
#if UNITY_IPHONE
                Yodo1U3dAdsIOS.ShowBannerAd();
#endif
            }
            else if (Application.platform == RuntimePlatform.Android)
            {
#if UNITY_ANDROID
                Yodo1U3dAdsAndroid.ShowBannerAd();
#endif
            }
#endif
        }

        /// <summary>
        /// Shows the banner ad.
        /// The banner ad will be displayed automatically after loaded.
        /// It is recommended that this method be called after the MAS SDK initialization successful.
        /// </summary>
        /// <param name="placementId">The ad placement</param>
        [System.Obsolete("Please use `Yodo1U3dBannerAdView` instead. You can get details from here https://developers.yodo1.com/knowledge-base/unity-integration/", false)]
        public static void ShowBannerAd(string placementId)
        {
#if UNITY_EDITOR
            Yodo1EditorAds.ShowStamdardBannerAdsInEditor("EditorVersion1");
#endif
            if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
#if UNITY_IPHONE
                Yodo1U3dAdsIOS.ShowBannerAd(placementId);
#endif
            }
            else if (Application.platform == RuntimePlatform.Android)
            {
#if UNITY_ANDROID
                Yodo1U3dAdsAndroid.ShowBannerAd(placementId);
#endif
            }
        }

        /// <summary>
        /// Shows the banner ad.
        /// The banner ad will be displayed automatically after loaded.
        /// It is recommended that this method be called after the MAS SDK initialization successful.
        /// </summary>
        /// <param name="align">The banner ad alignment</param>
        [System.Obsolete("Please use `Yodo1U3dBannerAdView` instead. You can get details from here https://developers.yodo1.com/knowledge-base/unity-integration/", false)]
        public static void ShowBannerAd(int align)
        {
#if UNITY_EDITOR
            Yodo1EditorAds.ShowBannerAdsInEditor("EditorVersion1", align, 0, 0, 0);
#endif
            if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
#if UNITY_IPHONE
                Yodo1U3dAdsIOS.ShowBannerAd(align);
#endif
            }
            else if (Application.platform == RuntimePlatform.Android)
            {
#if UNITY_ANDROID
                Yodo1U3dAdsAndroid.ShowBannerAd(align);
#endif
            }
        }

        /// <summary>
        /// Shows the banner ad.
        /// The banner ad will be displayed automatically after loaded.
        /// It is recommended that this method be called after the MAS SDK initialization successful.
        /// </summary>
        /// <param name="align">The banner ad position</param>
        /// <param name="offsetX">horizontal offset, offsetX > 0, offset right. offsetX < 0, offset left. if align = Yodo1Mas.BannerLeft, offsetX < 0 is invalid (Only Android)</param>
        /// <param name="offsetY">vertical offset, offsetY > 0, offset bottom. offsetY < 0, offset top.if align = Yodo1Mas.BannerTop, offsetY < 0 is invalid(Only Android)</param>
        [System.Obsolete("Please use `Yodo1U3dBannerAdView` instead. You can get details from here https://developers.yodo1.com/knowledge-base/unity-integration/", false)]
        public static void ShowBannerAd(int align, int offsetX, int offsetY)
        {
#if UNITY_EDITOR
            Yodo1EditorAds.ShowBannerAdsInEditor("EditorVersion1", align, 0, offsetX, offsetY);
#endif
            if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
#if UNITY_IPHONE
                Yodo1U3dAdsIOS.ShowBannerAd(align, offsetX, offsetY);
#endif
            }
            else if (Application.platform == RuntimePlatform.Android)
            {
#if UNITY_ANDROID
                Yodo1U3dAdsAndroid.ShowBannerAd(align, offsetX, offsetY);
#endif
            }
        }

        /// <summary>
        /// Shows the banner ad.
        /// The banner ad will be displayed automatically after loaded.
        /// It is recommended that this method be called after the MAS SDK initialization successful.
        /// </summary>
        /// <param name="placementId">The banner ad placement</param>
        /// <param name="align">The banner ad position</param>
        /// <param name="offsetX">horizontal offset, offsetX > 0, the banner will move to the right. offsetX < 0, the banner will move to the left. if align = Yodo1Mas.BannerLeft, offsetX < 0 is invalid (Only Android</param>
        /// <param name="offsetY">vertical offset, offsetY > 0, the banner will move to the bottom. offsetY < 0, the banner will move to the top. if align = Yodo1Mas.BannerTop, offsetY < 0 is invalid(Only Android)</param>
        [System.Obsolete("Please use `Yodo1U3dBannerAdView` instead. You can get details from here https://developers.yodo1.com/knowledge-base/unity-integration/", false)]
        public static void ShowBannerAd(string placementId, int align, int offsetX, int offsetY)
        {
#if UNITY_EDITOR
            Yodo1EditorAds.ShowBannerAdsInEditor("EditorVersion1", align, 0, offsetX, offsetY);
#endif
            if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
#if UNITY_IPHONE
                Yodo1U3dAdsIOS.ShowBannerAd(placementId, align, offsetX, offsetY);
#endif
            }
            else if (Application.platform == RuntimePlatform.Android)
            {
#if UNITY_ANDROID
                Yodo1U3dAdsAndroid.ShowBannerAd(placementId, align, offsetX, offsetY);
#endif
            }
        }

        /// <summary>
        /// Hide banner ads
        /// </summary>
        [System.Obsolete("Please use `Yodo1U3dBannerAdView` instead. You can get details from here https://developers.yodo1.com/knowledge-base/unity-integration/", false)]
        public static void DismissBannerAd()
        {
#if UNITY_EDITOR
            Yodo1EditorAds.HideBannerAdsInEditor("EditorVersion1");
#endif
            if (!Yodo1U3dMasCallback.isInitialized())
            {
                Debug.LogError(Yodo1U3dMas.TAG + "The SDK has not been initialized yet. Please initialize the SDK first.");
                return;
            }
            if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
#if UNITY_IPHONE
                Yodo1U3dAdsIOS.DismissBannerAd();
#endif
            }
            else if (Application.platform == RuntimePlatform.Android)
            {
#if UNITY_ANDROID
                Yodo1U3dAdsAndroid.DismissBannerAd();
#endif
            }
        }

        /// <summary>
        /// hide banner ads
        /// </summary>
        /// <param name="destroy">if destroy == true, the ads displayed in the next call to showBanner are different. if destroy == false, the ads displayed in the next call to showBanner are same</param>
        [System.Obsolete("Please use `Yodo1U3dBannerAdView` instead. You can get details from here https://developers.yodo1.com/knowledge-base/unity-integration/", false)]
        public static void DismissBannerAd(bool destroy)
        {
#if UNITY_EDITOR
            Yodo1EditorAds.HideBannerAdsInEditor("EditorVersion1");
#endif
            if (!Yodo1U3dMasCallback.isInitialized())
            {
                Debug.LogError(Yodo1U3dMas.TAG + "The SDK has not been initialized yet. Please initialize the SDK first.");
                return;
            }
            if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
#if UNITY_IPHONE
                Yodo1U3dAdsIOS.DismissBannerAd(destroy);
#endif
            }
            else if (Application.platform == RuntimePlatform.Android)
            {
#if UNITY_ANDROID
                Yodo1U3dAdsAndroid.DismissBannerAd(destroy);
#endif
            }
        }

        #endregion

        #region Interstitial Ad Methods
        /// <summary>
        /// Whether the interstitial ads have been loaded.
        /// </summary>
        /// <returns><c>true</c>, if the interstitial ads have been loaded complete, <c>false</c> otherwise.</returns>
        public static bool IsInterstitialAdLoaded()
        {
#if UNITY_EDITOR
            return true;
#endif
#if !UNITY_EDITOR
            if (!Yodo1U3dMasCallback.isInitialized())
            {
                Debug.LogError(Yodo1U3dMas.TAG + "The SDK has not been initialized yet. Please initialize the SDK first.");
                return false;
            }

#if UNITY_IPHONE
            return Yodo1U3dAdsIOS.IsInterstitialLoaded();
#elif UNITY_ANDROID
            return Yodo1U3dAdsAndroid.IsInterstitialLoaded();
#else
            return false;
#endif
#endif
        }

        /// <summary>
        /// Shows the interstitial ad.
        /// </summary>
        public static void ShowInterstitialAd()
        {
#if UNITY_EDITOR
            Yodo1EditorAds.ShowInterstitialAdsInEditor();
#endif
#if !UNITY_EDITOR
            if (!Yodo1U3dMasCallback.isInitialized())
            {
                Debug.LogError(Yodo1U3dMas.TAG + "The SDK has not been initialized yet. Please initialize the SDK first.");
                return;
            }

#if UNITY_IPHONE
            Yodo1U3dAdsIOS.ShowInterstitialAd();
#elif UNITY_ANDROID
            Yodo1U3dAdsAndroid.ShowInterstitialAd();
#endif
#endif
        }

        /// <summary>
        /// Shows the interstitial ad with placement id.
        /// </summary>
        /// <param name="placementId"></param>
        public static void ShowInterstitialAd(string placementId)
        {
#if UNITY_EDITOR
            Yodo1EditorAds.ShowInterstitialAdsInEditor();
#endif
            if (!Yodo1U3dMasCallback.isInitialized())
            {
                Debug.LogError(Yodo1U3dMas.TAG + "The SDK has not been initialized yet. Please initialize the SDK first.");
                return;
            }

#if UNITY_IPHONE
            Yodo1U3dAdsIOS.ShowInterstitialAd(placementId);
#elif UNITY_ANDROID
            Yodo1U3dAdsAndroid.ShowInterstitialAd(placementId);
#endif
        }

        #endregion

        #region Rewarded Ad Methods
        /// <summary>
        /// Whether the reward video ads have been loaded.
        /// </summary>
        /// <returns><c>true</c>, if the reward video ads have been loaded complete, <c>false</c> otherwise.</returns>
        public static bool IsRewardedAdLoaded()
        {
#if UNITY_EDITOR
            return true;
#endif
#if !UNITY_EDITOR
            if (!Yodo1U3dMasCallback.isInitialized())
            {
                Debug.LogError(Yodo1U3dMas.TAG + "The SDK has not been initialized yet. Please initialize the SDK first.");
                return false;
            }

#if UNITY_IPHONE
            return Yodo1U3dAdsIOS.IsRewardedAdLoaded();
#elif UNITY_ANDROID
            return Yodo1U3dAdsAndroid.IsRewardedAdLoaded();
#else
            return false;
#endif

#endif
        }

        /// <summary>
        /// Shows the reward video ad.
        /// </summary>
        public static void ShowRewardedAd()
        {

#if UNITY_EDITOR
            Yodo1EditorAds.ShowRewardedVideodsInEditor();
#endif

#if !UNITY_EDITOR
            if (!Yodo1U3dMasCallback.isInitialized())
            {
                Debug.LogError(Yodo1U3dMas.TAG + "The SDK has not been initialized yet. Please initialize the SDK first.");
                return;
            }

#if UNITY_IPHONE
            Yodo1U3dAdsIOS.ShowRewardedAd();
#elif UNITY_ANDROID
            Yodo1U3dAdsAndroid.ShowRewardedAd();
#endif
#endif
        }

        /// <summary>
        /// Shows the reward video ad.
        /// </summary>
        /// <param name="placementId"></param>
        public static void ShowRewardedAd(string palcementId)
        {
#if UNITY_EDITOR
            Yodo1EditorAds.ShowRewardedVideodsInEditor();
#endif
            if (!Yodo1U3dMasCallback.isInitialized())
            {
                Debug.LogError(Yodo1U3dMas.TAG + "The SDK has not been initialized yet. Please initialize the SDK first.");
                return;
            }

#if UNITY_IPHONE
            Yodo1U3dAdsIOS.ShowRewardedAd(palcementId);
#elif UNITY_ANDROID
            Yodo1U3dAdsAndroid.ShowRewardedAd(palcementId);
#endif
        }
        #endregion
    }
}