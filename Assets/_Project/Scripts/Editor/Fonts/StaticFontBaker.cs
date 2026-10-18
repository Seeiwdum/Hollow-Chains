using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEditor;
using UnityEditor.Localization;
using UnityEngine;
using UnityEngine.Localization.Tables;

namespace HollowChains.Editor.Fonts
{
    /// <summary>
    /// Bakes the project's TMP font assets into static atlases (B-608): dynamic atlases grew and re-saved every time
    /// new text appeared in play mode, and a build must not depend on the source TTF at runtime.
    /// Character set = ASCII + the whole Thai block (U+0E01–U+0E5B) + every character used in any string table.
    /// Menu: Hollow Chains/Fonts/Bake Static Font Atlases — run again after adding text with new symbols.
    /// </summary>
    public static class StaticFontBaker
    {
        private const string FontFolder = "Assets/_Project/UI/Fonts";

        [MenuItem("Hollow Chains/Fonts/Bake Static Font Atlases")]
        public static void BakeAll()
        {
            string characters = CollectCharacters();
            var report = new StringBuilder($"chars={characters.Length}\n");
            foreach (var guid in AssetDatabase.FindAssets("t:TMP_FontAsset", new[] { FontFolder }))
            {
                var font = AssetDatabase.LoadAssetAtPath<TMP_FontAsset>(AssetDatabase.GUIDToAssetPath(guid));
                report.AppendLine(Bake(font, characters));
            }
            AssetDatabase.SaveAssets();
            Debug.Log("[StaticFontBaker] " + report);
        }

        public static string CollectCharacters()
        {
            var set = new SortedSet<char>();
            for (char c = (char)0x20; c <= 0x7E; c++) set.Add(c);
            for (char c = (char)0x0E01; c <= 0x0E5B; c++) set.Add(c);
            foreach (var c in "—–…·•‘’“”«»×") set.Add(c);
            foreach (var collection in LocalizationEditorSettings.GetStringTableCollections())
            foreach (var table in collection.StringTables)
            foreach (var entry in table.Values)
            {
                if (string.IsNullOrEmpty(entry.Value)) continue;
                foreach (var c in entry.Value)
                    if (!char.IsControl(c)) set.Add(c);
            }
            return new string(set.ToArray());
        }

        private static string Bake(TMP_FontAsset font, string characters)
        {
            if (font.sourceFontFile == null) return $"{font.name}: skipped (no source font)";
            font.atlasPopulationMode = AtlasPopulationMode.Dynamic;
            font.isMultiAtlasTexturesEnabled = false;
            font.ClearFontAssetData(true);
            font.TryAddCharacters(characters, out string missing);
            font.atlasPopulationMode = AtlasPopulationMode.Static;
            EditorUtility.SetDirty(font);
            if (font.atlasTexture != null) EditorUtility.SetDirty(font.atlasTexture);
            int notInFont = string.IsNullOrEmpty(missing) ? 0 : missing.Count(c => !char.IsWhiteSpace(c));
            return $"{font.name}: {font.characterTable.Count} glyphs, missing in font {notInFont}";
        }
    }
}
