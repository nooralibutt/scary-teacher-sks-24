using System;
using System.Collections;
using System.Collections.Generic;
using GoogleMobileAds.Api;
using UnityEngine;
namespace SKS.Ads
{
    public class AdmobAd : AdsObject
    {
        private static bool StartInit = false;
        private static bool isInitialized = false;
        static System.Action OnSDKInitialized;
        BannerView _bannerView;
        private InterstitialAd interstitialAd;
        private RewardedAd rewardedAd;
        bool islocallyIntialized = false;
        int interstitialAttempt, retryRewardAttempt, BannerAdAttempt;
        bool isBannerAvailable = false;
        public override void Initialize(int interstitialPriority, int rewardedVideoPriority, int bannerAdPriority, RewardedVideoType adType, bool hasUserConsentForGDPR, bool isAgeRestricted, string appId = null, string interstitialId = null, string rewardedVideoId = null, string BannerId = null, BannerType type = BannerType.SmartBanner, BannerPosition pos = BannerPosition.Bottom, List<string> testDevicesIds = null)
        {
            this.MyType = adType;
            this.BannerAdPriority = bannerAdPriority;
            this.InterstitialPriority = interstitialPriority;
            this.RewardedVideoPriority = rewardedVideoPriority;
            this.interstitialId = interstitialId;
            this.rewardedVideoId = rewardedVideoId;
            this.bannerAdId = BannerId;
            this.crntBannerType = type;
            this.crntBannerPosition = pos;
            MobileAds.RaiseAdEventsOnUnityMainThread = true;

            if (!StartInit)
            {
                StartInit = true;
                MobileAds.Initialize(initStatus =>
                {
                    Debug.Log("=====>AdmobInitialized");
                    isInitialized = true;
                    OnSDKInitialized?.Invoke();
                });
            }
            if (isInitialized)
            {
                CacheAds();
            }
            else
            {
                OnSDKInitialized -= CacheAds;
                OnSDKInitialized += CacheAds;
            }
        }
        void CacheAds()
        {
            Debug.Log("=====>AdCache");

            CacheRewardedVideo();
            CacheInterstitial();
            CacheBanner();
        }
        public void LoadInterstitialAd()
        {
            if (string.IsNullOrEmpty(interstitialId) || (interstitialAd != null && interstitialAd.CanShowAd()))
            {
                return;
            }

            // Clean up the old ad before loading a new one.

            if (interstitialAd != null)
            {
                interstitialAd.Destroy();
                interstitialAd = null;
            }

            Debug.Log("===========>Loading the interstitial ad.");

            // create our request used to load the ad.
            var adRequest = new AdRequest.Builder()
                    .AddKeyword("unity-admob-sample")
                    .Build();

            // send the request to load the ad.
            InterstitialAd.Load(interstitialId, adRequest,
                (InterstitialAd ad, LoadAdError error) =>
                {
                    // if error is not null, the load request failed.
                    if (error != null || ad == null)
                    {
                        Debug.LogError("========>interstitial ad failed to load an ad " + "with error : " + error);
                        return;
                    }

                    Debug.Log("=======>Interstitial ad loaded with response : " + ad.GetResponseInfo());
                    interstitialAttempt = 0;
                    interstitialAd = ad;
                    RegisterReloadHandler(interstitialAd);
                });
        }
        public void LoadRewardedAd()
        {
            // Clean up the old ad before loading a new one.
            if (rewardedAd != null)
            {
                rewardedAd.Destroy();
                rewardedAd = null;
            }

            Debug.Log("Loading the rewarded ad.");

            // create our request used to load the ad.
            var adRequest = new AdRequest.Builder().Build();

            // send the request to load the ad.
            RewardedAd.Load(rewardedVideoId, adRequest,
                (RewardedAd ad, LoadAdError error) =>
                {
                    // if error is not null, the load request failed.
                    if (error != null || ad == null)
                    {
                        Debug.LogError("========>Rewarded ad failed to load an ad " +
                                       "with error : " + error);
                        return;
                    }

                    Debug.Log("=======>Rewarded ad loaded with response : "
                              + ad.GetResponseInfo());
                    retryRewardAttempt = 0;
                    rewardedAd = ad;
                    RegisterReloadHandler(rewardedAd);
                });
        }
        public void LoadBannerAd()
        {
            // create an instance of a banner view first.
            if (_bannerView == null)
            {
                CreateBannerView();
            }
            _bannerView.OnBannerAdLoaded -= OnBannerAdLoaded;
            _bannerView.OnBannerAdLoadFailed -= OnBannerAdLoadFailed;
            _bannerView.OnAdFullScreenContentClosed -= OnBannerClose;

            _bannerView.OnAdFullScreenContentClosed += OnBannerClose;
            _bannerView.OnBannerAdLoaded += OnBannerAdLoaded;
            _bannerView.OnBannerAdLoadFailed += OnBannerAdLoadFailed;

            // create our request used to load the ad.
            var adRequest = new AdRequest.Builder()
                .AddKeyword("unity-admob-sample")
                .Build();

            // send the request to load the ad.
            Debug.Log("=========>Loading banner ad.");
            _bannerView.LoadAd(adRequest);
        }
        void OnBannerClose()
        {
            isBannerAvailable = false;
            CacheBanner();
        }
        private void OnBannerAdLoaded()
        {
            Debug.Log("=========>Banner view loaded an ad with response : "
                     + _bannerView.GetResponseInfo());

            isBannerAvailable = true;
            _bannerView.Hide();
            if (this.crntBannerType == BannerType.BigBanner)
            {
                AdsManager.OnBigBannerAdInitialized?.Invoke();
            }
            else
            {
                AdsManager.OnSmartBannerAdInitialized?.Invoke();
            }
            //Debug.Log("Ad Height: {0}, width: {1}",_bannerView.GetHeightInPixels(), _bannerView.GetWidthInPixels());
        }

