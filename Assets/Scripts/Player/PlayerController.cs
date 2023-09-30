using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Experimental.AI;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : Singleton<PlayerController>
{
    private PlayerInputActions _playerInputActions;
    private InputAction _movement;
    private InputAction _look;
    // Put new actions here
    private CharacterController _controller;

    private Transform cameraTransform;
    Matrix4x4 _isoMatrix;

    [SerializeField] private float _movementSpeed = 10f;
    [SerializeField] private float _turnSpeed = 360f;

    private void Awake()
    {
        _playerInputActions = new PlayerInputActions();

        _controller = GetComponent<CharacterController>();
    }

    private void OnEnable()
    {
        //InputSystem.onActionChange += InputActionChangeCallback;

        _movement = _playerInputActions.DefaultControls.Movement;
        _movement.Enable();
        
        _look = _playerInputActions.DefaultControls.Look;
        _look.Enable();
    }

    private void Start()
    {
        cameraTransform = Camera.main.transform;
        _isoMatrix = Matrix4x4.Rotate(Quaternion.Euler(0, 45, 0));
    }

    private void OnDisable()
    {
        _movement.Disable();
        _look.Disable();
    }

    // Detects change in input devices to determine if controls have changed.
    private void InputActionChangeCallback(object obj, InputActionChange change)
    {
        if (!typeof(InputAction).IsAssignableFrom(obj.GetType())) return;
        InputAction receivedInputAction = (InputAction)obj;
        if (receivedInputAction.activeControl == null) return;
        InputDevice lastDevice = receivedInputAction.activeControl.device;
  
        if (lastDevice.name.Equals("Mouse"))
            _look.Enable();
        else
            _look.Disable();
    }

    private void Update()
    {
        Vector2 movementValues = _movement.ReadValue<Vector2>();
        Vector3 movementDir = movementValues.y * cameraTransform.forward + movementValues.x * cameraTransform.right;
        Vector3 movementVector = new Vector3(movementDir.x, 0, movementDir.z).normalized;

        _controller.Move(movementVector * Time.deltaTime * _movementSpeed);
        
        if (movementVector != Vector3.zero)
            RotatePlayer(movementVector);
    }

    private void RotatePlayer(Vector3 towards)
    {
        Vector3 isoVector = _isoMatrix.MultiplyPoint3x4(towards);
        Quaternion newRotation = Quaternion.LookRotation(towards, Vector3.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, newRotation, Time.deltaTime * _turnSpeed);
    }
}
