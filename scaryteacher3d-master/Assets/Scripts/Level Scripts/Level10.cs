using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level10 : BasicLevelStructure
{
    [SerializeField] private GameObject waterPrefab;
    [SerializeField] private GameObject waterTap;
    [SerializeField] private GameObject completePipe;

    public override void OnInteractableButtonClicked()
    {
        if (currentSelectedProp != GameConstants.InGameConstants.LevelProps.None.ToString())
        {
            prevSelectedProp = currentSelectedProp;
            currentSelectedProp = GameConstants.InGameConstants.LevelProps.None.ToString();
            objectInteraction.gameObject.SetActive(false);
        }

        if (prevSelectedProp == GameConstants.InGameConstants.LevelProps.WaterTap.ToString())
        {
            completePipe.SetActive(false);
            waterPrefab.SetActive(true);
            AudioManager.Instance.PlaySpecialAudios(GameConstants.InGameConstants.SpecialAudioClips.TeacherCrying);
            LevelCompletionSequence();
        }
        if (prevSelectedProp == GameConstants.InGameConstants.LevelProps.Pipe.ToString())
        {
            if (!propsInventory.Contains(GameConstants.InGameConstants.LevelProps.Pipe.ToString()))
            {
                schoolBoyController.DisplayLevelProp(PropToBeInstantiated(GameConstants.InGameConstants.LevelProps.Pipe));
                propsInventory.Add(GameConstants.InGameConstants.LevelProps.Pipe.ToString());
                if (GameObject.FindGameObjectWithTag(prevSelectedProp) != null && GameObject.FindGameObjectWithTag(prevSelectedProp).GetComponent<Collider>() != null)
                    Destroy(GameObject.FindGameObjectWithTag(prevSelectedProp).gameObject);
            }
        }
        if (prevSelectedProp == GameConstants.InGameConstants.LevelProps.PipeEndBoundary.ToString())
        {
            schoolBoyController.RemoveLevelProps();
            propsInventory.Add(GameConstants.InGameConstants.LevelProps.PipeEndBoundary.ToString());
            if (GameObject.FindGameObjectWithTag(prevSelectedProp) != null && GameObject.FindGameObjectWithTag(prevSelectedProp).GetComponent<Collider>() != null)
                Destroy(GameObject.FindGameObjectWithTag(prevSelectedProp).gameObject);
            waterTap.GetComponent<Collider>().enabled = true;
            completePipe.SetActive(true);
        }
    }

}
