using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ModeSelectionPanel : MonoBehaviour
{
    [SerializeField] public Sprite selectedState;

    [SerializeField] private List<Image> modeButtons;

    [SerializeField] private Image nextButton;

    [SerializeField] private LevelSelectionScreen levelSeclectionScreen;

    private GameConstants.InGameConstants.GameModes selectedMode = GameConstants.InGameConstants.GameModes.None;

    private void Start()
    {
        for (int i = 0; i < modeButtons.Count; i++)
        {
            modeButtons[i].gameObject.transform.localScale = Vector3.zero;
        }

        float delay = 0f;
        for (int i = 0; i < modeButtons.Count; i++)
        {
            LeanTween.scale(modeButtons[i].gameObject, Vector3.one, 0.5f).setDelay(delay).setEaseOutBack();
            delay += 0.5f;
        }
    }

    public void OnModeSelected(int index)
    {
        nextButton.gameObject.transform.localScale = Vector3.zero;
        LeanTween.scale(nextButton.gameObject, Vector3.one, 0.8f).setEaseOutBack();
        nextButton.gameObject.SetActive(true);
        modeButtons[index].sprite = selectedState;
        modeButtons[index].color = new Color(1f, 1f, 1f, .5f);
        selectedMode = GameConstants.InGameConstants.GameModes.StoryMode;
    }

    public void NextButtonClicked()
    {
        if (selectedMode == GameConstants.InGameConstants.GameModes.StoryMode)
        {
            //SceneTransitionManager.Instance.LoadScene("StoryModeScene");
            GetComponent<CanvasGroup>().alpha = 0;
            levelSeclectionScreen.gameObject.SetActive(true);
            levelSeclectionScreen.PrepareLevelSelectionScreen();
        }
    }

    public void HomeButtonClicked()
    {
        FindObjectOfType<MainMenuPanel>().GetComponent<CanvasGroup>().alpha = 1;
        gameObject.SetActive(false);
    }
}
