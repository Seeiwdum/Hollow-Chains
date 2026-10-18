using UnityEngine;

namespace HollowChains.Core.Events
{
    /// <summary>Event channel with an int payload, e.g. current HP for the HUD.</summary>
    [CreateAssetMenu(menuName = "Hollow Chains/Events/Int Event Channel", fileName = "EVT_NewInt")]
    public sealed class IntEventChannelSO : EventChannelSO<int> { }
}
