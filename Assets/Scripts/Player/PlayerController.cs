using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using FMOD.Studio;

[RequireComponent(typeof(CharacterController), typeof(PlayerInput))]
public class PlayerController : Singleton<PlayerController>
{
    private PlayerInputActions _playerInputActions;
    private PlayerInput _playerInput;
    public InputAction _movement { get; private set; }
    private InputAction _look; // for keyboard/mouse attack direction
    public InputAction _attackRight { get; private set; } // get, set these controls so other scripts can control when player shouldn't be able to do stuff
    public InputAction _attackLeft { get; private set; }
    public InputAction _legsAbility { get; private set; }
    public InputAction _swapLimbs { get; private set; }
    public InputAction _interact { get; private set; }
    private InputAction _pause;
    private InputAction _unpause;
    private InputAction _openEM;
    private InputAction _closeEM;
    private InputAction _select;
    private InputAction _switchToLeftArm;
    private InputAction _switchToRightArm;
    // Put new actions here
    public CharacterController _controller;

    private Matrix4x4 _isoMatrix;

    private string previousScheme = "";
    private const string gamepadScheme = "Gamepad";
    private const string mouseScheme = "Keyboard&Mouse"; 

    [SerializeField] private GameObject EquipMenu;

    private bool menuToggle;

    [HideInInspector] public LimbSwapMenu limbSwapMenu;


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
    public List<Head> allHeads;
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
    [SerializeField] Head coreHead;

    public Arm currentLeftArm {get; private set;}
    public Arm currentRightArm {get; private set;}
    public Legs currentLegs { get; private set; }
    public Head currentHead { get; private set;}

    [SerializeField] Transform attackRangeLeftOrigin;
    [SerializeField] Transform attackRangeRightOrigin;

    [SerializeField] Animator animator;
    [field: SerializeField] public float LimbDissolveDuration { get; private set; }
    public Animator Animator { get { return animator; } }
    public Transform AttackRangeLeftOrigin { get { return attackRangeLeftOrigin; } }
    public Transform AttackRangeRightOrigin { get { return attackRangeRightOrigin; } }

    private EventInstance footsteps;

    public static Action OnDamageReceived;
    public static Action OnArmSwapped;
    public static Action OnGamePaused;
    public static Action ToggleMenuPause;
    public static Action OnDie;

    // Fixing the floating bug
    float startingYPos;
    bool firstMove;

    // for when player should not be able to be damaged - Amon
    public bool isInvincible { get; private set; } = false;   

    private Vector2 movementValues;
    public Vector3 movementDir { get; private set; }
    private Vector3 movementVector;

    public float totalBones; // delete these and hook up bones to new currency manager - Amon
    public float bonesMultiplier;

    private bool canAttack = true;
    private bool interacting;
    private List<Drop> touchedDrops;
    private Drop nearestDrop;

    protected override void Init()
    {
        _playerInput = GetComponent<PlayerInput>();
        _playerInputActions = new PlayerInputActions();
        _controller = GetComponent<CharacterController>();
        touchedDrops = new List<Drop>();
        
    }

    // Enable new player input actions in this method
    private void OnEnable()
    {
        _playerInput.onControlsChanged += ChangeControlSchemes;
        GameManager.OnUnpause += Unpause;
        FloorManager.LoadNextRoom += Deactivate;
        FloorManager.NextRoomLoaded += SetStartPosition;
        FloorManager.NextRoomLoaded += EnableAllDefaultControls;
        FloorManager.LeaveRoom += DisableAllDefaultControls;
        AttackRange.AttackEnded += EnableAllDefaultControls;
        AttackRange.AttackEnded += () => _movementSpeed = currentLegs.MovementSpeed;
        // FloorManager.EnableFloor += EnableAllDefaultControls;

        // Assign default controls
        _movement = _playerInputActions.DefaultControls.Movement;
        _look = _playerInputActions.DefaultControls.Look;
        _attackRight = _playerInputActions.DefaultControls.AttackRight;
        _attackLeft = _playerInputActions.DefaultControls.AttackLeft;
        _legsAbility = _playerInputActions.DefaultControls.LegsAbility;
        _swapLimbs = _playerInputActions.DefaultControls.SwapLimbs;
        _interact = _playerInputActions.DefaultControls.Interact;
        _pause = _playerInputActions.DefaultControls.Pause;
        _openEM = _playerInputActions.DefaultControls.OpenEM;

        // Assign UI controls
        _unpause = _playerInputActions.UI.UnPause;
        _closeEM = _playerInputActions.UI.CloseEM;
        _select = _playerInputActions.UI.Select;
        _switchToLeftArm = _playerInputActions.UI.SwitchToLeftArm;
        _switchToRightArm = _playerInputActions.UI.SwitchToRightArm;
    }

