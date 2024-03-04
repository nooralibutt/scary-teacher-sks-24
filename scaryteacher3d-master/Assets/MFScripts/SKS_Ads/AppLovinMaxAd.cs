using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public enum RewardedVideoType { None, Admob, Chartboost, Applovin, Custom }

namespace SKS.Ads
{

    public class AppLovinMaxAd : AdsObject
    {
        bool isBannerAvailable = false;

        private static bool isInitialized = false;
        int InterAttempt, bannerRetryAttempt, retryAttempt;
        protected override void OnDestroy()
        {
            MaxSdkCallbacks.OnSdkInitializedEvent -= MaxSdkCallbacks_OnSdkInitializedEvent;

            MaxSdkCallbacks.Rewarded.OnAdLoadedEvent -= OnRewardedAdLoadedEvent;
            MaxSdkCallbacks.Rewarded.OnAdLoadFailedEvent -= OnRewardedAdLoadFailedEvent;
            MaxSdkCallbacks.Rewarded.OnAdDisplayedEvent -= OnRewardedAdDisplayedEvent;
            MaxSdkCallbacks.Rewarded.OnAdClickedEvent -= OnRewardedAdClickedEvent;
            MaxSdkCallbacks.Rewarded.OnAdHiddenEvent -= OnRewardedAdHiddenEvent;
            MaxSdkCallbacks.Rewarded.OnAdDisplayFailedEvent -= OnRewardedAdFailedToDisplayEvent;
            MaxSdkCallbacks.Rewarded.OnAdReceivedRewardEvent -= OnRewardedAdReceivedRewardEvent;

            MaxSdkCallbacks.Interstitial.OnAdLoadedEvent -= OnInterstitialLoadedEvent;
            MaxSdkCallbacks.Interstitial.OnAdLoadFailedEvent -= OnInterstitialLoadFailedEvent;
            MaxSdkCallbacks.Interstitial.OnAdDisplayedEvent -= OnInterstitialDisplayedEvent;
            MaxSdkCallbacks.Interstitial.OnAdHiddenEvent -= OnInterstitialHiddenEvent;
            MaxSdkCallbacks.Interstitial.OnAdDisplayFailedEvent -= OnInterstitialAdFailedToDisplayEvent;

            MaxSdkCallbacks.Banner.OnAdLoadedEvent -= OnBannerAdLoadedEvent;
            MaxSdkCallbacks.Banner.OnAdLoadFailedEvent -= OnBannerAdLoadFailedEvent;
            MaxSdkCallbacks.Banner.OnAdClickedEvent -= OnBannerAdClickedEvent;
            MaxSdkCallbacks.Banner.OnAdRevenuePaidEvent -= OnBannerAdRevenuePaidEvent;
            MaxSdkCallbacks.Banner.OnAdExpandedEvent -= OnBannerAdExpandedEvent;
            MaxSdkCallbacks.Banner.OnAdCollapsedEvent -= OnBannerAdCollapsedEvent;
            base.OnDestroy();
        }

        public override void Initialize(int interstitialPriority, int rewardedVideoPriority, int bannerAdPriority, RewardedVideoType adType, bool hasUserConsentForGDPR, bool isAgeRestricted, string appId = null, string interstitialId = null, string rewardedVideoId = null, string BannerId = null, BannerType type = BannerType.SmartBanner, BannerPosition pos = BannerPosition.Bottom, List<string> testDevicesIds = null)
        {
            this.MyType = adType;
            this.crntBannerPosition = pos;
            this.interstitialId = interstitialId;
            this.rewardedVideoId = rewardedVideoId;

            this.InterstitialPriority = interstitialPriority;
            this.RewardedVideoPriority = rewardedVideoPriority;


            if (!isInitialized)
            {
                isInitialized = true;
                MaxSdk.SetSdkKey(GameIDs.APPLOVIN_AD_SDK_KEY);
                MaxSdk.InitializeSdk();

            }

            MaxSdkCallbacks.OnSdkInitializedEvent += MaxSdkCallbacks_OnSdkInitializedEvent;
        }

