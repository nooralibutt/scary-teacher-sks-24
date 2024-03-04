using static SKS.Ads.AdsObject;


public static class GameIDs
{
    #region AppLovin
    //***********************APP_LOVIN*************************//

    public const string APPLOVIN_AD_SDK_KEY = "-8UPgneTQnQwkR_y4zSOx3RZE1HnncAlle9QPpurZIlIlkEphgCcldBzfTlR4vBqTKEz77ohPX-9GDyyv9Zdom";

#if UNITY_ANDROID
    public const string AppLovinRewardAdUnitID1 = "";
    public const string AppLovinRewardAdUnitID2 = "";
    public const string AppLovinInterstitialAdUnitID1 = "";
    public const string AppLovinInterstitialAdUnitID2 = "";

    public const string AppLovinBannerAdUnitID1 = "";
    public const BannerType AppLovinBannerID1Type = BannerType.SmartBanner;
    public const BannerPosition AppLovinBannerID1pos = BannerPosition.Bottom;

    public const string AppLovinBannerAdUnitID2 = "";
    public const BannerType AppLovinBannerID2Type = BannerType.SmartBanner;
    public const BannerPosition AppLovinBannerID2pos = BannerPosition.Bottom;

#else
    public const string AppLovinRewardAdUnitID1 = "774d1600645cc555";
    public const string AppLovinRewardAdUnitID2 = "";
    public const string AppLovinInterstitialAdUnitID1 = "29cbeb948cd16a18";
    public const string AppLovinInterstitialAdUnitID2 = "";

    public const string AppLovinBannerAdUnitID1 = "6cb37762c1296748";
    public const BannerType AppLovinBannerID1Type = BannerType.SmartBanner;
    public const BannerPosition AppLovinBannerID1pos = BannerPosition.Bottom;

    public const string AppLovinBannerAdUnitID2 = "";
    public const BannerType AppLovinBannerID2Type = BannerType.SmartBanner;
    public const BannerPosition AppLovinBannerID2pos = BannerPosition.Bottom;

#endif
    #endregion
    //***********************Admob ADS*************************//

    #region Admob
#if UNITY_ANDROID
    public const string Admob_App_ID = "ca-app-pub-3940256099942544~3347511713";

    public const string Admob_Reward_AdUnit_Id1 = "";
    public const string Admob_Reward_AdUnit_Id2 = "";
    public const string Admob_Reward_AdUnit_Id3 = "";
    public const string Admob_Interstitial_AdUnit_Id1 = "ca-app-pub-3940256099942544/1033173712";
    public const string Admob_Interstitial_AdUnit_Id2 = "";
    public const string Admob_Interstitial_AdUnit_Id3 = "";

    public const string AdmobBannerAdUnitID1 = "ca-app-pub-8949389946267549/1124271913";
    public const BannerType AdmobBannerID1Type = BannerType.SmartBanner;
    public const BannerPosition AdmobBannerID1pos = BannerPosition.Top;

    public const string AdmobBannerAdUnitID2 = "ca-app-pub-8949389946267549/5063516924";
    public const BannerType AdmobBannerID2Type = BannerType.SmartBanner;
    public const BannerPosition AdmobBannerID2pos = BannerPosition.Top;

    public const string AdmobBannerAdUnitID3 = "ca-app-pub-8949389946267549/6376598592";
    public const BannerType AdmobBannerID3Type = BannerType.BigBanner;
    public const BannerPosition AdmobBannerID3pos = BannerPosition.BottomLeft;
#else
    public const string Admob_App_ID = "ca-app-pub-8949389946267549~9486015655";

    public const string Admob_Reward_AdUnit_Id1 = "ca-app-pub-8949389946267549/6223721206";
    public const string Admob_Reward_AdUnit_Id2 = "a-app-pub-8949389946267549/2152135328";
    public const string Admob_Reward_AdUnit_Id3 = "ca-app-pub-8949389946267549/3273645307";
    public const string Admob_Interstitial_AdUnit_Id1 = "ca-app-pub-8949389946267549/3822324117";
    public const string Admob_Interstitial_AdUnit_Id2 = "ca-app-pub-8949389946267549/4969870356";
    public const string Admob_Interstitial_AdUnit_Id3 = "ca-app-pub-8949389946267549/8717543676";

    public const string AdmobBannerAdUnitID1 = "ca-app-pub-8949389946267549/6376598592";
    public const BannerType AdmobBannerID1Type = BannerType.SmartBanner;
    public const BannerPosition AdmobBannerID1pos = BannerPosition.Top;

    public const string AdmobBannerAdUnitID2 = "ca-app-pub-8949389946267549/5063516924";
    public const BannerType AdmobBannerID2Type = BannerType.SmartBanner;
    public const BannerPosition AdmobBannerID2pos = BannerPosition.Top;

    public const string AdmobBannerAdUnitID3 = "ca-app-pub-8949389946267549/1124271913";
    public const BannerType AdmobBannerID3Type = BannerType.BigBanner;
    public const BannerPosition AdmobBannerID3pos = BannerPosition.BottomLeft;


#endif
    #endregion

}
