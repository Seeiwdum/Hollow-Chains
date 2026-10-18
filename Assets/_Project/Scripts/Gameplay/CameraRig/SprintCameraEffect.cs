using HollowChains.Gameplay.Player;
using Unity.Cinemachine;
using UnityEngine;

namespace HollowChains.Gameplay.CameraRig
{
    /// <summary>
    /// Widens the field of view and pulls the orbit closer while the player sprints (SC-03 "dynamic" camera).
    /// Setup: on CM_Explore next to CinemachineCamera + CinemachineOrbitalFollow; assign the PlayerController.
    /// </summary>
    public sealed class SprintCameraEffect : MonoBehaviour
    {
        [SerializeField] private PlayerController _player;
        [SerializeField] private CinemachineCamera _camera;
        [SerializeField] private CinemachineOrbitalFollow _orbit;
        [SerializeField] private float _sprintFovBonus = 12f;
        [SerializeField, Range(0.5f, 1f)] private float _sprintRadiusScale = 0.85f;
        [SerializeField, Min(0.01f)] private float _blendSpeed = 4f;

        private float _baseFov;
        private float _baseRadius;
        private float _blend;

        private void Reset()
        {
            _camera = GetComponent<CinemachineCamera>();
            _orbit = GetComponent<CinemachineOrbitalFollow>();
        }

        private void Start()
        {
            if (_camera != null) _baseFov = _camera.Lens.FieldOfView;
            if (_orbit != null) _baseRadius = _orbit.Radius;
        }

        private void LateUpdate()
        {
            if (_player == null || _camera == null || _orbit == null) return;
            float target = _player.IsSprinting ? 1f : 0f;
            _blend = Mathf.MoveTowards(_blend, target, _blendSpeed * Time.deltaTime);
            float eased = Mathf.SmoothStep(0f, 1f, _blend);

            var lens = _camera.Lens;
            lens.FieldOfView = _baseFov + _sprintFovBonus * eased;
            _camera.Lens = lens;
            _orbit.Radius = _baseRadius * Mathf.Lerp(1f, _sprintRadiusScale, eased);
        }
    }
}
