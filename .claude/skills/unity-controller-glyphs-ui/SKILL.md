---
name: unity-controller-glyphs-ui
description: Device-aware button hints for Hollow Chains — detecting the last used device (keyboard/mouse vs gamepad) with the Input System, swapping "[E]" for the gamepad button in prompts, hints and skip text, TMP sprite or text glyphs. Use when any on-screen text names a button, or when prompts must change between keyboard and controller.
---

# Controller Glyphs (Hollow Chains, skill #34)

- One service decides the current scheme: listen to `InputSystem.onActionChange` / `InputUser.onChange` or check
  `action.activeControl.device` on performed → raise `EVT_InputSchemeChanged` (string "kbm" / "gamepad").
- Text never hard-codes a key: use a token like `{interact}` in localized strings and resolve it with
  `action.GetBindingDisplayString(group)` (group "Keyboard&Mouse" or "Gamepad"), or a glyph table SO
  (`InputGlyphSO`: binding path → text/sprite) for TMP `<sprite>` icons later.
- Places that show keys today: HUD interact prompt format `[E]  {0}`, `comic_skip`, `hint_move`, `hint_quest`,
  beat prompts. Update them all through the same resolver.
- Demo can use text glyphs (`[E]` / `(A)`, `[Esc]` / `(Start)`); sprite icons are polish if time allows.
