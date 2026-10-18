---
name: unity-menu-flow
description: Menus and game flow for Hollow Chains — Title (New/Continue/Settings/Quit), Pause (Resume/Quest Log/Settings/Quit to Title), minimal Settings, demo End screen, game-flow state machine and input map switching. Use when building or changing any menu, pausing, or the flow between title, gameplay, cutscene and end.
---

# Menu Flow (Hollow Chains, skill #31)

- Game flow = `StateMachine` (Core) with states Title / Playing / Cutscene / Paused / End, owned by a
  `GameFlowService` in `_Boot`. States switch input maps (Player ↔ UI ↔ Cutscene) and cursor lock.
- Menus derive from `MenuView : UIView`; each has an explicit first-selected button for gamepad.
- Title: New Game (S00 → S01), Continue (if save exists, via ISaveService), Settings, Quit.
- Pause (Esc / Start): Resume, Quest Log, Settings, Quit to Title. Time.timeScale = 0 while paused; UI uses
  unscaled time.
- Settings (demo minimum): master/music/SFX volume, subtitle size, invert camera Y. Persist with PlayerPrefs.
- End screen after SC-06: Title + "To be continued" + back to Title.
