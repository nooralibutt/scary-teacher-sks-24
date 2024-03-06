using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
//using SKS.Ads;

public class Level1 : BasicLevelStructure
{
    [SerializeField] private GameObject lockInGame;

    //private void Start()
    //{
    //    StoryModeGameManager.Instance.TogglePlayerMovement(false);
    //    LeanTween.moveY(keys, keys.transform.position.y + 0.5f, 2f).setLoopPingPong();
    //    Invoke(nameof(ShowObjectivePanel), (float)CutScene.duration);
    //}

    public override void SetCurrentSelectedProp(string tag)
    {
        base.SetCurrentSelectedProp(tag);
    }

    public override void OnInteractableButtonClicked()
    {
        base.OnInteractableButtonClicked();
        if (prevSelectedProp == GameConstants.InGameConstants.LevelProps.HouseMainDoor.ToString())
        {
            lockInGame.SetActive(false);
            AudioManager.Instance.PlaySpecialAudios(GameConstants.InGameConstants.SpecialAudioClips.TeacherAngry);
            LevelCompletionSequence();
        }

        if (prevSelectedProp == GameConstants.InGameConstants.LevelProps.Key.ToString())
        {
            schoolBoyController.DisplayLevelProp(PropToBeInstantiated(GameConstants.InGameConstants.LevelProps.Key));
        }
    }

    public override void DecideInteractability()
    {
        base.DecideInteractability();
    }
}
