---
name: unity-csharp-architecture
description: C# architecture rules for Hollow Chains — assembly definitions, namespaces, ScriptableObject-driven data, ScriptableObject event channels (Event Bus), state machines, service access without singleton sprawl, coding style and the "how to use" doc each script needs. Use this before writing or reviewing any C# in the project, when designing a new system (quest, dialogue, save, HUD, combat, form swap), or when deciding where code should live.
---

# Unity C# Architecture (Hollow Chains, skill #2)

00 §8 asks for separated assemblies, SO-driven data, an Event Bus, state machines, and no spaghetti or singleton
sprawl. These rules make that concrete so every system plugs together the same way.

## Assemblies (`Assets/_Project/Scripts/<Name>/<Name>.asmdef`)
| Assembly | Depends on | Holds |
|---|---|---|
| `HollowChains.Core` | — | Event channels, state machine, service locator, utilities, IDs |
| `HollowChains.Data` | Core | ScriptableObject definitions (QuestSO, DialogueSO, ItemSO, CharacterStatsSO) |
| `HollowChains.Gameplay` | Core, Data | Player, camera, interaction, quest runtime, Humanity, checkpoint |
| `HollowChains.Narrative` | Core, Data | Dialogue/subtitle runner, Timeline signal receivers, ComicPlayer hook (`IComicPage`) |
| `HollowChains.UI` | Core, Data | HUD, menus, TMP Thai helpers |
| `HollowChains.Combat` | Core, Data | (P4) turn engine, form swap — kept separate so the demo builds without it |
| `HollowChains.Editor` | all, Editor-only | Tools, validators |
| `HollowChains.Tests.*` | as needed | EditMode / PlayMode tests |
Dependencies point toward Core only. UI never references Gameplay directly; it listens to events.

## OOP & SOLID (owner requirement, D-020)
The owner wants clean, object-oriented code that is easy to read and extend. Apply the four pillars deliberately,
and remember Unity is component-based, so **composition first, inheritance only where there is a real "is-a"**.
- **Encapsulation** — state is `[SerializeField] private`; expose read-only properties and intent-revealing methods
  (`health.TakeDamage(1)`, not `health.current = 1`). No public mutable fields.
- **Abstraction** — systems talk through interfaces (`IInteractable`, `IState`, `ISaveable`, `IDamageable`,
  `IComicPage`, `IService`). Callers never type-check concrete classes.
- **Inheritance** — shallow (≤ 2 levels under Unity's base types) and only for shared behaviour + template methods
  (`InteractableBase`, `UIView`, `EventChannelSO<T>`, `PlayerState`). Prefer adding a component over a subclass.
- **Polymorphism** — new behaviour = new class implementing an interface/overriding an abstract method, not a new
  `switch`/`if` on a type or enum. (Open/Closed)
- SOLID: one reason to change per class (split input, movement, animation); depend on abstractions; small
  interfaces; subclasses must work anywhere the base works.
- Keep `Docs/P1/CLASS_DIAGRAM.md` (Mermaid) in sync: any new public type, interface or relationship gets added to the
  diagram in the same commit.

## Communication
- **Event channels**: `ScriptableObject` assets (`VoidEventChannelSO`, `IntEventChannelSO`, `QuestEventChannelSO`…)
  stored in `Data/Events/`. Raisers and listeners reference the asset, not each other. Unsubscribe in `OnDisable`.
- **Services**: a small `ServiceLocator` in Core registered from a `_Boot` scene for true globals (SaveService,
  AudioService). No `static Instance` singletons on MonoBehaviours.
- **State machines**: plain C# `IState { Enter(); Tick(); Exit(); }` + `StateMachine` in Core; used by player, quest
  phases and cutscene flow.

## Data
- Design data in ScriptableObjects; runtime state in plain serializable classes (so save = serialize state, not SOs).
- Stable string IDs (`Q001`, `Q001-P2`, `ITEM_BUCKET`) on every SO that save data or events refer to.

## Style
- Namespace = assembly name (+ subfolder). `PascalCase` types/methods, `_camelCase` private fields,
  `[SerializeField] private` over public fields. One public type per file, file name = type name.
- Thai text never hard-coded in C#; it lives in data/localisation tables.
- Avoid per-frame allocations in `Update`; cache components.
- Every MonoBehaviour gets an XML summary that says: what it does, which GameObject it goes on, required components /
  assets, and events it raises or listens to. That doubles as the "how to use" note 00 §8 requires.

## Before committing code
Compile with no errors or warnings (check `Unity_GetConsoleLogs`), run EditMode tests if the assembly has them,
and make sure no assembly reference cycle was introduced.
