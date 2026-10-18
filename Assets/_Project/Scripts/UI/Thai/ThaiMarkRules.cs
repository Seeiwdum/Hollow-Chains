namespace HollowChains.UI.Thai
{
    /// <summary>What a Thai combining mark must do so it does not collide with its neighbours.</summary>
    public enum ThaiMarkAction
    {
        None,
        /// <summary>Tone mark after an upper vowel (ที่, กิ้): sit on top of the previous mark.</summary>
        StackOnPrevious,
        /// <summary>Tone mark before sara am (น้ำ): sit on top of the next character's nikhahit ring.</summary>
        StackOnNext,
        /// <summary>Lower vowel under a descender consonant (ฎุ, ฏู): drop below the descender.</summary>
        DropBelowBase,
    }

    /// <summary>
    /// Pure classification rules for Thai mark placement (no Unity types → unit-testable). Used by
    /// <see cref="ThaiMarkPositioner"/>, which applies them to TextMeshPro vertices because TMP in this Unity version
    /// does not import the font's GPOS mark-to-base / mark-to-mark data for dynamic font assets.
    /// </summary>
    public static class ThaiMarkRules
    {
        public static bool IsUpperVowel(char c) => c == 'ั' || (c >= 'ิ' && c <= 'ื') || c == '็';

        public static bool IsToneMark(char c) => c >= '่' && c <= '๎';

        public static bool IsLowerVowel(char c) => c >= 'ุ' && c <= 'ฺ';

        public static bool IsUpperMark(char c) => IsUpperVowel(c) || IsToneMark(c);

        public static bool IsMark(char c) => IsUpperMark(c) || IsLowerVowel(c);

        /// <summary>Consonants with a tall right stroke: marks above them must move left (ปี, ฝั่ง, ฟ้า, ฬ).</summary>
        public static bool IsTallConsonant(char c) => c == 'ป' || c == 'ฝ' || c == 'ฟ' || c == 'ฬ';

        /// <summary>Consonants with a descender: lower vowels must drop below it (ฎ, ฏ).</summary>
        public static bool IsDescenderConsonant(char c) => c == 'ฎ' || c == 'ฏ';

        public const char SaraAm = 'ำ';

        /// <summary>Vertical rule for the character at <paramref name="index"/> in <paramref name="text"/>.</summary>
        public static ThaiMarkAction VerticalAction(string text, int index)
        {
            if (string.IsNullOrEmpty(text) || index < 0 || index >= text.Length) return ThaiMarkAction.None;
            char c = text[index];

            if (IsToneMark(c))
            {
                if (index > 0 && IsUpperVowel(text[index - 1])) return ThaiMarkAction.StackOnPrevious;
                if (index + 1 < text.Length && text[index + 1] == SaraAm) return ThaiMarkAction.StackOnNext;
                return ThaiMarkAction.None;
            }

            if (IsLowerVowel(c))
            {
                int b = BaseIndex(text, index);
                if (b >= 0 && IsDescenderConsonant(text[b])) return ThaiMarkAction.DropBelowBase;
            }
            return ThaiMarkAction.None;
        }

        /// <summary>True when an upper mark sits on a tall consonant and must shift left.</summary>
        public static bool ShiftLeft(string text, int index)
        {
            if (string.IsNullOrEmpty(text) || index < 0 || index >= text.Length || !IsUpperMark(text[index])) return false;
            int b = BaseIndex(text, index);
            return b >= 0 && IsTallConsonant(text[b]);
        }

        /// <summary>Index of the consonant a mark belongs to (walks back over other marks), or -1.</summary>
        public static int BaseIndex(string text, int index)
        {
            for (int i = index - 1; i >= 0; i--)
                if (!IsMark(text[i])) return i;
            return -1;
        }
    }
}
