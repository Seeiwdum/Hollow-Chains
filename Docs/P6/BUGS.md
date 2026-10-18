# P6 — Bug list (Bug bash, B-601)

Sev: **S1** crash/blocker · **S2** breaks a story beat or progress · **S3** wrong but playable · **S4** cosmetic

## Run 1 — 2026-10-19 (team, MCP play-mode run)
Full flow: Title → New Game → S00 (skip) → S01 (watch, bucket) → S02 (CP_01, Elena thought + warm tone, CP_02) →
S03 (bucket carried, scream, bucket drops, sprint zone) → S04 climax (3 beats, HP 1, Soul Drain, quest complete) →
End screen → Quit to Title → Continue (S03, Q001-P2). **No errors or exceptions in the console.** Code review of flow,
scene loading, save, quest, player state, dialogue, comic and cutscene scripts.

| ID | Sev | Scene | Steps | Expected | Actual | Status |
|---|---|---|---|---|---|---|
| BUG-601 | S3 | S00 → S01 | New Game | The objective is shown when the player can act (S01) | The tracker showed it during the New Game fade/black intro and auto-hid it; nothing in S01 | ✅ fixed — QuestService re-shows the objective on every scene load and keeps it hidden in S00 (`_scenesWithoutTracker`) · re-tested: S00 alpha 0, S01 alpha 1 |
| BUG-602 | S3 | S04 | Climax beat → Esc (pause) → press/hold E | Beat waits until the game is resumed | `InteractiveBeatRunner` read Interact directly, so E behind the pause menu completed the beat | ✅ fixed — runner ignores input while `timeScale` = 0 · code-verified (simulated key input does not reach the unfocused Editor) → **owner test point** |
| BUG-603 | S3 | S04 → Title | Bitten (HP 1) → Esc → Quit to Title | Title screen without the red tint | Red critical tint stayed on the title (the player was destroyed, no HP event cleared it) | ✅ fixed — CriticalHealthVolume clears on "Title" · re-tested: 1.00 → 0.00 |
| BUG-604 | S4 | any → Title | Quit to Title while a subtitle is on screen | Subtitle clears | The current line finishes on the title screen | open (cheap; do with B-604 glyph work) |
| BUG-605 | S4 | S00 | Esc during the intro | — | Pause works but the narration keeps running (unscaled time) | open — acceptable for the demo |
