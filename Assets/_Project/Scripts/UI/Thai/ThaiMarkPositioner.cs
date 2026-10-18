using TMPro;
using UnityEngine;

namespace HollowChains.UI.Thai
{
    /// <summary>
    /// Fixes Thai tone marks / vowels in TextMeshPro (AC-02): raises tone marks above upper vowels and sara am,
    /// moves marks off the tall stroke of ป ฝ ฟ ฬ, drops lower vowels under ฎ ฏ. Works on the final vertices, so it
    /// is font-independent. Setup: add next to any TMP text that can show Thai (all UI text in the demo).
    /// </summary>
    [ExecuteAlways]
    [RequireComponent(typeof(TMP_Text))]
    public sealed class ThaiMarkPositioner : MonoBehaviour
    {
        [Tooltip("Gap between stacked marks, as a fraction of the font size.")]
        [SerializeField, Range(0f, 0.2f)] private float _stackGap = 0.04f;
        [Tooltip("How far marks move left on ป ฝ ฟ ฬ, as a fraction of the consonant width.")]
        [SerializeField, Range(0f, 0.6f)] private float _tallShift = 0.32f;

        private TMP_Text _text;

        private void OnEnable()
        {
            _text = GetComponent<TMP_Text>();
            _text.OnPreRenderText += Adjust;
            _text.SetVerticesDirty();
        }

        private void OnDisable()
        {
            if (_text != null) _text.OnPreRenderText -= Adjust;
        }

        private void Adjust(TMP_TextInfo info)
        {
            int count = info.characterCount;
            if (count == 0) return;

            // Characters as TMP sees them (after rich-text tags are removed), so rule indices match characterInfo.
            var chars = new char[count];
            for (int i = 0; i < count; i++) chars[i] = info.characterInfo[i].character;
            string plain = new string(chars);

            for (int i = 0; i < count; i++)
            {
                ref var ci = ref info.characterInfo[i];
                if (!ci.isVisible) continue;
                float size = ci.pointSize * ci.scale;
                float gap = _stackGap * size;
                float dx = 0f, dy = 0f;

                switch (ThaiMarkRules.VerticalAction(plain, i))
                {
                    case ThaiMarkAction.StackOnPrevious:
                        dy = RaiseOver(info, i - 1, i, gap);
                        break;
                    case ThaiMarkAction.StackOnNext:
                        dy = RaiseOver(info, i + 1, i, gap);
                        break;
                    case ThaiMarkAction.DropBelowBase:
                        int b = ThaiMarkRules.BaseIndex(plain, i);
                        float baseBottom = Bottom(info, b);
                        float markTop = Top(info, i);
                        if (markTop > baseBottom - gap) dy = baseBottom - gap - markTop;
                        break;
                }

                if (ThaiMarkRules.ShiftLeft(plain, i))
                {
                    int b = ThaiMarkRules.BaseIndex(plain, i);
                    dx = -_tallShift * Width(info, b);
                }

                if (dx != 0f || dy != 0f) Move(info, i, new Vector3(dx, dy, 0f));
            }
        }

        private static float RaiseOver(TMP_TextInfo info, int below, int mark, float gap)
        {
            if (below < 0 || below >= info.characterCount || !info.characterInfo[below].isVisible) return 0f;
            float delta = Top(info, below) + gap - Bottom(info, mark);
            return delta > 0f ? delta : 0f;
        }

        private static Vector3[] Verts(TMP_TextInfo info, int i, out int start)
        {
            var ci = info.characterInfo[i];
            start = ci.vertexIndex;
            return info.meshInfo[ci.materialReferenceIndex].vertices;
        }

        private static float Top(TMP_TextInfo info, int i) { var v = Verts(info, i, out int s); return v[s + 1].y; }

        private static float Bottom(TMP_TextInfo info, int i) { var v = Verts(info, i, out int s); return v[s].y; }

        private static float Width(TMP_TextInfo info, int i) { var v = Verts(info, i, out int s); return v[s + 2].x - v[s].x; }

        private static void Move(TMP_TextInfo info, int i, Vector3 offset)
        {
            var v = Verts(info, i, out int s);
            for (int k = 0; k < 4; k++) v[s + k] += offset;
        }
    }
}
