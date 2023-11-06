using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.SceneManagement;

using UnityEngine.Events;

[RequireComponent(typeof(CharacterController), typeof(PlayerInput))]
public class PlayerController : Singleton<PlayerController>
{
    private PlayerInputActions _playerInputActions;
    private PlayerInput _playerInput;
    private InputAction _movement;
    private InputAction _look; // for keyboard/mouse attack direction
    private InputAction _attackRight;
    private InputAction _attackLeft;
    private InputAction _legsAbility;
    private InputAction _swapLimbs;
    private InputAction _pause;
    private InputAction _unpause;
    private InputAction _openEM;
    private InputAction _closeEM;
    // Put new actions here
    public CharacterController _controller;

    private Matrix4x4 _isoMatrix;

    private string previousScheme = "";
    private const string gamepadScheme = "Gamepad";
    private const string mouseScheme = "Keyboard&Mouse"; 

    [SerializeField] private GameObject EquipMenu;

    [SerializeField] private Camera _mainCamera;
    private float _movementSpeed; // references current legs
    [SerializeField] private float _turnSpeed = 360f;
    [SerializeField] private bool smoothMovementEnabled;

    // Reference to SaveManager
    SaveManager saveManager;

    // Unity Events for Scriptable Object events (GameEvents class)
    [SerializeField] UnityEvent OnAttack;
    [SerializeField] UnityEvent OnDash;
    [SerializeField] UnityEvent OnTakeDamage;
    [SerializeField] UnityEvent OnSwapLimbs;

    // Limb References
    //public List<Heads> allHeads;
    public List<Arm> allArms;
    public List<Legs> allLegs;

    [SerializeField] PublicClassesSO publicClassesSO;
    [SerializeField] CurrentBaseStatsSO currentBaseStatsSO;
    [SerializeField] ModifiedStatsSO modifiedStatsSO;
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

    [SerializeField] Transform AttackRangeOrigin;
    [SerializeField] GameObject attackRangeRotator;

    [SerializeField] Animator animator;
    public Transform attackRangeOrigin { get { return AttackRangeOrigin; } }

    //public static Action PlayerSpawned;

    public static Action OnDamageReceived;
    public static Action OnArmSwapped;
    public static Action OnGamePaused;
    public static Action OnDie;

    public static Action<LimbDrop> OnLimbDropTriggerStay; // DEBUG

    //float startingYPos;  I don't think we need these anymore  - Amon
    //bool firstMove = false;

    private bool isLeftWolfArm = false;
    private bool isRightWolfArm = false;

    private Vector2 movementValues;
    private Vector3 movementDir;
    private Vector3 movementVector;

    public float totalBones;
    public float bonesMultiplier;

    protected override void Init()
    {
        _playerInput = GetComponent<PlayerInput>();
        _playerInputActions = new PlayerInputActions();
        _controller = GetComponent<CharacterController>();
        
    }

    // Enable new player input actions in this method
    private void OnEnable()
    {
        _playerInput.onControlsChanged += ChangeControlSchemes;
        GameManager.OnUnpause += Unpause;
        FloorManager.LoadNextRoom += Deactivate;
        FloorManager.NextRoomLoaded += SetStartPosition;
        FloorManager.LeaveRoom += DisableAllDefaultControls;
        // FloorManager.EnableFloor += EnableAllDefaultControls;

        // Assign default controls
        _movement = _playerInputActions.DefaultControls.Movement;
        _look = _playerInputActions.DefaultControls.Look;
        _attackRight = _playerInputActions.DefaultControls.AttackRight;
        _attackLeft = _playerInputActions.DefaultControls.AttackLeft;
        _legsAbility = _playerInputActions.DefaultControls.LegsAbility;
        _swapLimbs = _playerInputActions.DefaultControls.SwapLimbs;
        _pause = _playerInputActions.DefaultControls.Pause;
        _openEM = _playerInputActions.DefaultControls.OpenEM;

        EnableAllDefaultControls();

        // Assign UI controls
        _unpause = _playerInputActions.UI.UnPause;
        _closeEM = _playerInputActions.UI.CloseEM;
    }

    // Disable new player input actions in this method
    private void OnDisable()
    {
        _playerInput.onControlsChanged -= ChangeControlSchemes;
        GameManager.OnUnpause -= Unpause;
        FloorManager.LoadNextRoom -= Deactivate;
        FloorManager.NextRoomLoaded -= SetStartPosition;
        FloorManager.LeaveRoom -= DisableAllDefaultControls;
        // FloorManager.EnableFloor -= EnableAllDefaultControls;

        DisableAllDefaultControls();
        DisableAllUIControls();
    }

