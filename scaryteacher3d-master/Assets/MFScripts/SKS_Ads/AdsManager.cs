using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using static SKS.Ads.AdsObject;

namespace SKS.Ads
{

    public class AdsManager : MonoBehaviour
    {
        public class ExcludedAdData
        {
            public RewardedVideoType adType;
            public string rewardedVideoId;
        }
        public static Action OnInterstitialShown, OnInterstitialHidden, OnRewardedVideoShown, OnRewardedVideoHidden, OnBigBannerAdInitialized, OnSmartBannerAdInitialized;
        public static Action<bool> OnRewardedVideoAvailable;

        private static AdsManager sharedInstance;
        private List<AdsObject> ads = new List<AdsObject>();

        private System.Action<bool> RewardedVideoLocalCallback;
        static bool isBigBannerInitialized = false, isSmartBannerInitialized = false;
        private void OnDestroy()
        {
            if (ads != null && ads.Count > 0)
            {
                for (int i = 0; i < ads.Count; i++)
                {
                    if (ads[i])
                    {
                        ads[i].OnInterstitialHidden -= OnInterstitialHidden;
                        ads[i].OnInterstitialShown -= OnInterstitialShown;
                        ads[i].OnRewardedVideoAvailable -= OnRewardedVideoAvailabilityStatus;
                        ads[i].OnRewardedVideoHidden -= OnRewardedVideoHidden;
                        ads[i].OnRewardedVideoShown -= OnRewardedVideoShown;
                    }
                }
            }

            if (sharedInstance != null)
            {
                Destroy(sharedInstance);
                sharedInstance = null;
            }

            DefaultData.OnDefaultDataDownloadingCompleted -= OnDefaultDataDownloadComplete;
        }

        private void OnRewardedVideoAvailabilityStatus(bool isAvailable)
        {
            if (isAvailable)
            {
                OnRewardedVideoAvailable?.Invoke(true);
            }
            else
            {
                bool anyAdNetworkHasRewardedVideo = false;
                for (int i = 0; i < ads.Count; i++)
                {
                    if (ads[i].IsRewardedVideoAvailable())
                    {
                        anyAdNetworkHasRewardedVideo = true;
                        break;
                    }
                }

                OnRewardedVideoAvailable?.Invoke(anyAdNetworkHasRewardedVideo);
            }
        }

        private AdsObject GetAdNetworkObject(string adProvider, int interstitialPriority, int rewardedVideoPriority, int bannerAdPriority, System.Type adType, RewardedVideoType adTypeEnum, string appId = null, string interstitialId = null, string rewardedVideoId = null, string BannerId = null, BannerType type = BannerType.SmartBanner, BannerPosition pos = BannerPosition.Bottom, List<string> testDevicesIds = null)
        {
            GameObject obj = new GameObject(adProvider);
            obj.transform.SetParent(transform);
            AdsObject ads = (AdsObject)obj.AddComponent(adType);

            ads.OnInterstitialHidden += OnInterstitialHidden;
            ads.OnInterstitialShown += OnInterstitialShown;
            ads.OnRewardedVideoAvailable += OnRewardedVideoAvailabilityStatus;
            ads.OnRewardedVideoHidden += OnRewardedVideoHidden;
            ads.OnRewardedVideoShown += OnRewardedVideoShown;

            ads.Initialize(interstitialPriority, rewardedVideoPriority, bannerAdPriority, adTypeEnum, true, true, appId, interstitialId, rewardedVideoId, BannerId, type, pos, testDevicesIds);

            return ads;
        }

        private int GetPriority(RewardedVideoType adType, List<AdPriorityInfo> adPriorities)
        {
            int priority = -1;

            var priorityObject = adPriorities.Where(a => a.adType == adType).FirstOrDefault();
            if (priorityObject != null)
            {
                priority = priorityObject.priority;
            }

            return priority;
        }

