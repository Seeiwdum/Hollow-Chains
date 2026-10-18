---
name: unity-project-setup
description: Set up and maintain the Hollow Chains Unity project — Unity 6000.3 + URP, packages, folder layout, Assembly Definitions, project settings (Visible Meta, Force Text), Git + LFS, .gitignore/.gitattributes, tools/gitbatch.sh, and the Unity MCP connection. Use this for P1.3 project init, whenever adding/upgrading a package, changing project settings, creating folders or asmdefs, or when git/LFS/Unity-editor connection problems come up.
---

# Unity Project Setup (Hollow Chains, skill #1)

## Facts (verify, don't assume)
- Repo / project root: `D:\Git\Hollow-Chains` (remote `github.com/Seeiwdum/Hollow-Chains`, default branch `main`).
- Editor: Unity **6000.3.10f1** (D-007), batchmode exe `D:\Unity\Hub\Editor\6000.3.10f1\Editor\Unity.exe`.
- Read `Packages/manifest.json` for current package versions. Never write a package version from memory —
  look it up (Package Manager / web search) and record why it was chosen in the Tech Spec.
- Unity MCP tools (`mcp__unity-mcp__*`) talk to **whichever project the Editor has open**. Before any MCP write,
  confirm with a read-only `Unity_RunCommand` logging `Application.dataPath`; it must be
  `D:/Git/Hollow-Chains/Assets`. If not, stop and ask the owner to open the right project.
  The MCP server comes from package `com.unity.ai.assistant` (Project Settings > AI > Unity MCP Server).

## Folder layout
```
Assets/_Project/
  Art/ (Characters, Environment, Props, Materials, Textures)
  Audio/ (Music, SFX, Mixers)
  Data/ (ScriptableObject assets: Quests, Dialogue, Events, Items, Config)
  Prefabs/  Scenes/ (S00_ComicIntro_Placeholder, S01_Bedroom, S02_Astraon, S03_Forest, S04_StreamCliff, S05_EndCredit, _Boot)
  Scripts/ (one folder per assembly — see unity-csharp-architecture)
  Settings/ (URP assets, Volume profiles, Input actions)
  UI/ (Fonts, Sprites, UXML/USS)
  VFX/  Timeline/
Assets/ThirdParty/   (imported assets, each with LICENSE note)
Assets/Placeholders/ (primitives/greybox, safe to delete later)
```
Underscore keeps project content sorted above third-party folders.

## Project settings checklist
- Version Control mode: Visible Meta Files. Asset Serialization: Force Text.
- Color space Linear; URP asset assigned in Graphics + every Quality level.
- Active Input Handling: Input System Package (New).
- Company/Product name set (product name follows Q-002; working title `Hollow Chains`).

## Git
- `.gitignore`: GitHub Unity template + entries from `09 §6`; never ignore `.meta`.
- `.gitattributes`: exactly `09 §6` (LFS for binaries, unityyamlmerge for YAML). Run `git lfs install` once, and
  `git lfs track` matches the file. Add `.gitattributes` **before** committing any binary, otherwise binaries land in
  normal history.
- `tools/gitbatch.sh` from `09 §9`; commit it early.
- Never commit `Library/ Temp/ Logs/ UserSettings/ Build*/`. Check `git status -sb` before each batch.
- Work on `phase/*`, no force push, no direct push to `main` without owner approval.

## MCP editor-script pitfalls (learned in P2/P3)
- `EditorSceneManager.OpenScene/NewScene` unloads unused assets: **load ScriptableObjects (event channels,
  configs) after opening the scene**, otherwise `objectReferenceValue` silently saves `{fileID: 0}`. After building
  a scene, grep its `.unity` for `fileID: 0}` on fields that must be wired.
- Commands that delete assets (`AssetDatabase.DeleteAsset`) or pop dialogs (first TMP use → import TMP Essentials
  via `TMP_PackageResourceImporter.ImportResources(true, false, false)`) are rejected by MCP; delete with git instead.
- Play mode does not tick while the Editor is unfocused: set `EditorApplication.isPaused = true` and call
  `EditorApplication.Step()` in a loop to advance frames for smoke tests.
- After writing `.cs` files, call `AssetDatabase.Refresh()`; the MCP bridge disconnects during the domain reload —
  wait, then check `Unity_GetConsoleLogs`.

## Making changes
Prefer editing settings through the Editor (MCP `Unity_RunCommand` with `EditorSettings`, `PlayerSettings`,
`AssetDatabase`) so Unity rewrites YAML consistently; hand-edit YAML only when the Editor is closed. After changes,
check `Unity_GetConsoleLogs` for errors before committing.
