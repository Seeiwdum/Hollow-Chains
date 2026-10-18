using HollowChains.Core.StateMachines;
using HollowChains.Data.Player;
using HollowChains.Gameplay.Input;
using HollowChains.Gameplay.Player.States;
using UnityEngine;

namespace HollowChains.Gameplay.Player
{
    /// <summary>
    /// Composition root of the player: owns the movement state machine and wires input → states → motor.
    /// Setup: Player GameObject with CharacterController (height 1.65, radius 0.3), PlayerMotor, PlayerInputReader
    /// and this component; assign a PlayerMovementConfig asset. Uses Camera.main for camera-relative movement.
    /// Other systems call <see cref="EnterCutscene"/>, <see cref="SetCarrying"/>, <see cref="SetSprintAllowed"/>.
    /// </summary>
    [RequireComponent(typeof(PlayerMotor))]
    public sealed class PlayerController : MonoBehaviour
    {
        [SerializeField] private PlayerMovementConfigSO _config;
        [Tooltip("Component implementing IPlayerInput (normally PlayerInputReader on this GameObject).")]
        [SerializeField] private MonoBehaviour _inputSource;

        private readonly StateMachine _stateMachine = new StateMachine();
        private IPlayerInput _input;
        private Transform _camera;
        private bool _sprintAllowed;

        internal PlayerMotor Motor { get; private set; }
        internal PlayerMovementConfigSO Config => _config;
        internal Vector2 MoveInput => _input != null ? _input.Move : Vector2.zero;
        internal bool WantsSprint => _sprintAllowed && _input != null && _input.SprintHeld && !IsCarrying && MoveInput.sqrMagnitude > 0.01f;

        internal LocomotionState Locomotion { get; private set; }
        internal SprintState Sprint { get; private set; }
        internal CarryState Carry { get; private set; }
        internal LockedState Locked { get; private set; }

        public bool IsCarrying { get; private set; }

        public bool IsLocked => _stateMachine.Current == Locked;

        public IState CurrentState => _stateMachine.Current;

        private void Awake()
        {
            Motor = GetComponent<PlayerMotor>();
            _input = _inputSource as IPlayerInput ?? GetComponent<IPlayerInput>();
            if (_input == null) Debug.LogError($"{name}: no IPlayerInput found — add PlayerInputReader.", this);
            if (_config == null) Debug.LogError($"{name}: PlayerMovementConfig is not assigned.", this);
            _sprintAllowed = _config != null && _config.SprintAllowedByDefault;

            Locomotion = new LocomotionState(this);
            Sprint = new SprintState(this);
            Carry = new CarryState(this);
            Locked = new LockedState(this);
        }

        private void Start()
        {
            _camera = Camera.main != null ? Camera.main.transform : null;
            _stateMachine.ChangeState(Locomotion);
        }

        private void Update()
        {
            if (_config == null) return;
            _stateMachine.Tick(Time.deltaTime);
        }

        public void EnterCutscene() => _stateMachine.ChangeState(Locked);

        public void ExitCutscene()
        {
            if (IsLocked) _stateMachine.ChangeState(Locomotion);
        }

        public void SetCarrying(bool carrying) => IsCarrying = carrying;

        public void SetSprintAllowed(bool allowed) => _sprintAllowed = allowed;

        internal void ChangeTo(PlayerState next) => _stateMachine.ChangeState(next);

        /// <summary>Converts stick input to a world direction on the ground plane, relative to the camera.</summary>
        internal Vector3 CameraRelative(Vector2 input)
        {
            if (_camera == null) return new Vector3(input.x, 0f, input.y);
            var forward = Vector3.ProjectOnPlane(_camera.forward, Vector3.up).normalized;
            var right = Vector3.ProjectOnPlane(_camera.right, Vector3.up).normalized;
            return forward * input.y + right * input.x;
        }
    }
}
