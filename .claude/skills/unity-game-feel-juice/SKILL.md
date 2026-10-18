---
name: unity-game-feel-juice
description: Game feel for Hollow Chains — camera shake (Cinemachine Impulse), hit-stop, screen flashes, slow-mo, gamepad rumble, timing of emotional beats. Use when a moment must hit harder (wolf bite, roots burst, Elena's rejection), when tuning feedback, or adding shake/flash/rumble.
---

# Game Feel / Juice (Hollow Chains, skill #42)

The demo has no combat, so "juice" serves emotion, not damage numbers. Less is more: the Awakening should feel
violent, everything else quiet.

- **Shake:** `CinemachineImpulseSource` on the event object + `CinemachineImpulseListener` on cameras. Bite = one
  sharp impulse; roots burst = long rumble that fades.
- **Hit-stop / slow-mo:** briefly scale `Time.timeScale` (0.05–0.15 s) through a single `TimeScaleService` so pause
  and cutscenes don't fight; never leave timeScale ≠ 1 after a cutscene.
- **Screen flash:** full-screen UI image (white/purple) fading over 0.2–0.6 s — the flashback (SC-05).
- **Rumble:** `Gamepad.current?.SetMotorSpeeds(low, high)` for ≤ 0.5 s, always reset on pause/exit.
- **Silence:** cutting all audio before the chain sound is part of the juice (`06` SC-05 step 1).
- Accessibility (P6): option to reduce shake/flash.
