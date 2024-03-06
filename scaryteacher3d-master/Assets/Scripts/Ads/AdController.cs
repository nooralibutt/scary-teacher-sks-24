using System.Collections;
using EasyMobile;
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
using UnityEngine;
using CC;

public class AdController : MonoBehaviour
{
    //ios,android: ca-app-pub-9376647217016516~1550475186,ca-app-pub-9376647217016516~5124503084

    public static AdController Instance { get; private set; }
    private bool isInterstitialOrRewardedAdShowing = false;
    private static BannerAdNetwork lastDisplayedBannerAdNetwork;
    private int _adCount;
    private bool _isAlreadyShowingBanner;

    public bool IsInterstitialOrRewardedAdShowing
    {
        get => isInterstitialOrRewardedAdShowing;

        private set =>
            isInterstitialOrRewardedAdShowing = value;
    }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            AppStateEventNotifier.AppStateChanged += OnAppStateChanged;
        }
        else
        {
            Destroy(gameObject);
        }

        if (!Advertising.IsInitialized())
        {
            Advertising.Initialize();
        }

        _adCount = 1;
    }

    private void OnDestroy()
    {
        // Always unlisten to events when complete.
        AppStateEventNotifier.AppStateChanged -= OnAppStateChanged;
    }

    private void OnAppStateChanged(AppState state)
    {
        Debug.Log("App State changed to : " + state);

        // if the app is Foregrounded and the ad is available, show it.
        if (state == AppState.Foreground)
        {
            ShowAppOpenAd();
        }
    }

    private AppOpenAd _appOpenAd;


    public bool IsAppOpenAdAvailable
    {
        get
        {
            return _appOpenAd != null;
        }
    }

    /// <summary>
    /// Loads the app open ad.
    /// </summary>
    public void LoadAppOpenAd()
    {
        if (FreeRewardChecker.IsApproving) return;

        // Clean up the old ad before loading a new one.
        if (_appOpenAd != null)
        {
            UnRegisterEventHandlers();

            _appOpenAd.Destroy();
            _appOpenAd = null;
        }

        Debug.Log("Loading the app open ad.");

        // Create our request used to load the ad.
        var adRequest = new AdRequest.Builder()
        .AddKeyword("pregnant").AddKeyword("mother")
        .Build();

#if UNITY_ANDROID
        const string appOpenId = "ca-app-pub-8950507671126506/5319040869";
#elif UNITY_IOS
        const string appOpenId = "ca-app-pub-4261801828815757/8823558649";
#endif

        // send the request to load the ad.
        AppOpenAd.LoadAd(appOpenId, ScreenOrientation.LandscapeLeft, adRequest,
            (ad, error) =>
            {
                // if error is not null, the load request failed.
                if (error != null || ad == null)
                {
                    Debug.LogError("app open ad failed to load an ad " +
                                   "with error : " + error);
                    return;
                }

                Debug.Log("App open ad loaded with response : "
                          + ad.GetResponseInfo());

                _appOpenAd = ad;
                RegisterEventHandlers(ad);
            });
    }

    private void RegisterEventHandlers(AppOpenAd ad)
    {
        ad.OnAdDidDismissFullScreenContent += Ad_OnAdDidDismissFullScreenContent;
        ad.OnAdFailedToPresentFullScreenContent += Ad_OnAdFailedToPresentFullScreenContent;
    }

    private void UnRegisterEventHandlers()
    {
        if (_appOpenAd == null) return;

        _appOpenAd.OnAdDidDismissFullScreenContent -= Ad_OnAdDidDismissFullScreenContent;
        _appOpenAd.OnAdFailedToPresentFullScreenContent -= Ad_OnAdFailedToPresentFullScreenContent;
    }

    private void Ad_OnAdFailedToPresentFullScreenContent(object sender, AdErrorEventArgs error)
    {
        Debug.LogError("App open ad failed to open full screen content " +
                           "with error : " + error);

        // Reload the ad so that we can show another as soon as possible.
        LoadAppOpenAd();
    }

    private void Ad_OnAdDidDismissFullScreenContent(object sender, System.EventArgs e)
    {
        Debug.Log("App open ad full screen content closed.");

        // Reload the ad so that we can show another as soon as possible.
        LoadAppOpenAd();
    }

    /// <summary>
    /// Shows the app open ad.
    /// </summary>
    public bool ShowAppOpenAd()
    {
        if (FreeRewardChecker.IsApproving) return false;

        if (IsAppOpenAdAvailable)
        {
            Debug.Log("Showing app open ad.");
            _appOpenAd.Show();
            return true;
        }
        else
        {
            Debug.LogError("App open ad is not ready yet.");
            LoadAppOpenAd();
            return false;
        }
    }

    private void Start()
    {
        Advertising.LoadRewardedAd(RewardedAdNetwork.AdMob, AdPlacement.Default);
        //Advertising.LoadRewardedAd(RewardedAdNetwork.AppLovin, AdPlacement.Default);
        Advertising.LoadRewardedAd(RewardedAdNetwork.UnityAds, AdPlacement.Default);

        Advertising.LoadInterstitialAd(InterstitialAdNetwork.AdMob, AdPlacement.Default);
        Advertising.LoadInterstitialAd(InterstitialAdNetwork.AppLovin, AdPlacement.Default);
        Advertising.LoadInterstitialAd(InterstitialAdNetwork.UnityAds, AdPlacement.Default);
        
        Invoke(nameof(ShowAppOpenAd), 10);

        LoadAppOpenAd();
    }

    void OnEnable()
    {
        Advertising.InterstitialAdCompleted += InterstitialAdCompletedHandler;
    }

    void OnDisable()
    {
        Advertising.InterstitialAdCompleted -= InterstitialAdCompletedHandler;
    }

    public void DisplayDelayedRandomFullScreenAd(float time)
    {
        StartCoroutine(DisplayDelayedRandomFullScreenAdCoroutine(time));
    }

    private IEnumerator DisplayDelayedRandomFullScreenAdCoroutine(float seconds)
    {
        yield return new WaitForSecondsRealtime(seconds);
        
        ShowCountedInterstitialAd();
    }

    public bool ShowCountedInterstitialAd()
    {
        if (_adCount >= FreeRewardChecker.InterstitialAdCount)
        {
            _adCount = 1;
            return DisplayRandomFullscreenAd();
        }

        _adCount++;
        return false;

    }


    public bool DisplayRandomFullscreenAd()
    {
        if (Application.platform ==  RuntimePlatform.OSXEditor)
        {
            return DisplayUnityInterstitialAd() || DisplayAppLovinInterstitial() || DisplayAdmobInterstitial();
        }

        int index = Random.Range(0, 8);
        bool adDisplayed;
        switch (index)
        {
            case 0:
            case 1:
                adDisplayed = DisplayUnityInterstitialAd() || DisplayAdmobInterstitial() || DisplayAppLovinInterstitial();
                Debug.Log(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>DisplayRandomFullscreenAd()"+ index);
                break;
            case 2:
                adDisplayed = DisplayAppLovinInterstitial() || DisplayAdmobInterstitial() || DisplayUnityInterstitialAd();
                Debug.Log(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>DisplayRandomFullscreenAd()" + index);
                break;
            default:
                adDisplayed = DisplayAdmobInterstitial() || DisplayUnityInterstitialAd() || DisplayAppLovinInterstitial();
                Debug.Log(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>DisplayRandomFullscreenAd()" + index);
                break;
        }

        if (adDisplayed)
        {
            Instance.IsInterstitialOrRewardedAdShowing = true;
        }

        return adDisplayed;
    }

    void InterstitialAdCompletedHandler(InterstitialAdNetwork network, AdLocation location)
    {
        Instance.IsInterstitialOrRewardedAdShowing = false;
        Debug.Log(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>InterstitialAdCompletedHandler()");
    }



    public bool DisplayRandomRewardedVideoAd()
    {
        var index = Random.Range(0, 6);

        bool adDisplayed = index switch
        {
            0 => DisplayAdmobRewardedVideoAd() || DisplayUnityRewardedAd(),
            1 => DisplayAppLovinRewardedVideoAd() || DisplayUnityRewardedAd(),
            _ => DisplayUnityRewardedAd() || DisplayAdmobRewardedVideoAd()
        };

        if (adDisplayed)
        {
            Instance.IsInterstitialOrRewardedAdShowing = true;
        }

        return adDisplayed;
    }

    private static bool DisplayUnityRewardedAd()
    {
        print("UNITY REWARDED AD IS BEING DISPLAYED");
        if (Advertising.IsRewardedAdReady(RewardedAdNetwork.UnityAds, AdPlacement.Default))
        {
            Advertising.ShowRewardedAd(RewardedAdNetwork.UnityAds, AdPlacement.Default);
            return true;
        }

        return false;
    }

    private static bool DisplayAdmobRewardedVideoAd()
    {
        if (Advertising.IsRewardedAdReady(RewardedAdNetwork.AdMob, AdPlacement.Default))
        {
            Advertising.ShowRewardedAd(RewardedAdNetwork.AdMob, AdPlacement.Default);
            return true;
        }

        return false;
    }

    private static bool DisplayAppLovinRewardedVideoAd()
    {
        if (Advertising.IsRewardedAdReady(RewardedAdNetwork.AppLovin, AdPlacement.Default))
        {
            Advertising.ShowRewardedAd(RewardedAdNetwork.AppLovin, AdPlacement.Default);
            return true;
        }

        return false;
    }






    private static bool DisplayAppLovinInterstitial()
    {
        if (Advertising.IsInterstitialAdReady(InterstitialAdNetwork.AppLovin, AdPlacement.Default))
        {

            Debug.Log(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>DisplayAppLovinInterstitial()");
            Advertising.ShowInterstitialAd(InterstitialAdNetwork.AppLovin, AdPlacement.Default);
            return true;
        }

        return false;
    }

    private static bool DisplayUnityInterstitialAd()
    {
        if (Advertising.IsInterstitialAdReady(InterstitialAdNetwork.UnityAds, AdPlacement.Default))
        {
            Debug.Log(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>DisplayUnityInterstitialAd()");
            Advertising.ShowInterstitialAd(InterstitialAdNetwork.UnityAds, AdPlacement.Default);
            return true;
        }

        return false;
    }

    private static bool DisplayAdmobInterstitial()
    {
        if (Advertising.IsInterstitialAdReady(InterstitialAdNetwork.AdMob, AdPlacement.Default))
        {
            Debug.Log(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>DisplayAdmobInterstitial()");
            Advertising.ShowInterstitialAd(InterstitialAdNetwork.AdMob, AdPlacement.Default);
            return true;
        }

        return false;
    }

    public void ShowAdmobBanner()
    {
        print("Showing banner");
        Advertising.ShowBannerAd(BannerAdNetwork.AdMob, BannerAdPosition.Top, size: BannerAdSize.Banner);
    }

    public void DestroyBanner()
    {
        Advertising.DestroyBannerAd();
    }

    public void HideAdmobBanner()
    {
        Advertising.HideBannerAd(BannerAdNetwork.AdMob, AdPlacement.Default);
    }

    public void DisplayRewarded()
    {
        print("Showing Rewarded");
        Advertising.ShowRewardedAd(RewardedAdNetwork.UnityAds, AdPlacement.Default);
    }

    //public void ShowGameScreenAdmobBanner()
    //{
    //    print("Showing GameScreen banner");
    //    Advertising.ShowBannerAd(BannerAdNetwork.AdMob, AdPlacement.GameScreen, BannerAdPosition.TopRight, BannerAdSize.Banner);
    //}

    //public void HideGameScreenBanner()
    //{
    //    Advertising.HideBannerAd(BannerAdNetwork.AdMob,AdPlacement.GameScreen);
    //}
}
