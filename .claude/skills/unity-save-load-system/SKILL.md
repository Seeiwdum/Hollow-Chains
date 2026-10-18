---
name: unity-save-load-system
description: Checkpoint save/load for Hollow Chains — ISaveable contract, SaveService in _Boot, JSON save file with version, atomic write, checkpoints, Continue from title. Use when adding state that must persist (quests, HP, Humanity, carried item), adding a checkpoint, or debugging a save that does not load.
---

# Save / Load (Hollow Chains, skill #10)

- `ISaveable { string SaveKey; string CaptureState(); void RestoreState(string json); }` — each system serializes
  its own plain state class with `JsonUtility`. SaveService never knows concrete types (D-020).
- `SaveService` (Gameplay, `_Boot`, `ISaveService`): registers saveables, writes
  `Application.persistentDataPath/save_0.json` = `{ saveVersion, checkpointId, sceneName, entries[] }`.
  Write to `save_0.json.tmp` then replace — a crash never corrupts the old save.
- Checkpoints (GDD): CP#1 leaving the bedroom, CP#2 village gate. `CheckpointTrigger` (a `PlayerTriggerZone`)
  asks the service to save.
- Load: Title → Continue reads the file, restores saveables, then loads `sceneName` through `ISceneFlowService`.
- Bump `saveVersion` when a state shape changes; on mismatch start a new game rather than crash.
- EditMode test: round-trip a fake saveable through a temp folder.
