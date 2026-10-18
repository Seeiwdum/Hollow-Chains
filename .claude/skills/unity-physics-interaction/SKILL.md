---
name: unity-physics-interaction
description: Interaction system for Hollow Chains — IInteractable, InteractableBase and subclasses (Pickup, Inspect, Scripted), Interactor focus detection with overlap queries, layers, trigger zones and prompts. Use when making anything the player can press a button on, adding a trigger volume, or debugging prompts that do not show.
---

# Physics & Interaction (Hollow Chains, skill #8)

## Design (Class diagram §3)
- `IInteractable { Prompt; CanInteract(Interactor); Interact(Interactor); }`
- `InteractableBase` (abstract) implements it as a template method: checks `CanInteract`, calls protected abstract
  `OnInteract`, raises its optional event channel. Subclasses: `PickupInteractable`, `InspectInteractable`,
  `ScriptedInteractable` (Timeline: hug in SC-04, reach hand in SC-06).
- `Interactor` (on player): every few frames `Physics.OverlapSphereNonAlloc` on layer `Interactable` within ~1.8 m;
  best candidate by distance + facing; exposes `Focused`, raises `FocusChanged` for the prompt UI; `TryInteract()`.

## Layers & triggers
- Layers: `Player`, `Interactable`, `Trigger`, `Environment`.
- `TriggerZone` component (isTrigger collider on layer `Trigger`) raises event channels on enter/exit — quest phase,
  sprint zone, checkpoint.
- Greybox colours (D-022): interactables yellow, triggers transparent blue (renderer off in builds).

## Rules
- Cache components; NonAlloc physics queries (no GC per frame).
- Interactables never reference the player class; they receive the `Interactor` and talk through events.
