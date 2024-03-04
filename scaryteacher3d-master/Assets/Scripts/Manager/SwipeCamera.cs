using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeCamera : MonoBehaviour
{
    public float swipeSpeed = 0.5f;
    public float minSwipeDistance = 50f;

    private Vector2 startTouchPosition;
    private Vector2 endTouchPosition;
    private float swipeDistance;

    private void Update()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                startTouchPosition = touch.position;
                endTouchPosition = touch.position;
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                endTouchPosition = touch.position;
                swipeDistance = (endTouchPosition - startTouchPosition).magnitude;

                if (swipeDistance > minSwipeDistance)
                {
                    Vector2 swipeDirection = (endTouchPosition - startTouchPosition).normalized;
                    transform.position += new Vector3(swipeDirection.x, 0, swipeDirection.y) * swipeSpeed;
                }
            }
        }
    }
}
