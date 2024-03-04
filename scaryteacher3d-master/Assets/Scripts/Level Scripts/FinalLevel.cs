using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class FinalLevel : BasicLevelStructure
{
   [SerializeField] private TeacherController teacherController;
    [SerializeField] private Image playerDialogueBox;
    [SerializeField] private Image teacherDialogueBox;

    private void Awake()
    {
        Invoke(nameof(MakeTeacherSit), 1f);
    }

    public void MakeTeacherSit()
    {
        teacherController.AnimateTeacher(GameConstants.InGameConstants.TeacherAnimationNames.SittingIdle);
    }

    public override void OnInteractableButtonClicked()
    {

        if (currentSelectedProp != GameConstants.InGameConstants.LevelProps.None.ToString())
        {
            prevSelectedProp = currentSelectedProp;
            currentSelectedProp = GameConstants.InGameConstants.LevelProps.None.ToString();
            objectInteraction.gameObject.SetActive(false);
        }

        if (prevSelectedProp == GameConstants.InGameConstants.LevelProps.TeacherBoundary.ToString())
        {
            teacherController.AnimateTeacher(GameConstants.InGameConstants.TeacherAnimationNames.Idle);
            teacherController.transform.localPosition = new Vector3(teacherController.transform.localPosition.x, teacherController.transform.localPosition.y, teacherController.transform.localPosition.z-2);
            teacherController.transform.LookAt(schoolBoyController.transform);
            objectInteraction.SetActive(false);
            playerDialogueBox.gameObject.SetActive(true);
            playerDialogueBox.GetComponentInChildren<Text>().DOText("I apologise for these absurd misbehaviours, and I promise not to repeat them in the future.", 1f).onComplete = () =>
            {
                LeanTween.scale(playerDialogueBox.gameObject, Vector3.one * 1.25f, 1f).setLoopPingPong();
            };
            teacherDialogueBox.GetComponent<Button>().enabled = false;
        }

    }


    public void OnDialogueBoxClicked(bool isPlayerDialogueBox)
    {
        if (isPlayerDialogueBox)
        {
            LeanTween.cancel(playerDialogueBox.gameObject);
            playerDialogueBox.gameObject.SetActive(false);
            teacherDialogueBox.gameObject.SetActive(true);
            teacherDialogueBox.GetComponentInChildren<Text>().DOText("I see, my son. Do not do this kind of mischief again.", 1f).onComplete = () =>
            {
                LeanTween.scale(teacherDialogueBox.gameObject, Vector3.one * 1.25f, 1f).setLoopPingPong();
            };
            teacherDialogueBox.GetComponent<Button>().enabled = true;
        }
        else
        {
            LeanTween.cancel(teacherDialogueBox.gameObject);
            teacherDialogueBox.gameObject.SetActive(false);
            teacherController.AnimateTeacher(GameConstants.InGameConstants.TeacherAnimationNames.Clapping);
            LevelCompletionSequence();
        }
    }

}
