using UnityEngine;

namespace HollowChains.Data.Player
{
    /// <summary>
    /// Designer-tunable movement values for the player. One asset (Data/Config/PlayerMovementConfig) is assigned
    /// to the PlayerController; change numbers here instead of in code.
    /// </summary>
    [CreateAssetMenu(menuName = "Hollow Chains/Player/Movement Config", fileName = "PlayerMovementConfig")]
    public sealed class PlayerMovementConfigSO : ScriptableObject
    {
        [Header("Speeds (m/s)")]
        [SerializeField, Min(0f)] private float _walkSpeed = 2.0f;
        [SerializeField, Min(0f)] private float _runSpeed = 4.0f;
        [SerializeField, Min(0f)] private float _sprintSpeed = 6.5f;
        [SerializeField, Range(0.1f, 1f)] private float _carryMultiplier = 0.8f;

        [Header("Feel")]
        [Tooltip("Stick magnitude above which the player runs instead of walks.")]
        [SerializeField, Range(0.1f, 1f)] private float _runThreshold = 0.6f;
        [SerializeField, Min(0.01f)] private float _acceleration = 12f;
        [SerializeField, Min(0f)] private float _turnSpeedDegrees = 720f;

        [Header("Gravity")]
        [SerializeField] private float _gravity = -20f;
        [SerializeField] private float _groundedStickVelocity = -2f;

        [Header("Rules")]
        [Tooltip("Sprint is normally unlocked by a sprint zone (SC-03). Enable for greybox testing.")]
        [SerializeField] private bool _sprintAllowedByDefault;

        public float WalkSpeed => _walkSpeed;
        public float RunSpeed => _runSpeed;
        public float SprintSpeed => _sprintSpeed;
        public float CarryMultiplier => _carryMultiplier;
        public float RunThreshold => _runThreshold;
        public float Acceleration => _acceleration;
        public float TurnSpeedDegrees => _turnSpeedDegrees;
        public float Gravity => _gravity;
        public float GroundedStickVelocity => _groundedStickVelocity;
        public bool SprintAllowedByDefault => _sprintAllowedByDefault;
    }
}
