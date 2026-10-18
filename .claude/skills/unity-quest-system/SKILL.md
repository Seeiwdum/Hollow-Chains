---
name: unity-quest-system
description: Phase-based quest system for Hollow Chains — QuestSO / QuestPhaseSO data, PhaseConditionSO completion conditions bound to event channels, QuestService runtime state that survives scene loads, tracker events for the HUD, save integration. Use when creating or changing quests (Q001 "น้ำจากลำธาร"), adding a completion condition, wiring a scene event to a quest phase, or debugging a phase that does not advance.
---

# Quest System (Hollow Chains, skill #15)

Quests are told in **phases** (`04` §2): finishing a phase opens the next one. The demo has one quest, Q001,
with five phases (GDD §4); the system must still be generic so post-demo quests are data, not code.

## Model (Class diagram §2/§4)
- `QuestSO` (Data): `Id` ("Q001"), title key, ordered `QuestPhaseSO[]`.
- `QuestPhaseSO` (Data): `Id` ("Q001-P1"), objective key, `PhaseConditionSO CompleteWhen`.
- `PhaseConditionSO` (abstract, Data): `Bind(Action onMet)` / `Unbind()`. Concrete conditions are small classes
  (`EventRaisedCondition` listens to a `VoidEventChannelSO`). New condition types = new subclass (Open/Closed).
- `QuestService` (Gameplay, lives in `_Boot`, registered as `IQuestService`): holds `QuestState` per quest
  (plain serializable: quest id, phase index, complete flag); binds the current phase's condition; on met →
  advance, rebind, raise `OnQuestPhaseChanged` (string channel: phase id) and `OnObjectiveChanged` (string:
  objective text/key) for the HUD tracker.

## Rules
- Conditions bind to **event channels**, so scene objects (triggers, interactables) never know about quests.
- QuestService is in `_Boot` → survives content scene loads; scenes only raise events.
- Runtime state never lives in the SO (SOs are shared assets; writing to them leaks between play sessions).
- Starting a quest from data: `QuestStarter` component or bootstrap config; avoid hard-coded ids in code.
- Save: QuestService implements `ISaveable` (P3.4).
- Test the state machine in EditMode with fake conditions (no scenes needed).
