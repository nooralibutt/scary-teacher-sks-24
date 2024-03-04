using System.Collections;
using System.Collections.Generic;
using SKS.Ads;
using UnityEngine;
using UnityEngine.ParticleSystemJobs;

public class Level4 : BasicLevelStructure
{
    [SerializeField] private GameObject openedDoor;

    [SerializeField] private GameObject closedDoor;

    [SerializeField] private GameObject Bag;

    [SerializeField] private ParticleSystem glow;

    

    //private void Start()
    //{
    //    StoryModeGameManager.Instance.TogglePlayerMovement(false);
    //    Invoke(nameof(ShowObjectivePanel), (float)CutScene.duration);
    //}


    public override void OnInteractableButtonClicked()
    {
        //base.OnInteractableButtonClicked();
        if (currentSelectedProp != GameConstants.InGameConstants.LevelProps.None.ToString())
        {
            prevSelectedProp = currentSelectedProp;
            currentSelectedProp = GameConstants.InGameConstants.LevelProps.None.ToString();
            objectInteraction.gameObject.SetActive(false);
        }
        if (prevSelectedProp == GameConstants.InGameConstants.LevelProps.Cupboard.ToString())
        {
            closedDoor.SetActive(false);
            openedDoor.SetActive(true);
            closedDoor.GetComponentInParent<Collider>().enabled = false;
            Bag.GetComponent<Collider>().enabled = true;
        }

        if (prevSelectedProp == GameConstants.InGameConstants.LevelProps.Purse.ToString())
        {
            glow.gameObject.SetActive(false);
            Bag.gameObject.SetActive(false);
            propsInventory.Add(GameConstants.InGameConstants.LevelProps.Purse.ToString());
            //LevelCompletionSequence();
            schoolBoyController.DisplayLevelProp(PropToBeInstantiated(GameConstants.InGameConstants.LevelProps.Purse));
        }


        if (prevSelectedProp == GameConstants.InGameConstants.LevelProps.PurseBoundary.ToString())
        {
            if (propsInventory.Contains(GameConstants.InGameConstants.LevelProps.Purse.ToString()))
            {
                Invoke(nameof(AddSpecialSound), 2.5f);
                LevelCompletionSequence();
            }
        }
    }

    public void AddSpecialSound()
    {
        AudioManager.Instance.PlaySpecialAudios(GameConstants.InGameConstants.SpecialAudioClips.TeacherAngry);
    }
}