    // Disable new player input actions in this method
    private void OnDisable()
    {
        _playerInput.onControlsChanged -= ChangeControlSchemes;
        GameManager.OnUnpause -= Unpause;
        FloorManager.LoadNextRoom -= Deactivate;
        FloorManager.NextRoomLoaded -= SetStartPosition;
        FloorManager.NextRoomLoaded -= EnableAllDefaultControls;
        FloorManager.LeaveRoom -= DisableAllDefaultControls;
        AttackRange.AttackEnded -= EnableAllDefaultControls;
        AttackRange.AttackEnded -= () => _movementSpeed = currentLegs.MovementSpeed;
        // FloorManager.EnableFloor -= EnableAllDefaultControls;

        DisableAllDefaultControls();
        DisableAllUIControls();
        canAttack = false;
    }

    private void OnDestroy()
    {
        // Called when the player exits the room (loading a new scene destroys all current scene objects)
        if (core.Health == 0) { saveManager.Reset(); }
        else { saveManager.SaveLimbData(currentHead, currentLeftArm, currentRightArm, core, currentLegs); }
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy"), false);

    }

    public void EnableAllDefaultControls()
    {
        if (GameManager.CurrentGameState != GameState.IsPlaying) return;

        _movement.Enable();
        _look.Enable();
        _attackRight.Enable();
        _attackLeft.Enable();
        _legsAbility.Enable();
        _swapLimbs.Enable();
        _interact.Enable();
        _pause.Enable();
        _openEM.Enable();
    }

    public void DisableAllDefaultControls()
    {
        _movement.Disable();
        _look.Disable();
        _attackRight.Disable();
        _attackLeft.Disable();
        _legsAbility.Disable();
        _swapLimbs.Disable();
        _interact.Disable();
        _pause.Disable();
    }

    public void DisableAllUIControls()
    {
        _unpause.Disable();
        _closeEM.Disable();
        _select.Disable();
        _switchToLeftArm.Disable();
        _switchToRightArm.Disable();
    }

    public void EnableAllUIControls()
    {
        _select.Enable();
        _switchToLeftArm.Enable();
        _switchToRightArm.Enable();
    }

