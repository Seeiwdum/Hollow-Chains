---
name: unity-urp-lighting-postfx
description: URP 17.3 lighting and post-processing for Hollow Chains — per-scene Volume profiles (cold village, warm Elena moments, forest, critical HP, awakening), fog, light setup, palette from the Art Bible. Use when lighting a scene or greybox, creating or adjusting Volumes, or making a mood shift (cold to warm, critical, awakening).
---

# URP Lighting & PostFX (Hollow Chains, skill #36)

Source of truth: `Docs/P1/ART_BIBLE.md` §3 palette and §4 lighting/post per scene. Colour = emotion.

- URP assets in `Assets/_Project/Settings/` (`PC_RPAsset`, `PC_Renderer`); PC only. Linear colour space.
- One Global Volume per content scene; profiles in `Assets/_Project/Settings/Volumes/`:
  `VP_Village_Cold`, `VP_Warm_Elena` (blended by weight), `VP_Forest`, `VP_Critical`, `VP_Awakening`, `VP_Ending`.
  Mood shifts animate `Volume.weight` (code or Timeline) instead of swapping profiles abruptly.
- Typical overrides: Color Adjustments, Tonemapping (ACES), Vignette, low Bloom; Chromatic Aberration + Lens
  Distortion only for Awakening.
- Fog: exponential scene fog tinted per palette; forest denser.
- One directional light per scene; greybox stays realtime, baking later.
- Keep post cheap for the 60 FPS GTX 1060 target; no heavy effects until profiled in P6.
