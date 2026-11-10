using HollowChains.Core.Events;
using HollowChains.Core.Services;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

namespace HollowChains.Core.Input
{
    /// <summary>
    /// Watches which device the player used last and publishes it as an input scheme.
    /// Raises EVT_InputSchemeChanged ("kbm" / "gamepad") only when the scheme actually changes, and registers
    /// <see cref="IInputSchemeService"/> in the ServiceLocator.
    /// Mouse movement below a pixel threshold is ignored so a bumped mouse does not flip a gamepad player to keyboard.
    /// Setup: on the "Game" object in the _Boot scene; assign the string channel EVT_InputSchemeChanged.
    /// Listens to: InputSystem.onActionChange (enabled actions only). Raises: EVT_InputSchemeChanged.
    /// </summary>
    public sealed class InputSchemeDetector : MonoBehaviour, IInputSchemeService
    {
        [SerializeField] private StringEventChannelSO _onSchemeChanged;
        [Tooltip("Mouse delta (pixels per event) below this is treated as noise.")]
        [SerializeField, Min(0f)] private float _mouseNoiseThreshold = 2f;

        private bool _registered;

        public string CurrentScheme { get; private set; } = InputSchemes.KeyboardMouse;

        private void Awake()
        {
            // A second _Boot (Play pressed inside _Boot) must not throw on a duplicate registration.
            if (ServiceLocator.TryGet<IInputSchemeService>(out _)) { enabled = false; return; }
            ServiceLocator.Register<IInputSchemeService>(this);
            _registered = true;
        }

        private void OnEnable() => InputSystem.onActionChange += OnActionChange;

        private void OnDisable() => InputSystem.onActionChange -= OnActionChange;

        private void OnDestroy()
        {
            if (_registered) ServiceLocator.Unregister<IInputSchemeService>();
        }

        private void OnActionChange(object obj, InputActionChange change)
        {
            if (change != InputActionChange.ActionPerformed || !(obj is InputAction action)) return;
            var control = action.activeControl;
            if (control == null) return;
            var scheme = SchemeOf(control);
            if (scheme != null) SetScheme(scheme);
        }

        private string SchemeOf(InputControl control)
        {
            var device = control.device;
            if (device is Gamepad || device is Joystick) return InputSchemes.Gamepad;
            if (device is Keyboard) return InputSchemes.KeyboardMouse;
            if (device is Mouse)
            {
                if (control is ButtonControl) return InputSchemes.KeyboardMouse;
                float magnitude = control.EvaluateMagnitude(); // -1 = cannot be computed
                return magnitude >= 0f && magnitude < _mouseNoiseThreshold ? null : InputSchemes.KeyboardMouse;
            }
            return null; // touch, XR, pen: keep the current scheme
        }

        private void SetScheme(string scheme)
        {
            if (scheme == CurrentScheme) return;
            CurrentScheme = scheme;
            if (_onSchemeChanged != null) _onSchemeChanged.Raise(scheme);
        }
    }
}
