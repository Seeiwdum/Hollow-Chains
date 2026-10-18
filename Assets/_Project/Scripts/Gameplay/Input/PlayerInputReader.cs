using System;
using HollowChains.Core.Events;
using UnityEngine;
using UnityEngine.InputSystem;

namespace HollowChains.Gameplay.Input
{
    /// <summary>
    /// Adapts Input System actions to <see cref="IPlayerInput"/>.
    /// Put it on the Player GameObject and assign the Player map actions from
    /// Settings/Input/InputSystem_Actions (Move, Look, Sprint, Interact).
    /// </summary>
    public sealed class PlayerInputReader : MonoBehaviour, IPlayerInput
    {
        [SerializeField] private InputActionReference _move;
        [SerializeField] private InputActionReference _look;
        [SerializeField] private InputActionReference _sprint;
        [SerializeField] private InputActionReference _interact;
        [Tooltip("Game flow turns gameplay input off in menus/pause (EVT_GameplayInputEnabled).")]
        [SerializeField] private BoolEventChannelSO _inputEnabled;

        private bool _enabled = true;

        public Vector2 Move => _enabled ? Read(_move) : Vector2.zero;

        public Vector2 Look => _enabled ? Read(_look) : Vector2.zero;

        public bool SprintHeld => _enabled && _sprint != null && _sprint.action.IsPressed();

        public bool LookFromPointer => _look != null && _look.action.activeControl?.device is Pointer;

        public event Action InteractPressed;

        private void OnEnable()
        {
            Enable(_move);
            Enable(_look);
            Enable(_sprint);
            if (Enable(_interact)) _interact.action.performed += OnInteractPerformed;
            if (_inputEnabled != null) _inputEnabled.Subscribe(SetEnabled);
        }

        private void OnDisable()
        {
            if (_interact != null) _interact.action.performed -= OnInteractPerformed;
            if (_inputEnabled != null) _inputEnabled.Unsubscribe(SetEnabled);
        }

        private void SetEnabled(bool enabled) => _enabled = enabled;

        private void OnInteractPerformed(InputAction.CallbackContext _)
        {
            if (_enabled) InteractPressed?.Invoke();
        }

        private static Vector2 Read(InputActionReference reference) =>
            reference != null ? reference.action.ReadValue<Vector2>() : Vector2.zero;

        private static bool Enable(InputActionReference reference)
        {
            if (reference == null) return false;
            reference.action.Enable();
            return true;
        }
    }
}
