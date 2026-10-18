---
name: unity-build-pipeline
description: Building the Hollow Chains demo — Windows build profile (IL2CPP, x86_64), scene list, version/tag naming, batchmode build script, player log location, smoke test on another PC, zipping the demo for sharing. Use when making any build, changing build settings, or preparing the demo for testers.
---

# Build Pipeline (Hollow Chains, skill #14)

- **Scenes (order):** `_Boot`, `S00_ComicIntro_Placeholder`, `S01_Bedroom`, `S02_Astraon`, `S03_Forest`,
  `S04_StreamCliff`. Test scenes never go in.
- **Profile:** Windows, x86_64, IL2CPP, .NET Standard 2.1, Release; development builds only for profiling.
- **Version:** `PlayerSettings.bundleVersion` = `0.1.<n>`; the release build matches the git tag `p6-demo-v0.1`.
- **Script:** `Assets/_Project/Scripts/Editor/Build/DemoBuilder.cs` with a menu item and a static
  `BuildFromCli()` for `Unity.exe -batchmode -quit -executeMethod HollowChains.Editor.Build.DemoBuilder.BuildFromCli`.
  Output to `Builds/` (git-ignored). Fail the build on errors; print the BuildReport summary.
- **Logs:** `%USERPROFILE%/AppData/LocalLow/Hollow Chains Studio/Hollow Chains/Player.log`.
- **Smoke test:** fresh PC/user → launch → New Game → finish → End screen; Continue works; Thai text renders (fonts
  included as static atlases); no missing-shader pink.
- **Sharing:** zip the build folder (exclude `*_BurstDebugInformation_DoNotShip`), never commit builds. Uploading or
  sending the zip anywhere needs the owner's OK.
