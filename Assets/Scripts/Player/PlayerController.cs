using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController), typeof(PlayerInput))]
public class PlayerController : Singleton<PlayerController>
{
    private PlayerInputActions _playerInputActions;
    private PlayerInput _playerInput;
    private InputAction _movement;
    private InputAction _look; // for keyboard/mouse attack direction
    // Put new actions here
    private CharacterController _controller;

    private Camera _mainCamera;
    private Matrix4x4 _isoMatrix;

    private string previousScheme = "";
    private const string gamepadScheme = "Gamepad";
    private const string mouseScheme = "Keyboard&Mouse";

    [SerializeField] private float _movementSpeed = 10f;
    [SerializeField] private float _turnSpeed = 360f;
    [SerializeField] private bool smoothMovementEnabled;

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
    }

    // Disable new player input actions in this method
    private void OnDisable()
    {
        _movement.Disable();
        _look.Disable();
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
}