        private void MaxSdkCallbacks_OnSdkInitializedEvent(MaxSdkBase.SdkConfiguration obj)
        {
            MaxSdkCallbacks.Rewarded.OnAdLoadedEvent += OnRewardedAdLoadedEvent;
            MaxSdkCallbacks.Rewarded.OnAdLoadFailedEvent += OnRewardedAdLoadFailedEvent;
            MaxSdkCallbacks.Rewarded.OnAdDisplayedEvent += OnRewardedAdDisplayedEvent;
            MaxSdkCallbacks.Rewarded.OnAdClickedEvent += OnRewardedAdClickedEvent;
            MaxSdkCallbacks.Rewarded.OnAdHiddenEvent += OnRewardedAdHiddenEvent;
            MaxSdkCallbacks.Rewarded.OnAdDisplayFailedEvent += OnRewardedAdFailedToDisplayEvent;
            MaxSdkCallbacks.Rewarded.OnAdReceivedRewardEvent += OnRewardedAdReceivedRewardEvent;

            MaxSdkCallbacks.Interstitial.OnAdLoadedEvent += OnInterstitialLoadedEvent;
            MaxSdkCallbacks.Interstitial.OnAdLoadFailedEvent += OnInterstitialLoadFailedEvent;
            MaxSdkCallbacks.Interstitial.OnAdDisplayedEvent += OnInterstitialDisplayedEvent;
            MaxSdkCallbacks.Interstitial.OnAdHiddenEvent += OnInterstitialHiddenEvent;
            MaxSdkCallbacks.Interstitial.OnAdDisplayFailedEvent += OnInterstitialAdFailedToDisplayEvent;

            MaxSdkCallbacks.Banner.OnAdLoadedEvent += OnBannerAdLoadedEvent;
            MaxSdkCallbacks.Banner.OnAdLoadFailedEvent += OnBannerAdLoadFailedEvent;
            MaxSdkCallbacks.Banner.OnAdClickedEvent += OnBannerAdClickedEvent;
            MaxSdkCallbacks.Banner.OnAdRevenuePaidEvent += OnBannerAdRevenuePaidEvent;
            MaxSdkCallbacks.Banner.OnAdExpandedEvent += OnBannerAdExpandedEvent;
            MaxSdkCallbacks.Banner.OnAdCollapsedEvent += OnBannerAdCollapsedEvent;
            CacheInterstitial();
            CacheRewardedVideo();
            InitializeBannerAds();
        }
        // Banner
        public void InitializeBannerAds()
        {
            if (string.IsNullOrEmpty(this.bannerAdId))
            {
                return;
            }
            switch (this.crntBannerPosition)
            {
                case BannerPosition.Top:
                    MaxSdk.CreateBanner(this.bannerAdId, MaxSdkBase.BannerPosition.TopCenter);
                    break;
                case BannerPosition.Bottom:
                    MaxSdk.CreateBanner(this.bannerAdId, MaxSdkBase.BannerPosition.BottomCenter);
                    break;
                case BannerPosition.BottomLeft:
                    MaxSdk.CreateBanner(this.bannerAdId, MaxSdkBase.BannerPosition.BottomLeft);
                    break;
                case BannerPosition.BottomRight:
                    MaxSdk.CreateBanner(this.bannerAdId, MaxSdkBase.BannerPosition.BottomRight);
                    break;
                case BannerPosition.Center:
                    MaxSdk.CreateBanner(this.bannerAdId, MaxSdkBase.BannerPosition.Centered);
                    break;
                case BannerPosition.TopLeft:
                    MaxSdk.CreateBanner(this.bannerAdId, MaxSdkBase.BannerPosition.TopLeft);
                    break;
                case BannerPosition.TopRight:
                    MaxSdk.CreateBanner(this.bannerAdId, MaxSdkBase.BannerPosition.TopRight);
                    break;
            }


            MaxSdk.SetBannerExtraParameter(this.bannerAdId, "adaptive_banner", "false");
            CacheBanner();
        }
        public override bool IsBannerAvailable(bool isBigBanner)
        {
            if (isBigBanner)
            {
                return false;
            }
            return isBannerAvailable;
        }

