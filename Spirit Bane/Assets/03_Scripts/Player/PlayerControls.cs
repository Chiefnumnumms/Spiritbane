//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.4.4
//     from Assets/Scripts/PlayerControls.inputactions
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

public partial class @PlayerControls : IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerControls"",
    ""maps"": [
        {
            ""name"": ""PlayerMovement"",
            ""id"": ""d1f79b67-52c3-4ed9-ac5c-7ce65f7374f3"",
            ""actions"": [
                {
                    ""name"": ""Movement"",
                    ""type"": ""PassThrough"",
                    ""id"": ""49a01960-8099-453e-b076-95e3c4c4faba"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""WASD"",
                    ""id"": ""d7e01547-6e85-48d3-be1b-8b9440e84d43"",
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
                    ""id"": ""dc619689-7543-40ea-9f4d-31aa28cc1460"",
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
                    ""id"": ""dc0953c7-0df9-485c-ab15-0059bf62c06a"",
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
                    ""id"": ""84fa5048-2da2-49c6-816d-839ab21ba00d"",
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
                    ""id"": ""e0201caf-4ff9-4761-97be-595dd781ab6c"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                }
            ]
        },
        {
            ""name"": ""PlayerActions"",
            ""id"": ""5126be8f-3941-41d6-a75e-1aba1ed49598"",
            ""actions"": [
                {
                    ""name"": ""Sprint"",
                    ""type"": ""Button"",
                    ""id"": ""cea3df03-21e7-4c0a-9082-c0e8612e0004"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""12324a81-aba2-4ecf-8bd5-4630b6bc0169"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Pickup"",
                    ""type"": ""Button"",
                    ""id"": ""3d09a72f-ee7d-42cc-9779-10323dc3c9c0"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""7b28218f-f060-46f8-9d48-a55bf35c9100"",
                    ""path"": ""<Keyboard>/shift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Sprint"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""635ce94d-f717-469a-9cac-045d33089497"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9425eb63-665f-4c6a-8b95-5d803be84608"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Pickup"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""SwingingActions"",
            ""id"": ""d35b2b4a-d184-4eec-b86d-6199eb555f17"",
            ""actions"": [
                {
                    ""name"": ""Swing"",
                    ""type"": ""Button"",
                    ""id"": ""66bdd0ba-f446-4b81-94e5-daa8d8f72f1f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Adjust Rope"",
                    ""type"": ""Button"",
                    ""id"": ""ec6386da-bbd8-4954-b39e-15a88dc546ed"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Swing Forward"",
                    ""type"": ""Button"",
                    ""id"": ""4166adae-ecaa-42ae-ac1c-debef310c96c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Swing Left"",
                    ""type"": ""Button"",
                    ""id"": ""6eb2c02e-1eb8-44db-a1dc-794c8b1b31ee"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Swing Right"",
                    ""type"": ""Button"",
                    ""id"": ""130e0de0-e47e-41b4-aa9f-3a478c35e458"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""3c473bf9-ac50-4bd9-928a-b023d558fe2f"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": ""Hold(duration=0.1)"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Swing"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""70f4d905-6dd2-4879-872c-d8de1e93b19b"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": ""Hold(duration=0.1)"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Adjust Rope"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""83e6ba31-ecd6-47b9-a45b-c7d78971697c"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Swing Forward"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3f2cb8e9-1d25-44e1-96a5-204bdb30ba96"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Swing Left"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b8c8f427-2ea9-4cd6-b66f-6e497de25b13"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Swing Right"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""GrapplingActions"",
            ""id"": ""245d459b-5eb9-45a3-a760-fe087859953b"",
            ""actions"": [
                {
                    ""name"": ""Grapple"",
                    ""type"": ""Button"",
                    ""id"": ""3794c41a-ed8b-49c4-8490-6cb6c1cd64ee"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""62df24ac-f79a-4006-81a9-bc7d16c534f7"",
                    ""path"": ""<Keyboard>/r"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Grapple"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // PlayerMovement
        m_PlayerMovement = asset.FindActionMap("PlayerMovement", throwIfNotFound: true);
        m_PlayerMovement_Movement = m_PlayerMovement.FindAction("Movement", throwIfNotFound: true);
        // PlayerActions
        m_PlayerActions = asset.FindActionMap("PlayerActions", throwIfNotFound: true);
        m_PlayerActions_Sprint = m_PlayerActions.FindAction("Sprint", throwIfNotFound: true);
        m_PlayerActions_Jump = m_PlayerActions.FindAction("Jump", throwIfNotFound: true);
        m_PlayerActions_Pickup = m_PlayerActions.FindAction("Pickup", throwIfNotFound: true);
        // SwingingActions
        m_SwingingActions = asset.FindActionMap("SwingingActions", throwIfNotFound: true);
        m_SwingingActions_Swing = m_SwingingActions.FindAction("Swing", throwIfNotFound: true);
        m_SwingingActions_AdjustRope = m_SwingingActions.FindAction("Adjust Rope", throwIfNotFound: true);
        m_SwingingActions_SwingForward = m_SwingingActions.FindAction("Swing Forward", throwIfNotFound: true);
        m_SwingingActions_SwingLeft = m_SwingingActions.FindAction("Swing Left", throwIfNotFound: true);
        m_SwingingActions_SwingRight = m_SwingingActions.FindAction("Swing Right", throwIfNotFound: true);
        // GrapplingActions
        m_GrapplingActions = asset.FindActionMap("GrapplingActions", throwIfNotFound: true);
        m_GrapplingActions_Grapple = m_GrapplingActions.FindAction("Grapple", throwIfNotFound: true);
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

    // PlayerMovement
    private readonly InputActionMap m_PlayerMovement;
    private IPlayerMovementActions m_PlayerMovementActionsCallbackInterface;
    private readonly InputAction m_PlayerMovement_Movement;
    public struct PlayerMovementActions
    {
        private @PlayerControls m_Wrapper;
        public PlayerMovementActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Movement => m_Wrapper.m_PlayerMovement_Movement;
        public InputActionMap Get() { return m_Wrapper.m_PlayerMovement; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerMovementActions set) { return set.Get(); }
        public void SetCallbacks(IPlayerMovementActions instance)
        {
            if (m_Wrapper.m_PlayerMovementActionsCallbackInterface != null)
            {
                @Movement.started -= m_Wrapper.m_PlayerMovementActionsCallbackInterface.OnMovement;
                @Movement.performed -= m_Wrapper.m_PlayerMovementActionsCallbackInterface.OnMovement;
                @Movement.canceled -= m_Wrapper.m_PlayerMovementActionsCallbackInterface.OnMovement;
            }
            m_Wrapper.m_PlayerMovementActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Movement.started += instance.OnMovement;
                @Movement.performed += instance.OnMovement;
                @Movement.canceled += instance.OnMovement;
            }
        }
    }
    public PlayerMovementActions @PlayerMovement => new PlayerMovementActions(this);

    // PlayerActions
    private readonly InputActionMap m_PlayerActions;
    private IPlayerActionsActions m_PlayerActionsActionsCallbackInterface;
    private readonly InputAction m_PlayerActions_Sprint;
    private readonly InputAction m_PlayerActions_Jump;
    private readonly InputAction m_PlayerActions_Pickup;
    public struct PlayerActionsActions
    {
        private @PlayerControls m_Wrapper;
        public PlayerActionsActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Sprint => m_Wrapper.m_PlayerActions_Sprint;
        public InputAction @Jump => m_Wrapper.m_PlayerActions_Jump;
        public InputAction @Pickup => m_Wrapper.m_PlayerActions_Pickup;
        public InputActionMap Get() { return m_Wrapper.m_PlayerActions; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerActionsActions set) { return set.Get(); }
        public void SetCallbacks(IPlayerActionsActions instance)
        {
            if (m_Wrapper.m_PlayerActionsActionsCallbackInterface != null)
            {
                @Sprint.started -= m_Wrapper.m_PlayerActionsActionsCallbackInterface.OnSprint;
                @Sprint.performed -= m_Wrapper.m_PlayerActionsActionsCallbackInterface.OnSprint;
                @Sprint.canceled -= m_Wrapper.m_PlayerActionsActionsCallbackInterface.OnSprint;
                @Jump.started -= m_Wrapper.m_PlayerActionsActionsCallbackInterface.OnJump;
                @Jump.performed -= m_Wrapper.m_PlayerActionsActionsCallbackInterface.OnJump;
                @Jump.canceled -= m_Wrapper.m_PlayerActionsActionsCallbackInterface.OnJump;
                @Pickup.started -= m_Wrapper.m_PlayerActionsActionsCallbackInterface.OnPickup;
                @Pickup.performed -= m_Wrapper.m_PlayerActionsActionsCallbackInterface.OnPickup;
                @Pickup.canceled -= m_Wrapper.m_PlayerActionsActionsCallbackInterface.OnPickup;
            }
            m_Wrapper.m_PlayerActionsActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Sprint.started += instance.OnSprint;
                @Sprint.performed += instance.OnSprint;
                @Sprint.canceled += instance.OnSprint;
                @Jump.started += instance.OnJump;
                @Jump.performed += instance.OnJump;
                @Jump.canceled += instance.OnJump;
                @Pickup.started += instance.OnPickup;
                @Pickup.performed += instance.OnPickup;
                @Pickup.canceled += instance.OnPickup;
            }
        }
    }
    public PlayerActionsActions @PlayerActions => new PlayerActionsActions(this);

    // SwingingActions
    private readonly InputActionMap m_SwingingActions;
    private ISwingingActionsActions m_SwingingActionsActionsCallbackInterface;
    private readonly InputAction m_SwingingActions_Swing;
    private readonly InputAction m_SwingingActions_AdjustRope;
    private readonly InputAction m_SwingingActions_SwingForward;
    private readonly InputAction m_SwingingActions_SwingLeft;
    private readonly InputAction m_SwingingActions_SwingRight;
    public struct SwingingActionsActions
    {
        private @PlayerControls m_Wrapper;
        public SwingingActionsActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Swing => m_Wrapper.m_SwingingActions_Swing;
        public InputAction @AdjustRope => m_Wrapper.m_SwingingActions_AdjustRope;
        public InputAction @SwingForward => m_Wrapper.m_SwingingActions_SwingForward;
        public InputAction @SwingLeft => m_Wrapper.m_SwingingActions_SwingLeft;
        public InputAction @SwingRight => m_Wrapper.m_SwingingActions_SwingRight;
        public InputActionMap Get() { return m_Wrapper.m_SwingingActions; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(SwingingActionsActions set) { return set.Get(); }
        public void SetCallbacks(ISwingingActionsActions instance)
        {
            if (m_Wrapper.m_SwingingActionsActionsCallbackInterface != null)
            {
                @Swing.started -= m_Wrapper.m_SwingingActionsActionsCallbackInterface.OnSwing;
                @Swing.performed -= m_Wrapper.m_SwingingActionsActionsCallbackInterface.OnSwing;
                @Swing.canceled -= m_Wrapper.m_SwingingActionsActionsCallbackInterface.OnSwing;
                @AdjustRope.started -= m_Wrapper.m_SwingingActionsActionsCallbackInterface.OnAdjustRope;
                @AdjustRope.performed -= m_Wrapper.m_SwingingActionsActionsCallbackInterface.OnAdjustRope;
                @AdjustRope.canceled -= m_Wrapper.m_SwingingActionsActionsCallbackInterface.OnAdjustRope;
                @SwingForward.started -= m_Wrapper.m_SwingingActionsActionsCallbackInterface.OnSwingForward;
                @SwingForward.performed -= m_Wrapper.m_SwingingActionsActionsCallbackInterface.OnSwingForward;
                @SwingForward.canceled -= m_Wrapper.m_SwingingActionsActionsCallbackInterface.OnSwingForward;
                @SwingLeft.started -= m_Wrapper.m_SwingingActionsActionsCallbackInterface.OnSwingLeft;
                @SwingLeft.performed -= m_Wrapper.m_SwingingActionsActionsCallbackInterface.OnSwingLeft;
                @SwingLeft.canceled -= m_Wrapper.m_SwingingActionsActionsCallbackInterface.OnSwingLeft;
                @SwingRight.started -= m_Wrapper.m_SwingingActionsActionsCallbackInterface.OnSwingRight;
                @SwingRight.performed -= m_Wrapper.m_SwingingActionsActionsCallbackInterface.OnSwingRight;
                @SwingRight.canceled -= m_Wrapper.m_SwingingActionsActionsCallbackInterface.OnSwingRight;
            }
            m_Wrapper.m_SwingingActionsActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Swing.started += instance.OnSwing;
                @Swing.performed += instance.OnSwing;
                @Swing.canceled += instance.OnSwing;
                @AdjustRope.started += instance.OnAdjustRope;
                @AdjustRope.performed += instance.OnAdjustRope;
                @AdjustRope.canceled += instance.OnAdjustRope;
                @SwingForward.started += instance.OnSwingForward;
                @SwingForward.performed += instance.OnSwingForward;
                @SwingForward.canceled += instance.OnSwingForward;
                @SwingLeft.started += instance.OnSwingLeft;
                @SwingLeft.performed += instance.OnSwingLeft;
                @SwingLeft.canceled += instance.OnSwingLeft;
                @SwingRight.started += instance.OnSwingRight;
                @SwingRight.performed += instance.OnSwingRight;
                @SwingRight.canceled += instance.OnSwingRight;
            }
        }
    }
    public SwingingActionsActions @SwingingActions => new SwingingActionsActions(this);

    // GrapplingActions
    private readonly InputActionMap m_GrapplingActions;
    private IGrapplingActionsActions m_GrapplingActionsActionsCallbackInterface;
    private readonly InputAction m_GrapplingActions_Grapple;
    public struct GrapplingActionsActions
    {
        private @PlayerControls m_Wrapper;
        public GrapplingActionsActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Grapple => m_Wrapper.m_GrapplingActions_Grapple;
        public InputActionMap Get() { return m_Wrapper.m_GrapplingActions; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(GrapplingActionsActions set) { return set.Get(); }
        public void SetCallbacks(IGrapplingActionsActions instance)
        {
            if (m_Wrapper.m_GrapplingActionsActionsCallbackInterface != null)
            {
                @Grapple.started -= m_Wrapper.m_GrapplingActionsActionsCallbackInterface.OnGrapple;
                @Grapple.performed -= m_Wrapper.m_GrapplingActionsActionsCallbackInterface.OnGrapple;
                @Grapple.canceled -= m_Wrapper.m_GrapplingActionsActionsCallbackInterface.OnGrapple;
            }
            m_Wrapper.m_GrapplingActionsActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Grapple.started += instance.OnGrapple;
                @Grapple.performed += instance.OnGrapple;
                @Grapple.canceled += instance.OnGrapple;
            }
        }
    }
    public GrapplingActionsActions @GrapplingActions => new GrapplingActionsActions(this);
    public interface IPlayerMovementActions
    {
        void OnMovement(InputAction.CallbackContext context);
    }
    public interface IPlayerActionsActions
    {
        void OnSprint(InputAction.CallbackContext context);
        void OnJump(InputAction.CallbackContext context);
        void OnPickup(InputAction.CallbackContext context);
    }
    public interface ISwingingActionsActions
    {
        void OnSwing(InputAction.CallbackContext context);
        void OnAdjustRope(InputAction.CallbackContext context);
        void OnSwingForward(InputAction.CallbackContext context);
        void OnSwingLeft(InputAction.CallbackContext context);
        void OnSwingRight(InputAction.CallbackContext context);
    }
    public interface IGrapplingActionsActions
    {
        void OnGrapple(InputAction.CallbackContext context);
    }
}
