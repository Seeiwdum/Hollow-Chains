using HollowChains.Core.Events;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace HollowChains.UI.Views
{
    /// <summary>
    /// A screen that is visible only in certain game-flow states (Title menu → "Title", Pause menu → "Paused",
    /// HUD → "Playing", End screen → "End"). Selects its first button for gamepad navigation when shown.
    /// Setup: on the root of a menu/HUD group with CanvasGroup; assign EVT_GameStateChanged and the state names.
    /// </summary>
    public class GameStateView : UIView
    {
        [SerializeField] private StringEventChannelSO _onStateChanged;
        [SerializeField] private string[] _visibleInStates = { "Playing" };
        [SerializeField] private Selectable _firstSelected;

        protected virtual void OnEnable()
        {
            if (_onStateChanged != null) _onStateChanged.Subscribe(OnState);
        }

        protected virtual void OnDisable()
        {
            if (_onStateChanged != null) _onStateChanged.Unsubscribe(OnState);
        }

        protected override void OnShow()
        {
            if (_firstSelected != null && EventSystem.current != null)
                EventSystem.current.SetSelectedGameObject(_firstSelected.gameObject);
        }

        private void OnState(string state)
        {
            bool visible = System.Array.IndexOf(_visibleInStates, state) >= 0;
            if (visible && !IsVisible) Show();
            else if (!visible && IsVisible) Hide();
        }
    }
}
