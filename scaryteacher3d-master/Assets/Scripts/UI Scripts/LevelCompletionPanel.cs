using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelCompletionPanel : UIPopup
{
    [SerializeField] private List <Button> utilityButtons;

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
    }

    public void NextButtonClicked()
    {
        PlayerPrefs.SetInt(GameConstants.InGameConstants.LEVEL_IDENTIFIER + StoryModeLevelManager.Instance.CurrentLevelNumber.ToString(), 1);
        PlayerPrefs.Save();
        StoryModeLevelManager.Instance.onLevelCompleted?.Invoke(true);
        Destroy(this.gameObject);
    }

    public void ReplayButtonClicked()
    {
        StoryModeLevelManager.Instance.onLevelCompleted?.Invoke(false);
        Destroy(this.gameObject);
    }

    public void HomeButtonClicked()
    {
        PlayerPrefs.SetInt(GameConstants.InGameConstants.LEVEL_IDENTIFIER + StoryModeLevelManager.Instance.CurrentLevelNumber.ToString(), 1);
        PlayerPrefs.Save();
        SceneManager.LoadScene("MainMenu");
    }
}