        protected override void ShowBannerAdLocal(bool isBigBanner)
        {
            if (isBannerAvailable)
                MaxSdk.ShowBanner(this.bannerAdId);
        }

        protected override void HideBannerAdLocal(bool isBigBanner)
        {
            MaxSdk.HideBanner(this.bannerAdId);
        }

        protected override void CacheBanner()
        {
            if (!DefaultData.IsInternetAvailable())
            {
                return;
            }
            try
            {
                double bannerDelay = Math.Pow(2, Math.Min(6, bannerRetryAttempt));
                CancelInvoke(nameof(cacheBanner));
                Invoke(nameof(cacheBanner), (float)bannerDelay);
            }
            catch (Exception exc)
            {
            }
        }
        void cacheBanner()
        {
            if (string.IsNullOrEmpty(this.bannerAdId))
            {
                return;
            }
            MaxSdk.LoadBanner(this.bannerAdId);
        }
        private void OnBannerAdLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            isBannerAvailable = true;
            MaxSdk.HideBanner(this.bannerAdId);
            AdsManager.OnSmartBannerAdInitialized?.Invoke();
        }

        private void OnBannerAdLoadFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
        {
            isBannerAvailable = false;
        }

        private void OnBannerAdClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            CacheBanner();
        }

        private void OnBannerAdRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

        private void OnBannerAdExpandedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

        private void OnBannerAdCollapsedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

        //Interstitial...............
        private void OnInterstitialLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            // Interstitial ad is ready to be shown. MaxSdk.IsInterstitialReady(adUnitId) will now return 'true'

            // Reset retry attempt
            if (adUnitId == interstitialId)
            {
                InterAttempt = 0;
            }
        }

        private void OnInterstitialAdFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo adInfo)
        {
            // Interstitial ad failed to load 
            // We recommend retrying with exponentially higher delays up to a maximum delay (in this case 64 seconds)

            if (adUnitId == interstitialId)
            {
                CacheInterstitial();
                LogInterstitialFailed(errorInfo.Code.ToString());
            }

        }

        private void OnInterstitialLoadFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
        {
            // Interstitial ad failed to display. We recommend loading the next ad
            if (adUnitId == interstitialId)
            {
                OnInterstitialHidden?.Invoke();
                CacheInterstitial();
            }
        }

        private void OnInterstitialHiddenEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            // Interstitial ad is hidden. Pre-load the next ad
            if (adUnitId == interstitialId)
            {
                OnInterstitialHidden?.Invoke();
                CacheInterstitial();
            }
        }
        private void OnInterstitialDisplayedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            // Interstitial ad is Shown.
            if (adUnitId == interstitialId)
            {
                OnInterstitialShown?.Invoke();
                CacheInterstitial();
            }
        }
        //RewardType..................
        private void OnRewardedAdLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            // Rewarded ad is ready to be shown. MaxSdk.IsRewardedAdReady(adUnitId) will now return 'true'
            // Reset retry attempt
            if (adUnitId != rewardedVideoId)
            {
                return;
            }
            retryAttempt = 0;
        }

        private void OnRewardedAdLoadFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
        {
            // Rewarded ad failed to load 
            // We recommend retrying with exponentially higher delays up to a maximum delay (in this case 64 seconds)
            if (adUnitId != rewardedVideoId)
            {
                return;
            }
            CacheRewardedVideo();

            LogRewardedVideoFailed(errorInfo.Code.ToString());

            OnRewardedVideoAvailable?.Invoke(false);

            if (hasPlayedRewardedVideo)
            {
                OnRewardedVideoGiveAward?.Invoke(AdsObject.RewardedVideoAwardType.kFailedAfterCaching, MyType, rewardedVideoId);
                OnRewardedVideoGiveAward = null;
                hasPlayedRewardedVideo = false;
            }
        }

        private void OnRewardedAdFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo adInfo)
        {
            // Rewarded ad failed to display. We recommend loading the next ad
            if (adUnitId != rewardedVideoId)
            {
                return;
            }
            CacheRewardedVideo();
            LogRewardedVideoFailed(errorInfo.Code.ToString());

            OnRewardedVideoAvailable?.Invoke(false);

            if (hasPlayedRewardedVideo)
            {
                OnRewardedVideoGiveAward?.Invoke(AdsObject.RewardedVideoAwardType.kFailedAfterCaching, MyType, rewardedVideoId);
                OnRewardedVideoGiveAward = null;
                hasPlayedRewardedVideo = false;
            }
        }

        private void OnRewardedAdDisplayedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            if (adUnitId != rewardedVideoId)
            {
                return;
            }
            OnRewardedVideoShown?.Invoke();
        }

        private void OnRewardedAdClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

        private void OnRewardedAdHiddenEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            if (adUnitId != rewardedVideoId)
            {
                return;
            }
            // Rewarded ad is hidden. Pre-load the next ad
            CacheRewardedVideo();
            OnRewardedVideoHidden?.Invoke();
            OnRewardedVideoAvailable?.Invoke(false);

            if (DefaultData.SharedData().UseMaxSdkProperMethod)
            {
                OnRewardedVideoGiveAward?.Invoke(shouldGiveAward ? RewardedVideoAwardType.kCompleted : RewardedVideoAwardType.kSkipped, MyType, rewardedVideoId);
            }
            else
            {
                OnRewardedVideoGiveAward?.Invoke(RewardedVideoAwardType.kCompleted, MyType, rewardedVideoId);
            }
            OnRewardedVideoGiveAward = null;

            hasPlayedRewardedVideo = false;
        }

        private void OnRewardedAdReceivedRewardEvent(string adUnitId, MaxSdk.Reward reward, MaxSdkBase.AdInfo adInfo)
        {
            if (adUnitId != rewardedVideoId)
            {
                return;
            }
            // Rewarded ad was displayed and user should receive the reward
            shouldGiveAward = true;

        }
        private void AddLog(string log)
        {
            SKSDebug.Log(log);
        }


        public override bool IsInterstitialAvailable()
        {
            if (string.IsNullOrEmpty(this.interstitialId))
            {
                return false;
            }
            if (!MaxSdk.IsInterstitialReady(this.interstitialId))
            {
                CacheInterstitial();
            }
            return MaxSdk.IsInterstitialReady(this.interstitialId);
        }

        public override bool IsRewardedVideoAvailable()
        {
            if (string.IsNullOrEmpty(this.rewardedVideoId))
            {
                return false;
            }
            if (!MaxSdk.IsRewardedAdReady(this.rewardedVideoId))
            {
                CacheRewardedVideo();
            }
            return MaxSdk.IsRewardedAdReady(this.rewardedVideoId);
        }

        protected override void CacheInterstitial()
        {
            try
            {
                if (!DefaultData.IsInternetAvailable())
                {
                    return;
                }
            }
            catch (Exception exc)
            {
            }
            InterAttempt++;
            double retryDelay = Math.Pow(2, Math.Min(6, InterAttempt));

            Invoke("CacheInterAd", (float)retryDelay);
        }
        void CacheInterAd()
        {
            if (string.IsNullOrEmpty(this.interstitialId))
            {
                return;
            }
            if (!MaxSdk.IsInterstitialReady(this.interstitialId))
            {
                MaxSdk.LoadInterstitial(this.interstitialId);
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
            {
            }
            retryAttempt++;
            double retryDelay = Math.Pow(2, Math.Min(6, retryAttempt));
            Invoke("CacheRewardAd", (float)retryDelay);

        }
        void CacheRewardAd()
        {

            if (string.IsNullOrEmpty(this.rewardedVideoId))
            {
                return;
            }
            if (!MaxSdk.IsRewardedAdReady(this.rewardedVideoId))
            {
                MaxSdk.LoadRewardedAd(this.rewardedVideoId);
            }
        }
        protected override void ShowInterstitialLocal()
        {
            if (string.IsNullOrEmpty(this.interstitialId))
            {
                return;
            }
            MaxSdk.ShowInterstitial(this.interstitialId);
        }

        protected override void ShowRewardedVideoLocal()
        {
            shouldGiveAward = false;
            hasPlayedRewardedVideo = true;
            MaxSdk.ShowRewardedAd(this.rewardedVideoId);
        }


    }
}
