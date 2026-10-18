using UnityEngine;

namespace HollowChains.Core.Events
{
    /// <summary>Event channel with a bool payload, e.g. "gameplay input enabled".</summary>
    [CreateAssetMenu(menuName = "Hollow Chains/Events/Bool Event Channel", fileName = "EVT_NewBool")]
    public sealed class BoolEventChannelSO : EventChannelSO<bool> { }
}
