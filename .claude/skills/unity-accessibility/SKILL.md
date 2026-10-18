---
name: unity-accessibility
description: Accessibility for Hollow Chains — subtitle size and background, invert camera, reduce screen shake and flashes, hold-vs-toggle for hold beats, key remapping (Input System rebinding), colour-blind-safe UI, saving these in GameSettings. Use when adding or changing any option in Settings, any flash/shake/hold interaction, or reviewing UI readability.
---

# Accessibility (Hollow Chains, skill #33)

Demo minimum (`04` §6 #9, B-603): subtitle size ✅, invert Y ✅, **reduce flash/shake**, **hold beats can be pressed
instead**, remap = after the demo unless cheap.

- Every option lives in `GameSettings` (Core, PlayerPrefs) with a default, and is shown in `SettingsView`.
  Systems read `GameSettings.Current` — no option logic inside UI.
- **Reduce flashes:** `ScreenFlashView` scales its alpha (e.g. ×0.3) and the critical vignette pulses less.
- **Reduce shake:** scale `CinemachineImpulseSource` gain / skip rumble.
- **Hold beats:** with "press instead of hold" on, `InteractiveBeatRunner` treats Hold beats as Press.
- Subtitles: Thai text needs ≥ 26 px at 1080p, a dark backing panel, max 2 lines, speaker name coloured.
- Never rely on colour alone (critical HP = red tint + HP bar number/pulse).
- Remap (later): `InputActionRebindingExtensions.PerformInteractiveRebinding`, save overrides JSON in PlayerPrefs.