        private void SetupAdPriorities()
        {
            try
            {
                List<AdPriorityInfo> interstitialsInfo = DefaultData.SharedData().interstitalPriorities;
                List<AdPriorityInfo> rewardedVideosInfo = DefaultData.SharedData().rewardedVideoPriorities;
                List<AdPriorityInfo> BannerInfo = DefaultData.SharedData().BannerAdPriorities;

                int admobAdsInterstitialPriority = GetPriority(RewardedVideoType.Admob, interstitialsInfo);
                int admobAdsRewardedVideoPriority = GetPriority(RewardedVideoType.Admob, rewardedVideosInfo);
                int admobAdsBannerAdPriority = GetPriority(RewardedVideoType.Admob, BannerInfo);


                int appLovinInterstitialPriority = GetPriority(RewardedVideoType.Applovin, interstitialsInfo);
                int appLovinRewardedVideoPriority = GetPriority(RewardedVideoType.Applovin, rewardedVideosInfo);
                int appLovinBannerAdPriority = GetPriority(RewardedVideoType.Applovin, BannerInfo);

                for (int i = 0; i < ads.Count; i++)
                {
                    var adObj = ads[i];


                    if (adObj.MyType == RewardedVideoType.Admob)
                    {
                        adObj.InterstitialPriority = admobAdsInterstitialPriority;
                        adObj.RewardedVideoPriority = admobAdsRewardedVideoPriority;
                        adObj.BannerAdPriority = admobAdsBannerAdPriority;
                    }
                    else if (adObj.MyType == RewardedVideoType.Applovin)
                    {
                        adObj.InterstitialPriority = appLovinInterstitialPriority;
                        adObj.RewardedVideoPriority = appLovinRewardedVideoPriority;
                        adObj.BannerAdPriority = appLovinBannerAdPriority;
                    }
                }


            }
            catch (System.Exception exc)
            {
                SKSDebug.LogError("Exception setting up SKS Ads= " + exc.Message);
            }
        }

        private void OnDefaultDataDownloadComplete()
        {
            DefaultData.OnDefaultDataDownloadingCompleted -= OnDefaultDataDownloadComplete;
            SetupAdPriorities();
        }

        private static List<string> GetTestDevicesList(string testDevicesStr)
        {
            List<string> testDevicesIds = new List<string>();

            if (!string.IsNullOrEmpty(testDevicesStr))
            {
                testDevicesStr = testDevicesStr.Replace(" ", "");
                string[] testDevicesArr = testDevicesStr.Split(',');

                testDevicesIds = testDevicesArr.ToList();
            }

            return testDevicesIds;
        }

        private List<AdsObject> GetSortedInterstitialsList()
        {
            if (sharedInstance == null)
            {
                return new List<AdsObject>();
            }
            else
            {
                return sharedInstance.ads.Where(a => a.InterstitialPriority > 0).OrderBy(a => a.InterstitialPriority).ToList();
            }
        }
        private List<AdsObject> GetSortedBannerAdsList()
        {
            if (sharedInstance == null)
            {
                return new List<AdsObject>();
            }
            else
            {
                return sharedInstance.ads.Where(a => a.BannerAdPriority > 0).OrderBy(a => a.BannerAdPriority).ToList();
            }
        }
        private List<AdsObject> GetSortedRewardedVideosList(bool GetDescSortingOrder = false)
        {
            if (sharedInstance == null)
            {
                return new List<AdsObject>();
            }
            else
            {
                List<AdsObject> SortedAdsObj = new List<AdsObject>();
                if (!GetDescSortingOrder)
                {
                    SortedAdsObj = sharedInstance.ads.Where(a => a.RewardedVideoPriority > 0).OrderBy(a => a.RewardedVideoPriority).ToList();
                }
                else
                {
                    SortedAdsObj = sharedInstance.ads.Where(a => a.RewardedVideoPriority > 0).OrderByDescending(a => a.RewardedVideoPriority).ToList();
                }
                return SortedAdsObj;
            }
        }

