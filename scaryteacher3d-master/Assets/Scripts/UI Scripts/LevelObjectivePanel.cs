using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class LevelObjectivePanel : UIPopup
{
    [SerializeField] private Text objectiveText;
    [SerializeField] private Button PlayButton;

    public void PopulateObjective(string text)
    {
        objectiveText.DOText(text, 2f).SetEase(Ease.Linear).onComplete=()=>
        {
            PlayButton.transform.localScale = Vector3.zero;
            PlayButton.gameObject.SetActive(true);
            LeanTween.scale(PlayButton.gameObject, Vector3.one, 0.75f).setEaseOutBack();
        };
    }

    public void OKButtonClicked()
    {
        base.HidePopup(() =>
        {
            StoryModeGameManager.Instance.TogglePlayerMovement(true);
            StoryModeGameManager.Instance.currentLevel.ToggleCutSceneCamera(false);
            StoryModeGameManager.Instance.currentLevel.ToggleTeacherAnimation(true);
            StoryModeGameManager.Instance.isLevelBeingPlayed = true;
            UIManager.Instance.ToggleJoyStick(true);
            Destroy(gameObject); 
        });
    }
}