        private void OnBannerAdLoadFailed(LoadAdError error)
        {
            isBannerAvailable = false;
            Debug.LogError("========>Banner view failed to load an ad with error : "
                    + error);
        }

        public void CreateBannerView()
        {
            Debug.Log("======>Creating banner view");

            // If we already have a banner, destroy the old one.
            if (_bannerView != null)
            {
                DestroyAd();
            }
            switch (this.crntBannerType)
            {
                case BannerType.SmartBanner:
                    _bannerView = new BannerView(BannerAdId, AdSize.SmartBanner, (AdPosition)this.crntBannerPosition);
                    break;
                case BannerType.DefaultBanner:
                    _bannerView = new BannerView(BannerAdId, AdSize.Banner, (AdPosition)this.crntBannerPosition);
                    break;
                case BannerType.BigBanner:
                    _bannerView = new BannerView(BannerAdId, AdSize.MediumRectangle, (AdPosition)this.crntBannerPosition);
                    break;
            }
            // Create a 320x50 banner at top of the screen
        }
        public void DestroyAd()
        {
            if (_bannerView != null)
            {
                Debug.Log("========>Destroying banner ad.");
                _bannerView.Destroy();
                _bannerView = null;
            }
        }


        private void OnApplicationPause(bool pause)
        {
            if (!pause)
            {
                CacheAds();
            }
        }
        public override bool IsInterstitialAvailable()
        {
            bool isAdAvailable = false;

            if (!string.IsNullOrEmpty(this.interstitialId) && interstitialAd != null && interstitialAd.CanShowAd())
            {
                isAdAvailable = true;
            }
            return isAdAvailable;

        }

        public override bool IsRewardedVideoAvailable()
        {
            bool isAdAvailable = false;

            if (!string.IsNullOrEmpty(RewardedVideoId) && rewardedAd != null && rewardedAd.CanShowAd())
            {
                isAdAvailable = true;
            }
            return isAdAvailable;

        }
        private void RegisterReloadHandler(InterstitialAd ad)
        {
            // Raised when the ad closed full screen content.
            ad.OnAdFullScreenContentClosed += () =>
            {
                Debug.Log("Interstitial Ad full screen content closed.");
                // Reload the ad so that we can show another as soon as possible.
                CacheInterstitial();
            };
            // Raised when the ad failed to open full screen content.
            ad.OnAdFullScreenContentFailed += (AdError error) =>
            {
                Debug.LogError("Interstitial ad failed to open full screen content " +
                               "with error : " + error);
                // Reload the ad so that we can show another as soon as possible.
                CacheInterstitial();
            };
        }
        private void RegisterReloadHandler(RewardedAd ad)
        {
            // Raised when the ad closed full screen content.
            ad.OnAdFullScreenContentClosed += () =>
            {
                Debug.Log("Rewarded Ad full screen content closed.");
                hasPlayedRewardedVideo = false;
                // Reload the ad so that we can show another as soon as possible.
                LoadRewardedAd();
            };
            // Raised when the ad failed to open full screen content.
            ad.OnAdFullScreenContentFailed += (AdError error) =>
            {
                Debug.LogError("Rewarded ad failed to open full screen content " +
                               "with error : " + error);
                if (hasPlayedRewardedVideo)
                {
                    OnRewardedVideoGiveAward?.Invoke(AdsObject.RewardedVideoAwardType.kFailedAfterCaching, MyType, rewardedVideoId);
                    OnRewardedVideoGiveAward = null;
                    hasPlayedRewardedVideo = false;
                }
                // Reload the ad so that we can show another as soon as possible.
                LoadRewardedAd();
            };
        }

