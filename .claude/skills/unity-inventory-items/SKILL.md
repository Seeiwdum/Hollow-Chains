---
name: unity-inventory-items
description: Light item handling for the Hollow Chains demo — ItemSO key items (the bucket ITEM_BUCKET, the pocket watch), carried-item state that survives scene loads and saves. Use when adding an item, making something carryable, or persisting what the player holds. Not a full inventory (out of demo scope).
---

# Items (Hollow Chains, skill #20 — light)

Demo scope (GDD §2): key items only, no inventory UI.
- `ItemSO` (Data): `Id`, `LocalizedString DisplayName`, `IsKeyItem`, optional carry prefab.
- `CarryState` service in `_Boot` (or part of a PlayerState service) remembers the carried item id across scenes;
  the next scene's player spawns holding it (bucket SC-01 → SC-03 until it drops, D-015).
- Pickups raise their event channel (`EVT_BucketPicked`) so quests react without referencing items.
- Save via `ISaveable`.
