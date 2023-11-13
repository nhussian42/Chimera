using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

[RequireComponent(typeof(PlayerInput))]
public class MainMenuController : Singleton<MainMenuController>
{
    private PlayerInputActions _playerInputActions;
    private PlayerInput _playerInput;
    private InputAction _select;
    private string previousScheme = "";
    private const string gamepadScheme = "Gamepad";
    private const string mouseScheme = "Keyboard&Mouse"; 

    [SerializeField] private GameObject _mainMenuStart;

     protected override void Init()
    {
        _playerInput = GetComponent<PlayerInput>();
        _playerInputActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        _playerInput.onControlsChanged += ChangeControlSchemes;    
        MainMenu.StartPressed += DisableUIControllerControls;  
    }
    private void OnDisable()
    {
        _playerInput.onControlsChanged -= ChangeControlSchemes;
        MainMenu.StartPressed -= DisableUIControllerControls;  
    }

    private void EnableUIControllerControls()
    {
        _select = _playerInputActions.UI.Select;
        _select.Enable();

        EventSystem.current.SetSelectedGameObject(_mainMenuStart);
    }

    private void DisableUIControllerControls()
    {
        _select.Disable();
    }

    private void ChangeControlSchemes(PlayerInput input)
    {
        if (_playerInput.currentControlScheme == mouseScheme && previousScheme != mouseScheme)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            previousScheme = mouseScheme;

            DisableUIControllerControls();
        }
        else if (_playerInput.currentControlScheme == gamepadScheme && previousScheme != gamepadScheme)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            previousScheme = gamepadScheme;

            EnableUIControllerControls();
        }
    }
}