    public void DisableAttackControls()
    {
        //_movement.Disable();
        _look.Disable();
        _attackRight.Disable();
        _attackLeft.Disable();
        _legsAbility.Disable();
        _swapLimbs.Disable();
        _interact.Disable();
        // _pause.Disable();
        // _openEM.Disable();
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
        foreach (Head head in allHeads)
        {
            if (saveManager.firstLoad == true) { head.LoadDefaultStats(); }
            head.gameObject.SetActive(false);
        }

        if (saveManager.firstLoad == false)
        {
            // If not first load into scene, set limbs saved in SaveManager
            LoadSavedLimb(saveManager.SavedHead);
            LoadSavedLimb(saveManager.SavedLeftArm);
            LoadSavedLimb(saveManager.SavedRightArm);
            LoadSavedLimb(saveManager.SavedCore);
            LoadSavedLimb(saveManager.SavedLegs);
            Debug.Log("loaded saved limbs");

        }
        else
        {
            // If first load into scene, set default limbs
            coreHead.gameObject.SetActive(true);
            coreLeftArm.gameObject.SetActive(true);
            coreRightArm.gameObject.SetActive(true);
            coreLegs.gameObject.SetActive(true);
            currentHead = coreHead;
            currentLegs = coreLegs;
            currentLeftArm = coreLeftArm;
            currentRightArm = coreRightArm;
            currentLeftArm.Initialize(this);
            animator.SetFloat("LArmAtkSpeed", currentLeftArm.AttackSpeed);
            currentRightArm.Initialize(this);
            animator.SetFloat("RArmAtkSpeed", currentRightArm.AttackSpeed);
            
            core.LoadDefaultStats();
        }
        //Debug.Log(currentHead);

        canAttack = true;
        ResetAttackTriggers();

        // read default build to base rstats SO on first load
        //currentBaseStatsSO.UpdateCurrentBuild(core, currentLeftArm, currentRightArm, currentLegs);
        OnSwapLimbs.Invoke();

        #endregion
        _movementSpeed = currentLegs.MovementSpeed;

        // _mainCamera = Camera.main;

        _isoMatrix = Matrix4x4.Rotate(Quaternion.Euler(0, 45, 0));
        footsteps = AudioManager.Instance.CreateEventInstance(AudioEvents.Instance.OnPlayerWalk);
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
        Vector3 movementVector = new Vector3(movementDir.x, 0, movementDir.z);
        _controller.Move(movementVector * Time.deltaTime * _movementSpeed);

        animator.SetFloat("Speed", movementValues.magnitude * _movementSpeed / 10f);
        //PlayFootstepAudio(movementVector.magnitude);
        
        //if(transform.position.y > 1.5f)
        //{
        //    SetPlayerPosition(new Vector3(transform.position.x, 0, transform.position.z));
        //    Debug.Log("Artifical Gravity activated");
        //}
        
        if (movementVector != Vector3.zero)
            RotatePlayer(movementVector); 

        // Reads L and R mouse buttons 
        if (_attackRight.triggered == true && canAttack)
        {
            canAttack = false;
            //currentRightArm.PauseInput();
            
            DetermineAttackAnimation(currentRightArm, SideOfPlayer.Right);

            animator.SetTrigger("BaseAttack");
            animator.SetBool("LeftSide", false);
        }

        if (_attackLeft.triggered && canAttack)
        {
            canAttack = false;
            //currentRightArm.PauseInput();

            DetermineAttackAnimation(currentLeftArm, SideOfPlayer.Left);

            OnAttack?.Invoke();
        }

        if(_legsAbility.triggered == true && currentLegs.CanActivate == true)
        {
            OnDash.Invoke();

            // Plays the animation of the current legs which will call ActivateAbility() with an animation event
            currentLegs.PlayAnim();
            
        }

        if (_swapLimbs.triggered == true)
            SwitchArms();

        if (_pause.triggered == true)
            Pause();

        if (_switchToLeftArm.triggered == true && limbSwapMenu.proposedLimbType == LimbType.Arm)
            limbSwapMenu.SetToLeftArm();
        if (_switchToRightArm.triggered == true && limbSwapMenu.proposedLimbType == LimbType.Arm)
            limbSwapMenu.SetToRightArm();

        if (_openEM.triggered == true)
        {
            EquipMenu.gameObject.SetActive(!EquipMenu.gameObject.activeSelf);
            menuToggle = !menuToggle;
            EMScript.Instance.ListTrinkets();

            if (menuToggle)
            {
                DisableAllDefaultControls();
                EnableAllUIControls();
            }

            if (menuToggle == false)
            {
                EnableAllDefaultControls();
                DisableAllUIControls();
            }         
        }

        if (_interact.triggered)
        {
            if (nearestDrop != null)
            {
                if (nearestDrop is LimbDrop)
                    EnableLimbSwapMenu((LimbDrop)nearestDrop);
                if (nearestDrop is TrinketBagDrop)
                    EnableTrinketMenu();
                
                Drop dropToRemove = nearestDrop;
                RemoveFromDrops(nearestDrop);
                dropToRemove.DestroyDrop();
            }
        }

        interacting = _interact.triggered;
    }
    private void LateUpdate()
    {
        // FIXES FLOATING BUG (We hope) - Amon
        if (transform.position.y >= startingYPos + 0.01 || transform.position.y <= startingYPos - 0.01)
        {
            SetPlayerPosition(new Vector3(transform.position.x, startingYPos, transform.position.z));
            // Debug.Log("yPos = " + transform.position.y);
        }
    }

