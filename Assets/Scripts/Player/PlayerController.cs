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
    private InputAction _swapLimbs;
    // Put new actions here
    private CharacterController _controller;

    private Camera _mainCamera;
    private Matrix4x4 _isoMatrix;

    private string previousScheme = "";
    private const string gamepadScheme = "Gamepad";
    private const string mouseScheme = "Keyboard&Mouse";

    // public float health = 0;
    // public float damage = 0;
    // public float atkspeed = 0;

    [SerializeField] private float _movementSpeed = 10f;
    [SerializeField] private float _turnSpeed = 360f;
    [SerializeField] private bool smoothMovementEnabled;

    [SerializeField] private float coreHealth = 100f;
    public float CoreHealth { get { return coreHealth; } }

    // Limb References
    public List<Arm> allArms;

    [SerializeField] Arm coreLeftArm;
    [SerializeField] Arm coreRightArm;
    //Legs coreLegs;
    //Head coreHead;

    public Arm currentLeftArm {get; private set;}
    public Arm currentRightArm {get; private set;}
    //Legs currentLegs;
    //Head currentHead;

    Arm switchedArmRef; // holds ref to arm being switched on player

    [SerializeField] Transform AttackRangeOrigin;
    [SerializeField] GameObject attackRangeRotator;

    [SerializeField] Animator animator;
    public Transform attackRangeOrigin { get { return AttackRangeOrigin; } }

    //public static Action PlayerSpawned;

    public static Action OnDamageReceived;
    public static Action OnArmSwapped;

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

        _swapLimbs= _playerInputActions.DefaultControls.SwapLimbs;
        _swapLimbs.Enable();



        // Instantiate Default Limbs
        foreach(Arm arm in allArms)
        {
            arm.gameObject.SetActive(false);
        }
        coreLeftArm.gameObject.SetActive(true);
        coreRightArm.gameObject.SetActive(true);
        currentLeftArm = coreLeftArm;
        currentRightArm = coreRightArm;
        currentLeftArm.Initialize(this);
        currentRightArm.Initialize(this);
    }

    // Disable new player input actions in this method
    private void OnDisable()
    {
        _movement.Disable();
        _look.Disable();
        _attackRight.Disable();
        _attackLeft.Disable();
        _swapLimbs.Disable();
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
        animator.SetFloat("Speed", movementValues.magnitude);

        
        if (movementVector != Vector3.zero)
            RotatePlayer(movementVector);

        // Reads L and R mouse buttons 
        if (_attackRight.triggered == true)
            currentRightArm.Attack();
            
        if (_attackLeft.triggered == true)
            currentLeftArm.Attack();
        
        if (_swapLimbs.triggered == true)
            SwapLeftAndRightArms();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.TryGetComponent<ArmDrop>(out ArmDrop arm) != false)
        {
            if (_attackRight.triggered == true)
            {
                SwapLimb(arm, SideOfPlayer.Right);
                Destroy(arm.gameObject);

            }
            else if (_attackLeft.triggered == true)
            {
                SwapLimb(arm, SideOfPlayer.Left);
                Destroy(arm.gameObject);
            }
        }

        OnArmSwapped?.Invoke();
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
        switchedArmRef = currentRightArm;
        SwapLimb(currentLeftArm, SideOfPlayer.Right);
        SwapLimb(switchedArmRef, SideOfPlayer.Left);

        OnArmSwapped?.Invoke();
    }
    public void SwapLimb(ArmDrop newArm, SideOfPlayer side)
    {
        foreach (Arm arm in allArms)
        {
            if (arm.Weight == newArm.Weight && arm.Classification == newArm.Classification && arm.Side == side)
            {
                if (side == SideOfPlayer.Right)
                {
                    currentRightArm.Terminate();
                    currentRightArm.gameObject.SetActive(false);
                    arm.gameObject.SetActive(true);
                    currentRightArm = arm;
                    currentRightArm.Initialize(this);

                }
                else if (side == SideOfPlayer.Left)
                {
                    currentLeftArm.Terminate();
                    currentLeftArm.gameObject.SetActive(false);
                    arm.gameObject.SetActive(true);
                    currentLeftArm = arm;
                    currentLeftArm.Initialize(this);
                }
            }
        }

    }
    public void SwapLimb(Arm newArm, SideOfPlayer side)
    {
        foreach (Arm arm in allArms)
        {
            if (arm.Weight == newArm.Weight && arm.Classification == newArm.Classification && arm.Side == side)
            {
                if (side == SideOfPlayer.Right)
                {
                    currentRightArm.Terminate();
                    currentRightArm.gameObject.SetActive(false);
                    arm.gameObject.SetActive(true);
                    currentRightArm = arm;
                    currentRightArm.Initialize(this);

                }
                else if (side == SideOfPlayer.Left)
                {
                    currentLeftArm.Terminate();
                    currentLeftArm.gameObject.SetActive(false);
                    arm.gameObject.SetActive(true);
                    currentLeftArm = arm;
                    currentLeftArm.Initialize(this);
                }
            }
        }
    }

    public void DistributeDamage(float damage)
    {
        List<Limb> damagedLimbs = new List<Limb>();

        if (currentLeftArm != coreLeftArm)
            damagedLimbs.Add(currentLeftArm);
        if (currentRightArm != coreRightArm)
            damagedLimbs.Add(currentRightArm);

        foreach (Limb limb in damagedLimbs)
            limb.RemoveHealth(damage / (damagedLimbs.Count + 1));
        
        // maybe make core its own limb?
        coreHealth -= damage / (damagedLimbs.Count + 1);

        OnDamageReceived?.Invoke();
    }
}
