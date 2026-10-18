---
name: unity-input-system
description: Hollow Chains input with the Unity Input System 1.18 — action maps (Player, UI, Cutscene), PlayerInputReader, KB/M + gamepad bindings, map switching by game state, rebinding later. Use whenever adding or changing controls, reading input in code, interactive cutscene beats (hold/mash), or debugging input that does not fire.
---

# Unity Input System (Hollow Chains, skill #3)

- Asset: `Assets/_Project/Settings/Input/InputSystem_Actions.inputactions`. Code reads actions through
  `InputActionReference` fields or a generated C# class — never string lookups scattered in gameplay code.
- Maps: **Player** (Move, Look, Sprint, Interact, Pause) · **UI** (Navigate, Submit, Cancel) ·
  **Cutscene** (Hold, Mash, Skip). Only one gameplay map enabled at a time; switching belongs to the game-flow
  state machine, not to individual scripts.
- `PlayerInputReader` (Gameplay) implements `IPlayerInput`: read-only `Move`, `Look`, `SprintHeld` and C# events
  `InteractPressed`, `PausePressed`. Other classes depend on `IPlayerInput`, never on the Input System directly —
  keeps input swappable and testable (D-020).
- Every action needs a KB/M and a Gamepad binding. Mouse delta and right stick need separate scaling.
- Enable actions in `OnEnable`, disable and unsubscribe in `OnDisable`.
- Active Input Handling = Input System only (P1.3) — `UnityEngine.Input` is unavailable.
