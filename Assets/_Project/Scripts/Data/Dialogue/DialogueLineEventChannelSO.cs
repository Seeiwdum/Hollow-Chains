using HollowChains.Core.Events;
using UnityEngine;

namespace HollowChains.Data.Dialogue
{
    /// <summary>
    /// "Please play this line": raised by gameplay (inspect props, triggers) and handled by the DialogueRunner,
    /// so gameplay never references the narrative assembly.
    /// </summary>
    [CreateAssetMenu(menuName = "Hollow Chains/Events/Dialogue Line Event Channel", fileName = "EVT_PlayLine")]
    public sealed class DialogueLineEventChannelSO : EventChannelSO<DialogueLineSO> { }
}
