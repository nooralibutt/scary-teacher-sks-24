using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance;
    private Vector3 defaultCameraPosition;
    private Quaternion defaultCameraRotation;

    private float defaultCameraFOV;
    private CameraSetupData currentCameraSetupData;

    private Camera mainCamera;

    private void Awake()
    {
        Instance = this;
        mainCamera = GetComponent<Camera>();
        defaultCameraFOV = mainCamera.fieldOfView;
        defaultCameraPosition = gameObject.transform.position;
        defaultCameraRotation = gameObject.transform.rotation;
    }
    private void MoveCameraToPosition(Vector3 position, float duration)
    {
        Hashtable hashtable = new Hashtable
        {
            {"position", position},
            {"time", duration},
            {"easetype", iTween.EaseType.easeInOutQuad},
            {"oncomplete", nameof(OnFinalPosition)}
        };

        iTween.MoveTo(gameObject, hashtable);
    }

    private void MoveCameraRotation(Vector3 eulerAngles, float duration)
    {
        Hashtable hashtable = new Hashtable
        {
            {"rotation", eulerAngles},
            {"time", duration},
            {"easetype", iTween.EaseType.easeInOutQuad}
        };

        iTween.RotateTo(gameObject, hashtable);
    }

    private void ChangeCameraFOV(float fov, float duration)
    {
        Hashtable hashtable = new Hashtable
        {
            {"from", mainCamera.fieldOfView},
            {"to", fov},
            {"time", duration},
            {"onupdate", nameof(UpdateCameraFOV)},
            {"easetype", iTween.EaseType.easeInOutQuad}
        };

        iTween.ValueTo(gameObject, hashtable);
    }

    public void SetupCamera(CameraSetupData cameraSetupData)
    {
        defaultCameraFOV = mainCamera.fieldOfView;
        defaultCameraPosition = gameObject.transform.position;
        defaultCameraRotation = gameObject.transform.rotation;

        currentCameraSetupData = cameraSetupData;
        mainCamera.fieldOfView = currentCameraSetupData.initialFOV;
        gameObject.transform.position=currentCameraSetupData.initialPosition;
        //gameObject.transform.rotation= currentCameraSetupData.initialEulerAngles;
        MoveCameraToPosition(currentCameraSetupData.targetPosition, currentCameraSetupData.duration);
        MoveCameraRotation(currentCameraSetupData.targetEulerAngles, currentCameraSetupData.duration);
        ChangeCameraFOV(currentCameraSetupData.targetFOV, currentCameraSetupData.duration);
    }
    private void OnFinalPosition()
    {
        currentCameraSetupData.onPositionCompleteEvent?.Invoke();
    }
    private void UpdateCameraFOV(float value)
    {
        mainCamera.fieldOfView = value;
    }
}
