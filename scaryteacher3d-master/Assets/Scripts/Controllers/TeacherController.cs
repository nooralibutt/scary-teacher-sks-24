using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeacherController : MonoBehaviour
{
    private Animator CurrentAnimator;
    bool startFollowing = false;

    private bool moveTowardsPlayer=false;

    private void Start()
    {
        CurrentAnimator = GetComponent<Animator>();
    }

    public void AnimateTeacher(GameConstants.InGameConstants.TeacherAnimationNames teacherAnimationName)
    {
        CurrentAnimator.SetTrigger(teacherAnimationName.ToString());
    }

    Vector3 target;


    public void AssignCameraToPlayer()
    {
        //StoryModeGameManager.Instance.CameraFollwer.ChangeCameraTarget(StoryModeGameManager.Instance.GetSchoolBoyController().transform);
        StoryModeGameManager.Instance.TogglePlayerMovement(true);
    }

    public void AllowePlayerToMove()
    {
        StoryModeGameManager.Instance.TogglePlayerMovement(true);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("SchoolBoy"))
        {
            //StoryModeGameManager.Instance.currentLevel.TurnOffTeacherAnimationTimeline();
            //moveTowardsPlayer = true;
            //GetComponent<Rigidbody>().isKinematic = true;
            //GameObject.FindObjectOfType<SchoolBoyController>().GetComponent<Rigidbody>().isKinematic = true;
            //GameObject.FindObjectOfType<SchoolBoyController>().transform.LookAt(transform);
            //transform.LookAt(GameObject.FindObjectOfType<SchoolBoyController>().transform);
            //AnimateTeacher(GameConstants.InGameConstants.TeacherAnimationNames.Running);
        }
    }

    private void Update()
    {
        if(moveTowardsPlayer)
        {
            FailAnimation();
            if(Vector3.Distance(transform.position,GameObject.FindObjectOfType<SchoolBoyController>().transform.position)<2)
            {
                moveTowardsPlayer = false;
                StoryModeLevelManager.Instance.onLevelEnded?.Invoke(false);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!StoryModeGameManager.Instance.isLevelBeingPlayed)
            return;
        if (other.gameObject.CompareTag("SchoolBoy"))
        {
            if (StoryModeLevelManager.Instance.onLevelCompletionTriggered)
                return;
            else
                StoryModeLevelManager.Instance.onLevelFailTriggered = true;
            StoryModeGameManager.Instance.currentLevel.TurnOffTeacherAnimationTimeline();
            StoryModeGameManager.Instance.currentLevel.TogglePauseButton();
            UIManager.Instance.ToggleJoyStick(false);
            moveTowardsPlayer = true;
            //GetComponent<Rigidbody>().isKinematic = true;
            //GameObject.FindObjectOfType<SchoolBoyController>().GetComponent<Rigidbody>().isKinematic = true;
            GameObject.FindObjectOfType<SchoolBoyController>().transform.LookAt(transform);
            transform.LookAt(GameObject.FindObjectOfType<SchoolBoyController>().transform);
            AnimateTeacher(GameConstants.InGameConstants.TeacherAnimationNames.Running);
        }
    }

    public void FailAnimation()
    {
        transform.position = Vector3.MoveTowards(transform.position, GameObject.FindObjectOfType<SchoolBoyController>().transform.position, 3f * Time.deltaTime);
    }
}
