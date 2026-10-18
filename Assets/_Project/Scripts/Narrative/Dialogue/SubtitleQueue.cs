using System;
using System.Collections.Generic;
using HollowChains.Data.Dialogue;

namespace HollowChains.Narrative.Dialogue
{
    /// <summary>
    /// Timing logic of subtitles, kept free of MonoBehaviour so it can be unit-tested: lines play one after another
    /// for their duration. Priority lines (story, inspect) interrupt; ambient lines (barks) are dropped when busy.
    /// </summary>
    public sealed class SubtitleQueue
    {
        private readonly Queue<DialogueLineSO> _queue = new Queue<DialogueLineSO>();
        private float _remaining;

        public DialogueLineSO Current { get; private set; }

        public bool IsBusy => Current != null || _queue.Count > 0;

        /// <summary>Raised when a line appears (with the line) and when the screen clears (with null).</summary>
        public event Action<DialogueLineSO> LineChanged;

        /// <summary>Story/inspect line: clears pending lines and shows this one now.</summary>
        public void PlayNow(DialogueLineSO line)
        {
            if (line == null) return;
            _queue.Clear();
            Show(line);
        }

        /// <summary>Queues a line after the current ones (multi-line conversations).</summary>
        public void Enqueue(DialogueLineSO line)
        {
            if (line == null) return;
            if (Current == null) Show(line);
            else _queue.Enqueue(line);
        }

        /// <summary>Ambient line: shown only if nothing else is on screen. Returns whether it was shown.</summary>
        public bool TryPlayAmbient(DialogueLineSO line)
        {
            if (line == null || IsBusy) return false;
            Show(line);
            return true;
        }

        public void Tick(float deltaTime)
        {
            if (Current == null) return;
            _remaining -= deltaTime;
            if (_remaining > 0f) return;

            if (_queue.Count > 0) Show(_queue.Dequeue());
            else
            {
                Current = null;
                LineChanged?.Invoke(null);
            }
        }

        private void Show(DialogueLineSO line)
        {
            Current = line;
            _remaining = line.Duration;
            LineChanged?.Invoke(line);
        }
    }
}
