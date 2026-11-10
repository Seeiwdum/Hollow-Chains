using System;
using System.Collections.Generic;
using HollowChains.Data.Dialogue;

namespace HollowChains.Narrative.Dialogue
{
    /// <summary>
    /// Timing logic of subtitles, kept free of MonoBehaviour so it can be unit-tested: lines play one after another
    /// for their duration. Priority lines (story, inspect) interrupt; ambient lines (barks) are dropped when busy.
    /// While muted (title screen, BUG-604) nothing is on screen and every new line is ignored.
    /// </summary>
    public sealed class SubtitleQueue
    {
        private readonly Queue<DialogueLineSO> _queue = new Queue<DialogueLineSO>();
        private float _remaining;

        public DialogueLineSO Current { get; private set; }

        public bool IsBusy => Current != null || _queue.Count > 0;

        /// <summary>True between <see cref="Mute"/> and <see cref="Unmute"/>: lines are ignored, not queued.</summary>
        public bool IsMuted { get; private set; }

        /// <summary>Raised when a line appears (with the line) and when the screen clears (with null).</summary>
        public event Action<DialogueLineSO> LineChanged;

        /// <summary>Story/inspect line: clears pending lines and shows this one now.</summary>
        public void PlayNow(DialogueLineSO line)
        {
            if (line == null || IsMuted) return;
            _queue.Clear();
            Show(line);
        }

        /// <summary>Queues a line after the current ones (multi-line conversations).</summary>
        public void Enqueue(DialogueLineSO line)
        {
            if (line == null || IsMuted) return;
            if (Current == null) Show(line);
            else _queue.Enqueue(line);
        }

        /// <summary>Ambient line: shown only if nothing else is on screen. Returns whether it was shown.</summary>
        public bool TryPlayAmbient(DialogueLineSO line)
        {
            if (line == null || IsMuted || IsBusy) return false;
            Show(line);
            return true;
        }

        /// <summary>Drops the pending lines and takes the current one off screen now (raises LineChanged(null) if one was showing).</summary>
        public void Clear()
        {
            _queue.Clear();
            _remaining = 0f;
            if (Current == null) return;
            Current = null;
            LineChanged?.Invoke(null);
        }

        /// <summary>Clears the screen and ignores every line until <see cref="Unmute"/> (a scene that is still unloading cannot re-show text).</summary>
        public void Mute()
        {
            IsMuted = true;
            Clear();
        }

        public void Unmute() => IsMuted = false;

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
