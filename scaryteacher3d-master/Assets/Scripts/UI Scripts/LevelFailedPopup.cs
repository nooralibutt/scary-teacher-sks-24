using System.Collections;
using System.Collections.Generic;
using SKS.Ads;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelFailedPopup : UIPopup
{

    [SerializeField] private List<Button> utilityButtons;
    private void Start()
    {
        base.ShowPopup(()=>
        {

            for (int i = 0; i < utilityButtons.Count; i++)
            {
                utilityButtons[i].gameObject.SetActive(true);
                LeanTween.scale(utilityButtons[i].gameObject, Vector3.one, 0.5f).setEaseOutBack();
            }
        });
        ShowAd();
    }

    public void ReplayButtonClicked()
    {
        StoryModeLevelManager.Instance.onLevelCompleted?.Invoke(false);
        Destroy(this.gameObject);
    }

    public void HomeButtonClicked()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void ShowAd()
    {
        AdsManager.ShowInterstitial();
    }
}