        protected override void CacheInterstitial()
        {
            if (!DefaultData.IsInternetAvailable())
            {
                return;
            }
            try
            {
                double interstitialDelay = Math.Pow(2, Math.Min(6, interstitialAttempt));
                CancelInvoke(nameof(cacheAdmobInterAd));
                Invoke(nameof(cacheAdmobInterAd), (float)interstitialDelay);
            }
            catch (Exception exc)
            {
            }
        }
        void cacheAdmobInterAd()
        {
            try
            {
                if (!string.IsNullOrEmpty(interstitialId) && ((interstitialAd != null && !interstitialAd.CanShowAd()) || interstitialAd == null))
                {
                    LoadInterstitialAd();
                    interstitialAttempt++;
                }
                else
                {
                    interstitialAttempt = 0;
                }
            }
            catch (Exception exc)
            {

            }
        }
        void cacheAdmobRewardAd()
        {
            try
            {
                if (!string.IsNullOrEmpty(rewardedVideoId) && ((rewardedAd != null && !rewardedAd.CanShowAd()) || rewardedAd == null))
                {
                    LoadRewardedAd();
                    retryRewardAttempt++;
                }
                else
                {
                    retryRewardAttempt = 0;
                }
            }
            catch (Exception exc)
            {

            }
        }

        protected override void CacheRewardedVideo()
        {
            try
            {
                if (!DefaultData.IsInternetAvailable())
                {
                    return;
                }
            }
            catch (Exception exc)
            { }
            try
            {
                double retryDelay = Math.Pow(2, Math.Min(6, retryRewardAttempt));
                CancelInvoke(nameof(cacheAdmobRewardAd));
                Invoke(nameof(cacheAdmobRewardAd), (float)retryDelay);
            }
            catch (Exception exc)
            {
            }
        }

        protected override void ShowInterstitialLocal()
        {
            if (IsInterstitialAvailable())
            {
                Debug.Log("Showing interstitial ad.");
                interstitialAd.Show();
            }
        }

        protected override void ShowRewardedVideoLocal()
        {
            const string rewardMsg = "Rewarded ad rewarded the user. Type: {0}, amount: {1}.";

            if (IsRewardedVideoAvailable())
            {
                shouldGiveAward = false;
                hasPlayedRewardedVideo = true;
                rewardedAd.Show((Reward reward) =>
                {
                    shouldGiveAward = true;

                    if (DefaultData.SharedData().UseMaxSdkProperMethod)
                    {
                        OnRewardedVideoGiveAward?.Invoke(shouldGiveAward ? RewardedVideoAwardType.kCompleted : RewardedVideoAwardType.kSkipped, MyType, rewardedVideoId);
                    }
                    else
                    {
                        OnRewardedVideoGiveAward?.Invoke(RewardedVideoAwardType.kCompleted, MyType, rewardedVideoId);
                    }
                    shouldGiveAward = false;
                    OnRewardedVideoGiveAward = null;
                    hasPlayedRewardedVideo = false;
                    Debug.Log(String.Format(rewardMsg, reward.Type, reward.Amount));
                });
            }
        }

        public override bool IsBannerAvailable(bool isBigBanner)
        {
            if ((this.crntBannerType == BannerType.BigBanner && !isBigBanner) || (this.crntBannerType != BannerType.BigBanner && isBigBanner))
            {
                return false;
            }

            return isBannerAvailable;
        }

        protected override void ShowBannerAdLocal(bool isBigBanner)
        {
            if (IsBannerAvailable(isBigBanner))
            {
                _bannerView.Show();
            }
        }
        protected override void HideBannerAdLocal(bool isBigBanner)
        {
            _bannerView.Hide();
        }

        protected override void CacheBanner()
        {
            try
            {
                if (!DefaultData.IsInternetAvailable())
                {
                    return;
                }
            }

            catch (Exception exc)
            { }
            try
            {
                double retryDelay = Math.Pow(2, Math.Min(6, BannerAdAttempt));
                CancelInvoke(nameof(cacheAdmobBannerAd));
                Invoke(nameof(cacheAdmobBannerAd), (float)retryDelay);
            }
            catch (Exception exc)
            {
            }
        }
        void cacheAdmobBannerAd()
        {
            try
            {
                if (!string.IsNullOrEmpty(bannerAdId) && ((_bannerView != null && !isBannerAvailable) || _bannerView == null))
                {
                    LoadBannerAd();
                    BannerAdAttempt++;
                }
                else
                {
                    BannerAdAttempt = 0;
                }
            }
            catch (Exception exc)
            {

            }
        }

    }
}