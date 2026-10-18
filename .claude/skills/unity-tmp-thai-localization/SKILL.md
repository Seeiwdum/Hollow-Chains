---
name: unity-tmp-thai-localization
description: Thai text in Hollow Chains — TextMeshPro Thai font assets (Dynamic SDF + Latin fallback), tone mark/vowel stacking checks, Unity Localization string tables (locale th primary, en later), LocalizedString usage. Use whenever text appears on screen, a font is imported, a string table is edited, or Thai renders as boxes or with overlapping marks.
---

# TMP Thai + Localization (Hollow Chains, skill #28)

Demo locale = **th** (D-025); `en` is added later. Acceptance AC-02: tone marks/vowels must not overlap.

## Project facts (P3.3, verified)
- Fonts in use (D-033): `Sarabun-Regular SDF` (body/subtitles/prompts; Italic + SemiBold in its weight table),
  `Mali-Regular SDF` (headings, quest tracker), `AmaticSC-Bold SDF` (English logo, falls back to Mali for Thai).
  Assets in `Assets/_Project/UI/Fonts/`; TTF + OFL in `Assets/ThirdParty/Fonts/`. TMP Settings default = Sarabun, fallback = Mali.
- **TMP in Unity 6000.3 does not import GPOS mark-to-base/mark-to-mark for dynamic fonts** (the define
  `TEXTCORE_FONT_ENGINE_1_5_OR_NEWER` is off and the FontEngine calls are internal). Without a fix, tone marks vanish
  into upper vowels (ที่ → ทีี, ปู่ → ปู). Every TMP text that can show Thai **must have `ThaiMarkPositioner`**
  (UI assembly) — it fixes marks on the final vertices. Rules live in `ThaiMarkRules` (unit-tested).
- Known gap: ญ/ฐ + lower vowel still keep their tail (needs GSUB) — avoid in short UI strings or report.
- Visual check: `Assets/_Project/Scenes/Test_ThaiFont.unity` + render-to-PNG (see Docs/P3/img before/after).
- Locales: `th` (startup, SpecificLocaleSelector) and `en`. Tables: `UI`, `Dialogue`, `Quests`.
  Data uses `LocalizedString` + dev fallback string, resolved by `HollowChains.Data.Localization.LocalizedText`.
- Assigning a LocalizedString from an editor script: `serializedProperty.boxedValue = new LocalizedString(table, key)`.

## Fonts
- Use the owner-supplied fonts (UI-001 heading, UI-002 body). Verify license (OFL or equivalent) before import (D-004).
- Create TMP Font Assets with Atlas Population Mode **Dynamic**, sampling ~90, padding ~9, atlas 2048.
  Keep a Latin font as fallback (TMP Settings → fallback list) for digits/punctuation if the Thai font lacks them.
- Thai stacking: TMP needs the font's **GPOS/mark positioning**. Test strings with stacked marks
  (e.g. "ปู่", "ญี่ปุ่น", "น้ำ", "ที่นั่น", "ฟื้น") at small sizes; if marks overlap, try the font's
  "Thai shaping" variant or enable TMP's OpenType font features when available — check the TMP docs for the
  installed ugui version rather than assuming.
- Line breaking: Thai has no spaces between words — insert zero-width space (U+200B) at word boundaries in
  long strings, or keep subtitle lines short.

## Localization
- Tables: `UI`, `Dialogue_S01`…`S06`, `Quests`. Keys in English snake case (`q001_p1_objective`).
- Code uses `LocalizedString` fields; UI uses `LocalizeStringEvent` → TMP. No literal Thai in C# or prefabs.
- Locale `th` is the project/startup locale for the demo.
