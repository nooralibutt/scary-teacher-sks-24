using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class LevelPropsManager : MonoBehaviour
{
    public static LevelPropsManager _Instance;

    private void Awake()
    {
        _Instance = this;
    }

    public List<LevelWiseProps> LevelProps;

    public List<PropsDependency> PropsDependencies;

    public List<string> levelWiseObjective;


    public bool ValidatePropsAccordingToLevel(string tagOfObject, int levelNumber)
    {
        bool isValidated = false;
        for (int i=0;i<LevelProps.Count;i++)
        {
            if (StoryModeLevelManager.Instance.CurrentLevelNumber == LevelProps[i].levelNumber)
            {
                for(int j = 0; j < LevelProps[i].propsIncludedInLevel.Length;j++)
                {
                    if (LevelProps[i].propsIncludedInLevel[j].ToString().Equals(tagOfObject))
                    {
                        isValidated = true;
                        return isValidated;
                    }
                }
            }
        }
        return isValidated;
    }
}

[System.Serializable]
public class LevelWiseProps
{
    public GameConstants.InGameConstants.LevelProps[] propsIncludedInLevel;
    public int levelNumber;
}

[System.Serializable]
public class PropsDependency
{
    public GameConstants.InGameConstants.LevelProps collectedProp;
    public GameConstants.InGameConstants.LevelProps[] propsRequiredToEnableProp;
    public int levelNumber;
}


