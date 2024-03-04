using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


[System.Serializable]
public struct CameraSetupData
{
    public Vector3 initialPosition;
    //public Quaternion initialEulerAngles;
    public float initialFOV;
    [Space]
    public Vector3 targetPosition;
    public Vector3 targetEulerAngles;
    public float targetFOV;
    [Space]
    public float duration;
    [Space]
    public UnityEvent onPositionCompleteEvent;
}