    private void OnDestroy()
    {
        // Called when the player exits the room (loading a new scene destroys all current scene objects)
        if (core.Health == 0) { saveManager.Reset(); }
        else { saveManager.SaveLimbData(currentLeftArm, currentRightArm, core, currentLegs); }
        
    }

    private void EnableAllDefaultControls()
    {
        _movement.Enable();
        _look.Enable();
        _attackRight.Enable();
        _attackLeft.Enable();
        _legsAbility.Enable();
        _swapLimbs.Enable();
        _pause.Enable();
        _openEM.Enable();
    }

    private void DisableAllDefaultControls()
    {
        _movement.Disable();
        _look.Disable();
        _attackRight.Disable();
        _attackLeft.Disable();
        _legsAbility.Disable();
        _swapLimbs.Disable();
        _pause.Disable();
        _openEM.Disable();
    }

    private void DisableAllUIControls()
    {
        _unpause.Disable();
        _closeEM.Disable();
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
        // get reference to save manager here because it won't work in Init()
        saveManager = SaveManager.Instance;

        // pass reference of self to the PublicClassesSO class (so other UnityEvents can easily call UpdateStats())
        publicClassesSO.AddPlayerController(this);

        // Instantiate Limbs
        #region
        // Set all limbs' stats to default, then hide them
        foreach (Arm arm in allArms)
        {
            if(saveManager.firstLoad == true) { arm.LoadDefaultStats(); }
            arm.gameObject.SetActive(false);
        }
        foreach (Legs legs in allLegs)
        {
            if(saveManager.firstLoad == true) { legs.LoadDefaultStats(); }
            legs.gameObject.SetActive(false);
        }

        if (saveManager.firstLoad == false)
        {
            // If not first load into scene, set limbs saved in SaveManager
            LoadSavedLimb(saveManager.SavedLeftArm);
            LoadSavedLimb(saveManager.SavedRightArm);
            LoadSavedLimb(saveManager.SavedCore);
            LoadSavedLimb(saveManager.SavedLegs);
        }
        else
        {
            // If first load into scene, set default limbs
            coreLeftArm.gameObject.SetActive(true);
            coreRightArm.gameObject.SetActive(true);
            coreLegs.gameObject.SetActive(true);
            currentLegs = coreLegs;
            currentLeftArm = coreLeftArm;
            currentRightArm = coreRightArm;
            currentLeftArm.Initialize(this);
            currentRightArm.Initialize(this);
            core.LoadDefaultStats();
        }

        // read default build to base stats SO on first load
        //currentBaseStatsSO.UpdateCurrentBuild(core, currentLeftArm, currentRightArm, currentLegs);
        OnSwapLimbs.Invoke();

        #endregion
        _movementSpeed = currentLegs.MovementSpeed;

        // _mainCamera = Camera.main;

        _isoMatrix = Matrix4x4.Rotate(Quaternion.Euler(0, 45, 0));
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
        animator.SetFloat("Speed", movementValues.magnitude * _movementSpeed / 10f);
        
        if (movementVector != Vector3.zero)
            RotatePlayer(movementVector); 

        // Reads L and R mouse buttons 
        if (_attackRight.triggered == true && currentRightArm.CanAttack == true)
        {
            //currentRightArm.PauseInput();

            OnAttack.Invoke();

            animator.SetTrigger("RightAttack");

            if (isRightWolfArm)
                AudioManager.Instance.PlayPlayerSFX("WolfArm");
            else
                AudioManager.Instance.PlayPlayerSFX("DefaultAttack");
        }


        if (_attackLeft.triggered == true && currentLeftArm.CanAttack == true)
        {
            //currentRightArm.PauseInput();

            //OnAttack.Invoke();

            animator.SetTrigger("LeftAttack");

            if (isLeftWolfArm)
                AudioManager.Instance.PlayPlayerSFX("WolfArm");
            else
                AudioManager.Instance.PlayPlayerSFX("DefaultAttack");
        }

        if(_legsAbility.triggered == true && currentLegs.CanActivate == true)
        {
            OnDash.Invoke();

            // This will need refactoring for special leg animations, the line below will probably
            // be called by an animation event like the triggers above.
            currentLegs.ActivateAbility();
            
        }

        if (_swapLimbs.triggered == true)
            SwitchArms();

        if (_pause.triggered == true)
            Pause();
        
        if (_unpause.triggered == true || _closeEM.triggered == true)
        {
            UIManager.ResumePressed?.Invoke();
            EquipMenu.SetActive(false);
        }

        if (_openEM.triggered == true)
        {
            Pause();
            EquipMenu.SetActive(true);
        }
    }

