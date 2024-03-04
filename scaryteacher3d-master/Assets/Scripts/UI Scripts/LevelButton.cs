using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI; 

public class LevelButton : MonoBehaviour
{
    [SerializeField] private Sprite lockedState;
    [SerializeField] private Sprite selectedState;
    [SerializeField] private Sprite normalState;

    private bool isLocked;
    private int level;
    private bool levelSelected = false;

    public System.Action onButtonClicked;

    private void Start()
    {
        transform.localScale = Vector3.zero;
    }

    public void DisplayLevelCell(int levelNumber, bool isLocked, float delay)
    {
        LeanTween.scale(gameObject, Vector3.one, 0.5f).setDelay(delay).setEaseOutBack();
        this.isLocked = isLocked;
        level = levelNumber;
        if (isLocked)
        {
            GetComponent<Image>().sprite = lockedState;
        }
        GetComponentInChildren<Text>().text = (levelNumber + 1).ToString();
    }

    public void UpdatState(bool selectedStatebool)
    {
        if (selectedStatebool)
            GetComponent<Image>().sprite = selectedState;
        else
        {
            if (!isLocked)
            {
                GetComponent<Image>().sprite = normalState;
            }
        }
    }

    public void OnButtonClicked()
    {
        if (!this.isLocked)
        {
            levelSelected = true;
            //GetComponent<Image>().sprite = selectedState;
            GameConstants.InGameConstants.selectedLevel = level;
            onButtonClicked?.Invoke();
        }
    }

    
}