    private void DetermineAttackAnimation(Arm arm, SideOfPlayer side)
    {
        animator.SetBool("LeftSide", side == SideOfPlayer.Left);

        switch (arm.Weight)
        {
            case Weight.Core:
            _movementSpeed *= 0.5f;
                animator.SetTrigger("BaseAttack");
                break;
            case Weight.Heavy: // Covers croc + rhino, but not shark (may need to be a ground slam instead???)
                DisableAttackControls();
                _movementSpeed *= 0.1f;
                animator.SetTrigger("HeavyAttack");
                break;
            case Weight.Medium:
                // 11/8 Notes - Nick
                // Potentially all the medium creatures can give "line" attacks with the push and pull functionality, we might need to rework how these attacks are
                // If we had specific attacks for each creature it would cause us to have to rethink the rig
                break;
            case Weight.Light:
                DisableAttackControls();
                _movementSpeed *= 0.3f;
                animator.SetTrigger("LightAttack");
                break;
        }
    }

    public void ResetAttackTriggers()
    {
        animator.ResetTrigger("BaseAttack");
        animator.ResetTrigger("HeavyAttack");
        animator.ResetTrigger("LightAttack");
    }

    public void AddToDrops(Drop drop) { touchedDrops.Add(drop); SelectNearestDrop(); }

    public void RemoveFromDrops (Drop drop) { touchedDrops.Remove(drop); drop.DisablePickupIndicator(); SelectNearestDrop(); }

    private void SelectNearestDrop()
    {
        if (touchedDrops.Count <= 0) return;

        float closestDistance = Mathf.Infinity;
        Drop closestDrop = touchedDrops[0];

        foreach (Drop drop in touchedDrops)
        {
            drop.DisablePickupIndicator();

            float distanceFromDrop = Vector3.Distance(transform.position, drop.transform.position);
            if (distanceFromDrop < closestDistance)
                closestDrop = drop;                
        }

        closestDrop.EnablePickupIndicator();
        nearestDrop = closestDrop;
    }

    private void EnableLimbSwapMenu(LimbDrop drop)
    {
        limbSwapMenu.Enable(drop);
    }

    private void EnableTrinketMenu()
    {
        NewTrinketManager.Instance.gameObject.SetActive(true);
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
        startingYPos = transform.position.y;
        // Debug.Log("startingYPos = " + startingYPos);
    }
    
    private void SetPlayerPosition(Vector3 to)
    {
        transform.position = to;
        Physics.SyncTransforms();
    }

    private void LeftAttack()
    {
        // Called by animation event to enable attack collider at specific point in anim timeline.
        currentLeftArm.Attack();
        ResetAttackTriggers();
        canAttack = true;
    }