    private void FixedUpdate()
    {
        //SetPlayerPosition(new Vector3(transform.position.x, startingYPos, transform.position.z));
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.TryGetComponent<LimbDrop>(out LimbDrop newLimb) != false)
        {
            // Scrap Limb
            if (Input.GetKeyDown(KeyCode.K))
            {
                Debug.Log("Scrapped Item");
                Instance.AddBones(50);
                Destroy(newLimb.gameObject);
            }

            //Configure later for limb swap menu controls
            if (_attackRight.triggered == true)
            {
                SwapLimb(currentRightArm, newLimb);
                Destroy(newLimb.gameObject);
                OnArmSwapped?.Invoke();

            }
            else if (_attackLeft.triggered == true)
            {
                SwapLimb(currentLeftArm, newLimb);
                Destroy(newLimb.gameObject);
                OnArmSwapped?.Invoke();
            }
        }
    }

    private void RotatePlayer(Vector3 towards)
    {
        Vector3 isoVector = _isoMatrix.MultiplyPoint3x4(towards);
        Quaternion newRotation = Quaternion.LookRotation(towards, Vector3.up);

        transform.rotation = smoothMovementEnabled ? Quaternion.RotateTowards(transform.rotation, newRotation, Time.deltaTime * _turnSpeed) : newRotation;
    }

    private void SetStartPosition()
    {
        SetPlayerPosition(FloorManager.Instance.StartTransform.position);
    }
    
    private void SetPlayerPosition(Vector3 to)
    {
        transform.position = to;

        // Lets the character controller know that the position was manually set by a transform
        // this gave me (Nick) two hours of headaches figuring this out
        Physics.SyncTransforms();
    }

    private void LeftAttack()
    {
        // Called by animation event to enable attack collider at specific point in anim timeline.
        currentLeftArm.Attack();
    }

    private void RightAttack()
    {
        // Called by animation event to enable attack collider at specific point in anim timeline.
        currentRightArm.Attack();
    }

    private void ActivateLegs()
    {
        // Called by animation event to enable attack collider at specific point in anim timeline.
        currentLegs.ActivateAbility();
    }

    // Called to switch left and right arm positions
    private void SwitchArms()
    {
        // Store all relevant data about current left arm in these variables, to use later
        Classification switchClass = currentLeftArm.Classification;
        Weight switchWeight = currentLeftArm.Weight;
        float switchAtkDmg = currentLeftArm.AttackDamage;
        float switchAtkSpd = currentLeftArm.AttackSpeed;
        float switchMxHP = currentLeftArm.MaxHealth;
        float switchHP = currentLeftArm.Health;

        // Search all arms in player prefab to find the left arm that matches the current right arm's weight and classification
        foreach (Arm arm in allArms)
        {
            if(arm.Weight == currentRightArm.Weight && arm.Classification == currentRightArm.Classification && arm.Side == SideOfPlayer.Left)
            {
                // If there is an arm already active and set as the currentLeftArm, deactivate it
                if (currentLeftArm != null)
                {
                    // Destroy its attack collider and set it active to false
                    currentLeftArm.Terminate();
                    currentLeftArm.gameObject.SetActive(false);
                }

                // Set the new arm ref found by the foreach loop to be the new current left arm, instantiate its attack collider,
                // load the stats of the current right arm into it, and set it to active
                arm.gameObject.SetActive(true);
                currentLeftArm = arm;
                currentLeftArm.Initialize(this);
                currentLeftArm.LoadStats(
                    currentRightArm.AttackDamage,
                    currentRightArm.AttackSpeed,
                    currentRightArm.MaxHealth,
                    currentRightArm.Health);
            }
        }

        // Search all arms in player prefab to find the right arm that matches the current left arm's weight and classification, using the values
        // stored in the variables declared above
        foreach (Arm arm in allArms)
        {
            if (arm.Weight == switchWeight && arm.Classification == switchClass && arm.Side == SideOfPlayer.Right)
            {
                // If there is an arm already active and set as the currentRightArm, deactivate it
                if (currentRightArm != null)
                {
                    // Destroy its attack collider and set it active to false
                    currentRightArm.Terminate();
                    currentRightArm.gameObject.SetActive(false);
                }

                // Set the new arm ref found by the foreach loop to be the new current right arm, instantiate its attack collider,
                // load the stats of the current left arm into it, and set it to active
                arm.gameObject.SetActive(true);
                currentRightArm = arm;
                currentRightArm.Initialize(this);
                currentRightArm.LoadStats(
                    switchAtkDmg,
                    switchAtkSpd,
                    switchMxHP,
                    switchHP);
            }
        }

        //currentBaseStatsSO.UpdateCurrentBuild(core, currentLeftArm, currentRightArm, currentLegs);
        //modifiedStatsSO.ResetValues();
        OnSwapLimbs.Invoke();
        //modifiedStatsSO.CalculateFinalValues();
        OnArmSwapped?.Invoke();
    }

    // Called to swap a current limb with a limb drop
    //private void SwapLimb(Head originalHead, LimbDrop newHead)
    private void SwapLimb(Legs originalLegs, LimbDrop newLegs)
    {
        foreach (Legs legs in allLegs)
        {
            if (legs.Weight == newLegs.Weight && legs.Classification == newLegs.Classification)
            {
                originalLegs.gameObject.SetActive(false);
                legs.gameObject.SetActive(true);
                currentLegs = legs;
                // add function here for overwriting current health of equipped legs to match the stored health of the pickup
            }
        }
        OnSwapLimbs.Invoke();
    }
    private void SwapLimb(Arm originalArm, LimbDrop newArm)
    {
        foreach (Arm arm in allArms)
        {
            if (arm.Weight == newArm.Weight && arm.Classification == newArm.Classification && arm.Side == originalArm.Side)
            {
                if (originalArm.Side == SideOfPlayer.Right)
                {
                    isRightWolfArm = true; // Refactor later to be have sounds play for all limbs
                    if (currentRightArm != null)
                    {
                        currentRightArm.Terminate();
                        currentRightArm.gameObject.SetActive(false);
                    }
                    arm.gameObject.SetActive(true);
                    currentRightArm = arm;
                    currentRightArm.Initialize(this);
                    currentRightArm.LoadDefaultStats();
                    currentRightArm.Health = newArm.LimbHealth;

                }
                else if (originalArm.Side == SideOfPlayer.Left)
                {
                    isLeftWolfArm = true; // Refactor later to be have sounds play for all limbs
                    if (currentLeftArm != null)
                    {
                        currentLeftArm.Terminate();
                        currentLeftArm.gameObject.SetActive(false);
                    }
                    arm.gameObject.SetActive(true);
                    currentLeftArm = arm;
                    currentLeftArm.Initialize(this);
                    currentLeftArm.LoadDefaultStats();
                    currentLeftArm.Health = newArm.LimbHealth;
                }
            }
        }
        OnSwapLimbs.Invoke();
    }

    // Called to instantiate a limb drop after swapping it
    private void DropLimb(Limb droppedLimb)
    {
        // call after swap limb to drop your current limb on the ground
    }

    // Called to load saved data into limbs after loading a new scene
    //private void LoadSavedLimb(Head savedHead)
    private void LoadSavedLimb(Arm savedArm)
    {
        foreach (Arm arm in allArms)
        {
            if (arm.Weight == savedArm.Weight && arm.Classification == savedArm.Classification && arm.Side == savedArm.Side)
            {
                if (savedArm.Side == SideOfPlayer.Right)
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
                        savedArm.AttackDamage,
                        savedArm.AttackSpeed,
                        savedArm.MaxHealth,
                        savedArm.Health);
                    //Debug.Log("savedArm.Health: " + savedArm.Health);
                    //Debug.Log("currentRightArm.Health: " + currentRightArm.Health);
                }
                else if (savedArm.Side == SideOfPlayer.Left)
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
                        savedArm.AttackDamage,
                        savedArm.AttackSpeed,
                        savedArm.MaxHealth,
                        savedArm.Health);
                    //Debug.Log("savedArm.Health: " + savedArm.Health);
                    //Debug.Log("currentLeftArm.Health: " + currentLeftArm.Health);
                }
            }
        } 
    }  
    private void LoadSavedLimb(Legs savedLegs)
    {
        foreach (Legs legs in allLegs)
        {
            if (legs.Weight == savedLegs.Weight && legs.Classification == savedLegs.Classification)
            {
                if(currentLegs != null)
                {
                    currentLegs.gameObject.SetActive(false);
                }
                legs.gameObject.SetActive(true);
                currentLegs = legs;
                currentLegs.LoadStats(savedLegs.MovementSpeed, savedLegs.CooldownTime, savedLegs.MaxHealth, savedLegs.Health);
            }
        }
    } 
    private void LoadSavedLimb(Core savedCore) 
    {
        core.LoadStats(savedCore.MaxHealth, savedCore.Health);  
    }

    // Called when a limb is disintegrated (no health)
    //public void RevertToDefault (Head currentHead)
    public void RevertToDefault(Arm currentArm)
    {
        if(currentArm.Side == SideOfPlayer.Right)
        {
            currentRightArm.Terminate();
            currentRightArm.LoadDefaultStats();
            currentRightArm.gameObject.SetActive(false);
            currentRightArm = coreRightArm;
            currentRightArm.gameObject.SetActive(true);
            currentRightArm.Initialize(this);
        }
        else
        {
            currentLeftArm.Terminate();
            currentLeftArm.LoadDefaultStats();
            currentLeftArm.gameObject.SetActive(false);
            currentLeftArm = coreLeftArm;
            currentLeftArm.gameObject.SetActive(true);
            currentLeftArm.Initialize(this);
        }
    }
    public void RevertToDefault(Legs currentLegs)
    {
        currentLegs.LoadDefaultStats();
        currentLegs.gameObject.SetActive(false);
        coreLegs.gameObject.SetActive(true);
        currentLegs = coreLegs;
    }

    // Called to update the stats of all limbs after modifying equipment (picking up trinkets or swapping limbs)
    public void UpdateStats()
    {
        // head here     
        core.LoadStats(
            modifiedStatsSO.coreMaxHealth.value, 
            modifiedStatsSO.coreHealth.value);
        currentLeftArm.LoadStats(
            modifiedStatsSO.leftArmAttackDamage.value,
            modifiedStatsSO.leftArmAttackSpeed.value,
            modifiedStatsSO.leftArmMaxHealth.value,
            modifiedStatsSO.leftArmHealth.value);
        currentRightArm.LoadStats(
            modifiedStatsSO.rightArmAttackDamage.value,
            modifiedStatsSO.rightArmAttackSpeed.value,
            modifiedStatsSO.rightArmMaxHealth.value,
            modifiedStatsSO.rightArmHealth.value);
        currentLegs.LoadStats(
            modifiedStatsSO.legsMovementSpeed.value,
            modifiedStatsSO.legsCooldown.value,
            modifiedStatsSO.legsMaxHealth.value,
            modifiedStatsSO.legsHealth.value);
    }

    // Called when player picks up a trinket or swaps a limb, sends stats to the base stats SO for calculation
    public void UpdateBaseStats()
    {
        currentBaseStatsSO.UpdateCurrentBuild(core, currentLeftArm, currentRightArm, currentLegs);
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
            if(limb.Health <= 0) 
            { 
                if(limb == core) { Die(); }
                else { limb.Disintegrate(); }
            }
        }
        //Debug.Log(core.Health);
        OnTakeDamage.Invoke();
        OnDamageReceived?.Invoke();
    }

    private void Die()
    {
        DisableAllDefaultControls();
        ChimeraSceneManager.Instance.LoadScene(0);
        //OnDie?.Invoke();
    }

    // This function is obsolete, delete later when other scripts refactor 
    public void UpdateCoreHealth(float amount)
    {
        core.Health = Mathf.Clamp(core.Health + amount, 0, 100);
    }

    // temporary function to update move speed called by trinket script
    // (_movementSpeed can't be edited from other scripts to my knowledge)
    public void MulesKick(float amount)
    {
        _movementSpeed += amount;
        Debug.Log(_movementSpeed.ToString());
    }

    private void Pause()
    {
        DisableAllDefaultControls();
        _unpause.Enable();
        _closeEM.Enable();
        OnGamePaused?.Invoke();
    }

    private void Unpause()
    {
        EnableAllDefaultControls();
        _unpause.Disable();
        _closeEM.Disable();
    }

    public void AddBones(float amount)
    {
        if(bonesMultiplier > 1)
        {
            totalBones += amount * bonesMultiplier;
        }
        else
        {
            totalBones += amount;
        }
        
        Debug.Log(totalBones.ToString("F2"));
    }

    private void Deactivate()
    {
        gameObject.SetActive(false);
    }

}
