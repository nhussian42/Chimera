//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.5.1
//     from Assets/Scripts/Player/PlayerInputActions.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @PlayerInputActions: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerInputActions()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerInputActions"",
    ""maps"": [
        {
            ""name"": ""DefaultControls"",
            ""id"": ""a26918e3-1635-4aa8-b24b-915e854f1cd8"",
            ""actions"": [
                {
                    ""name"": ""Movement"",
                    ""type"": ""Value"",
                    ""id"": ""fc51dac5-950d-4fac-a0ac-1c9c4b7db391"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Look"",
                    ""type"": ""Value"",
                    ""id"": ""15b3c20a-221c-4db4-8b8a-b3096160d433"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""AttackRight"",
                    ""type"": ""Button"",
                    ""id"": ""2b7bc443-70d5-484a-985c-5af35bac1603"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""AttackLeft"",
                    ""type"": ""Button"",
                    ""id"": ""b108deae-f374-481c-8c3f-2bbdceeaa5c0"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Pause"",
                    ""type"": ""Button"",
                    ""id"": ""98becb5b-2ee2-40f6-8410-51878efa5807"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""SwapLimbs"",
                    ""type"": ""Button"",
                    ""id"": ""6c39b9e2-4458-49e0-ad53-f1cf46ad8300"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""LegsAbility"",
                    ""type"": ""Button"",
                    ""id"": ""3f7762f3-f9d0-4811-84c4-997b0ce963a9"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""WASD"",
                    ""id"": ""073930b6-8e41-4c40-a39f-da50e61fb775"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""5fc08958-68fa-40b3-99d3-17e4581d2ab9"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""7ba3fccd-f2bb-40ba-b60d-c7f5dfcb70ce"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""b7ca3731-a29d-4fb9-be15-2098fbc2d7e4"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""c91f2c40-d79d-4456-bf39-0269ce7a417b"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""9964a4b0-0f2a-4ce2-9fa7-7e87e52dc647"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d3abc430-dc65-4dfd-9a5d-f8951669b256"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""26689cd6-174b-4340-a08e-2b3e7ff1e5e3"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""AttackRight"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""94989a90-f05f-4f34-9775-d2b3cd59de1c"",
                    ""path"": ""<Gamepad>/rightTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""AttackRight"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5bc8bb69-991e-4447-806a-4035387191a5"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""AttackLeft"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""82f46499-4519-4dc0-874f-2c189bd91e7d"",
                    ""path"": ""<Gamepad>/leftTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""AttackLeft"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3e244011-b9d9-4723-a236-0d927ece8518"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Pause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""45cc22bf-91d9-40dd-ae49-3a3c741fa942"",
                    ""path"": ""<Gamepad>/select"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Pause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""dcf19eb8-ef87-4417-b301-855bb94f1dbe"",
                    ""path"": ""<Keyboard>/n"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SwapLimbs"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d4396f22-dab1-48fd-ad94-48951b091650"",
                    ""path"": ""<Keyboard>/leftShift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""LegsAbility"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4e14c917-4a79-468c-b047-7fa88225b7e4"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""LegsAbility"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""UI"",
            ""id"": ""3794e561-67af-4791-bf7c-15b270219935"",
            ""actions"": [
                {
                    ""name"": ""UnPause"",
                    ""type"": ""Button"",
                    ""id"": ""766ddcfd-2f22-40ff-8912-b1a64d25cc25"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""OpenEquipMenu"",
                    ""type"": ""Button"",
                    ""id"": ""b08b0f06-5429-4719-a6a6-4ed79a5ff45c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""71293b93-d7b0-443a-83e9-ba01e1c3947c"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""UnPause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c979f680-9230-41f2-9c0a-15f741e2407b"",
                    ""path"": ""<Gamepad>/select"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""UnPause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b4bd3823-a1aa-4eed-8741-afe2317b09a1"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""OpenEquipMenu"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d2e7f63d-9f75-435b-bc86-3c3e1100c942"",
                    ""path"": ""<Gamepad>/buttonNorth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""OpenEquipMenu"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Keyboard&Mouse"",
            ""bindingGroup"": ""Keyboard&Mouse"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""Gamepad"",
            ""bindingGroup"": ""Gamepad"",
            ""devices"": [
                {
                    ""devicePath"": ""<Gamepad>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // DefaultControls
        m_DefaultControls = asset.FindActionMap("DefaultControls", throwIfNotFound: true);
        m_DefaultControls_Movement = m_DefaultControls.FindAction("Movement", throwIfNotFound: true);
        m_DefaultControls_Look = m_DefaultControls.FindAction("Look", throwIfNotFound: true);
        m_DefaultControls_AttackRight = m_DefaultControls.FindAction("AttackRight", throwIfNotFound: true);
        m_DefaultControls_AttackLeft = m_DefaultControls.FindAction("AttackLeft", throwIfNotFound: true);
        m_DefaultControls_Pause = m_DefaultControls.FindAction("Pause", throwIfNotFound: true);
        m_DefaultControls_SwapLimbs = m_DefaultControls.FindAction("SwapLimbs", throwIfNotFound: true);
        m_DefaultControls_LegsAbility = m_DefaultControls.FindAction("LegsAbility", throwIfNotFound: true);
        // UI
        m_UI = asset.FindActionMap("UI", throwIfNotFound: true);
        m_UI_UnPause = m_UI.FindAction("UnPause", throwIfNotFound: true);
        m_UI_OpenEquipMenu = m_UI.FindAction("OpenEquipMenu", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }

    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // DefaultControls
    private readonly InputActionMap m_DefaultControls;
    private List<IDefaultControlsActions> m_DefaultControlsActionsCallbackInterfaces = new List<IDefaultControlsActions>();
    private readonly InputAction m_DefaultControls_Movement;
    private readonly InputAction m_DefaultControls_Look;
    private readonly InputAction m_DefaultControls_AttackRight;
    private readonly InputAction m_DefaultControls_AttackLeft;
    private readonly InputAction m_DefaultControls_Pause;
    private readonly InputAction m_DefaultControls_SwapLimbs;
    private readonly InputAction m_DefaultControls_LegsAbility;
    public struct DefaultControlsActions
    {
        private @PlayerInputActions m_Wrapper;
        public DefaultControlsActions(@PlayerInputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @Movement => m_Wrapper.m_DefaultControls_Movement;
        public InputAction @Look => m_Wrapper.m_DefaultControls_Look;
        public InputAction @AttackRight => m_Wrapper.m_DefaultControls_AttackRight;
        public InputAction @AttackLeft => m_Wrapper.m_DefaultControls_AttackLeft;
        public InputAction @Pause => m_Wrapper.m_DefaultControls_Pause;
        public InputAction @SwapLimbs => m_Wrapper.m_DefaultControls_SwapLimbs;
        public InputAction @LegsAbility => m_Wrapper.m_DefaultControls_LegsAbility;
        public InputActionMap Get() { return m_Wrapper.m_DefaultControls; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(DefaultControlsActions set) { return set.Get(); }
        public void AddCallbacks(IDefaultControlsActions instance)
        {
            if (instance == null || m_Wrapper.m_DefaultControlsActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_DefaultControlsActionsCallbackInterfaces.Add(instance);
            @Movement.started += instance.OnMovement;
            @Movement.performed += instance.OnMovement;
            @Movement.canceled += instance.OnMovement;
            @Look.started += instance.OnLook;
            @Look.performed += instance.OnLook;
            @Look.canceled += instance.OnLook;
            @AttackRight.started += instance.OnAttackRight;
            @AttackRight.performed += instance.OnAttackRight;
            @AttackRight.canceled += instance.OnAttackRight;
            @AttackLeft.started += instance.OnAttackLeft;
            @AttackLeft.performed += instance.OnAttackLeft;
            @AttackLeft.canceled += instance.OnAttackLeft;
            @Pause.started += instance.OnPause;
            @Pause.performed += instance.OnPause;
            @Pause.canceled += instance.OnPause;
            @SwapLimbs.started += instance.OnSwapLimbs;
            @SwapLimbs.performed += instance.OnSwapLimbs;
            @SwapLimbs.canceled += instance.OnSwapLimbs;
            @LegsAbility.started += instance.OnLegsAbility;
            @LegsAbility.performed += instance.OnLegsAbility;
            @LegsAbility.canceled += instance.OnLegsAbility;
        }

        private void UnregisterCallbacks(IDefaultControlsActions instance)
        {
            @Movement.started -= instance.OnMovement;
            @Movement.performed -= instance.OnMovement;
            @Movement.canceled -= instance.OnMovement;
            @Look.started -= instance.OnLook;
            @Look.performed -= instance.OnLook;
            @Look.canceled -= instance.OnLook;
            @AttackRight.started -= instance.OnAttackRight;
            @AttackRight.performed -= instance.OnAttackRight;
            @AttackRight.canceled -= instance.OnAttackRight;
            @AttackLeft.started -= instance.OnAttackLeft;
            @AttackLeft.performed -= instance.OnAttackLeft;
            @AttackLeft.canceled -= instance.OnAttackLeft;
            @Pause.started -= instance.OnPause;
            @Pause.performed -= instance.OnPause;
            @Pause.canceled -= instance.OnPause;
            @SwapLimbs.started -= instance.OnSwapLimbs;
            @SwapLimbs.performed -= instance.OnSwapLimbs;
            @SwapLimbs.canceled -= instance.OnSwapLimbs;
            @LegsAbility.started -= instance.OnLegsAbility;
            @LegsAbility.performed -= instance.OnLegsAbility;
            @LegsAbility.canceled -= instance.OnLegsAbility;
        }

        public void RemoveCallbacks(IDefaultControlsActions instance)
        {
            if (m_Wrapper.m_DefaultControlsActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IDefaultControlsActions instance)
        {
            foreach (var item in m_Wrapper.m_DefaultControlsActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_DefaultControlsActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public DefaultControlsActions @DefaultControls => new DefaultControlsActions(this);

    // UI
    private readonly InputActionMap m_UI;
    private List<IUIActions> m_UIActionsCallbackInterfaces = new List<IUIActions>();
    private readonly InputAction m_UI_UnPause;
    private readonly InputAction m_UI_OpenEquipMenu;
    public struct UIActions
    {
        private @PlayerInputActions m_Wrapper;
        public UIActions(@PlayerInputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @UnPause => m_Wrapper.m_UI_UnPause;
        public InputAction @OpenEquipMenu => m_Wrapper.m_UI_OpenEquipMenu;
        public InputActionMap Get() { return m_Wrapper.m_UI; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(UIActions set) { return set.Get(); }
        public void AddCallbacks(IUIActions instance)
        {
            if (instance == null || m_Wrapper.m_UIActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_UIActionsCallbackInterfaces.Add(instance);
            @UnPause.started += instance.OnUnPause;
            @UnPause.performed += instance.OnUnPause;
            @UnPause.canceled += instance.OnUnPause;
            @OpenEquipMenu.started += instance.OnOpenEquipMenu;
            @OpenEquipMenu.performed += instance.OnOpenEquipMenu;
            @OpenEquipMenu.canceled += instance.OnOpenEquipMenu;
        }

        private void UnregisterCallbacks(IUIActions instance)
        {
            @UnPause.started -= instance.OnUnPause;
            @UnPause.performed -= instance.OnUnPause;
            @UnPause.canceled -= instance.OnUnPause;
            @OpenEquipMenu.started -= instance.OnOpenEquipMenu;
            @OpenEquipMenu.performed -= instance.OnOpenEquipMenu;
            @OpenEquipMenu.canceled -= instance.OnOpenEquipMenu;
        }

        public void RemoveCallbacks(IUIActions instance)
        {
            if (m_Wrapper.m_UIActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IUIActions instance)
        {
            foreach (var item in m_Wrapper.m_UIActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_UIActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public UIActions @UI => new UIActions(this);
    private int m_KeyboardMouseSchemeIndex = -1;
    public InputControlScheme KeyboardMouseScheme
    {
        get
        {
            if (m_KeyboardMouseSchemeIndex == -1) m_KeyboardMouseSchemeIndex = asset.FindControlSchemeIndex("Keyboard&Mouse");
            return asset.controlSchemes[m_KeyboardMouseSchemeIndex];
        }
    }
    private int m_GamepadSchemeIndex = -1;
    public InputControlScheme GamepadScheme
    {
        get
        {
            if (m_GamepadSchemeIndex == -1) m_GamepadSchemeIndex = asset.FindControlSchemeIndex("Gamepad");
            return asset.controlSchemes[m_GamepadSchemeIndex];
        }
    }
    public interface IDefaultControlsActions
    {
        void OnMovement(InputAction.CallbackContext context);
        void OnLook(InputAction.CallbackContext context);
        void OnAttackRight(InputAction.CallbackContext context);
        void OnAttackLeft(InputAction.CallbackContext context);
        void OnPause(InputAction.CallbackContext context);
        void OnSwapLimbs(InputAction.CallbackContext context);
        void OnLegsAbility(InputAction.CallbackContext context);
    }
    public interface IUIActions
    {
        void OnUnPause(InputAction.CallbackContext context);
        void OnOpenEquipMenu(InputAction.CallbackContext context);
    }
}
