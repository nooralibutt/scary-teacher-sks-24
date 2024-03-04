using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level9 : BasicLevelStructure
{
    public override void OnInteractableButtonClicked()
    {
        if (currentSelectedProp != GameConstants.InGameConstants.LevelProps.None.ToString())
        {
            prevSelectedProp = currentSelectedProp;
            currentSelectedProp = GameConstants.InGameConstants.LevelProps.None.ToString();
            objectInteraction.gameObject.SetActive(false);
        }

        if (prevSelectedProp == GameConstants.InGameConstants.LevelProps.Needle.ToString())
        {
            if (!propsInventory.Contains(GameConstants.InGameConstants.LevelProps.Needle.ToString()))
            {
                schoolBoyController.DisplayLevelProp(PropToBeInstantiated(GameConstants.InGameConstants.LevelProps.Needle));
                propsInventory.Add(GameConstants.InGameConstants.LevelProps.Key.ToString());
                if (GameObject.FindGameObjectWithTag(prevSelectedProp) != null && GameObject.FindGameObjectWithTag(prevSelectedProp).GetComponent<Collider>() != null)
                    Destroy(GameObject.FindGameObjectWithTag(prevSelectedProp).gameObject);
            }
        }

        if (prevSelectedProp == GameConstants.InGameConstants.LevelProps.Tyre.ToString())
        {
            AudioManager.Instance.PlaySpecialAudios(GameConstants.InGameConstants.SpecialAudioClips.TeacherCrying);
            LevelCompletionSequence();
        }
    }
}
