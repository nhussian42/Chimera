using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

[RequireComponent(typeof(CharacterController), typeof(PlayerInput))]
public class PlayerController : Singleton<PlayerController>
{
    private PlayerInputActions _playerInputActions;
    private PlayerInput _playerInput;
    private InputAction _movement;
    private InputAction _look; // for keyboard/mouse attack direction
    private InputAction _attackRight;
    private InputAction _attackLeft;
    // Put new actions here
    private CharacterController _controller;

    private Camera _mainCamera;
    private Matrix4x4 _isoMatrix;

    private string previousScheme = "";
    private const string gamepadScheme = "Gamepad";
    private const string mouseScheme = "Keyboard&Mouse";

    public float health = 0;
    public float damage = 0;
    public float atkspeed = 0;

    [SerializeField] private float _movementSpeed = 10f;
    [SerializeField] private float _turnSpeed = 360f;
    [SerializeField] private bool smoothMovementEnabled;

    // Limb References
    [SerializeField] Arm coreLeftArm;
    [SerializeField] Arm coreRightArm;
    //Legs coreLegs;
    //Head coreHead;

    Arm currentLeftArm;
    Arm currentRightArm;
    //Legs currentLegs;
    //Head currentHead;

    Arm switchedArmRef; // holds ref to arm being switched on player

    [SerializeField] Transform leftArmPos;
    [SerializeField] Transform rightArmPos;
    //Transform legsPos;
    //Transform headPos;

    //public static Action PlayerSpawned;

    protected override void Init()
    {
        _playerInput = GetComponent<PlayerInput>();
        _playerInputActions = new PlayerInputActions();
        _controller = GetComponent<CharacterController>();
    }

    // Enable new player input actions in this method
    private void OnEnable()
    {
        _playerInput.onControlsChanged += ChangeControls;

        _movement = _playerInputActions.DefaultControls.Movement;
        _movement.Enable();
        
        _look = _playerInputActions.DefaultControls.Look;
        _look.Enable();

        _attackRight = _playerInputActions.DefaultControls.AttackRight;
        _attackRight.Enable();

        _attackLeft = _playerInputActions.DefaultControls.AttackLeft;
        _attackLeft.Enable();

        // Instantiate Default Limbs
        currentLeftArm = Instantiate(coreLeftArm.gameObject, leftArmPos.position, Quaternion.identity, leftArmPos).GetComponent<Arm>();
        currentRightArm = Instantiate(coreRightArm.gameObject, rightArmPos.position, Quaternion.identity, rightArmPos).GetComponent<Arm>();

    }

    // Disable new player input actions in this method
    private void OnDisable()
    {
        _movement.Disable();
        _look.Disable();
        _attackRight.Disable();
        _attackLeft.Disable();
    }

    // Switches between control schemes
    private void ChangeControls(PlayerInput input)
    {
        if (_playerInput.currentControlScheme == mouseScheme && previousScheme != mouseScheme)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            previousScheme = mouseScheme;
        }
        else if (_playerInput.currentControlScheme == gamepadScheme && previousScheme != gamepadScheme)
        {
            
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            previousScheme = gamepadScheme;
        }
    }

    private void Start()
    {
        _mainCamera = Camera.main;
        _isoMatrix = Matrix4x4.Rotate(Quaternion.Euler(0, 45, 0));

        Instance.gameObject.SetActive(true);
        // StartCoroutine(WaitForTransform(5f));
        
        SetPlayerPosition(FloorManager.Instance.StartTransform);
    }

    // Debug
    // IEnumerator WaitForTransform(float maxWaitTime)
    // {
    //     while (maxWaitTime > 0 && Instance.gameObject.GetComponent<Transform>())
    //     {
    //         maxWaitTime -= Time.deltaTime;
    //         yield return new WaitForEndOfFrame();
    //     }
    //     PlayerSpawned?.Invoke();
    // }

    private void Update()
    {
        Vector2 movementValues = _movement.ReadValue<Vector2>();
        Vector3 movementDir = movementValues.y * _mainCamera.transform.forward + movementValues.x * _mainCamera.transform.right;
        Vector3 movementVector = new Vector3(movementDir.x, 0, movementDir.z).normalized;
        
        _controller.Move(movementVector * Time.deltaTime * _movementSpeed);
        
        if (movementVector != Vector3.zero)
            RotatePlayer(movementVector);


        // Reads L and R mouse buttons 
        if (_attackRight.triggered == true)
            currentRightArm.Attack();
            //Debug.Log("attack right");
        if (_attackLeft.triggered == true)
            currentLeftArm.Attack();
            //Debug.Log("attack left");
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.TryGetComponent<ArmDrop>(out ArmDrop armDrop) != false)
        {
            if (_attackRight.triggered == true)
            {
                SwapLimb(armDrop.armReference, SideOfPlayer.Right);
                Destroy(armDrop.gameObject);
            }
            else if (_attackLeft.triggered == true)
            {
                SwapLimb(armDrop.armReference, SideOfPlayer.Left);
                Destroy(armDrop.gameObject);
            }
        }
    }

    private void RotatePlayer(Vector3 towards)
    {
        Vector3 isoVector = _isoMatrix.MultiplyPoint3x4(towards);
        Quaternion newRotation = Quaternion.LookRotation(towards, Vector3.up);

        transform.rotation = smoothMovementEnabled ? Quaternion.RotateTowards(transform.rotation, newRotation, Time.deltaTime * _turnSpeed) : newRotation;
    }
    
    private void SetPlayerPosition(Transform to)
    {
        transform.position = to.position;

        // Lets the character controller know that the position was manually set by a transform
        // this gave me (Nick) two hours of headaches figuring this out
        Physics.SyncTransforms();
    }

    private void SwapLeftAndRightArms()
    {

    }

    public void SwapLimb(Arm newArm, SideOfPlayer side)
    {
        if(side == SideOfPlayer.Right)
        {
            Destroy(currentRightArm.gameObject);
            currentRightArm = Instantiate(newArm.gameObject, rightArmPos.position, Quaternion.identity, rightArmPos).GetComponent<Arm>();
        }
        else
        {
            Destroy(currentLeftArm.gameObject);
            currentLeftArm = Instantiate(newArm.gameObject, leftArmPos.position, Quaternion.identity, leftArmPos).GetComponent<Arm>();
        }
    }
}
