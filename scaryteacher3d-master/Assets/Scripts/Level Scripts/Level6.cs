using System.Collections;
using System.Collections.Generic;
using SKS.Ads;
using UnityEngine;

public class Level6 : BasicLevelStructure
{

    [SerializeField] private GameObject testPaper;

    public override void OnInteractableButtonClicked()
    {

        if (currentSelectedProp != GameConstants.InGameConstants.LevelProps.None.ToString())
        {
            prevSelectedProp = currentSelectedProp;
            currentSelectedProp = GameConstants.InGameConstants.LevelProps.None.ToString();
            objectInteraction.gameObject.SetActive(false);
        }

        if (prevSelectedProp == GameConstants.InGameConstants.LevelProps.TestPaperDestination.ToString())
        {
            if (propsInventory.Contains(GameConstants.InGameConstants.LevelProps.TestPaper.ToString()))
            {

                if (propsInventory.Contains(GameConstants.InGameConstants.LevelProps.TestPaperDestination.ToString()) && propsInventory.Contains(GameConstants.InGameConstants.LevelProps.Lighter.ToString()))
                {
                    Invoke(nameof(AddSpecialSound), 2.5f);
                    LevelCompletionSequence();
                }

                else if (!propsInventory.Contains(GameConstants.InGameConstants.LevelProps.TestPaperDestination.ToString()))
                {
                    testPaper.SetActive(true);
                    schoolBoyController.RemoveLevelProps();
                    propsInventory.Add(GameConstants.InGameConstants.LevelProps.TestPaperDestination.ToString());
                }
            }
        }

        if (prevSelectedProp == GameConstants.InGameConstants.LevelProps.TestPaper.ToString())
        {
            if (!propsInventory.Contains(GameConstants.InGameConstants.LevelProps.TestPaper.ToString()))
            {
                schoolBoyController.DisplayLevelProp(PropToBeInstantiated(GameConstants.InGameConstants.LevelProps.TestPaper));
                propsInventory.Add(GameConstants.InGameConstants.LevelProps.TestPaper.ToString());
                if (GameObject.FindGameObjectWithTag(prevSelectedProp) != null && GameObject.FindGameObjectWithTag(prevSelectedProp).GetComponent<Collider>() != null)
                    Destroy(GameObject.FindGameObjectWithTag(prevSelectedProp).gameObject);
            }
        }

        if (prevSelectedProp == GameConstants.InGameConstants.LevelProps.Lighter.ToString())
        {
            if (propsInventory.Contains(GameConstants.InGameConstants.LevelProps.TestPaper.ToString()) && propsInventory.Contains(GameConstants.InGameConstants.LevelProps.TestPaperDestination.ToString()))
            {
                if (!propsInventory.Contains(GameConstants.InGameConstants.LevelProps.Lighter.ToString()))
                {
                    schoolBoyController.DisplayLevelProp(PropToBeInstantiated(GameConstants.InGameConstants.LevelProps.Lighter));
                    propsInventory.Add(GameConstants.InGameConstants.LevelProps.Lighter.ToString());
                }
                if (GameObject.FindGameObjectWithTag(prevSelectedProp) != null && GameObject.FindGameObjectWithTag(prevSelectedProp).GetComponent<Collider>() != null)
                    Destroy(GameObject.FindGameObjectWithTag(prevSelectedProp).gameObject);
            }
        }
    }
    public void AddSpecialSound()
    {
        AudioManager.Instance.PlaySpecialAudios(GameConstants.InGameConstants.SpecialAudioClips.TeacherAngry);
    }
}
