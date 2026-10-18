using HollowChains.Gameplay.Player;
using Unity.Cinemachine;
using UnityEngine;

namespace HollowChains.Gameplay.CameraRig
{
    /// <summary>
    /// Feeds the player's Look input into a CinemachineOrbitalFollow (third-person orbit). Kept separate from
    /// Cinemachine's own input controller so camera input goes through IPlayerInput like everything else.
    /// Setup: on the CM_Explore camera; assign the PlayerController and the OrbitalFollow on the same camera.
    /// </summary>
    public sealed class OrbitLookInput : MonoBehaviour
    {
        [SerializeField] private PlayerController _player;
        [SerializeField] private CinemachineOrbitalFollow _orbit;
        [Tooltip("Degrees per pixel of mouse movement.")]
        [SerializeField] private Vector2 _mouseDegreesPerPixel = new Vector2(0.12f, 0.08f);
        [Tooltip("Degrees per second at full stick deflection.")]
        [SerializeField] private Vector2 _stickDegreesPerSecond = new Vector2(180f, 110f);
        [SerializeField] private bool _invertY;

        private void Reset() => _orbit = GetComponent<CinemachineOrbitalFollow>();

        private void Update()
        {
            if (_player == null || _orbit == null || _player.Input == null || _player.IsLocked) return;

            var look = _player.Input.Look;
            var delta = _player.Input.LookFromPointer
                ? Vector2.Scale(look, _mouseDegreesPerPixel)
                : Vector2.Scale(look, _stickDegreesPerSecond) * Time.deltaTime;
            if (!_invertY) delta.y = -delta.y;

            _orbit.HorizontalAxis.Value = _orbit.HorizontalAxis.ClampValue(_orbit.HorizontalAxis.Value + delta.x);
            _orbit.VerticalAxis.Value = _orbit.VerticalAxis.ClampValue(_orbit.VerticalAxis.Value + delta.y);
        }
    }
}
