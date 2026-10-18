using HollowChains.Core.Events;
using UnityEngine;
using UnityEngine.UI;

namespace HollowChains.UI.Views
{
    /// <summary>
    /// A menu button that only raises an event channel ("new game", "resume", "quit to title"…). The game flow
    /// listens; the UI never calls gameplay code. Setup: on a uGUI Button; assign the request channel.
    /// </summary>
    [RequireComponent(typeof(Button))]
    public sealed class ChannelButton : MonoBehaviour
    {
        [SerializeField] private VoidEventChannelSO _onClick;

        private void Awake() => GetComponent<Button>().onClick.AddListener(Raise);

        private void OnDestroy() => GetComponent<Button>().onClick.RemoveListener(Raise);

        private void Raise()
        {
            if (_onClick != null) _onClick.Raise();
        }
    }
}
