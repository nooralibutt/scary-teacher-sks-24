using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level8 : BasicLevelStructure
{

    [SerializeField] private GameObject teacherMainDoor;
    [SerializeField] private GameObject teacherOpenDoor;

    public override void OnInteractableButtonClicked()
    {
        if (currentSelectedProp != GameConstants.InGameConstants.LevelProps.None.ToString())
        {
            prevSelectedProp = currentSelectedProp;
            currentSelectedProp = GameConstants.InGameConstants.LevelProps.None.ToString();
            objectInteraction.gameObject.SetActive(false);
        }

        if (prevSelectedProp == GameConstants.InGameConstants.LevelProps.Key.ToString())
        {
            if (!propsInventory.Contains(GameConstants.InGameConstants.LevelProps.Key.ToString()))
            {
                propsInventory.Add(GameConstants.InGameConstants.LevelProps.Key.ToString());
                schoolBoyController.DisplayLevelProp(PropToBeInstantiated(GameConstants.InGameConstants.LevelProps.Key));
            }
            if (GameObject.FindGameObjectWithTag(prevSelectedProp) != null && GameObject.FindGameObjectWithTag(prevSelectedProp).GetComponent<Collider>() != null)
                Destroy(GameObject.FindGameObjectWithTag(prevSelectedProp).gameObject);
        }


        if (prevSelectedProp == GameConstants.InGameConstants.LevelProps.TeacherRoomDoor.ToString())
        {
            if (propsInventory.Contains(GameConstants.InGameConstants.LevelProps.Key.ToString()))
            {
                schoolBoyController.RemoveLevelProps();
                teacherMainDoor.SetActive(false);
                teacherOpenDoor.SetActive(true);
                teacherMainDoor.GetComponent<Collider>().enabled = false;
                teacherMainDoor.GetComponentInChildren<Collider>().enabled = false;
                teacherMainDoor.GetComponentInChildren<ParticleSystem>().Stop();
            }
        }

        if (prevSelectedProp == GameConstants.InGameConstants.LevelProps.Phone.ToString())
        {
            Invoke(nameof(AddSpecialSound), 2.5f);
            LevelCompletionSequence();
        }
    }

    public void AddSpecialSound()
    {
        AudioManager.Instance.PlaySpecialAudios(GameConstants.InGameConstants.SpecialAudioClips.TeacherAngry);
    }
}
