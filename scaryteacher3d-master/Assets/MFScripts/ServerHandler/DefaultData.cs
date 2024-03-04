using System;
using System.Collections;
using System.Collections.Generic;
using SKS.Ads;
using UnityEngine;

public class DefaultData : MonoBehaviour
{
    public static Action OnDefaultDataDownloadingCompleted;
    public int InterstititalCoolDownTimeAfterRewardAd = 7;
    [HideInInspector]
    public bool EligbleForAd = true;
    public List<AdPriorityInfo> interstitalPriorities = new List<AdPriorityInfo>();
    public List<AdPriorityInfo> rewardedVideoPriorities = new List<AdPriorityInfo>();
    public List<AdPriorityInfo> BannerAdPriorities = new List<AdPriorityInfo>();

    private static DefaultData sharedInstance = null;
    private bool isDownloadCallbackCalled = false;
    [HideInInspector]
    public bool UseMaxSdkProperMethod = true;

    void Awake()
    {
#if UNITY_EDITOR
        Application.targetFrameRate = 120;
#else
        if (UseNewMethod())
        {
            //QualitySettings.vSyncCount = 1;
            Application.targetFrameRate = Screen.currentResolution.refreshRate;
        }
        else
        {
            //QualitySettings.vSyncCount = 1;
            Application.targetFrameRate = 30;
        }
#endif
        if (sharedInstance == null)
        {
            sharedInstance = this;
            DontDestroyOnLoad(sharedInstance.gameObject);

        }
        else if (sharedInstance != this)
        {
            Destroy(gameObject);
        }
    }
    public void RewardAdDisplayed()
    {
        EligbleForAd = false;
        CancelInvoke(nameof(ResetAfterCoolDowntime));
        Invoke(nameof(ResetAfterCoolDowntime), InterstititalCoolDownTimeAfterRewardAd);
    }
    void ResetAfterCoolDowntime()
    {
        EligbleForAd = true;
    }
    private void Start()
    {
        Initialize();
    }
    bool UseNewMethod()
    {
        bool useNewMthod = true;
        try
        {
#if UNITY_ANDROID
            if (SystemInfo.systemMemorySize < 1500)
            {
                useNewMthod = false;
            }
#endif
        }
        catch (Exception exc)
        {
        }
        return useNewMthod;
    }
    void OnDestroy()
    {
        if (sharedInstance != null && sharedInstance == this)
        {
            Destroy(sharedInstance);
        }
    }
    public bool IsDownloadCallbackCalled()
    {
        return isDownloadCallbackCalled;
    }
    public static DefaultData SharedData()
    {
        if (!sharedInstance)
        {
            sharedInstance = FindObjectOfType(typeof(DefaultData)) as DefaultData;

            if (!sharedInstance)
            {
                var obj = new GameObject("DefaultData");
                sharedInstance = obj.AddComponent<DefaultData>();
            }
            else
            {
                sharedInstance.gameObject.name = "DefaultData";
            }
        }
        return sharedInstance;
    }
    public void Initialize()
    {
        interstitalPriorities.Add(new AdPriorityInfo(RewardedVideoType.Admob, 1));
        interstitalPriorities.Add(new AdPriorityInfo(RewardedVideoType.Applovin, 2));

        rewardedVideoPriorities.Add(new AdPriorityInfo(RewardedVideoType.Admob, 1));
        rewardedVideoPriorities.Add(new AdPriorityInfo(RewardedVideoType.Applovin, 2));

        BannerAdPriorities.Add(new AdPriorityInfo(RewardedVideoType.Admob, 1));
        BannerAdPriorities.Add(new AdPriorityInfo(RewardedVideoType.Applovin, 2));
        AdsManager.Initialize(true);

        isDownloadCallbackCalled = true;
        OnDefaultDataDownloadingCompleted?.Invoke();
    }
    public static bool IsInternetAvailable()
    {
        return Application.internetReachability == NetworkReachability.NotReachable ? false : true;
    }
}
public class AdPriorityInfo
{
    public RewardedVideoType adType;
    public int priority;

    public AdPriorityInfo(RewardedVideoType adType, int priority)
    {
        this.adType = adType;
        this.priority = priority;
    }
}
