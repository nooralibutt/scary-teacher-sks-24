using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SKS.Ads
{
    public abstract class AdsObject : MonoBehaviour
    {

        public enum RewardedVideoAwardType
        {
            kCompleted,
            kSkipped,
            kFailed,
            kNotAvailable,
            kFailedAfterCaching
        }
        public enum BannerType
        {
            SmartBanner,
            DefaultBanner,
            BigBanner
        }
        public enum BannerPosition
        {
            Top,
            Bottom,
            TopLeft,
            TopRight,
            BottomLeft,
            BottomRight,
            Center
        }
        public System.Action OnInterstitialShown, OnInterstitialHidden, OnRewardedVideoShown, OnRewardedVideoHidden, OnUnityAdsInititalized;
        public System.Action<bool> OnRewardedVideoAvailable;

        protected System.Action<RewardedVideoAwardType, RewardedVideoType, string> OnRewardedVideoGiveAward;

        protected BannerType crntBannerType ;
        protected BannerPosition crntBannerPosition;

        protected List<string> testDevicesIds;

        protected string interstitialId, rewardedVideoId,bannerAdId;

        protected bool shouldGiveAward = false;

        protected bool hasPlayedRewardedVideo = false;

        public abstract bool IsRewardedVideoAvailable();
        public abstract bool IsInterstitialAvailable();
        public abstract bool IsBannerAvailable(bool isBigBanner);

        protected abstract void ShowInterstitialLocal();
        protected abstract void ShowRewardedVideoLocal();
        protected abstract void ShowBannerAdLocal(bool isBigBanner);
        protected abstract void HideBannerAdLocal(bool isBigBanner);

        protected abstract void CacheInterstitial();
        protected abstract void CacheBanner();
        protected abstract void CacheRewardedVideo();

        protected virtual void OnDestroy()
        {
            OnInterstitialShown = null;
            OnInterstitialHidden = null;
            OnRewardedVideoShown = null;
            OnRewardedVideoHidden = null;
            OnRewardedVideoAvailable = null;
            OnUnityAdsInititalized = null;
            OnRewardedVideoGiveAward = null;
        }

        private void OnApplicationPause(bool pause)
        {
            if (!pause)
            {
                CacheInterstitial();
                CacheRewardedVideo();
            }
        }

        public abstract void Initialize(int interstitialPriority, int rewardedVideoPriority, int bannerAdPriority, RewardedVideoType adType, bool hasUserConsentForGDPR, bool isAgeRestricted, string appId = null, string interstitialId = null, string rewardedVideoId = null, string BannerId = null, BannerType type = BannerType.SmartBanner, BannerPosition pos = BannerPosition.Bottom, List<string> testDevicesIds = null);

        protected void LogInterstitialFailed(string error)
        {
            SKSDebug.Log("SKS Interstitial " + gameObject.name + " failed with error= " + error);
        }

        protected void LogRewardedVideoFailed(string error)
        {
            SKSDebug.Log("SKS RewardedVideo " + gameObject.name + " failed with error= " + error);
        }

        public bool ShowInterstitial()
        {
            if (IsInterstitialAvailable())
            {
                ShowInterstitialLocal();

                return true;
            }
            else
            {
                Invoke("CacheInterstitial", UnityEngine.Random.Range(2, 6));
                return false;
            }
        }
        public bool ShowBanner(bool isBigBanner)
        {
            if (IsBannerAvailable(isBigBanner))
            {
                ShowBannerAdLocal(isBigBanner);

                return true;
            }
            else
            {
                Invoke(nameof(CacheBanner), UnityEngine.Random.Range(2, 6));
                return false;
            }
        }
        public bool HideBanner(bool isBigBanner)
        {
            if (IsBannerAvailable(isBigBanner))
            {
                HideBannerAdLocal(isBigBanner);

                return true;
            }
            else
            {
                Invoke(nameof(CacheBanner), UnityEngine.Random.Range(2, 6));
                return false;
            }
        }
        public bool ShowRewardedVideo(bool IsSingleAd, System.Action<RewardedVideoAwardType, RewardedVideoType, string> Callback)
        {
            if (IsRewardedVideoAvailable())
            {
                if (IsSingleAd)
                {

                    OnRewardedVideoGiveAward = Callback;
                    ShowRewardedVideoLocal();
                }
                else
                {
                    OnRewardedVideoGiveAward = Callback;
                    ShowRewardedVideoLocal();
                }
                return true;
            }
            else
            {
                Callback(RewardedVideoAwardType.kNotAvailable, MyType, this.rewardedVideoId);
                Invoke("CacheRewardedVideo", UnityEngine.Random.Range(2, 6));
                return false;
            }
        }


        public int InterstitialPriority
        {
            set;
            get;
        }

        public int RewardedVideoPriority
        {
            set;
            get;
        }

        public int BannerAdPriority
        {
            set;
            get;
        }

        public RewardedVideoType MyType
        {
            protected set;
            get;
        }
        public string InterstitialVideoId
        {
            get
            {
                return interstitialId;
            }
        }
        public string RewardedVideoId
        {
            get
            {
                return rewardedVideoId;
            }
        }
        public string BannerAdId
        {
            get
            {
                return bannerAdId;
            }
        }
    }
}