---
name: unity-tmp-thai-localization
description: Thai text in Hollow Chains — TextMeshPro Thai font assets (Dynamic SDF + Latin fallback), tone mark/vowel stacking checks, Unity Localization string tables (locale th primary, en later), LocalizedString usage. Use whenever text appears on screen, a font is imported, a string table is edited, or Thai renders as boxes or with overlapping marks.
---

# TMP Thai + Localization (Hollow Chains, skill #28)

Demo locale = **th** (D-025); `en` is added later. Acceptance AC-02: tone marks/vowels must not overlap.

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
