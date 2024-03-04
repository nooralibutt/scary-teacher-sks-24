using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public enum ZoomState { ZoomedIn, ZoomedOut, UltraZoomedOut }
    public Transform lookAt;

    private Vector3 prevPosition;

    public Vector3 zoomInOffset = new Vector3(0f, 10.1f, -13.1f);
    public Vector3 zoomOutOffset = new Vector3(0f, 10.1f, -13.1f);

    public Vector3 rotation = new Vector3(35f, 0f, 0f);

    public float followSpeed = 10.0f;
    private Vector3 offset;

    //private bool zoomedIn = true;
    private ZoomState zoomState = ZoomState.ZoomedIn;
    private bool startFollowing = true;
    private void Start()
    {
        offset = zoomInOffset;
        ZoomOut();
    }
    private void LateUpdate()
    {
        if (!startFollowing)
        {
            return;
        }
        if (lookAt == null)
        {
            return;
        }

        Vector3 targetPos = lookAt.position + offset;
        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * followSpeed);

        transform.rotation = Quaternion.Euler(rotation);
    }
    public void StopFollowing()
    {
        startFollowing = false;
    }
    public void StartFollowing()
    {
        startFollowing = true;
    }
    public void ZoomOut()
    {
        //if (!zoomedIn)
        if (zoomState == ZoomState.ZoomedOut)
            return;

        StartCoroutine(ZoomChangeEffect(offset, zoomOutOffset, 1));
        //zoomedIn = false;
        zoomState = ZoomState.ZoomedOut;
    }
    public void ZoomIn()
    {
        //if (zoomedIn)
        if (zoomState == ZoomState.ZoomedIn)
            return;

        StartCoroutine(ZoomChangeEffect(offset, zoomInOffset, 1));
        //zoomedIn = true;
        zoomState = ZoomState.ZoomedIn;
    }
    private IEnumerator ZoomChangeEffect(Vector3 initial, Vector3 final, float duration)
    {
        float elapsedTime = 0;

        while (elapsedTime < 1)
        {
            elapsedTime += Time.deltaTime / duration;
            offset = Vector3.Lerp(initial, final, elapsedTime);
            yield return null;
        }
    }

    public void ChangeCameraTarget(Transform target)
    {
        this.lookAt = target;
    }

    private void Update()
    {
        
    }
}
