using UnityEngine;

namespace HollowChains.UI.Views
{
    /// <summary>
    /// Base for every screen and HUD element: show/hide through a CanvasGroup (template methods OnShow/OnHide).
    /// Views only listen to event channels — they never reference gameplay classes (D-020).
    /// </summary>
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class UIView : MonoBehaviour
    {
        private CanvasGroup _group;

        public bool IsVisible { get; private set; }

        protected virtual void Awake()
        {
            _group = GetComponent<CanvasGroup>();
            SetVisible(false);
        }

        public void Show()
        {
            SetVisible(true);
            OnShow();
        }

        public void Hide()
        {
            SetVisible(false);
            OnHide();
        }

        protected virtual void OnShow() { }

        protected virtual void OnHide() { }

        private void SetVisible(bool visible)
        {
            IsVisible = visible;
            _group.alpha = visible ? 1f : 0f;
            _group.interactable = visible;
            _group.blocksRaycasts = visible;
        }
    }
}
