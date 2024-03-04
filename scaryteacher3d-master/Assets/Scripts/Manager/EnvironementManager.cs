using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnvironementManager : MonoBehaviour
{
    public Transform mainLeftGate;
    public Transform mainRightGate;
    public Transform mainHouseDoor;



    public void OpenHouseMainDoor(Action completionCallback = null)
    {
        LeanTween.rotateY(mainHouseDoor.gameObject, 90, 0.5f).setOnComplete(() =>
        {
            completionCallback?.Invoke();
        });
    }
    public void OpenBuildingMainGate(Action completionCallback)
    {
        LeanTween.rotateY(mainRightGate.gameObject, -90, 0.5f);
        LeanTween.rotateY(mainLeftGate.gameObject, -90, 0.5f).setOnComplete(() =>
        {
            completionCallback?.Invoke();
        });
    }
}
