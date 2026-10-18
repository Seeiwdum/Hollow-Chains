using HollowChains.Core.Settings;
using HollowChains.Gameplay.Player;
using UnityEngine;

namespace HollowChains.Gameplay.Audio
{
    /// <summary>
    /// Plays a random footstep every few metres of movement (placeholder until the P5 audio pass with surfaces and a
    /// mixer). Volume follows Master × SFX from Settings. Setup: on the Player; assign clips (Kenney footstep00–09).
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    public sealed class FootstepAudio : MonoBehaviour
    {
        [SerializeField] private PlayerMotor _motor;
        [SerializeField] private AudioClip[] _clips;
        [SerializeField, Min(0.1f)] private float _strideMetres = 0.7f;
        [SerializeField, Range(0f, 1f)] private float _volume = 0.55f;
        [SerializeField, Range(0f, 0.3f)] private float _pitchJitter = 0.08f;

        private AudioSource _source;
        private float _travelled;
        private int _last = -1;

        private void Awake()
        {
            _source = GetComponent<AudioSource>();
            _source.playOnAwake = false;
            _source.spatialBlend = 0f;
            if (_motor == null) _motor = GetComponent<PlayerMotor>();
        }

        private void Update()
        {
            if (_motor == null || _clips == null || _clips.Length == 0 || !_motor.IsGrounded) return;
            float speed = _motor.CurrentSpeed;
            if (speed < 0.2f) { _travelled = 0f; return; }
            _travelled += speed * Time.deltaTime;
            float stride = _strideMetres * Mathf.Lerp(1f, 1.6f, Mathf.InverseLerp(2f, 6.5f, speed)); // longer strides when running
            if (_travelled < stride) return;
            _travelled = 0f;
            Step();
        }

        private void Step()
        {
            int i = _clips.Length == 1 ? 0 : (_last + 1 + Random.Range(0, _clips.Length - 1)) % _clips.Length;
            _last = i;
            var s = GameSettings.Current;
            _source.pitch = 1f + Random.Range(-_pitchJitter, _pitchJitter);
            _source.PlayOneShot(_clips[i], _volume * s.MasterVolume * s.SfxVolume);
        }
    }
}
