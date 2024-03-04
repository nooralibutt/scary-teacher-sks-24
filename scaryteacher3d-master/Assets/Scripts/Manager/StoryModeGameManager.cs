using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using SKS.Ads;

public class StoryModeGameManager : MonoBehaviour
{

    private TeacherController teacherController;
    private SchoolBoyController schoolBoyController;

    public static StoryModeGameManager Instance;

    [HideInInspector] public bool isLevelBeingPlayed = false;


    [SerializeField] private List<BasicLevelStructure> StoryModeLevels;
    [SerializeField] private LevelCompletionPanel LevelCompletionPanel;
    [SerializeField] private LevelFailedPopup LevelFailedPanel;
    [SerializeField] private LevelObjectivePanel LevelObjectivePanel;


    [HideInInspector] public BasicLevelStructure currentLevel;

    private bool isAllowedToMove;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayLevelMusic();
        //AdsManager.ShowBanner(false);
        isAllowedToMove = true;
        teacherController = GameObject.FindObjectOfType<TeacherController>();
        schoolBoyController = GameObject.FindObjectOfType<SchoolBoyController>();
        StoryModeLevelManager.Instance.onLevelEnded += ShowLevelCompletionScreenAccordingly;
        StoryModeLevelManager.Instance.onLevelCompleted += DecideAndLoadLevel;
        LoadSelectedLevel();
        //DecideAndLoadLevel(true);
    }

    private void OnDestroy()
    {
        StoryModeLevelManager.Instance.onLevelEnded -= ShowLevelCompletionScreenAccordingly;
        StoryModeLevelManager.Instance.onLevelCompleted -= DecideAndLoadLevel;
    }

    public void LoadSelectedLevel()
    {
        if (GameConstants.InGameConstants.selectedLevel == -1)
        {
            DecideAndLoadLevel(true);
            return;
        }
        if (currentLevel != null)
            Destroy(currentLevel.gameObject);
        currentLevel = Instantiate(StoryModeLevels[GameConstants.InGameConstants.selectedLevel]);
        StoryModeLevelManager.Instance.CurrentLevelNumber = GameConstants.InGameConstants.selectedLevel;
    }

    private void DecideAndLoadLevel(bool playNextLevel)
    {
        

        if (StoryModeLevelManager.Instance.CurrentLevelNumber == StoryModeLevels.Count - 1 && PlayerPrefs.HasKey(GameConstants.InGameConstants.LEVEL_IDENTIFIER + StoryModeLevelManager.Instance.CurrentLevelNumber))
            ResetLevelsData();

        if (GameConstants.InGameConstants.selectedLevel == StoryModeLevels.Count - 1)
            GameConstants.InGameConstants.selectedLevel = -1;

        if (GameConstants.InGameConstants.selectedLevel == -1 && !playNextLevel)
            GameConstants.InGameConstants.selectedLevel = StoryModeLevels.Count - 1;

        //if (GameConstants.InGameConstants.selectedLevel != -1)
        //{

        if (playNextLevel)
            GameConstants.InGameConstants.selectedLevel++;

        if (currentLevel != null)
                Destroy(currentLevel.gameObject);
            currentLevel = Instantiate(StoryModeLevels[GameConstants.InGameConstants.selectedLevel]);
            StoryModeLevelManager.Instance.CurrentLevelNumber = GameConstants.InGameConstants.selectedLevel;
            return;
        //}


        //for (int i = 0; i < StoryModeLevels.Count; i++)
        //{
        //    if (!PlayerPrefs.HasKey(GameConstants.InGameConstants.LEVEL_IDENTIFIER + i))
        //    {
        //        if (currentLevel != null)
        //            Destroy(currentLevel.gameObject);
        //        currentLevel = Instantiate(StoryModeLevels[i]);
        //        StoryModeLevelManager.Instance.CurrentLevelNumber = i;
        //        return;
        //    }
        //}
    }

    public void ResetLevelsData()
    {
        PlayerPrefs.SetInt(GameConstants.InGameConstants.ALL_LEVELS_UNLOCKED, 1);
        for (int i = 0; i < StoryModeLevels.Count; i++)
        {
            if (PlayerPrefs.HasKey(GameConstants.InGameConstants.LEVEL_IDENTIFIER + i))
            {
                //PlayerPrefs.DeleteKey(GameConstants.InGameConstants.LEVEL_IDENTIFIER + i);
            }
        }
    }

    public void TogglePlayerMovement(bool toggle)
    {
        isAllowedToMove = toggle;
        currentLevel.TogglePlayerCamera(toggle);
    }

    public bool IsPlayerAllowedToMove()
    {
        return isAllowedToMove;
    }

    public SchoolBoyController GetSchoolBoyController()
    {
        return schoolBoyController;
    }

    public TeacherController GetTeacherController()
    {
        return teacherController;
    }

    //public void PlayNextGameLevel()
    //{
    //    DecideAndLoadLevel();
    //}

    public void ShowLevelCompletionScreenAccordingly(bool levelResult)
    {
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayBackgroundMusic();
        if (levelResult)
        {
            UIManager.Instance.InstantiatePopupInGamePlay(LevelCompletionPanel.gameObject);
        }
        else
        {
            StoryModeGameManager.Instance.TogglePlayerMovement(false);
            StoryModeGameManager.Instance.currentLevel.ToggleCutSceneCamera(true);
            StoryModeGameManager.Instance.currentLevel.ToggleFinalFailAnimation();
            Invoke(nameof(InstantiateLevelFailedPanel), 4f);
        }
    }

    public void InstantiateLevelFailedPanel()
    {
        UIManager.Instance.InstantiatePopupInGamePlay(LevelFailedPanel.gameObject);
    }

    public void InstantiateObjectivePanel()
    {
        GameObject popup =  UIManager.Instance.InstantiatePopupInGamePlay(LevelObjectivePanel.gameObject);
        popup.GetComponent<LevelObjectivePanel>().ShowPopup(() =>
        {
            popup.GetComponent<LevelObjectivePanel>().PopulateObjective(LevelPropsManager._Instance.levelWiseObjective[StoryModeLevelManager.Instance.CurrentLevelNumber]);
        });

    }

}
