using System.Collections;
using System.Collections.Generic;
//using SKS.Ads;
using UnityEngine;

public class Level7 : BasicLevelStructure
{

    //private void Start()
    //{
    //    StoryModeGameManager.Instance.TogglePlayerMovement(false);
    //    Invoke(nameof(ShowObjectivePanel), (float)CutScene.duration);
    //}

    [SerializeField] private GameObject particleEffects;

    public override void OnInteractableButtonClicked()
    {
        if (currentSelectedProp != GameConstants.InGameConstants.LevelProps.None.ToString())
        {
            prevSelectedProp = currentSelectedProp;
            currentSelectedProp = GameConstants.InGameConstants.LevelProps.None.ToString();
            objectInteraction.gameObject.SetActive(false);
        }


        if (prevSelectedProp == GameConstants.InGameConstants.LevelProps.OilBottleBoundary.ToString())
        {
            if (propsInventory.Contains(GameConstants.InGameConstants.LevelProps.OilBottle.ToString()))
            {
                particleEffects.SetActive(false);
                Invoke(nameof(AddSpecialSound), 2.8f);
                LevelCompletionSequence();
            }
        }


        if (prevSelectedProp == GameConstants.InGameConstants.LevelProps.OilBottle.ToString())
        {
            if (!propsInventory.Contains(GameConstants.InGameConstants.LevelProps.OilBottle.ToString()))
            {
                schoolBoyController.DisplayLevelProp(PropToBeInstantiated(GameConstants.InGameConstants.LevelProps.OilBottle));
                propsInventory.Add(GameConstants.InGameConstants.LevelProps.OilBottle.ToString());
            }
            if (GameObject.FindGameObjectWithTag(prevSelectedProp) != null && GameObject.FindGameObjectWithTag(prevSelectedProp).GetComponent<Collider>() != null)
                Destroy(GameObject.FindGameObjectWithTag(prevSelectedProp).gameObject);
        }
    }

    public void AddSpecialSound()
    {
        AudioManager.Instance.PlaySpecialAudios(GameConstants.InGameConstants.SpecialAudioClips.TeacherAngry);
    }
}
