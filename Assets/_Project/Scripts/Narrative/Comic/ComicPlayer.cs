using System.Collections.Generic;
using HollowChains.Core.Services;
using UnityEngine;
using UnityEngine.InputSystem;

namespace HollowChains.Narrative.Comic
{
    /// <summary>
    /// Plays the intro comic pages in order, then loads the first gameplay scene. Holding Interact skips the intro.
    /// Setup: in S00_ComicIntro_Placeholder; list page components (anything implementing IComicPage).
    /// </summary>
    public sealed class ComicPlayer : MonoBehaviour
    {
        [Tooltip("Components implementing IComicPage, in order.")]
        [SerializeField] private List<MonoBehaviour> _pages = new List<MonoBehaviour>();
        [SerializeField] private string _nextScene = "S01_Bedroom";
        [SerializeField] private InputActionReference _skipAction;
        [SerializeField, Min(0.1f)] private float _holdToSkipSeconds = 1f;

        private int _index = -1;
        private float _held;
        private bool _done;

        private void OnEnable()
        {
            if (_skipAction != null) _skipAction.action.Enable();
        }

        private void Start() => Next();

        private void Update()
        {
            if (_done || _skipAction == null) return;
            _held = _skipAction.action.IsPressed() ? _held + Time.unscaledDeltaTime : 0f;
            if (_held >= _holdToSkipSeconds) SkipAll();
        }

        public void SkipAll()
        {
            if (_done) return;
            _done = true; // set first: the skipped page's callback (Next) must not start another page
            if (_index >= 0 && _index < _pages.Count && _pages[_index] is IComicPage page) page.Skip();
            LoadNext();
        }

        private void Next()
        {
            if (_done) return;
            _index++;
            if (_index >= _pages.Count) { Finish(); return; }
            if (_pages[_index] is IComicPage page) page.Play(Next);
            else Next();
        }

        private void Finish()
        {
            if (_done) return;
            _done = true;
            LoadNext();
        }

        private void LoadNext()
        {
            if (ServiceLocator.TryGet<ISceneFlowService>(out var flow)) flow.Load(_nextScene);
        }
    }
}
