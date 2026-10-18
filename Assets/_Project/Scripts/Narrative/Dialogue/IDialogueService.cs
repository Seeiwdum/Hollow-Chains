using HollowChains.Core.Services;
using HollowChains.Data.Dialogue;

namespace HollowChains.Narrative.Dialogue
{
    /// <summary>Shows subtitles / thoughts / barks. Lives in _Boot.</summary>
    public interface IDialogueService : IService
    {
        bool IsBusy { get; }

        void PlayNow(DialogueLineSO line);

        void Enqueue(DialogueLineSO line);

        bool TryPlayAmbient(DialogueLineSO line);
    }
}
