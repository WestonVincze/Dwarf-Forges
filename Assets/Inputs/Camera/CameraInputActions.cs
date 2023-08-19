//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.5.1
//     from Assets/Inputs/Camera/CameraInputActions.inputactions
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

public partial class @CameraInputActions: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @CameraInputActions()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""CameraInputActions"",
    ""maps"": [
        {
            ""name"": ""CameraControls"",
            ""id"": ""d02cb783-5845-4654-9d51-aab6bd8fdcfe"",
            ""actions"": [
                {
                    ""name"": ""Pan"",
                    ""type"": ""PassThrough"",
                    ""id"": ""ef5a00f2-6d1f-47d3-9ece-cb034634ba1f"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""FastPan"",
                    ""type"": ""Button"",
                    ""id"": ""73e6177e-b763-4393-a768-0b5b9743c4fd"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Zoom"",
                    ""type"": ""Value"",
                    ""id"": ""465ee1da-6b05-4e52-88f6-8a3835e9f3ba"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": ""Invert"",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Rotate"",
                    ""type"": ""Value"",
                    ""id"": ""6a557eee-e5f5-4c53-a43d-5414995156b1"",
                    ""expectedControlType"": ""Delta"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""ToggleCameraLock"",
                    ""type"": ""Button"",
                    ""id"": ""98790847-03fd-4163-a864-4b9d0e5f6d16"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""UnlockCamera"",
                    ""type"": ""Button"",
                    ""id"": ""8f5348de-a1b4-43ee-8371-b506a7cf7d97"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Hold"",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""abafc5a4-68ae-41b1-af26-ac96009e7699"",
                    ""path"": ""<Mouse>/scroll/y"",
                    ""interactions"": """",
                    ""processors"": ""Clamp(min=-10,max=10)"",
                    ""groups"": """",
                    ""action"": ""Zoom"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""5e51d2a2-85c4-450f-bcb0-114c7f71786e"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Pan"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""c9102520-da20-4570-b0df-a5a3fcd7a11d"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Pan"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""65f5c40e-9d90-4716-a6d2-420cd1025227"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Pan"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""af8dbe57-f8a1-4d2e-953d-3dc3c80d869a"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Pan"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""85bf751b-579b-44b8-8d0a-de448487a0c7"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Pan"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""38abe179-46c4-4c8a-9589-62f07520213c"",
                    ""path"": ""<Keyboard>/shift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""FastPan"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""X"",
                    ""id"": ""ee7235ae-1744-4fb1-9828-054bfbddaa42"",
                    ""path"": ""OneModifier"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Rotate"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""modifier"",
                    ""id"": ""59e5b12e-d91f-4171-8409-f8ad5486e8d6"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Rotate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""binding"",
                    ""id"": ""d2edd126-c7e6-409c-b280-008c1ba9f259"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": ""Clamp(min=-10,max=10)"",
                    ""groups"": """",
                    ""action"": ""Rotate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""9d8a33b8-e566-4dd0-8184-735b20b045ea"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ToggleCameraLock"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""1fb9de72-23d6-48b9-aa4f-c500866bbf24"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""UnlockCamera"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""30e62f14-49d9-4e68-81a5-980b8b22de79"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""UnlockCamera"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7a7ae599-c995-4d29-bd89-fca128158aa9"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""UnlockCamera"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""19bd063a-9041-457f-a5d0-4e35e2873fc0"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""UnlockCamera"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5b298f4b-30df-45d7-a304-ee571397688f"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""UnlockCamera"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""ScrollY"",
                    ""id"": ""848978e3-d197-4d05-a320-f8f1d585c9a2"",
                    ""path"": ""1DAxis"",
                    ""interactions"": ""Hold(duration=1.401298E-45)"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""UnlockCamera"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""fb61d73d-8176-4e85-bd88-b642ff1d0ffe"",
                    ""path"": ""<Mouse>/scroll/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""UnlockCamera"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""51f37588-6cab-4be5-913e-cdf25e89bfea"",
                    ""path"": ""<Mouse>/scroll/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""UnlockCamera"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // CameraControls
        m_CameraControls = asset.FindActionMap("CameraControls", throwIfNotFound: true);
        m_CameraControls_Pan = m_CameraControls.FindAction("Pan", throwIfNotFound: true);
        m_CameraControls_FastPan = m_CameraControls.FindAction("FastPan", throwIfNotFound: true);
        m_CameraControls_Zoom = m_CameraControls.FindAction("Zoom", throwIfNotFound: true);
        m_CameraControls_Rotate = m_CameraControls.FindAction("Rotate", throwIfNotFound: true);
        m_CameraControls_ToggleCameraLock = m_CameraControls.FindAction("ToggleCameraLock", throwIfNotFound: true);
        m_CameraControls_UnlockCamera = m_CameraControls.FindAction("UnlockCamera", throwIfNotFound: true);
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

    // CameraControls
    private readonly InputActionMap m_CameraControls;
    private List<ICameraControlsActions> m_CameraControlsActionsCallbackInterfaces = new List<ICameraControlsActions>();
    private readonly InputAction m_CameraControls_Pan;
    private readonly InputAction m_CameraControls_FastPan;
    private readonly InputAction m_CameraControls_Zoom;
    private readonly InputAction m_CameraControls_Rotate;
    private readonly InputAction m_CameraControls_ToggleCameraLock;
    private readonly InputAction m_CameraControls_UnlockCamera;
    public struct CameraControlsActions
    {
        private @CameraInputActions m_Wrapper;
        public CameraControlsActions(@CameraInputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @Pan => m_Wrapper.m_CameraControls_Pan;
        public InputAction @FastPan => m_Wrapper.m_CameraControls_FastPan;
        public InputAction @Zoom => m_Wrapper.m_CameraControls_Zoom;
        public InputAction @Rotate => m_Wrapper.m_CameraControls_Rotate;
        public InputAction @ToggleCameraLock => m_Wrapper.m_CameraControls_ToggleCameraLock;
        public InputAction @UnlockCamera => m_Wrapper.m_CameraControls_UnlockCamera;
        public InputActionMap Get() { return m_Wrapper.m_CameraControls; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(CameraControlsActions set) { return set.Get(); }
        public void AddCallbacks(ICameraControlsActions instance)
        {
            if (instance == null || m_Wrapper.m_CameraControlsActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_CameraControlsActionsCallbackInterfaces.Add(instance);
            @Pan.started += instance.OnPan;
            @Pan.performed += instance.OnPan;
            @Pan.canceled += instance.OnPan;
            @FastPan.started += instance.OnFastPan;
            @FastPan.performed += instance.OnFastPan;
            @FastPan.canceled += instance.OnFastPan;
            @Zoom.started += instance.OnZoom;
            @Zoom.performed += instance.OnZoom;
            @Zoom.canceled += instance.OnZoom;
            @Rotate.started += instance.OnRotate;
            @Rotate.performed += instance.OnRotate;
            @Rotate.canceled += instance.OnRotate;
            @ToggleCameraLock.started += instance.OnToggleCameraLock;
            @ToggleCameraLock.performed += instance.OnToggleCameraLock;
            @ToggleCameraLock.canceled += instance.OnToggleCameraLock;
            @UnlockCamera.started += instance.OnUnlockCamera;
            @UnlockCamera.performed += instance.OnUnlockCamera;
            @UnlockCamera.canceled += instance.OnUnlockCamera;
        }

        private void UnregisterCallbacks(ICameraControlsActions instance)
        {
            @Pan.started -= instance.OnPan;
            @Pan.performed -= instance.OnPan;
            @Pan.canceled -= instance.OnPan;
            @FastPan.started -= instance.OnFastPan;
            @FastPan.performed -= instance.OnFastPan;
            @FastPan.canceled -= instance.OnFastPan;
            @Zoom.started -= instance.OnZoom;
            @Zoom.performed -= instance.OnZoom;
            @Zoom.canceled -= instance.OnZoom;
            @Rotate.started -= instance.OnRotate;
            @Rotate.performed -= instance.OnRotate;
            @Rotate.canceled -= instance.OnRotate;
            @ToggleCameraLock.started -= instance.OnToggleCameraLock;
            @ToggleCameraLock.performed -= instance.OnToggleCameraLock;
            @ToggleCameraLock.canceled -= instance.OnToggleCameraLock;
            @UnlockCamera.started -= instance.OnUnlockCamera;
            @UnlockCamera.performed -= instance.OnUnlockCamera;
            @UnlockCamera.canceled -= instance.OnUnlockCamera;
        }

        public void RemoveCallbacks(ICameraControlsActions instance)
        {
            if (m_Wrapper.m_CameraControlsActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(ICameraControlsActions instance)
        {
            foreach (var item in m_Wrapper.m_CameraControlsActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_CameraControlsActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public CameraControlsActions @CameraControls => new CameraControlsActions(this);
    public interface ICameraControlsActions
    {
        void OnPan(InputAction.CallbackContext context);
        void OnFastPan(InputAction.CallbackContext context);
        void OnZoom(InputAction.CallbackContext context);
        void OnRotate(InputAction.CallbackContext context);
        void OnToggleCameraLock(InputAction.CallbackContext context);
        void OnUnlockCamera(InputAction.CallbackContext context);
    }
}