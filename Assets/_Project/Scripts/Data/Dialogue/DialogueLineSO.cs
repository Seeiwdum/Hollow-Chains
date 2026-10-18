using UnityEngine;

namespace HollowChains.Data.Dialogue
{
    /// <summary>
    /// One subtitle line: who says it, the text, how long it stays. Thoughts (Phineas' inner voice) are shown in
    /// italics without a name. Text is placeholder until the Thai string tables arrive (P3.3).
    /// </summary>
    [CreateAssetMenu(menuName = "Hollow Chains/Dialogue/Line", fileName = "DL_New")]
    public sealed class DialogueLineSO : GameDataSO
    {
        [SerializeField] private string _speakerName;
        [SerializeField, TextArea(2, 4)] private string _text;
        [SerializeField] private bool _isThought;
        [Tooltip("Seconds on screen. 0 = automatic from text length.")]
        [SerializeField, Min(0f)] private float _duration;

        public string SpeakerName => _speakerName;
        public string Text => _text;
        public bool IsThought => _isThought;

        /// <summary>Explicit duration, or ~0.06 s per character clamped to 2–6 s.</summary>
        public float Duration => _duration > 0f ? _duration : Mathf.Clamp(_text != null ? _text.Length * 0.06f : 2f, 2f, 6f);
    }
}
