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
| BUG-604 | S4 | any → Title | Quit to Title while a subtitle is on screen | Subtitle clears | The current line finishes on the title screen | ✅ fixed P6.3 — DialogueRunner mutes the subtitle queue while the game state is Title · re-tested 2026-11-12 in Play mode: no line on Title, lines play again after Continue |
| BUG-605 | S4 | S00 | Esc during the intro | — | Pause works but the narration keeps running (unscaled time) | fixed P6.3 — comic page timer and hold-to-skip stop at timeScale 0; the current line replays on resume · re-tested 2026-11-12 with real keys: same line for 12 s of pause, hold E does not skip while paused |
| BUG-606 | S3 | S01 → Title → any | Stand where an interact prompt shows ("[E] มองใต้เตียง" at the S01 spawn) → Esc → Quit to Title → Continue or New Game | No interact prompt until the player is next to something | The old prompt stayed on the HUD in S03 and over the S00 comic and never cleared (the destroyed `Interactor` raised no empty prompt; the HUD lives in `_Boot`) | ✅ fixed 2026-11-12 — `Interactor.OnDisable` clears the focus and raises an empty prompt (skipped on application quit) · re-tested in Play mode: S03 and S00 clean · evidence `bug606-before/after…` · no automated test (no PlayMode assembly) |
| BUG-607 | S4 | S00 | New Game, watch the intro comic | No gameplay HUD over the comic | The HUD health bar (`UI Root/HUD/HealthBar`) is drawn bottom-left during the comic · evidence `bug607-healthbar-in-s00.jpg` · also: the dim skip hint stays visible under the pause menu in S00 | ✅ fixed 2026-11-12 — new `CanvasGroupGate` (UI): on `HUD/HealthBar` it hides the bar while the active scene is `S00_ComicIntro_Placeholder`; on S00 `SkipHint` it hides the hint while the state is `Paused` · re-tested in the Editor: `bug607-after-s00-no-healthbar.jpg`, `bug607-after-s00-pause-no-skip-hint.jpg`, `bug607-after-s01-healthbar-back.jpg` · EditMode 54/54 |
| BUG-608 | S4 | S00 | New Game → Esc during the intro comic | Pause menu without an empty quest block | The pause menu shows the "ภารกิจปัจจุบัน" heading with no objective under it (S00 has no quest; see `bug607-after-s00-pause-no-skip-hint.jpg`) | open — found while re-testing BUG-607, cosmetic |
