using System;
using System.Collections;
using HollowChains.Core.Services;
using HollowChains.Data.Dialogue;
using HollowChains.Narrative.Dialogue;
using UnityEngine;

namespace HollowChains.Narrative.Comic
{
    /// <summary>Placeholder page: plays narration lines as subtitles on a black screen, one after another (CANON text, `02` §4).</summary>
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
                yield return new WaitForSecondsRealtime(line.Duration + _gapSeconds);
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
