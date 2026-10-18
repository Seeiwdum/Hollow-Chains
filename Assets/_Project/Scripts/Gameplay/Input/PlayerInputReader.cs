using System;
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

        public Vector2 Move => Read(_move);

        public Vector2 Look => Read(_look);

        public bool SprintHeld => _sprint != null && _sprint.action.IsPressed();

        public event Action InteractPressed;

        private void OnEnable()
        {
            Enable(_move);
            Enable(_look);
            Enable(_sprint);
            if (Enable(_interact)) _interact.action.performed += OnInteractPerformed;
        }

        private void OnDisable()
        {
            if (_interact != null) _interact.action.performed -= OnInteractPerformed;
        }

        private void OnInteractPerformed(InputAction.CallbackContext _) => InteractPressed?.Invoke();

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
