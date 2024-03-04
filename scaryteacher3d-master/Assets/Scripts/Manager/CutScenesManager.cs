using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CutScenesManager : MonoBehaviour
{

    public Action<int> enableLevel;


    public CameraSetupData introCutSceneCameraData;

    public List<CameraSetupData> CameraSceneSequence;




    public void ExecuteStoryLevelSequence(int levelNumber)
    {
        enableLevel?.Invoke(levelNumber);
    }

}
