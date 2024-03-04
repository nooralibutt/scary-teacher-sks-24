using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UIPopup : MonoBehaviour
{
    [SerializeField] private GameObject popupContainer;
    public void ShowPopup(Action completionCallback = null)
    {
        popupContainer.transform.localScale = Vector3.zero;
        LeanTween.scale(popupContainer, Vector3.one, 0.5f).setEaseOutBack().setOnComplete(completionCallback);
    }

    public void HidePopup(Action completionCallback = null)
    {
        LeanTween.scale(popupContainer, Vector3.zero, 0.5f).setEaseInBack().setOnComplete(completionCallback);
    }
}