        public static void Initialize(bool UpdatePriority)
        {
#if !UNITY_STANDALONE
            if (sharedInstance == null)
            {
                sharedInstance = new GameObject("SKSAdsManager").AddComponent<AdsManager>();
                DontDestroyOnLoad(sharedInstance.gameObject);

                int admobAdsInterstitialPriority = 1;
                int admobAdsRewardedVideoPriority = 1;
                int admobAdsBannerPriority = 1;

                int ApplovinInterstitialPriority = 2;
                int ApplovinRewardedVideoPriority = 2;
                int ApplovinBannerPriority = 2;

                if (ApplovinInterstitialPriority >= 0 || ApplovinRewardedVideoPriority >= 0)
                {
                    AppLovinMaxAd appLovinAd1 = (AppLovinMaxAd)sharedInstance.GetAdNetworkObject("AppLovinMax", ApplovinInterstitialPriority, ApplovinRewardedVideoPriority, ApplovinBannerPriority, typeof(AppLovinMaxAd), RewardedVideoType.Applovin, null, GameIDs.AppLovinInterstitialAdUnitID1, GameIDs.AppLovinRewardAdUnitID1, GameIDs.AppLovinBannerAdUnitID1, GameIDs.AppLovinBannerID1Type, GameIDs.AppLovinBannerID1pos);

                    AppLovinMaxAd appLovinAd2 = (AppLovinMaxAd)sharedInstance.GetAdNetworkObject("AppLovinMax", ApplovinInterstitialPriority, ApplovinRewardedVideoPriority, ApplovinBannerPriority, typeof(AppLovinMaxAd), RewardedVideoType.Applovin, null, GameIDs.AppLovinInterstitialAdUnitID2, GameIDs.AppLovinRewardAdUnitID2, GameIDs.AppLovinBannerAdUnitID2, GameIDs.AppLovinBannerID2Type, GameIDs.AppLovinBannerID2pos);

                    sharedInstance.ads.Add(appLovinAd1);
                    sharedInstance.ads.Add(appLovinAd2);

                }
                if (admobAdsInterstitialPriority >= 0 || admobAdsRewardedVideoPriority >= 0)
                {
                    AdmobAd AdmobAd1 = (AdmobAd)sharedInstance.GetAdNetworkObject("AdmobAd", admobAdsInterstitialPriority, admobAdsRewardedVideoPriority, admobAdsBannerPriority, typeof(AdmobAd), RewardedVideoType.Admob, GameIDs.Admob_App_ID, GameIDs.Admob_Interstitial_AdUnit_Id1, GameIDs.Admob_Reward_AdUnit_Id1, GameIDs.AdmobBannerAdUnitID1, GameIDs.AdmobBannerID1Type, GameIDs.AdmobBannerID1pos);
                    sharedInstance.ads.Add(AdmobAd1);

                    AdmobAd AdmobAd2 = (AdmobAd)sharedInstance.GetAdNetworkObject("AdmobAd", admobAdsInterstitialPriority, admobAdsRewardedVideoPriority, admobAdsBannerPriority, typeof(AdmobAd), RewardedVideoType.Admob, GameIDs.Admob_App_ID, GameIDs.Admob_Interstitial_AdUnit_Id2, GameIDs.Admob_Reward_AdUnit_Id2, GameIDs.AdmobBannerAdUnitID2, GameIDs.AdmobBannerID2Type, GameIDs.AdmobBannerID2pos);
                    sharedInstance.ads.Add(AdmobAd2);

                    AdmobAd AdmobAd3 = (AdmobAd)sharedInstance.GetAdNetworkObject("AdmobAd", admobAdsInterstitialPriority, admobAdsRewardedVideoPriority, admobAdsBannerPriority, typeof(AdmobAd), RewardedVideoType.Admob, GameIDs.Admob_App_ID, GameIDs.Admob_Interstitial_AdUnit_Id3, GameIDs.Admob_Reward_AdUnit_Id3, GameIDs.AdmobBannerAdUnitID3, GameIDs.AdmobBannerID3Type, GameIDs.AdmobBannerID3pos);
                    sharedInstance.ads.Add(AdmobAd3);
                }

            }
#endif
            if (UpdatePriority)
            {
                if (DefaultData.SharedData().IsDownloadCallbackCalled())
                {
                    sharedInstance.SetupAdPriorities();
                }
                else
                {
                    DefaultData.OnDefaultDataDownloadingCompleted += sharedInstance.OnDefaultDataDownloadComplete;
                }
            }
        }

