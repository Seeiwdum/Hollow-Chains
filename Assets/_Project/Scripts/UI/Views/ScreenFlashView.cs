using HollowChains.Core.Events;
using UnityEngine;
using UnityEngine.UI;

namespace HollowChains.UI.Views
{
    /// <summary>
    /// Full-screen flash that fades out (SC-05 flashback: the wolf's last memories hitting Phineas, skill #42).
    /// Setup: a full-screen Image (raycastTarget off) on top of the HUD; assign EVT_Flash.
    /// </summary>
    [RequireComponent(typeof(Image))]
    public sealed class ScreenFlashView : MonoBehaviour
    {
        [SerializeField] private VoidEventChannelSO _onFlash;
        [SerializeField] private Color _color = new Color(0.85f, 0.75f, 0.95f, 0.85f);
        [SerializeField, Min(0.05f)] private float _fadeSeconds = 0.45f;

        private Image _image;
        private float _alpha;

        private void Awake()
        {
            _image = GetComponent<Image>();
            _image.raycastTarget = false;
            Apply(0f);
        }

        private void OnEnable()
        {
            if (_onFlash != null) _onFlash.Subscribe(Flash);
        }

        private void OnDisable()
        {
            if (_onFlash != null) _onFlash.Unsubscribe(Flash);
        }

        private void Update()
        {
            if (_alpha <= 0f) return;
            Apply(Mathf.Max(0f, _alpha - Time.unscaledDeltaTime / _fadeSeconds));
        }

        private void Flash() => Apply(1f);

        private void Apply(float alpha)
        {
            _alpha = alpha;
            var c = _color;
            c.a *= alpha;
            _image.color = c;
        }
    }
}
