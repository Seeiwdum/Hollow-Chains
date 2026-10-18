---
name: unity-dialogue-system
description: Dialogue, subtitles, barks and internal monologue for Hollow Chains — DialogueLineSO data with Localization keys, DialogueRunner service, subtitle view, spatial barks, Timeline-driven lines. Use when adding any spoken or thought line, subtitle, villager bark, or when wiring dialogue into cutscenes.
---

# Dialogue System (Hollow Chains, skill #16)

The demo has **no branching** (Tech Spec §6), so the system stays light: a line has a speaker, text and a duration.
- `DialogueLineSO` (Data): `SpeakerId`, `LocalizedString Text`, `Duration` (auto from text length if 0).
- `DialogueRunner` (Narrative, in `_Boot`): queue of lines, raises `OnLineStarted` / `OnLineEnded` channels;
  `SubtitleView` (UI) listens and displays. Inspect thoughts and monologue use the same runner.
- `BarkEmitter` (Narrative): spatial villager whispers within a radius, cooldown, no immediate repeats.
- Timeline: a signal/clip calls the runner so subtitles sync with cutscenes (P5).
- CANON lines (Elena SC-06, father's message, SC-00 narration) are copied **exactly** from `06`/`02` into the Thai
  string table — never paraphrase (D-025).
- Writing voice: Thai, Phineas quiet and gentle; villagers cold; keep lines short for subtitles (≤ 2 lines).
