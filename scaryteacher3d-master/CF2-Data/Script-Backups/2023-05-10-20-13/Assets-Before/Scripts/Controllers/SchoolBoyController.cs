using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SchoolBoyController : MonoBehaviour
{
    private Animator CurrentAnimator;

    [SerializeField] private FloatingJoystick _joystick;

    [SerializeField] private AnimatorController _animatorController;

    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _rotateSpeed;

    [SerializeField] float gravity;


    public FixedTouchField TouchField;

    protected float CameraAngle;
    protected float CameraAngleSpeed = 0.2f;

    private Rigidbody _rigidbody;

    private Vector3 _moveVector;

    private Vector3 velocity;

    private bool OnStairs = false;

    [SerializeField] CharacterController controller;
    [SerializeField] float playerSpeed;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    void Start()
    {
        CurrentAnimator = GetComponent<Animator>();

    }

    public void AnimateStudent(GameConstants.InGameConstants.StudentAnimationNames studentAnimationName)
    {
        CurrentAnimator.SetTrigger(studentAnimationName.ToString());
    }

    //private void FixedUpdate()
    //{
    //    if (StoryModeGameManager.Instance.IsPlayerAllowedToMove())
    //        Move();
    //}

    private void FixedUpdate()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * playerSpeed * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    private void Move()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * playerSpeed * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

    }


    //private void Move()
    //{
    //    //_moveVector = Vector3.zero;
    //    _moveVector.x = 1 * _joystick.Horizontal * _moveSpeed * Time.deltaTime;
    //    _moveVector.z = 1 * _joystick.Vertical * _moveSpeed * Time.deltaTime;

    //    if (_joystick.Horizontal != 0 || _joystick.Vertical != 0)
    //    {
    //        _animatorController.PlayAnimation(GameConstants.InGameConstants.StudentAnimationNames.Walking.ToString());
    //    }
    //    else if (_joystick.Horizontal == 0 && _joystick.Vertical == 0)
    //    {
    //        _animatorController.PlayAnimation(GameConstants.InGameConstants.StudentAnimationNames.SadIdle.ToString());
    //        _rigidbody.velocity = Vector3.zero; 
    //    }

    //    //if (!OnStairs)
    //    {
    //        Vector3 direction = Vector3.RotateTowards(transform.forward, Camera.main.transform.forward, _rotateSpeed * Time.deltaTime, 0.0f);
    //        direction.y = 0;
    //        transform.rotation = Quaternion.LookRotation(direction);
    //    }
    //    transform.Translate(_moveVector);

    //}

    //private void LateUpdate()
    //{
    //    if (StoryModeGameManager.Instance.IsPlayerAllowedToMove())
    //    {
    //        transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, transform.localEulerAngles.z);

    //        if (TouchField != null)
    //        {
    //            CameraAngle += TouchField.TouchDist.x * CameraAngleSpeed /*+ TouchField.TouchDist.y * CameraAngleSpeed + TouchField.TouchDist.z * CameraAngleSpeed*/;

    //            Camera.main.transform.position = transform.position + Quaternion.AngleAxis(CameraAngle, Vector3.up) * new Vector3(0, 3f, 1f);
    //            Camera.main.transform.rotation = Quaternion.LookRotation(transform.position + Vector3.up * 3f - Camera.main.transform.position, Vector3.up);

    //        }
    //    }
    //}

    private void OnCollisionEnter(Collision collision)
    {

        //if (collision.gameObject.CompareTag("Stairs"))
        //    OnStairs = true;
        //if (LevelPropsManager._Instance.ValidatePropsAccordingToLevel(collision.gameObject.tag, StoryModeLevelManager._Instance.CurrentLevelNumber))
        //{
        //    StoryModeGameManager.Instance.currentLevel.SetCurrentSelectedProp(collision.gameObject.tag);
        //    StoryModeGameManager.Instance.currentLevel.DecideInteractability();
        //}
    }

    private void OnCollisionExit(Collision collision)
    {
        //if (collision.gameObject.CompareTag("Stairs"))
        //    OnStairs = false;
        //StoryModeGameManager.Instance.currentLevel.ToggleObjectInteraction(false);
    }


    public void OnCollisionDetection(string collisionObject)
    {
        if (collisionObject.Equals("Stairs"))
            OnStairs = true;
        if (LevelPropsManager._Instance.ValidatePropsAccordingToLevel(collisionObject, StoryModeLevelManager._Instance.CurrentLevelNumber))
        {
            StoryModeGameManager.Instance.currentLevel.SetCurrentSelectedProp(collisionObject);
            StoryModeGameManager.Instance.currentLevel.DecideInteractability();
        }
    }


    public void OnCollisionLeft(string collision)
    {
        if (collision.Equals("Stairs"))
            OnStairs = false;
        StoryModeGameManager.Instance.currentLevel.ToggleObjectInteraction(false);
    }
}
