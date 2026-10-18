using HollowChains.Data.Localization;
using UnityEngine;
using UnityEngine.Localization;

namespace HollowChains.Data.Dialogue
{
    /// <summary>
    /// One subtitle line: who says it, the text, how long it stays. Thoughts (Phineas' inner voice) are shown in
    /// italics without a name. Text comes from the Thai "Dialogue" string table (D-025); the dev strings are fallbacks.
    /// </summary>
    [CreateAssetMenu(menuName = "Hollow Chains/Dialogue/Line", fileName = "DL_New")]
    public sealed class DialogueLineSO : GameDataSO
    {
        [Tooltip("Thai speaker name from the Dialogue table. Empty = use the dev name below.")]
        [SerializeField] private LocalizedString _localizedSpeaker = new LocalizedString();
        [Tooltip("Thai line from the Dialogue table. Empty = use the dev text below.")]
        [SerializeField] private LocalizedString _localizedText = new LocalizedString();
        [Header("Dev fallback (used when no table entry is set)")]
        [SerializeField] private string _speakerName;
        [SerializeField, TextArea(2, 4)] private string _text;
        [SerializeField] private bool _isThought;
        [Tooltip("Seconds on screen. 0 = automatic from text length.")]
        [SerializeField, Min(0f)] private float _duration;

        public string SpeakerName => LocalizedText.Resolve(_localizedSpeaker, _speakerName);
        public string Text => LocalizedText.Resolve(_localizedText, _text);
        public bool IsThought => _isThought;

        /// <summary>Explicit duration, or ~0.06 s per character clamped to 2–6 s.</summary>
        public float Duration
        {
            get
            {
                if (_duration > 0f) return _duration;
                var text = Text;
                return Mathf.Clamp(text != null ? text.Length * 0.06f : 2f, 2f, 6f);
            }
        }
    }
}
