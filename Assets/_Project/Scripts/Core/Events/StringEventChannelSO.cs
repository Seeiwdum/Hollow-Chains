using UnityEngine;

namespace HollowChains.Core.Events
{
    /// <summary>Event channel with a string payload, e.g. a quest phase ID or checkpoint ID.</summary>
    [CreateAssetMenu(menuName = "Hollow Chains/Events/String Event Channel", fileName = "EVT_NewString")]
    public sealed class StringEventChannelSO : EventChannelSO<string> { }
}
