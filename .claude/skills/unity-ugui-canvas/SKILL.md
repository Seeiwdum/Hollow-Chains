---
name: unity-ugui-canvas
description: uGUI + TextMeshPro UI for Hollow Chains — canvas setup, UIView base class, HUD (HP only), world-space interact prompts, menus, Thai text via Localization. Use when creating any screen, HUD element, prompt or menu, or when text, scaling or sorting looks wrong.
---

# uGUI Canvas (Hollow Chains, skill #27)

Tech Spec: uGUI + TMP for everything in the demo (TMP ships inside `com.unity.ugui` 2.0 in Unity 6).

- **Every Canvas needs a `GraphicRaycaster`** or the mouse cannot click anything (bug found in P3.3b — the
  canvas was created with only Canvas + CanvasScaler). The `_Boot` EventSystem uses `InputSystemUIInputModule`.
  Turn `raycastTarget` off on decorative graphics (labels, icons) so they never swallow clicks.
- Verify menus in play mode with `EventSystem.current.RaycastAll` at the button's screen point (mouse) and by
  checking `currentSelectedGameObject` + a Submit event (keyboard/gamepad).
- One `UI Root` in `_Boot`: Screen Space Overlay, Canvas Scaler = Scale With Screen Size 1920×1080, match 0.5.
  Separate child canvases for HUD / Menus / Subtitles so rebuilds stay local.
- Every screen/element derives from `UIView` (abstract `Show`/`Hide`, protected `OnShow`/`OnHide`) and listens only
  to event channels — no references to gameplay classes (D-020, class diagram §6).
- World-space prompt follows `Interactor.Focused`, faces the camera, shows the localized prompt + input glyph.
- All text through Localization (`LocalizedString` / `LocalizeStringEvent`) — never type text into TMP fields.
  Demo locale = `th` (D-025): Thai font asset + Latin fallback once UI-001/002 arrive; Thai font spike in P3.3.
- HUD shows HP only (D-002).
- Gamepad navigation: explicit first-selected object per menu.
