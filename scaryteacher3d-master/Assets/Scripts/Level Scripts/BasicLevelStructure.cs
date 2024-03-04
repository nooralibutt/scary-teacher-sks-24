using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.Timeline;
using SKS.Ads;

public class BasicLevelStructure : MonoBehaviour
{

    [SerializeField] protected List<PropsShownDuringGameplay> propsShownDuringGameplays;

    [SerializeField] protected GameObject objectInteraction;

    [SerializeField] protected TimelineAsset CutScene;

    [SerializeField] protected GameObject teacherAnimation;

    [SerializeField] protected TimelineAsset levelEndCutScene;

    [SerializeField] protected GameObject levelEndCutScenTimeline;

    [SerializeField] protected List<string> propsInventory = new List<string>();

    [SerializeField] protected GameObject cutSceneCamera;

    [SerializeField] protected GameObject playerCamera;

    [SerializeField] public GameObject FinalFailAnimation;

    [SerializeField] protected GameObject levelCanvas;

    [SerializeField] protected SchoolBoyController schoolBoyController;

    protected string currentSelectedProp = GameConstants.InGameConstants.LevelProps.None.ToString();

    protected string prevSelectedProp = GameConstants.InGameConstants.LevelProps.None.ToString();

    private void Start()
    {
        StoryModeLevelManager.Instance.onLevelFailTriggered = false;
        StoryModeLevelManager.Instance.onLevelCompletionTriggered = false;
        StoryModeGameManager.Instance.TogglePlayerMovement(false);
        Invoke(nameof(ShowObjectivePanel), (float)CutScene.duration);
    }

    public void TurnOffTeacherAnimationTimeline()
    {
        if (teacherAnimation != null)
            teacherAnimation.SetActive(false);
        StoryModeGameManager.Instance.isLevelBeingPlayed = false;
    }

    public virtual void DecideInteractability()
    {
        //objectInteraction.gameObject.SetActive(toggle);

        bool conditionsFulfilled = true;
        for (int i = 0; i < LevelPropsManager._Instance.PropsDependencies.Count; i++)
        {
            if (i == LevelPropsManager._Instance.PropsDependencies[i].levelNumber)
            {
                if (LevelPropsManager._Instance.PropsDependencies[i].collectedProp.ToString().Equals(currentSelectedProp))
                {
                    for (int j = 0; j < LevelPropsManager._Instance.PropsDependencies[i].propsRequiredToEnableProp.Length; j++)
                    {
                        if (!propsInventory.Contains(LevelPropsManager._Instance.PropsDependencies[i].propsRequiredToEnableProp[j].ToString()))
                        {
                            conditionsFulfilled = false;
                        }
                    }
                }
            }
        }
        if (conditionsFulfilled)
        {
            objectInteraction.gameObject.SetActive(true);
        }
    }

    protected void AllowPlayerMovement()
    {
        StoryModeGameManager.Instance.TogglePlayerMovement(true);
    }

    protected void ShowObjectivePanel()
    {
        
        StoryModeGameManager.Instance.InstantiateObjectivePanel();
    }

    public void ToggleCutSceneCamera(bool toggle)
    {
        cutSceneCamera.SetActive(toggle);
    }


    public virtual void SetCurrentSelectedProp(string tag)
    {
        currentSelectedProp = tag;
    }

    public virtual void OnInteractableButtonClicked()
    {
        if(currentSelectedProp != GameConstants.InGameConstants.LevelProps.None.ToString())
        {
            if (!propsInventory.Contains(currentSelectedProp))
                propsInventory.Add(currentSelectedProp);
            prevSelectedProp = currentSelectedProp;
            if (GameObject.FindGameObjectWithTag(currentSelectedProp) != null && GameObject.FindGameObjectWithTag(currentSelectedProp).GetComponent<Collider>() != null)
                Destroy(GameObject.FindGameObjectWithTag(currentSelectedProp).gameObject);
                //GameObject.FindGameObjectWithTag(currentSelectedProp).GetComponent<Collider>().enabled = false;
            currentSelectedProp = GameConstants.InGameConstants.LevelProps.None.ToString();
            objectInteraction.gameObject.SetActive(false);
        }
    }

    public void ToggleObjectInteraction(bool toggle)
    {
        objectInteraction.SetActive(toggle);
    }

    public void TogglePlayerCamera(bool toggle)
    {
        playerCamera.SetActive(toggle);
    }

    public void ToggleTeacherAnimation(bool toggle)
    {
        if (teacherAnimation != null)
            teacherAnimation.SetActive(toggle);
    }

    public void ToggleFinalFailAnimation()
    {
        FinalFailAnimation.SetActive(true);
    }

    public void EndLevel()
    {
        AdsManager.ShowInterstitial();
        StoryModeLevelManager.Instance.onLevelEnded?.Invoke(true);
    }


    public void LevelCompletionSequence()
    {
        TogglePauseButton();
        if (StoryModeLevelManager.Instance.onLevelFailTriggered)
            return;
        else
            StoryModeLevelManager.Instance.onLevelCompletionTriggered = true;
        UIManager.Instance.ToggleJoyStick(false);
        StoryModeGameManager.Instance.TogglePlayerMovement(false);
        StoryModeGameManager.Instance.currentLevel.ToggleCutSceneCamera(true);
        TurnOffTeacherAnimationTimeline();
        levelEndCutScenTimeline.SetActive(true);
        objectInteraction.SetActive(false);
        Invoke(nameof(EndLevel), (float)levelEndCutScene.duration + 1f);
    }

    public Transform GamePlayCanvas()
    {
        return levelCanvas.transform;
    }


    public GameObject PropToBeInstantiated(GameConstants.InGameConstants.LevelProps levelProp)
    {
        for (int i = 0; i < propsShownDuringGameplays.Count; i++)
        {
            if (levelProp == propsShownDuringGameplays[i].propName)
                return propsShownDuringGameplays[i].propsToBeInstantiated;
        }
        return null;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.W))
            LevelCompletionSequence();
    }

    public void OnPauseButtonPressed()
    {
        AdsManager.ShowInterstitial();
        UIManager.Instance.DisplayGamePausePanel();
    }

    public void TogglePauseButton()
    {
        GameObject.FindGameObjectWithTag("PauseButton").gameObject.SetActive(false);
    }
}

[System.Serializable]
public class PropsShownDuringGameplay
{
    public GameConstants.InGameConstants.LevelProps propName;
    public GameObject propsToBeInstantiated;
}
