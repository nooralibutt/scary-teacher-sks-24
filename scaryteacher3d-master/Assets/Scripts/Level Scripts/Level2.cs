using System.Collections;
using System.Collections.Generic;
using SKS.Ads;
using UnityEngine;

public class Level2 : BasicLevelStructure
{
    [SerializeField] private ParticleSystem billFire;


    //private void Start()
    //{
    //    StoryModeGameManager.Instance.TogglePlayerMovement(false);
    //    Invoke(nameof(ShowObjectivePanel), (float)CutScene.duration);
    //}

    public override void OnInteractableButtonClicked()
    {
        base.OnInteractableButtonClicked();
        if (prevSelectedProp == GameConstants.InGameConstants.LevelProps.ElectricityBill.ToString())
        {
            billFire.Play();
            AudioManager.Instance.PlaySpecialAudios(GameConstants.InGameConstants.SpecialAudioClips.TeacherAngry);
            LevelCompletionSequence();
        }

        if (prevSelectedProp == GameConstants.InGameConstants.LevelProps.Matchbox.ToString())
        {
            schoolBoyController.DisplayLevelProp(PropToBeInstantiated(GameConstants.InGameConstants.LevelProps.Matchbox));
        }

    }


    public override void DecideInteractability()
    {
        base.DecideInteractability();
    }

    public override void SetCurrentSelectedProp(string tag)
    {
        base.SetCurrentSelectedProp(tag);
    }
}
