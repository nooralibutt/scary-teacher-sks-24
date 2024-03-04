using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;

public class StoryModeLevelManager : MonoBehaviour
{
    [HideInInspector] public int CurrentLevelNumber = 0;

    public Action onLevelStarted;
    public Action <bool> onLevelCompleted;
    public Action<bool> onLevelEnded;
    public bool onLevelFailTriggered = false;
    public bool onLevelCompletionTriggered = false;

    public static StoryModeLevelManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

}
