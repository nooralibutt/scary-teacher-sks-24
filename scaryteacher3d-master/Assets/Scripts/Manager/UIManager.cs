using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SKS.Ads;

public class UIManager : MonoBehaviour
{

    public static UIManager Instance;

    [SerializeField] private GameObject joyStickReference;
    [SerializeField] private GameObject trackpad;
    [SerializeField] private GamePausePanel gamePausePanel;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public GameObject InstantiatePopupInGamePlay(GameObject popup)
    {
        return Instantiate(popup, StoryModeGameManager.Instance.currentLevel.GamePlayCanvas());
    }

    public void ToggleJoyStick(bool toggle)
    {
        joyStickReference.SetActive(toggle);
        trackpad.SetActive(toggle);
    }

    public void DisplayGamePausePanel()
    {
        Time.timeScale = 0;
        Instantiate(gamePausePanel.gameObject, StoryModeGameManager.Instance.currentLevel.GamePlayCanvas());
    }
}
