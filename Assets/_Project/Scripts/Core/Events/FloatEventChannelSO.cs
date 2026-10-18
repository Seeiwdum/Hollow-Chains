using UnityEngine;

namespace HollowChains.Core.Events
{
    /// <summary>Event channel with a float payload, e.g. HP as 0–1 for the HUD bar.</summary>
    [CreateAssetMenu(menuName = "Hollow Chains/Events/Float Event Channel", fileName = "EVT_NewFloat")]
    public sealed class FloatEventChannelSO : EventChannelSO<float> { }
}
