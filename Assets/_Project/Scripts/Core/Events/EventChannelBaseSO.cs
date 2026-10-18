using UnityEngine;

namespace HollowChains.Core.Events
{
    /// <summary>
    /// Common base for every ScriptableObject event channel (the project's Event Bus).
    /// Raisers and listeners both reference the channel asset instead of each other, so systems stay decoupled.
    /// Create concrete channels from the Assets/Create/Hollow Chains/Events menu and store them in Data/Events.
    /// </summary>
    public abstract class EventChannelBaseSO : ScriptableObject
    {
        [SerializeField, TextArea] private string _description;

        /// <summary>Designer note explaining who raises this channel and who listens.</summary>
        public string Description => _description;
    }
}
