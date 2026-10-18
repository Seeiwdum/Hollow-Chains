using TMPro;
using UnityEngine;
using UnityEngine.Localization;

namespace HollowChains.UI.Views
{
    /// <summary>
    /// Fixed UI text (button labels, headings) from a string table; updates automatically if the locale changes.
    /// Setup: next to a TMP text; pick the table entry (usually table "UI").
    /// </summary>
    [RequireComponent(typeof(TMP_Text))]
    public sealed class LocalizedLabel : MonoBehaviour
    {
        [SerializeField] private LocalizedString _text = new LocalizedString();

        private TMP_Text _label;

        private void OnEnable()
        {
            _label = GetComponent<TMP_Text>();
            if (!_text.IsEmpty) _text.StringChanged += OnStringChanged;
        }

        private void OnDisable()
        {
            if (!_text.IsEmpty) _text.StringChanged -= OnStringChanged;
        }

        private void OnStringChanged(string value) => _label.text = value;
    }
}