        public static void ShowInterstitial()
        {
            if (sharedInstance != null)
            {
                //if (InAppPurchaseScript.isProUser() || !DefaultData.SharedData().EligbleForAd)
                //{
                //    return;
                //}

                var adsList = sharedInstance.GetSortedInterstitialsList();

                for (int i = 0; i < adsList.Count; i++)
                {
                    Debug.Log("AdAvailable:" + adsList[i].IsInterstitialAvailable() + "::AdName::" + adsList[i].InterstitialPriority);
                    if (adsList[i].IsInterstitialAvailable())
                    {

                        adsList[i].ShowInterstitial();
                        break;
                    }
                }
            }
        }
        static void CheckToShowBigBannerAd()
        {
            ShowBanner(true);
        }
        static void CheckToShowSmartBannerAd()
        {
            ShowBanner(false);
        }
        static void CheckIfBannerInitialized(bool isBigBanner)
        {
            if (isBigBanner && !isBigBannerInitialized)
            {
                isBigBannerInitialized = true;
            }
            else if (!isBigBanner && !isSmartBannerInitialized)
            {
                isSmartBannerInitialized = true;
            }
        }
        public static void ShowBanner(bool isBigBanner)
        {
            if (sharedInstance != null)
            {
                HideBanner(false);
                UnInitilaizeBannerRegisterMethod(isBigBanner);
                //if (InAppPurchaseScript.isProUser())
                //{
                //    return;
                //}
                var adsList = sharedInstance.GetSortedBannerAdsList();

                for (int i = 0; i < adsList.Count; i++)
                {
                    if (adsList[i].IsBannerAvailable(isBigBanner))
                    {
                        CheckIfBannerInitialized(isBigBanner);

                        adsList[i].ShowBanner(isBigBanner);
                        break;
                    }
                }
                if (isBigBanner && !isBigBannerInitialized)
                {
                    OnBigBannerAdInitialized += CheckToShowBigBannerAd;
                }
                else if (!isBigBanner && !isSmartBannerInitialized)
                {
                    OnSmartBannerAdInitialized += CheckToShowSmartBannerAd;
                }
            }
        }
        static void UnInitilaizeBannerRegisterMethod(bool isBigBanner)
        {
            if (isBigBanner)
            {
                OnBigBannerAdInitialized -= CheckToShowBigBannerAd;
            }
            else
            {
                OnSmartBannerAdInitialized -= CheckToShowSmartBannerAd;
            }
        }
        public static void HideBanner(bool isBigBanner)
        {
            if (sharedInstance != null)
            {
                UnInitilaizeBannerRegisterMethod(isBigBanner);
                var adsList = sharedInstance.GetSortedBannerAdsList();

                for (int i = 0; i < adsList.Count; i++)
                {
                    adsList[i].HideBanner(isBigBanner);
                }
            }
        }

        public static bool isAdAvailableAndReady(string placementId, bool isInterstitial)
        {
            bool isAvailable = false;
            if (sharedInstance != null)
            {
                var adsList = sharedInstance.GetSortedInterstitialsList();

                for (int i = 0; i < adsList.Count; i++)
                {
                    if (isInterstitial)
                    {
                        if (adsList[i].InterstitialPriority.Equals(placementId) && adsList[i].IsInterstitialAvailable())
                        {
                            isAvailable = true;
                            break;
                        }
                    }
                    else
                    {
                        if (adsList[i].RewardedVideoId.Equals(placementId) && adsList[i].IsRewardedVideoAvailable())
                        {
                            isAvailable = true;
                            break;
                        }
                    }
                }
            }

            return isAvailable;
        }
        public static bool IsBannerAvailable(bool isBigBanner)
        {
            bool isAvailable = false;
            if (sharedInstance != null)
            {

                var adsList = sharedInstance.GetSortedBannerAdsList();

                for (int i = 0; i < adsList.Count; i++)
                {
                    if (adsList[i].IsBannerAvailable(isBigBanner))
                    {
                        isAvailable = true;
                        break;
                    }
                }
            }

            return isAvailable;
        }
        public static bool IsInterstitialAvailable()
        {
            bool isAvailable = false;
            if (sharedInstance != null)
            {

                var adsList = sharedInstance.GetSortedInterstitialsList();

                for (int i = 0; i < adsList.Count; i++)
                {
                    if (adsList[i].IsInterstitialAvailable())
                    {
                        isAvailable = true;
                        break;
                    }
                }
            }

            return isAvailable;
        }
        private static void CheckAndPlayForRewardedVideo(bool IsSingleAd, List<ExcludedAdData> excludedData, System.Action<bool> Callback, Action<bool, RewardedVideoType, string> CallbackDetailedForSuccessOnly, bool GetDescSortingOrder = false, bool RestartAdIfFailed = true)
        {
            if (sharedInstance == null)
            {
                Callback(false);
            }

            else
            {
                var adsList = sharedInstance.GetSortedRewardedVideosList(GetDescSortingOrder);

                List<AdsObject> availableAdsList = new List<AdsObject>();

                if (excludedData == null)
                {
                    availableAdsList = adsList;
                }
                else
                {
                    for (int i = 0; i < adsList.Count; i++)
                    {
                        var obj = adsList[i];

                        var adNetworkToExclude = excludedData.Where(d => d.adType == obj.MyType && d.rewardedVideoId == obj.RewardedVideoId).FirstOrDefault();

                        if (adNetworkToExclude == null)
                        {
                            availableAdsList.Add(obj);
                        }
                    }
                }

                if (availableAdsList.Count > 0)
                {

                    bool isShowingAd = false;
                    Resources.UnloadUnusedAssets();
                    System.GC.Collect();
                    for (int i = 0; i < availableAdsList.Count; i++)
                    {
                        if (availableAdsList[i].IsRewardedVideoAvailable())
                        {
                            SKSDebug.Log("ShowingAd" + availableAdsList[i].MyType.ToString());

                            bool IsAdAvailable = availableAdsList[i].ShowRewardedVideo(IsSingleAd, (AdsObject.RewardedVideoAwardType resultType, RewardedVideoType adType, string rewardedVideoId) =>
                            {

                                if (resultType == AdsObject.RewardedVideoAwardType.kCompleted)
                                {
                                    DefaultData.SharedData().RewardAdDisplayed();
                                    SKSDebug.Log("AdCompleted" + adType.ToString());

                                    Callback?.Invoke(true);
                                    CallbackDetailedForSuccessOnly?.Invoke(true, adType, rewardedVideoId);
                                }
                                else if (resultType == AdsObject.RewardedVideoAwardType.kFailedAfterCaching && RestartAdIfFailed)
                                {
                                    if (excludedData == null)
                                    {
                                        excludedData = new List<ExcludedAdData>();
                                    }
                                    SKSDebug.Log("AdFailedAfterCaching" + adType.ToString());

                                    excludedData.Add(new ExcludedAdData { adType = adType, rewardedVideoId = rewardedVideoId });

                                    CheckAndPlayForRewardedVideo(IsSingleAd, excludedData, Callback, CallbackDetailedForSuccessOnly, false, RestartAdIfFailed);

                                }
                                else
                                {
                                    SKSDebug.Log("AdSkipped" + adType.ToString());
                                    Callback?.Invoke(false);
                                }

                            });
                            if (IsAdAvailable)
                            {
                                isShowingAd = true;
                                break;
                            }
                        }
                    }

                    if (!isShowingAd)
                    {
                        Callback?.Invoke(false);
                    }
                }
                else
                {
                    Callback?.Invoke(false);
                }
            }
        }

