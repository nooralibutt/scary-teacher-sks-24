using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SKS.Ads;

public class SettingsPanel : MonoBehaviour
{
    [SerializeField] private Toggle soundToggle, musicToggle;

    // Start is called before the first frame update
    void Start()
    {
        AdsManager.ShowBanner(true);
        soundToggle.onValueChanged.AddListener(ToggleSound);
        musicToggle.onValueChanged.AddListener(ToggleMusic);
    }

    public void ExitButtonClicked()
    {
        AdsManager.HideBanner(true);
        gameObject.SetActive(false);
    }

    public void ToggleSound(bool toggle)
    {
        AudioManager.Instance.isSoundEnabled = toggle;
        
    }
    public void ToggleMusic(bool toggle)
    {
        AudioManager.Instance.isMusicEnabled = toggle;
        if (toggle)
            AudioManager.Instance.GetComponent<AudioSource>().volume = 1;
        else
            AudioManager.Instance.GetComponent<AudioSource>().volume = 0;
    }
}
