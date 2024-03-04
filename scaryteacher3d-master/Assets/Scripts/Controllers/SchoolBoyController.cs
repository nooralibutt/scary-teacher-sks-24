using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SchoolBoyController : MonoBehaviour
{
    private Animator CurrentAnimator;

    //[SerializeField] private FloatingJoystick _joystick;

    [SerializeField] private AnimatorController _animatorController;

    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _rotateSpeed;

    [SerializeField] float gravity;

    [SerializeField] private Transform propsHolder;


    //public FixedTouchField TouchField;

    protected float CameraAngle;
    protected float CameraAngleSpeed = 0.2f;

    private Rigidbody _rigidbody;

    private Vector3 _moveVector;

    private Vector3 velocity;

    private bool OnStairs = false;

    [SerializeField] private GameObject Renderer;

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


    private void FixedUpdate()
    {
        float x = ControlFreak2.CF2Input.GetAxis("Horizontal");
        float z = ControlFreak2.CF2Input.GetAxis("Vertical");
        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * playerSpeed * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    private void Move()
    {
        float x = ControlFreak2.CF2Input.GetAxis("Horizontal");
        float z = ControlFreak2.CF2Input.GetAxis("Vertical");
        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * playerSpeed * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

    }

    public void OnCollisionDetection(string collisionObject)
    {
        if (LevelPropsManager._Instance.ValidatePropsAccordingToLevel(collisionObject, StoryModeLevelManager.Instance.CurrentLevelNumber))
        {
            StoryModeGameManager.Instance.currentLevel.SetCurrentSelectedProp(collisionObject);
            StoryModeGameManager.Instance.currentLevel.DecideInteractability();
        }
    }


    public void OnCollisionLeft(string collision)
    {
        StoryModeGameManager.Instance.currentLevel.ToggleObjectInteraction(false);
    }


    public void TogglePlayerMesh()
    {
        Renderer.GetComponent<SkinnedMeshRenderer>().enabled = true;
    }


    public void DisplayLevelProp(GameObject prop)
    {
        var gameProp = Instantiate(prop, propsHolder);
        gameProp.transform.localPosition = Vector3.zero;
        if (gameProp.GetComponent<Collider>() != null)
            gameProp.GetComponent<Collider>().enabled = false;
    }

    public void RemoveLevelProps()
    {
        foreach (Transform child in propsHolder)
            Destroy(child.gameObject);
    }
}
