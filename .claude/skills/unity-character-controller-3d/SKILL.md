---
name: unity-character-controller-3d
description: Third-person player controller for Hollow Chains — PlayerController, PlayerMotor on CharacterController, state machine states (Locomotion, Carry, Sprint, Locked), slopes, gravity, camera-relative movement, cutscene lock. Use when building or tuning player movement, adding a movement state, or fixing grounding, slope or collision issues.
---

# 3D Character Controller (Hollow Chains, skill #4)

The demo is about walking with feeling, not acrobatics: no jump, no combat. Movement must be smooth, readable and
lockable for cutscenes.

## Structure (Docs/P1/CLASS_DIAGRAM.md §3)
- `PlayerController` (composition root): owns a `StateMachine`, references `PlayerMotor`, `IPlayerInput`,
  later `Interactor` and `Health`. Holds no movement math.
- `PlayerMotor`: wraps Unity `CharacterController`; `Move(Vector3 worldDir, float speed)`, gravity, ground snap,
  acceleration, rotate toward movement. Knows nothing about input.
- States derive from `PlayerState : IState`: `LocomotionState` (walk/run), `SprintState` (only where allowed —
  SC-03), `CarryState` (slower, bucket), `LockedState` (cutscene/dialogue — no movement).
- Tunables in `PlayerMovementConfigSO` (walk ≈ 2.0 m/s, run ≈ 4.0, sprint ≈ 6.5, carry ×0.8, turn speed, accel).

## Rules
- Movement is camera-relative: project camera forward/right on the ground plane.
- `CharacterController` (not Rigidbody) for predictable narrow rooms/stairs; slope 45°, step 0.3.
- Gravity every frame; small negative vertical velocity when grounded to stay snapped.
- Sprint permission comes from data/events (e.g. a sprint zone), not hard-coded scene checks.
- Placeholder capsule: height 1.65 m, radius 0.3 (D-023).
