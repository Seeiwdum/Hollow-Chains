using System;
using HollowChains.Core.Input;
using HollowChains.Data.Input;
using NUnit.Framework;
using UnityEngine;

namespace HollowChains.Tests.Glyphs
{
    public class GlyphResolverTests
    {
        private sealed class FakeLookup : IGlyphLookup
        {
            public bool TryGetGlyph(string action, string scheme, out string glyph)
            {
                glyph = null;
                switch (action + "|" + scheme)
                {
                    case "Interact|kbm": glyph = "[E]"; break;
                    case "Interact|gamepad": glyph = "(Y)"; break;
                    case "Pause|kbm": glyph = "[Esc]"; break;
                    case "Pause|gamepad": glyph = "(Start)"; break;
                }
                return glyph != null;
            }
        }

        private GlyphResolver _resolver;

        [SetUp]
        public void Setup() => _resolver = new GlyphResolver(new FakeLookup());

        [Test]
        public void Keyboard_ReplacesToken() =>
            Assert.AreEqual("[E]  Pick up", _resolver.Resolve("{key:Interact}  Pick up", InputSchemes.KeyboardMouse));

        [Test]
        public void Gamepad_ReplacesToken() =>
            Assert.AreEqual("(Y)  Pick up", _resolver.Resolve("{key:Interact}  Pick up", InputSchemes.Gamepad));

        [Test]
        public void UnknownToken_IsLeftIntact() =>
            Assert.AreEqual("Press {key:Nope} now", _resolver.Resolve("Press {key:Nope} now", InputSchemes.Gamepad));

        [Test]
        public void MultipleTokens_AreAllReplaced() =>
            Assert.AreEqual("(Start) or (Y)", _resolver.Resolve("{key:Pause} or {key:Interact}", InputSchemes.Gamepad));

        [Test]
        public void KnownAndUnknownTokens_MixCorrectly() =>
            Assert.AreEqual("[Esc] {key:Nope}", _resolver.Resolve("{key:Pause} {key:Nope}", InputSchemes.KeyboardMouse));

        [Test]
        public void NoToken_ReturnsSameText() =>
            Assert.AreEqual("Plain {0} text", _resolver.Resolve("Plain {0} text", InputSchemes.Gamepad));

        [Test]
        public void NullAndEmpty_PassThrough()
        {
            Assert.IsNull(_resolver.Resolve(null, InputSchemes.KeyboardMouse));
            Assert.AreEqual(string.Empty, _resolver.Resolve(string.Empty, InputSchemes.KeyboardMouse));
        }

        [Test]
        public void NullLookup_Throws() => Assert.Throws<ArgumentNullException>(() => new GlyphResolver(null));

        [Test]
        public void ReResolvingSameSource_FollowsSchemeChange()
        {
            const string source = "{key:Interact}";
            Assert.AreEqual("[E]", _resolver.Resolve(source, InputSchemes.KeyboardMouse));
            Assert.AreEqual("(Y)", _resolver.Resolve(source, InputSchemes.Gamepad));
        }
    }

    public class InputGlyphTableTests
    {
        private InputGlyphTableSO _table;

        [SetUp]
        public void Setup()
        {
            _table = ScriptableObject.CreateInstance<InputGlyphTableSO>();
            var so = new UnityEditor.SerializedObject(_table);
            var entries = so.FindProperty("_entries");
            Add(entries, "Interact", "kbm", "[E]");
            Add(entries, "Interact", "gamepad", "(Y)");
            Add(entries, "Look", "kbm", "[Mouse]");
            so.ApplyModifiedPropertiesWithoutUndo();
        }

        [TearDown]
        public void TearDown() => UnityEngine.Object.DestroyImmediate(_table);

        private static void Add(UnityEditor.SerializedProperty list, string action, string scheme, string glyph)
        {
            list.InsertArrayElementAtIndex(list.arraySize);
            var e = list.GetArrayElementAtIndex(list.arraySize - 1);
            e.FindPropertyRelative("Action").stringValue = action;
            e.FindPropertyRelative("Scheme").stringValue = scheme;
            e.FindPropertyRelative("Glyph").stringValue = glyph;
        }

        [Test]
        public void Lookup_IsCaseInsensitive()
        {
            Assert.IsTrue(_table.TryGetGlyph("interact", "GAMEPAD", out var glyph));
            Assert.AreEqual("(Y)", glyph);
        }

        [Test]
        public void MissingSchemeRow_FallsBackToKeyboard()
        {
            Assert.IsTrue(_table.TryGetGlyph("Look", "gamepad", out var glyph));
            Assert.AreEqual("[Mouse]", glyph);
        }

        [Test]
        public void UnknownAction_ReturnsFalse() => Assert.IsFalse(_table.TryGetGlyph("Nope", "kbm", out _));
    }
}