    private void RightAttack()
    {
        // Called by animation event to enable attack collider at specific point in anim timeline.
        currentRightArm.Attack();
        ResetAttackTriggers();
        canAttack = true;
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

                animator.SetFloat("LArmAtkSpeed", currentLeftArm.AttackSpeed);
                animator.SetFloat("RArmAtkSpeed", currentRightArm.AttackSpeed);
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

                animator.SetFloat("LArmAtkSpeed", currentLeftArm.AttackSpeed);
                animator.SetFloat("RArmAtkSpeed", currentRightArm.AttackSpeed);
                
            }
        }

        //currentBaseStatsSO.UpdateCurrentBuild(core, currentLeftArm, currentRightArm, currentLegs);
        //modifiedStatsSO.ResetValues();
        OnSwapLimbs.Invoke();
        //modifiedStatsSO.CalculateFinalValues();
        OnArmSwapped?.Invoke();
    }

    // Called to swap a current limb with a limb drop
    public void SwapLimb(Head originalHead, LimbDrop newHead)
    {
        if(newHead.LimbType == LimbType.Head)
        foreach (Head head in allHeads)
        {
                if (head.Weight == newHead.Weight && head.Classification == newHead.Classification)
                {
                    originalHead.gameObject.SetActive(false);
                    head.gameObject.SetActive(true);
                    head.LoadDefaultStats();
                    currentHead = head;
                    if (newHead.LimbHealth <= 0) { newHead.OverwriteLimbHealth(currentHead.DefaultMaxHealth); }
                    currentHead.Health = newHead.LimbHealth;
                }
                Debug.Log(currentHead);
        }
        OnSwapLimbs.Invoke();
    }
    public void SwapLimb(Legs originalLegs, LimbDrop newLegs)
    {
        if(newLegs.LimbType == LimbType.Legs)
        foreach (Legs legs in allLegs)
        {
            if (legs.Weight == newLegs.Weight && legs.Classification == newLegs.Classification)
            {
                originalLegs.gameObject.SetActive(false);
                legs.gameObject.SetActive(true);
                legs.LoadDefaultStats();
                currentLegs = legs;
                _movementSpeed = currentLegs.MovementSpeed;
                if (newLegs.LimbHealth <= 0) { newLegs.OverwriteLimbHealth(currentLegs.DefaultMaxHealth); }
                currentLegs.Health = newLegs.LimbHealth;    
            }
        }
        OnSwapLimbs.Invoke();
    }
    public void SwapLimb(Arm originalArm, LimbDrop newArm)
    {
        if(newArm.LimbType == LimbType.Arm)
        foreach (Arm arm in allArms)
        {
            if (arm.Weight == newArm.Weight && arm.Classification == newArm.Classification && arm.Side == originalArm.Side)
            {
                if (originalArm.Side == SideOfPlayer.Right)
                {
                    if (currentRightArm != null)
                    {
                        currentRightArm.Terminate();
                        currentRightArm.gameObject.SetActive(false);
                    }
                    arm.gameObject.SetActive(true);
                    currentRightArm = arm;
                    currentRightArm.Initialize(this);
                    currentRightArm.LoadDefaultStats();
                    animator.SetFloat("RArmAtkSpeed", currentRightArm.AttackSpeed);
                    if (newArm.LimbHealth <= 0) { newArm.OverwriteLimbHealth(currentRightArm.DefaultMaxHealth); }
                    currentRightArm.Health = newArm.LimbHealth;

                }
                else if (originalArm.Side == SideOfPlayer.Left)
                {
                    if (currentLeftArm != null)
                    {
                        currentLeftArm.Terminate();
                        currentLeftArm.gameObject.SetActive(false);
                    }
                    arm.gameObject.SetActive(true);
                    currentLeftArm = arm;
                    currentLeftArm.Initialize(this);
                    currentLeftArm.LoadDefaultStats();
                    animator.SetFloat("LArmAtkSpeed", currentLeftArm.AttackSpeed);
                    if (newArm.LimbHealth <= 0) { newArm.OverwriteLimbHealth(currentLeftArm.DefaultMaxHealth); }
                    currentLeftArm.Health = newArm.LimbHealth;
                }
            }
        }
        OnSwapLimbs.Invoke();
        OnArmSwapped?.Invoke();
    }
    

    // Called to instantiate a limb drop after swapping it
    private void DropLimb(Limb droppedLimb)
    {
        // call after swap limb to drop your current limb on the ground
    }

    // Called to load saved data into limbs after loading a new scene
    private void LoadSavedLimb(Head savedHead)
    {
        foreach (Head head in allHeads)
        {
            if (head.Weight == savedHead.Weight && head.Classification == savedHead.Classification)
            {
                if (currentHead != null)
                {
                    currentHead.gameObject.SetActive(false);
                }
                Debug.Log("currentHead: " + currentHead.StringName);
                Debug.Log("savedHead: " + savedHead.StringName);
                head.gameObject.SetActive(true);
                currentHead = head;
                currentHead.LoadStats(savedHead.MaxHealth, savedHead.Health);
            }
        }
    }
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
                    animator.SetFloat("RArmAtkSpeed", currentRightArm.AttackSpeed);
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
                    animator.SetFloat("LArmAtkSpeed", currentLeftArm.AttackSpeed);
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
    public void RevertToDefault (Head previousHead)
    {
        previousHead.LoadDefaultStats();
        // previousHead.gameObject.SetActive(false); // dissolve script handles it
        coreHead.gameObject.SetActive(true);
        currentHead = coreHead;
    }
    public void RevertToDefault(Arm previousArm)
    {
        // Arm currentArm = previousArm.Side == SideOfPlayer.Right ? currentRightArm : currentLeftArm;

        // currentArm.Terminate();
        // currentArm.LoadDefaultStats();
        
        // currentRightArm = coreRightArm;
        // currentLeftArm = coreLeftArm;

        if(previousArm.Side == SideOfPlayer.Right)
        {
            currentRightArm.Terminate();
            currentRightArm.LoadDefaultStats();
            
            // currentRightArm.gameObject.SetActive(false); // dissolve script handles it
            currentRightArm = coreRightArm;
            currentRightArm.gameObject.SetActive(true);
            currentRightArm.Initialize(this);
            animator.SetFloat("RArmAtkSpeed", currentRightArm.AttackSpeed);
        }
        else
        {
            currentLeftArm.Terminate();
            currentLeftArm.LoadDefaultStats();
            // currentLeftArm.gameObject.SetActive(false); // dissolve script handles it
            currentLeftArm = coreLeftArm;
            currentLeftArm.gameObject.SetActive(true);
            currentLeftArm.Initialize(this);
            animator.SetFloat("LArmAtkSpeed", currentLeftArm.AttackSpeed);
        }
    }
    public void RevertToDefault(Legs previousLegs)
    {
        previousLegs.LoadDefaultStats();
        // previousLegs.gameObject.SetActive(false); // dissolve script handles it
        currentLegs = coreLegs;
        _movementSpeed = currentLegs.MovementSpeed;
        coreLegs.gameObject.SetActive(true);        
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

    // Called when player needs to be invincible
    public void ToggleInvincibility()
    {
        if(isInvincible == false)
        {
            //Debug.Log("invincible");
            isInvincible = true;
            _attackLeft.Disable();
            _attackRight.Disable();
            _legsAbility.Disable();
            _swapLimbs.Disable();
            _interact.Disable();
            Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy"), true);            
        }
        else
        {
            //Debug.Log("not invincible");
            isInvincible = false;
            _attackLeft.Enable();
            _attackRight.Enable();
            _legsAbility.Enable();
            _swapLimbs.Enable();
            _interact.Enable();
            Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy"), false);
        }
    }

    public void DistributeDamage(float damage)
    {
        if (isInvincible == false)
        {
            animator.SetTrigger("TakeDamage");
            DetermineDamageEffects(damage);

            List<Limb> damagedLimbs = new List<Limb>();
            damagedLimbs.Add(core);

            if (currentLeftArm != coreLeftArm)
                damagedLimbs.Add(currentLeftArm);
            if (currentRightArm != coreRightArm)
                damagedLimbs.Add(currentRightArm);
            if (currentLegs != coreLegs)
                damagedLimbs.Add(currentLegs);
            if (currentHead != coreHead)
                damagedLimbs.Add(currentHead);

            foreach (Limb limb in damagedLimbs)
            {
                float caclulatedDamage = -1 * (damage / (damagedLimbs.Count + 1));
                limb.UpdateHealth(caclulatedDamage);
                if (limb.Health <= 0)
                {
                    if (limb == core) { Die(); }
                    else { limb.Disintegrate(); }
                }
            }
            //Debug.Log(core.Health);
            OnTakeDamage.Invoke();
            OnDamageReceived?.Invoke();
        }
    }

    private void DetermineDamageEffects(float damage)
    {
        if (damage <= core.MaxHealth / 3f)
        {
            AudioManager.PlaySound2D(AudioEvents.Instance.OnPlayerDamagedSmall);
            // CameraShake.Instance.TakeLightDmgShake();
        }
        else
        {
            AudioManager.PlaySound2D(AudioEvents.Instance.OnPlayerDamagedLarge);
            // CameraShake.Instance.TakeHeavyDmgShake();
        }
    }

    private void Die()
    {
        AudioManager.PlaySound2D(AudioEvents.Instance.OnPlayerDeath);
        OnDie?.Invoke();
        DisableAllDefaultControls();
        ToggleInvincibility();
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

    public void Pause()
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

    // Refactored into CurrencyManager
    public void AddBones(float amount)
    {
        if (bonesMultiplier > 1)
        {
            totalBones += amount * bonesMultiplier;
        }
        else
        {
            totalBones += amount;
        }
    }

    private void Deactivate()
    {
        gameObject.SetActive(false);
    }

    private void PlayFootstepAudio()
    {
        // if (movementSpeed > 0)
        // {
        //     PLAYBACK_STATE playbackState;
        //     footsteps.getPlaybackState(out playbackState);
        //     if (playbackState.Equals(PLAYBACK_STATE.STOPPED))
        //     {
        //         footsteps.start();
        //     }
        // }
        // else
        // {
        //     footsteps.stop(STOP_MODE.ALLOWFADEOUT);
        // }

        AudioManager.PlaySound2D(AudioEvents.Instance.OnPlayerWalk);
    }
}
