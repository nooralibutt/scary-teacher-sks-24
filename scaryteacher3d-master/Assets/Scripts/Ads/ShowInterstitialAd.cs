using UnityEngine;

public class ShowInterstitialAd : MonoBehaviour
{
    private void OnEnable()
    {
        AdController.Instance.HideAdmobBanner();
        AdController.Instance.ShowCountedInterstitialAd();
    }

    private void OnDisable()
    {
        AdController.Instance.ShowAdmobBanner();
    }
}
