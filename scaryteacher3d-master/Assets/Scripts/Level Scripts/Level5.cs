using System.Collections;
using System.Collections.Generic;
//using SKS.Ads;
using UnityEngine;

public class Level5 : BasicLevelStructure
{
    [SerializeField] private GameObject crackersInBoundary;


    //private void Start()
    //{
    //    StoryModeGameManager.Instance.TogglePlayerMovement(false);
    //    Invoke(nameof(ShowObjectivePanel), (float)CutScene.duration);
    //}

    public override void OnInteractableButtonClicked()
    {

        if (currentSelectedProp != GameConstants.InGameConstants.LevelProps.None.ToString())
        {
            prevSelectedProp = currentSelectedProp;
            currentSelectedProp = GameConstants.InGameConstants.LevelProps.None.ToString();
            objectInteraction.gameObject.SetActive(false);
        }

        if (prevSelectedProp == GameConstants.InGameConstants.LevelProps.CrackerBoundary.ToString())
        {
            if (propsInventory.Contains(GameConstants.InGameConstants.LevelProps.FireCrackers.ToString()))
            {

                if (propsInventory.Contains(GameConstants.InGameConstants.LevelProps.CrackerBoundary.ToString()) && propsInventory.Contains(GameConstants.InGameConstants.LevelProps.Lighter.ToString()))
                {
                    AudioManager.Instance.PlaySpecialAudios(GameConstants.InGameConstants.SpecialAudioClips.TeacherCrying);
                    LevelCompletionSequence();
                }

                else if (!propsInventory.Contains(GameConstants.InGameConstants.LevelProps.CrackerBoundary.ToString()))
                {
                    crackersInBoundary.SetActive(true);
                    schoolBoyController.RemoveLevelProps();
                    propsInventory.Add(GameConstants.InGameConstants.LevelProps.CrackerBoundary.ToString());
                }
            }
        }

        if (prevSelectedProp == GameConstants.InGameConstants.LevelProps.FireCrackers.ToString())
        {
            if (!propsInventory.Contains(GameConstants.InGameConstants.LevelProps.FireCrackers.ToString()))
            {
                propsInventory.Add(GameConstants.InGameConstants.LevelProps.FireCrackers.ToString());
                schoolBoyController.DisplayLevelProp(PropToBeInstantiated(GameConstants.InGameConstants.LevelProps.FireCrackers));
                if (GameObject.FindGameObjectWithTag(prevSelectedProp) != null && GameObject.FindGameObjectWithTag(prevSelectedProp).GetComponent<Collider>() != null)
                    Destroy(GameObject.FindGameObjectWithTag(prevSelectedProp).gameObject);
            }
        }

        if (prevSelectedProp == GameConstants.InGameConstants.LevelProps.Lighter.ToString())
        {
           if(propsInventory.Contains(GameConstants.InGameConstants.LevelProps.FireCrackers.ToString()) && propsInventory.Contains(GameConstants.InGameConstants.LevelProps.CrackerBoundary.ToString()))
            {
                if (!propsInventory.Contains(GameConstants.InGameConstants.LevelProps.Lighter.ToString()))
                {
                    propsInventory.Add(GameConstants.InGameConstants.LevelProps.Lighter.ToString());
                    schoolBoyController.DisplayLevelProp(PropToBeInstantiated(GameConstants.InGameConstants.LevelProps.Lighter));
                }
                if (GameObject.FindGameObjectWithTag(prevSelectedProp) != null && GameObject.FindGameObjectWithTag(prevSelectedProp).GetComponent<Collider>() != null)
                    Destroy(GameObject.FindGameObjectWithTag(prevSelectedProp).gameObject);
            }
        }
    }
}
