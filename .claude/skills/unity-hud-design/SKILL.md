---
name: unity-hud-design
description: HUD for the Hollow Chains demo — HP bar only (D-002), critical-HP vignette, interact prompt, quest tracker that appears on change, no Humanity or Polarity UI. Use when adding or changing anything on the in-game HUD.
---

# HUD (Hollow Chains, skill #29)

- **Shows:** HP bar (bottom-left, thin, no numbers), interact prompt, quest objective tracker (top-left, fades in
  on `OnObjectiveChanged`, fades out after ~6 s, re-shown from Pause → Quest Log).
- **Never shows:** Humanity, Polarity, minimap, level/EXP (D-002, AC-07).
- Critical HP (SC-04, HP = 1): red vignette via `VP_Critical` volume weight + heartbeat SFX — driven by
  `OnHealthChanged` (int channel), not by the HUD polling the player.
- All HUD elements are `UIView`s listening to channels (unity-ugui-canvas skill). Style per Art Bible §7:
  cream on 70 % black, thin lines, quiet.