        public static void ShowRewardedVideo(Action<bool> Callback, bool IsSingleAd = true, List<ExcludedAdData> excludedAds = null, Action<bool, RewardedVideoType, string> CallbackDetailedForSuccessOnly = null, bool GetDescSortingOrder = false, bool RestartAdIfFailed = true)
        {

            CheckAndPlayForRewardedVideo(IsSingleAd, excludedAds, Callback, CallbackDetailedForSuccessOnly, GetDescSortingOrder, RestartAdIfFailed);
        }

        public static bool IsRewardedVideoAvailable(List<ExcludedAdData> excludedData = null)
        {
            //#if UNITY_EDITOR
            //            return true;

            if (!DefaultData.IsInternetAvailable())
            {
                return false;
            }

            //#else
            if (sharedInstance == null)
            {
                return false;
            }

            bool isAvailable = false;

            var adsList = sharedInstance.GetSortedRewardedVideosList();

            List<AdsObject> availableAdsList = new List<AdsObject>();

            if (excludedData == null)
            {
                availableAdsList = adsList;
            }
            else
            {
                for (int i = 0; i < adsList.Count; i++)
                {
                    var obj = adsList[i];

                    var adNetworkToExclude = excludedData.Where(d => d.adType == obj.MyType && d.rewardedVideoId == obj.RewardedVideoId).FirstOrDefault();

                    if (adNetworkToExclude == null)
                    {
                        availableAdsList.Add(obj);
                    }
                }
            }

            if (availableAdsList.Count > 0)
            {
                for (int i = 0; i < availableAdsList.Count; i++)
                {
                    if (availableAdsList[i].IsRewardedVideoAvailable())
                    {
                        isAvailable = true;
                        break;
                    }
                }
            }

            return isAvailable;
            //#endif

        }

        public static bool IsDoubleRewardedVideoAvailable()
        {
            if (sharedInstance == null)
            {
                return false;
            }

            int availableAds = 0;

            var adsList = sharedInstance.GetSortedRewardedVideosList();

            for (int i = 0; i < adsList.Count; i++)
            {
                if (adsList[i].IsRewardedVideoAvailable())
                {
                    availableAds++;
                }
            }

            return availableAds >= 2;
        }
    }
}