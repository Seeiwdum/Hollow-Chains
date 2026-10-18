---
name: unity-testing-qa
description: Testing and QA for Hollow Chains — EditMode/PlayMode tests with the Unity Test Framework, running tests through MCP or batchmode CLI, MCP play-mode stepping tests, bug bash, bug report format and triage (S1–S4), debug/cheat tools. Use when writing tests, verifying a phase, running a bug bash, triaging bugs, or adding a debug shortcut.
---

# Testing & QA (Hollow Chains, skill #13)

## Running tests
- **MCP (Editor open):** `TestRunnerApi` + `ExecutionSettings{ runSynchronously = true }`, collect results in an
  `ICallbacks` that writes to an instance field (statics/nested classes get hoisted by the tool).
- **CLI (Editor closed):** `Unity.exe -batchmode -nographics -projectPath D:/Git/Hollow-Chains -runTests
  -testPlatform EditMode -testResults <xml> -logFile <log>` (Unity at `D:/Unity/Hub/Editor/6000.3.10f1`). Fails if the
  project is open in the Editor.
- Tests live in `Assets/_Project/Tests/EditMode` (plain C# models: HealthModel, QuestRunner, SubtitleQueue,
  ThaiMarkRules, SaveFile). Logic that needs testing goes into a plain class first, MonoBehaviour second.

## Play-mode checks through MCP
Enter play paused, loop `EditorApplication.Step()` (deltaTime 0.02), subscribe to event channels to log what fired,
teleport the player by disabling the CharacterController first. `JobTempAlloc` / "PlayerLoop recursively" warnings are
harness noise. `PlayableDirector.state` reads Paused while the Editor is paused — use `director.time`.

## Bug report (Docs/P6/BUGS.md)
`| ID | Sev | Scene | Steps | Expected | Actual | Status |` — Sev: **S1** crash/blocker, **S2** breaks a story beat
or progress, **S3** wrong but playable, **S4** cosmetic. Fix S1–S2 before the demo build; S3 when cheap.

## Debug tools (dev builds only)
Guard with `#if DEVELOPMENT_BUILD || UNITY_EDITOR`. F-keys: skip cutscene beat (`InteractiveBeatRunner.Complete`),
jump to scene, heal. Never ship them in the release build.
