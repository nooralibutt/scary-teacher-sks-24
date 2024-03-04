using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using SKS.Ads;
using UnityEngine.UI;

public class GamePausePanel : MonoBehaviour
{
    [SerializeField] private Button resumeButton;

    private void Start()
    {
        AdsManager.ShowBanner(true);
        LeanTween.scale(resumeButton.gameObject, Vector3.one * 1.25f, 1).setLoopPingPong();
        Time.timeScale = 0;
    }

    public void ReumeButtonClicked()
    {
        AdsManager.ShowInterstitial();
        Time.timeScale = 1;
        AdsManager.HideBanner(true);
        Destroy(this.gameObject);
    }
    
    public void ReplayButtonClicked()
    {
        AdsManager.ShowInterstitial();
        StoryModeLevelManager.Instance.onLevelCompleted?.Invoke(false);
        Time.timeScale = 1;
        AdsManager.HideBanner(true);
        Destroy(this.gameObject);
    }

    public void HomeButtonClicked()
    {
        AdsManager.ShowInterstitial();
        Time.timeScale = 1;
        AdsManager.HideBanner(true);
        SceneManager.LoadScene("MainMenu");
    }
}
