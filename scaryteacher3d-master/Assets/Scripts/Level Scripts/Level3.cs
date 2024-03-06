using System.Collections;
using System.Collections.Generic;
//using SKS.Ads;
using UnityEngine;

public class Level3 : BasicLevelStructure
{
    private int hitCount = 0;

    [SerializeField] private int numberOfHitsForBaby;


    [SerializeField] private TeacherController teacherController;
    [SerializeField] private BabyController babyController;

    //private void Start()
    //{
    //    StoryModeGameManager.Instance.TogglePlayerMovement(false);
    //    Invoke(nameof(ShowObjectivePanel), (float)CutScene.duration);
    //    //    //teacherController.AnimateTeacher(GameConstants.InGameConstants.TeacherAnimationNames.SittingIdle);
    //        Invoke(nameof(AnimateTeacher), 2f);
    //}

    public void AnimateTeacher()
    {
        //teacherController.AnimateTeacher(GameConstants.InGameConstants.TeacherAnimationNames.SittingIdle);
    }

    public override void DecideInteractability()
    {
        if(currentSelectedProp.Equals("BabyCot"))
        {
            objectInteraction.gameObject.SetActive(true);
        }
    }

    public override void OnInteractableButtonClicked()
    {
        hitCount++;
        if(hitCount==1)
            AudioManager.Instance.PlaySpecialAudios(GameConstants.InGameConstants.SpecialAudioClips.BabyCrying);
        //babyController.PlayAnimation(GameConstants.InGameConstants.BabyAnimation.Crying);
        if (hitCount == numberOfHitsForBaby)
        {
            objectInteraction.gameObject.SetActive(false);
            Invoke(nameof(AddSpecialSound), 2f);
            LevelCompletionSequence();
        }
    }

    public void AddSpecialSound()
    {
        AudioManager.Instance.PlaySpecialAudios(GameConstants.InGameConstants.SpecialAudioClips.TeacherCrying);
    }    
}
