//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.11.2
//     from Assets/Mobile Controls.inputactions
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

namespace mobileInputs
{
    public partial class @mobileInputActions: IInputActionCollection2, IDisposable
    {
        public InputActionAsset asset { get; }
        public @mobileInputActions()
        {
            asset = InputActionAsset.FromJson(@"{
    ""name"": ""Mobile Controls"",
    ""maps"": [
        {
            ""name"": ""Mobile"",
            ""id"": ""43823b65-ce77-4aa3-a9b4-f648b70c4cf5"",
            ""actions"": [
                {
                    ""name"": ""Tap"",
                    ""type"": ""Value"",
                    ""id"": ""47c7bcc9-a09e-4aec-a9b0-e3c54d03c1a4"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""95680fe5-1fee-45b9-a0e9-ab8c2b6db01f"",
                    ""path"": ""<Touchscreen>/touch0/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Mobile"",
                    ""action"": ""Tap"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Mobile"",
            ""bindingGroup"": ""Mobile"",
            ""devices"": [
                {
                    ""devicePath"": ""<Touchscreen>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
            // Mobile
            m_Mobile = asset.FindActionMap("Mobile", throwIfNotFound: true);
            m_Mobile_Tap = m_Mobile.FindAction("Tap", throwIfNotFound: true);
        }

        ~@mobileInputActions()
        {
            UnityEngine.Debug.Assert(!m_Mobile.enabled, "This will cause a leak and performance issues, mobileInputActions.Mobile.Disable() has not been called.");
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

        // Mobile
        private readonly InputActionMap m_Mobile;
        private List<IMobileActions> m_MobileActionsCallbackInterfaces = new List<IMobileActions>();
        private readonly InputAction m_Mobile_Tap;
        public struct MobileActions
        {
            private @mobileInputActions m_Wrapper;
            public MobileActions(@mobileInputActions wrapper) { m_Wrapper = wrapper; }
            public InputAction @Tap => m_Wrapper.m_Mobile_Tap;
            public InputActionMap Get() { return m_Wrapper.m_Mobile; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(MobileActions set) { return set.Get(); }
            public void AddCallbacks(IMobileActions instance)
            {
                if (instance == null || m_Wrapper.m_MobileActionsCallbackInterfaces.Contains(instance)) return;
                m_Wrapper.m_MobileActionsCallbackInterfaces.Add(instance);
                @Tap.started += instance.OnTap;
                @Tap.performed += instance.OnTap;
                @Tap.canceled += instance.OnTap;
            }

            private void UnregisterCallbacks(IMobileActions instance)
            {
                @Tap.started -= instance.OnTap;
                @Tap.performed -= instance.OnTap;
                @Tap.canceled -= instance.OnTap;
            }

            public void RemoveCallbacks(IMobileActions instance)
            {
                if (m_Wrapper.m_MobileActionsCallbackInterfaces.Remove(instance))
                    UnregisterCallbacks(instance);
            }

            public void SetCallbacks(IMobileActions instance)
            {
                foreach (var item in m_Wrapper.m_MobileActionsCallbackInterfaces)
                    UnregisterCallbacks(item);
                m_Wrapper.m_MobileActionsCallbackInterfaces.Clear();
                AddCallbacks(instance);
            }
        }
        public MobileActions @Mobile => new MobileActions(this);
        private int m_MobileSchemeIndex = -1;
        public InputControlScheme MobileScheme
        {
            get
            {
                if (m_MobileSchemeIndex == -1) m_MobileSchemeIndex = asset.FindControlSchemeIndex("Mobile");
                return asset.controlSchemes[m_MobileSchemeIndex];
            }
        }
        public interface IMobileActions
        {
            void OnTap(InputAction.CallbackContext context);
        }
    }
}
