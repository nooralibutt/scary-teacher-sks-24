using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using SKS.Ads;

public class SceneTransitionManager : MonoBehaviour
{
    public static SceneTransitionManager Instance;

    [SerializeField] private GameObject _loaderCanvas;
    [SerializeField] private Image _progressBar;
    private float target;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public async void LoadScene(string sceneName)
    {
        target = 0f;
        _progressBar.fillAmount = 0f;
        var scene = SceneManager.LoadSceneAsync(sceneName);
        scene.allowSceneActivation = false;
        
        _loaderCanvas.SetActive(true);

        do
        {
            await System.Threading.Tasks.Task.Delay(100);
            target = scene.progress; 

        } while (scene.progress < 0.9f);

        await Task.Delay(1000);

        scene.allowSceneActivation = true;
        _loaderCanvas.SetActive(false);
    }


    public async void ShowLoadingScreen(float seconds, int delay, System.Action completionCallback, bool showBigBanner = false, bool showInterstitial = false)
    {
        target = 0f;
        _progressBar.fillAmount = 0f;
        _loaderCanvas.SetActive(true);
        if (showBigBanner)
            AdsManager.ShowBanner(true);
        bool interstitialShown = false;
        do
        {
            await System.Threading.Tasks.Task.Delay(delay);
            target += 1 / seconds;
            if (target > 0.7)
                if (!interstitialShown)
                {
                    if (showInterstitial)
                    {
                        AdsManager.ShowInterstitial();
                        interstitialShown = true;
                    }
                }

        } while (target < 0.95f);

        AdsManager.HideBanner(true);
        _loaderCanvas.SetActive(false);
        completionCallback?.Invoke();
    }

    private void Update()
    {
        _progressBar.fillAmount = Mathf.MoveTowards(_progressBar.fillAmount, target, Time.deltaTime * 3);
    }

}
