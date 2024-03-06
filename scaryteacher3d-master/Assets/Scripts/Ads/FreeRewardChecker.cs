using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

namespace CC
{
    public class FreeRewardChecker : MonoBehaviour
    {
        /// <summary>
		/// Url to load
		/// e.g. https://nooralibutt.github.io/unity/scary.stranger.json
		/// </summary>
		[SerializeField]
        private string url;

        private static RemoteConfig Config = new RemoteConfig();
        public static bool IsApproving
        {
            get
            {
#if UNITY_ANDROID
                return IsAndroidApproving;
#elif UNITY_IPHONE
                return IsIosApproving;
#else
                return true;
#endif
            }
        }

        private static bool IsIosApproving { get { return Config.ad_settings.is_ios_approving; } }
        private static bool IsAndroidApproving { get { return Config.ad_settings.is_android_approving; } }
        public static int InterstitialAdCount { get { return Config.ad_settings.interstitial_ad_count; } }

        void Start()
        {
            if (string.IsNullOrEmpty(url))
            {
                Debug.LogError("Remote json url is null. Go to Free Reward Checker script and assign the url"); 
            }
            else
            {
                StartCoroutine(GetSettings());
            }
        }

        IEnumerator GetSettings()
        {
            using (UnityWebRequest www = UnityWebRequest.Get(url))
            {
                yield return www.SendWebRequest();

                if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
                {
                    Debug.LogError(www.error);
                }
                else
                {
                    // Show results as text
                    string json = www.downloadHandler.text.Replace("\n", "");
                    Debug.Log("Remote Json:\n" + json);
                    Config = RemoteConfig.CreateFromJSON(json);
                }
            }
        }
    }
}


[System.Serializable]
public class RemoteConfig
{
    public AdSettings ad_settings = new AdSettings();

    public static RemoteConfig CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<RemoteConfig>(jsonString);
    }

    public override string ToString()
    {
        return string.Format("Ad Settings:\n{0}", ad_settings.ToString());
    }
}

[System.Serializable]
public class AdSettings
{
    public int interstitial_ad_count = 2;
    public bool is_ios_approving = true;
    public bool is_android_approving = true;

    public override string ToString()
    {
        return string.Format("interstitialCounter:{0}, isIosApproving:{1}, isAndroidApproving:{1}", interstitial_ad_count, is_ios_approving, is_android_approving);
    }
}