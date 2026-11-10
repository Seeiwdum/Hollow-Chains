using System;
using System.Collections;
using HollowChains.Core.Services;
using HollowChains.Data.Dialogue;
using HollowChains.Narrative.Dialogue;
using UnityEngine;

namespace HollowChains.Narrative.Comic
{
    /// <summary>
    /// Placeholder page: plays narration lines as subtitles on a black screen, one after another (CANON text, `02` §4).
    /// Line timing runs on scaled time, so it stops while the game is paused (Time.timeScale 0). The subtitle service
    /// keeps counting during a pause, so after a pause the line on screen is shown again from its start (BUG-605).
    /// </summary>
    public sealed class SubtitleComicPage : MonoBehaviour, IComicPage
    {
        [SerializeField] private DialogueLineSO[] _lines;
        [SerializeField, Min(0f)] private float _gapSeconds = 0.6f;

        private Coroutine _routine;
        private Action _onFinished;

        public void Play(Action onFinished)
        {
            _onFinished = onFinished;
            _routine = StartCoroutine(Run());
        }

        public void Skip()
        {
            if (_routine != null) StopCoroutine(_routine);
            Finish();
        }

        private IEnumerator Run()
        {
            ServiceLocator.TryGet<IDialogueService>(out var dialogue);
            foreach (var line in _lines)
            {
                if (line == null) continue;
                dialogue?.PlayNow(line);
                float remaining = line.Duration + _gapSeconds;
                bool wasPaused = false;
                while (remaining > 0f)
                {
                    yield return null;
                    if (Time.timeScale <= 0f) { wasPaused = true; continue; }
                    if (wasPaused)
                    {
                        wasPaused = false;
                        // The subtitle may have timed out behind the pause menu: show the line again in full,
                        // unless only the silent gap after it was left.
                        if (remaining > _gapSeconds)
                        {
                            dialogue?.PlayNow(line);
                            remaining = line.Duration + _gapSeconds;
                        }
                    }
                    remaining -= Time.deltaTime;
                }
            }
            Finish();
        }

        private void Finish()
        {
            var callback = _onFinished;
            _onFinished = null;
            callback?.Invoke();
        }
    }
}
