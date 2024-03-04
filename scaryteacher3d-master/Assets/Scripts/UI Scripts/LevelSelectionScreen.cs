using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectionScreen : MonoBehaviour
{
    [SerializeField] private LevelButton levelButton;
    [SerializeField] private Transform contentParent;

    [SerializeField] private Button playButton;

    private List<LevelButton> levelButtons = new List<LevelButton>();

    public void PrepareLevelSelectionScreen()
    {
        float delay = 0f;
        for (int i = 0; i < GameConstants.InGameConstants.totalLevelsInStoryMode; i++)
        {
            var cell = Instantiate(levelButton, contentParent);
            cell.onButtonClicked = UpdateButtonUI;
            levelButtons.Add(cell);
            if (PlayerPrefs.HasKey(GameConstants.InGameConstants.LEVEL_IDENTIFIER + i) || i == 0 || PlayerPrefs.HasKey(GameConstants.InGameConstants.LEVEL_IDENTIFIER + (i-1).ToString()))
            {
                cell.DisplayLevelCell(i, false, delay);
            }
            else
            {
                cell.DisplayLevelCell(i, true, delay);
            }
            delay += 0.25f;
        }
    }

    //private void OnDisable()
    //{
    //    RemoveAllContent();
    //}

    public void UpdateButtonUI()
    {
        playButton.gameObject.transform.localScale = Vector3.zero;
        LeanTween.scale(playButton.gameObject, Vector3.one, 1f).setEaseOutBack();
        playButton.gameObject.SetActive(true);
        for (int i = 0; i < levelButtons.Count; i++)
        {
            if (GameConstants.InGameConstants.selectedLevel == i)
                levelButtons[i].UpdatState(true);
            else
                levelButtons[i].UpdatState(false);
        }
    }

    public void RemoveAllContent()
    {
        foreach (Transform child in contentParent)
        {
            Destroy(child.gameObject);
        }
        levelButtons.Clear();
    }

    public void NextButtonClicked()
    {
        SceneTransitionManager.Instance.LoadScene("StoryModeScene");
    }

    public void HomeButtonClicked()
    {
        RemoveAllContent();
        FindObjectOfType<ModeSelectionPanel>().GetComponent<CanvasGroup>().alpha = 1;
        gameObject.SetActive(false);
    }
}
