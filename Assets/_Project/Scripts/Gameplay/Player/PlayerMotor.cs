using HollowChains.Data.Player;
using UnityEngine;

namespace HollowChains.Gameplay.Player
{
    /// <summary>
    /// Moves the player's CharacterController: horizontal acceleration, facing, gravity and ground snap.
    /// Knows nothing about input or states — <see cref="PlayerController"/> tells it where and how fast to go.
    /// Requires a CharacterController on the same GameObject.
    /// </summary>
    [RequireComponent(typeof(CharacterController))]
    public sealed class PlayerMotor : MonoBehaviour
    {
        private CharacterController _controller;
        private Vector3 _horizontalVelocity;
        private float _verticalVelocity;

        public bool IsGrounded => _controller.isGrounded;

        public float CurrentSpeed => _horizontalVelocity.magnitude;

        private void Awake() => _controller = GetComponent<CharacterController>();

        /// <summary>Moves toward <paramref name="worldDirection"/> (length ≤ 1) at up to <paramref name="speed"/> m/s.</summary>
        public void Move(Vector3 worldDirection, float speed, PlayerMovementConfigSO config, float deltaTime)
        {
            var target = Vector3.ClampMagnitude(worldDirection, 1f) * speed;
            _horizontalVelocity = Vector3.MoveTowards(_horizontalVelocity, target, config.Acceleration * speed * deltaTime);

            if (target.sqrMagnitude > 0.0001f)
            {
                var facing = Quaternion.LookRotation(target, Vector3.up);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, facing, config.TurnSpeedDegrees * deltaTime);
            }

            ApplyGravity(config, deltaTime);
            _controller.Move((_horizontalVelocity + Vector3.up * _verticalVelocity) * deltaTime);
        }

        /// <summary>Stops horizontal motion immediately (cutscenes) while still keeping the player on the ground.</summary>
        public void Halt(PlayerMovementConfigSO config, float deltaTime)
        {
            _horizontalVelocity = Vector3.zero;
            ApplyGravity(config, deltaTime);
            _controller.Move(Vector3.up * (_verticalVelocity * deltaTime));
        }

        private void ApplyGravity(PlayerMovementConfigSO config, float deltaTime)
        {
            if (_controller.isGrounded && _verticalVelocity < 0f)
                _verticalVelocity = config.GroundedStickVelocity;
            else
                _verticalVelocity += config.Gravity * deltaTime;
        }
    }
}
