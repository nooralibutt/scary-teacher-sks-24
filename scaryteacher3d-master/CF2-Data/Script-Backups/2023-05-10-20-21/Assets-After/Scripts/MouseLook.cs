using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    [SerializeField] Transform playerBody;
    [SerializeField] float mouseSensitivity;
    float xRoration = 0f;

    private void Start()
    {
        ControlFreak2.CFCursor.lockState = CursorLockMode.Locked;
    }
    private void Update()
    {
        float mouseX = ControlFreak2.CF2Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = ControlFreak2.CF2Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        playerBody.Rotate(Vector3.up * mouseX);

        xRoration -= mouseY;
        xRoration = Mathf.Clamp(xRoration, -90f, 90f);
        transform.localRotation = Quaternion.Euler(xRoration, 0, 0);
    }
}
