using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
//using SKS.Ads;

public class MainMenuPanel : MonoBehaviour
{

    [SerializeField] private ModeSelectionPanel modeSelectionPanel;
    [SerializeField] private SettingsPanel settingsPanel;
    [SerializeField] private Button[] utilityButtons;
    [SerializeField] private Button playButton;
    [SerializeField] private Image background;

    private void Start()
    {
        //AdsManager.ShowBanner(false);
        //SceneTransitionManager.Instance.ShowLoadingScreen(100, ()=>
        //{
        //    GetComponent<Image>().color = Color.black;
        //    for (int i = 0; i < utilityButtons.Length; i++)
        //    {
        //        utilityButtons[i].transform.localScale = Vector3.zero;
        //    }
        //    playButton.transform.localScale = Vector3.zero;


        //    AnimateMainMenuPanel();
        //});
        GetComponent<Image>().color = Color.black;
        for (int i = 0; i < utilityButtons.Length; i++)
        {
            utilityButtons[i].transform.localScale = Vector3.zero;
        }
        playButton.transform.localScale = Vector3.zero;


        AnimateMainMenuPanel();

    }

    public void PlayGameButtonClicked()
    {
        GetComponent<CanvasGroup>().alpha = 0;
        SceneTransitionManager.Instance.ShowLoadingScreen(10, 100, () => {
            modeSelectionPanel.gameObject.SetActive(true);
        }, true, true);

    }

    public void AnimateMainMenuPanel()
    {
        background.DOColor(Color.white, 0.75f).SetDelay(0.25f).onComplete = () =>
        {
            float delay = 0f;
            for (int i = 0; i < utilityButtons.Length; i++)
            {
                LeanTween.scale(utilityButtons[i].gameObject,Vector3.one,0.25f).setEaseOutBack().setDelay(delay);
                delay += 0.25f;
            }
            LeanTween.scale(playButton.gameObject, Vector3.one, 0.75f).setEaseOutBack().setDelay(delay).setOnComplete(()=>
            {
                LeanTween.scale(playButton.gameObject, Vector3.one * 1.25f, 1.5f).setLoopPingPong();
            });
        };
    }

    public void OnSettingPanelClicked()
    {
        settingsPanel.gameObject.SetActive(true);
    }
}
