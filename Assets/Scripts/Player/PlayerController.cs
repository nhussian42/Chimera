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
    private InputAction _pause;
    private InputAction _unpause;
    // Put new actions here
    private CharacterController _controller;

    private Camera _mainCamera;
    private Matrix4x4 _isoMatrix;

    private string previousScheme = "";
    private const string gamepadScheme = "Gamepad";
    private const string mouseScheme = "Keyboard&Mouse";

    [SerializeField] private float _movementSpeed = 10f; // reference to current legs later
    [SerializeField] private float _turnSpeed = 360f;
    [SerializeField] private bool smoothMovementEnabled;

    // Reference to SaveManager
    SaveManager saveManager;

    // Limb References
    //public List<Heads> allHeads;
    public List<Arm> allArms;
    public List<Legs> allLegs;

    [SerializeField] Core core;
    public Core Core { get { return core; } }
    [SerializeField] Arm coreLeftArm;
    [SerializeField] Arm coreRightArm;
    [SerializeField] Legs coreLegs;
    //Head coreHead;

    public Arm currentLeftArm {get; private set;}
    public Arm currentRightArm {get; private set;}
    public Legs currentLegs { get; private set; }
    //Head currentHead;

    Arm switchedArmRef; // holds ref to arm being switched on player

    [SerializeField] Transform AttackRangeOrigin;
    [SerializeField] GameObject attackRangeRotator;

    [SerializeField] Animator animator;
    public Transform attackRangeOrigin { get { return AttackRangeOrigin; } }

    //public static Action PlayerSpawned;

    public static Action OnDamageReceived;
    public static Action OnArmSwapped;
    public static Action OnGamePaused;

    //float startingYPos;  I don't think we need these anymore  - Amon
    //bool firstMove = false;

    private bool isLeftWolfArm = false;
    private bool isRightWolfArm = false;

    private Vector2 movementValues;
    private Vector3 movementDir;
    private Vector3 movementVector;
    protected override void Init()
    {
        _playerInput = GetComponent<PlayerInput>();
        _playerInputActions = new PlayerInputActions();
        _controller = GetComponent<CharacterController>();
        saveManager = SaveManager.Instance;
    }

    // Enable new player input actions in this method
    private void OnEnable()
    {
        _playerInput.onControlsChanged += ChangeControlSchemes;
        GameManager.OnUnpause += Unpause;

        // Assign default controls
        _movement = _playerInputActions.DefaultControls.Movement;
        _look = _playerInputActions.DefaultControls.Look;
        _attackRight = _playerInputActions.DefaultControls.AttackRight;
        _attackLeft = _playerInputActions.DefaultControls.AttackLeft;
        _swapLimbs = _playerInputActions.DefaultControls.SwapLimbs;
        _pause = _playerInputActions.DefaultControls.Pause;

        EnableAllDefaultControls();

        // Assign UI controls
        _unpause = _playerInputActions.UI.UnPause;

        // Instantiate Limbs
        #region
        // Deactivate all limbs first
        foreach (Arm arm in allArms) arm.gameObject.SetActive(false);
        foreach (Legs legs in allLegs) legs.gameObject.SetActive(false);
  
        if (saveManager.firstLoad == true)
        {
            // If first load into scene, set default limbs
            coreLeftArm.gameObject.SetActive(true);
            coreRightArm.gameObject.SetActive(true);
            //coreLegs.gameObject.SetActive(true);
            //currentLegs = coreLegs;
            currentLeftArm = coreLeftArm;
            currentRightArm = coreRightArm;
            currentLeftArm.Initialize(this);
            currentRightArm.Initialize(this);
        }
        else
        {
            // If not first load into scene, set limbs saved in SaveManager
            LoadLimb(saveManager.SavedLeftArm);
            LoadLimb(saveManager.SavedRightArm);
            //LoadLimb(currentLegs, saveManager.SavedLegs);
        }
        #endregion

    }

    // Disable new player input actions in this method
    private void OnDisable()
    {
        DisableAllDefaultControls();
        DisableAllUIControls();
    }

    private void OnDestroy()
    {
        // Called when the player exits the room (loading a new scene destroys all current scene objects)
        Debug.Log("called save manager");
        saveManager.SaveLimbData(currentLeftArm, currentRightArm);
    }

    private void EnableAllDefaultControls()
    {
        _movement.Enable();
        _look.Enable();
        _attackRight.Enable();
        _attackLeft.Enable();
        _swapLimbs.Enable();
        _pause.Enable();
    }

    private void DisableAllDefaultControls()
    {
        _movement.Disable();
        _look.Disable();
        _attackRight.Disable();
        _attackLeft.Disable();
        _swapLimbs.Disable();
        _pause.Disable();
    }

    private void DisableAllUIControls()
    {
        _unpause.Disable();
    }

    private void ChangeControlSchemes(PlayerInput input)
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
        
        SetPlayerPosition(FloorManager.Instance.StartTransform.position);
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
        movementValues = _movement.ReadValue<Vector2>();
        movementDir = movementValues.y * _mainCamera.transform.forward + movementValues.x * _mainCamera.transform.right;
        Vector3 movementVector = new Vector3(movementDir.x, 0, movementDir.z).normalized;
        _controller.Move(movementVector * Time.deltaTime * _movementSpeed);
        if(transform.position.y > 1.5f)
        {
            SetPlayerPosition(new Vector3(transform.position.x, 1.5f, transform.position.z));
            Debug.Log("Artifical Gravity activated");
        }
        animator.SetFloat("Speed", movementValues.magnitude);
        
        if (movementVector != Vector3.zero)
            RotatePlayer(movementVector); 

        // Reads L and R mouse buttons 
        if (_attackRight.triggered == true && currentRightArm.CanAttack == true)
        {
            currentRightArm.Attack();
            animator.SetTrigger("RightAttack");

            if (isRightWolfArm)
                AudioManager.Instance.PlayPlayerSFX("WolfArm");
            else
                AudioManager.Instance.PlayPlayerSFX("DefaultAttack");
        }


        if (_attackLeft.triggered == true && currentLeftArm.CanAttack == true)
        {
            currentLeftArm.Attack();
            animator.SetTrigger("LeftAttack");
            AudioManager.Instance.PlayPlayerSFX("DefaultAttack");

            if (isLeftWolfArm)
                AudioManager.Instance.PlayPlayerSFX("WolfArm");
            else
                AudioManager.Instance.PlayPlayerSFX("DefaultAttack");
        }

            
        
        if (_swapLimbs.triggered == true)
            SwapLeftAndRightArms();

        if (_pause.triggered == true)
            Pause();
        
        if (_unpause.triggered == true)
        {
            UIManager.ResumePressed?.Invoke();
        }
    }

    private void FixedUpdate()
    {
        //SetPlayerPosition(new Vector3(transform.position.x, startingYPos, transform.position.z));
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.TryGetComponent<LimbDrop>(out LimbDrop limb) != false)
        {
            //Configure later for limb swap menu controls
            if (_attackRight.triggered == true)
            {
                SwapLimb(currentRightArm, limb);
                Destroy(limb.gameObject);

            }
            else if (_attackLeft.triggered == true)
            {
                SwapLimb(currentLeftArm, limb);
                Destroy(limb.gameObject);
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
    
    private void SetPlayerPosition(Vector3 to)
    {
        transform.position = to;

        // Lets the character controller know that the position was manually set by a transform
        // this gave me (Nick) two hours of headaches figuring this out
        Physics.SyncTransforms();
    }

    private void SwapLeftAndRightArms()
    {
        //switchedArmRef = currentRightArm;
        ////Debug.Log("switchedArmRef Health: " + switchedArmRef.Health + " (after setting ref)");
        ////Debug.Log("currentRightArm Health: " + currentRightArm.Health + " (after setting ref)");
        //SwapLimb(currentLeftArm, SideOfPlayer.Right);
        ////Debug.Log("switchedArmRef Health: " + switchedArmRef.Health + " (after SwapLimb)");
        ////Debug.Log("currentRightArm Health: " + currentRightArm.Health + " (after SwapLimb)");
        //currentRightArm.LoadStats(
        //    currentLeftArm.AttackDamage,
        //    currentLeftArm.AttackSpeed,
        //    currentLeftArm.MaxHealth,
        //    currentLeftArm.Health);
        ////Debug.Log("switchedArmRef Health: " + switchedArmRef.Health + " (after LoadStats)");
        ////Debug.Log("currentRightArm Health: " + currentRightArm.Health + " (after LoadStats)");

        //SwapLimb(switchedArmRef, SideOfPlayer.Left);
        //currentLeftArm.LoadStats(
        //    switchedArmRef.AttackDamage,
        //    switchedArmRef.AttackSpeed,
        //    switchedArmRef.MaxHealth,
        //    switchedArmRef.Health);

        //OnArmSwapped?.Invoke();
    }
    public void SwapLimb(ArmDrop newArm, SideOfPlayer side)
    {
        foreach (Arm arm in allArms)
        {
            if (arm.Weight == newArm.Weight && arm.Classification == newArm.Classification && arm.Side == side)
            {
                if (side == SideOfPlayer.Right)
                {
                    isRightWolfArm = true;
                    currentRightArm.Terminate();
                    currentRightArm.gameObject.SetActive(false);
                    arm.Health = currentRightArm.Health;
                    arm.gameObject.SetActive(true);
                    currentRightArm = arm;
                    currentRightArm.Initialize(this);
                }
                else if (side == SideOfPlayer.Left)
                {
                    isLeftWolfArm = true;
                    currentLeftArm.Terminate();
                    currentLeftArm.gameObject.SetActive(false);
                    arm.Health = currentLeftArm.Health;
                    arm.gameObject.SetActive(true);
                    currentLeftArm = arm;
                    currentLeftArm.Initialize(this);
                }
            }
        }

    }

    public void SwapLimb(LimbDrop newLimb)
    {
        switch (newLimb.LimbType)
        {
            case LimbType.Head:
                Debug.Log("Heads are not implemented yet!");
                break;
            case LimbType.Arm:

                break;
            case LimbType.Legs:
                foreach (Legs legs in allLegs)
                {
                    if (legs.Weight == newLimb.Weight && legs.Classification == newLimb.Classification)
                    {
                        currentLegs.gameObject.SetActive(false);
                        legs.gameObject.SetActive(true);
                        currentLegs = legs;
                        // add function here for overwriting current health of equipped legs to match the stored health of the pickup
                    }
                }
                break;
        }

    }

    private void LoadLimb(Arm newArm)
    {
        foreach (Arm arm in allArms)
        {
            if (arm.Weight == newArm.Weight && arm.Classification == newArm.Classification && arm.Side == newArm.Side)
            {
                if (newArm.Side == SideOfPlayer.Right)
                {
                    if (currentRightArm != null)
                    {
                        currentRightArm.Terminate();
                        currentRightArm.gameObject.SetActive(false);
                    }
                    arm.gameObject.SetActive(true);
                    currentRightArm = arm;
                    currentRightArm.Initialize(this);
                    currentRightArm.LoadStats(
                        newArm.AttackDamage,
                        newArm.AttackSpeed,
                        newArm.MaxHealth,
                        newArm.Health);

                }
                else if (newArm.Side == SideOfPlayer.Left)
                {
                    if (currentLeftArm != null)
                    {
                        currentLeftArm.Terminate();
                        currentLeftArm.gameObject.SetActive(false);
                    }
                    arm.gameObject.SetActive(true);
                    currentLeftArm = arm;
                    currentLeftArm.Initialize(this);
                    currentLeftArm.LoadStats(
                        newArm.AttackDamage,
                        newArm.AttackSpeed,
                        newArm.MaxHealth,
                        newArm.Health);
                }
            }
        }
    }

    private void LoadLimb(Legs newLegs)
    {
        foreach (Legs legs in allLegs)
        {
            if (legs.Weight == newLegs.Weight && legs.Classification == newLegs.Classification)
            {
                currentLegs.gameObject.SetActive(false);
                legs.gameObject.SetActive(true);
                currentLegs = legs;
                currentLegs.LoadStats(newLegs.Health, newLegs.MovementSpeed);
            }
        }
    }

    public void SwapLimb(Arm currentArm, LimbDrop newArm)
    {
        foreach (Arm arm in allArms)
        {
            if (arm.Weight == newArm.Weight && arm.Classification == newArm.Classification && arm.Side == currentArm.Side)
            {
                if (currentArm.Side == SideOfPlayer.Right)
                {
                    if (currentRightArm != null)
                    {
                        currentRightArm.Terminate();
                        currentRightArm.gameObject.SetActive(false);
                    }
                    arm.gameObject.SetActive(true);
                    currentRightArm = arm;
                    currentRightArm.Initialize(this);

                }
                else if (currentArm.Side == SideOfPlayer.Left)
                {
                    if (currentLeftArm != null)
                    {
                        currentLeftArm.Terminate();
                        currentLeftArm.gameObject.SetActive(false);
                    }
                    arm.gameObject.SetActive(true);
                    currentLeftArm = arm;
                    currentLeftArm.Initialize(this);
                }
            }
        }
    }

    public void DistributeDamage(float damage)
    {
        AudioManager.Instance.PlayPlayerSFX("MinHit");
        List<Limb> damagedLimbs = new List<Limb>();

        damagedLimbs.Add(core);

        if (currentLeftArm != coreLeftArm)
            damagedLimbs.Add(currentLeftArm);
        if (currentRightArm != coreRightArm)
            damagedLimbs.Add(currentRightArm);

        foreach (Limb limb in damagedLimbs)
        {
            float caclulatedDamage = -1 * (damage / (damagedLimbs.Count + 1));
            limb.UpdateHealth(caclulatedDamage);
        }
        OnDamageReceived?.Invoke();
    }

    private void Pause()
    {
        DisableAllDefaultControls();
        _unpause.Enable();
        OnGamePaused?.Invoke();
    }

    private void Unpause()
    {
        EnableAllDefaultControls();
        _unpause.Disable();
    }

}
