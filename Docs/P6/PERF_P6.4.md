# Performance Report -- P6.4 (B-602) -- 2026-11-12

Branch `phase/P6-polish`. Unity 6000.3.10f1, URP 17.3. Greybox demo (ProBuilder, capsules, no animation).

## 1. Verdict

- **Editor, GTX 1650, 1080p, stationary camera at spawn: PASS** against the only budget set (16.67 ms). Worst scene average is 3.43 ms and the worst single frame is 12.00 ms, so there is large headroom.
- **"60 FPS at 1080p on a GTX 1060 in the Windows IL2CPP build" (AC-04): NOT ASSESSED.** Wrong GPU, Editor rather than a player, and no traversal. It closes when P6.5's IL2CPP build is run on a GTX 1060-class machine (or the closest available) and a 2-5 minute full-route run (S00 to S04, including cutscenes and the climax) holds at or below 16.67 ms at the 95th and 99th percentile with no multi-frame hitches.
- Static review found nothing that looks like a frame-time problem. Findings are mostly GPU-side settings that cost more on a GTX 1060 than on the measured machine.

## 2. Method, machine, caveats

Editor Play mode, Game view 1920x1080, VSync off (`vSyncCount 0`, `targetFrameRate -1`), quality level "PC", timeScale 1, state Playing, player standing at spawn with the default camera. Per scene: loaded through `ISceneFlowService.Load`, 180 warm-up frames, 600 sampled frames. Frame time is `Time.unscaledDeltaTime`; counters via `ProfilerRecorder` (averages). Machine: GTX 1650 4 GB (D3D12), i5-11400, 12 GB RAM. Collected by the Manager; this report adds the static review.

Caveats:
1. Editor, not the Windows IL2CPP player. Editor overhead is in the numbers.
2. GTX 1650, not the GTX 1060 target. Context only, not a result: public benchmarks place the two in a broadly similar class (a 1060 6 GB is usually somewhat faster, a 1060 3 GB roughly equal). That is not enough to treat the 1650 result as a 1060 result.
3. Stationary at spawn: no traversal, no worst-case view, no Timeline/cutscene/climax, no dialogue UI open. S00 not sampled.
4. Load times and load hitches not measured.
5. GC ~16.8 KB/frame is near-identical in every scene (see section 4, F4). Not attributed.
6. "Total Used Memory" is the whole Editor process. Player memory is NOT ASSESSED.
7. Single run per scene except S01 (two runs agree within 5%). No variance data across sessions; Editor frame time was uncapped so avg ms is not a Game-view FPS the player would see.
8. Mono is the default scripting backend for Standalone in the current ProjectSettings (see F5), so even Editor-to-player differences cannot be assumed small.

## 3. Budgets and measurements

`project.yaml` performance block: `target_framerate: 60`, `frame_budget_ms: 16.67`, `enforce: warn`. Nothing else is set.

| Budget | Set value | Measured | Status |
|---|---|---|---|
| Frame time | 16.67 ms | Editor/GTX 1650: avg 3.07-3.43, p99 up to 4.83, max 12.00 (S03) | PASS (Editor, non-target GPU) |
| Frame time on target (GTX 1060, IL2CPP player) | 16.67 ms | not measured | NOT ASSESSED |
| Per-category split (logic/render/physics/AI/audio) | none | not measured | NOT ASSESSED - no budget set |
| Draw calls / batches / SetPass | none | 72-526 / 39-94 / 16-20 | NOT ASSESSED - no budget set |
| Triangles / vertices | none | up to 39,378 / 88,453 | NOT ASSESSED - no budget set |
| GC allocation per frame | none | ~16.8 KB avg | NOT ASSESSED - no budget set (see F4) |
| Memory | none | Editor process 2148-2207 MB | NOT ASSESSED - no budget set; player memory not measured |
| Load time | none | not measured | NOT ASSESSED - no budget set |

| Scene | avg ms | median | p95 | p99 | max | draws | batches | SetPass | tris | verts | GC avg B | GC max B | Mem MB (Editor) |
|---|---|---|---|---|---|---|---|---|---|---|---|---|---|
| S01 run 1 | 3.07 | 3.02 | 3.54 | 3.89 | 4.50 | 72 | 62 | 16 | 7,871 | 10,801 | 16,845 | 51,150 | 2148 |
| S01 run 2 | 3.21 | 3.15 | 3.67 | 4.05 | 4.54 | 73 | 63 | 17 | 8,086 | 11,203 | 16,780 | 33,692 | 2159 |
| S02 | 3.43 | 3.33 | 3.94 | 4.83 | 7.27 | 182 | 94 | 16 | 37,826 | 38,195 | 16,843 | 40,534 | 2173 |
| S03 | 3.37 | 3.30 | 3.87 | 4.14 | 12.00 | 526 | 60 | 20 | 39,378 | 88,453 | 16,773 | 33,692 | 2194 |
| S04 | 3.09 | 3.08 | 3.41 | 3.61 | 4.17 | 158 | 39 | 16 | 13,853 | 24,863 | 16,772 | 33,692 | 2207 |

Single-frame maxima (S03 12.00 ms, S02 7.27 ms) are under budget but are 3-4x the average; cause unknown (shader/asset first-use, GC, or Editor). Memory rose about 59 MB over the scene sequence: requires investigation, not called a leak.

## 4. Static review findings (ranked)

Estimates are estimates, not measurements. Effort: S under 1 h, M under 1 day, L longer.

**F1. PC URP asset stacks several full-screen GPU costs (WARNING for GTX 1060, unmeasured).**
`Assets/_Project/Settings/PC_RPAsset.asset`: `m_RequireOpaqueTexture: 1` (:23), `m_RequireDepthTexture: 1` (:22), `m_SupportsHDR: 1` (:26), 4 shadow cascades (:58) at 50 m (:57), 2048 shadow map (:46), soft shadows quality 3 (:69). `PC_Renderer.asset`: Deferred rendering mode (`m_RenderingMode: 2`, :56) and an active SSAO feature (:71-73, `Samples: 1`, `Downsample: 0` i.e. full-resolution). The quality asset `ProjectSettings/QualitySettings.asset` PC level (:64-) uses it (:104, also `GraphicsSettings.asset:40`) with its own `shadowDistance 40`/`shadowCascades 2`, which URP ignores in favour of the RP asset. Why it matters: the opaque-texture copy, full-res SSAO and 4 cascades are fill-rate and bandwidth work that scales with resolution; the GTX 1650/1060 class is where it shows first. Estimated impact: a few ms on a GTX 1060-class GPU at 1080p, based on general URP cost experience, not measured here. Cheapest to test first: opaque texture off (nothing in the project is known to sample it; verify no shader/Volume uses it), cascades 4 to 2, SSAO downsample on. Effort S per change, but each needs a visual check. Owner: technical-artist. Do this only if the player-build measurement is tight; if the numbers pass, leave it.

**F2. No occlusion culling data baked; everything casts shadows (WARNING, low impact for current scene sizes).**
`S01..S04 .unity` header: `m_OcclusionCullingData: {fileID: 0}` (line 12 in each) while static objects carry Occluder/Occludee flags (`m_StaticEditorFlags: 22`). S03 has 267 mesh renderers, 266 static, 267 with `m_CastShadows: 1`. The directional light is realtime with soft shadows (`m_Shadows m_Type: 2`, S03 light intensity 0.6). Why it matters: S03's 526 draw calls are largely repeated geometry drawn into the main pass plus 4 shadow cascades. Estimated impact: small now (CPU 3.4 ms avg total), could grow if S03 gets denser. Fix: bake occlusion for S03/S02 (S), or reduce cascades (see F1). Owner: technical-artist.

**F3. S03 "526 draws vs 60 batches" means batching works.**
SRP Batcher is on (`PC_RPAsset.asset:72`), dynamic batching off (:73), meshes are flagged batching-static. Many draws share a few shader variants and are submitted in 60 batches; SetPass stays at 20. Draw calls in the Stats counter include shadow-cascade passes. No action needed. Note: the draw call count alone is not a budget problem here, and no budget is set. GPU Resident Drawer is off (`m_GPUResidentDrawerMode: 0`, :86): not worth enabling for the greybox.

**F4. Constant ~16.8 KB/frame GC: no source proven in game code.**
Every game `Update`/`LateUpdate` reviewed is allocation-free: `HealthBarView.cs:33`, `ScreenFlashView.cs:42`, `FlashOnEventView.cs:27`, `StringChannelTextView.cs:63`, `ComicPlayer.cs:32`, `DialogueRunner.cs:55`, `InteractiveBeatRunner.cs:68`, `SprintCameraEffect.cs:36`, `OrbitLookInput.cs:25`, `CriticalHealthVolume.cs:50`, `FootstepAudio.cs:32`, `VolumeBlendZone.cs:39`, `PlayerController.cs:68`, `Interactor.cs:84`. Details:
- `Interactor.cs:84-101` rescans every 0.1 s with `OverlapSphereNonAlloc` into a preallocated buffer. No per-frame alloc; `GetComponentInParent<IInteractable>` at :107 is a per-hit lookup every 0.1 s, fine.
- `Camera.main` is only used in `Start` and as a null fallback (`PlayerController.cs:64,90`), not per frame.
- No `Find*`, LINQ or `OnGUI` in runtime code (Editor-only scripts excluded). Strings are built only on events (`StringChannelTextView.cs:73-77`, `InteractiveBeatRunner.cs:48`).
- `CriticalHealthVolume.cs:51` and `VolumeBlendZone.cs:48` write `Volume.weight` per frame; allocation-free.
Because the value is the same in S01-S04, a game-side per-object source is unlikely (scene object counts differ 10x). Candidates, all unproven: Editor/Game view overhead, Input System or Cinemachine internals, the sampler itself, UGUI canvas rebuild of the `_Boot` HUD. Closing it needs a Deep Profile capture or `ProfilerRecorder` of "GC.Alloc" call stacks in the player. Do not spend effort until it is shown to exist in the player build. Effort to attribute: S-M.

**F5. Standalone scripting backend is not IL2CPP in the saved settings.**
`ProjectSettings/ProjectSettings.asset:831-832` lists only `Android: 1` under `scriptingBackend`; Standalone therefore defaults to Mono, while the studio rules and `.claude/skills/unity-build-pipeline/SKILL.md:10` say Windows IL2CPP. Possibly the batchmode build script sets it (not found in `Assets/_Project/Scripts/Editor`). Why it matters: the P6.5 measurement must be taken on the backend that ships. Effort S. Owner: devops-engineer (B-605).

**F6. Minor: UI per-frame work.** `HealthBarView.cs:36-37` sets `fillAmount` and `color` every frame even when idle. UGUI's setters ignore unchanged values, so cost is negligible; not worth changing.

Quick wins (all optional, only if the player build is tight): F1 opaque texture off and cascade count 4 to 2; F2 bake occlusion for S03; F5 confirm IL2CPP.
Not worth doing for the greybox demo: GPU Resident Drawer, GPU instancing work, mesh LOD, texture atlasing, rewriting UI views, pooling (no runtime spawning in hot paths found), code-level micro-optimisation of any `Update`.

## 5. Requires investigation / NOT ASSESSED

- Target hardware (GTX 1060) and IL2CPP player frame time (closing measurement in section 1).
- Traversal, worst-case views, cutscene/Timeline/climax frames, dialogue UI open, S00 comic.
- Load times and load-time hitches (S00 to S04 transitions, `_Boot`).
- Origin of the 16.8 KB/frame GC and the 33-51 KB max-frame spikes; whether they exist in the player.
- Origin of the 12.00 ms (S03) and 7.27 ms (S02) single-frame maxima.
- +59 MB Editor-process growth across the scene sequence (could be Editor caches).
- Player memory by category (textures, meshes, audio, UI): no budget set and not measured.
- Not checked: camera-level AA/post-processing flags (`m_MSAA` in the RP asset is 1 = off; no camera AA mode was verified), volume profile contents in use, texture import sizes/compression, whether any shader samples the opaque texture, whether Static Batching is enabled in Player Settings.
- Budgets for draw calls, memory, load time, GC are absent from `project.yaml`; setting them is a technical-director decision.

## 6. Recommended next actions

1. Run the P6.5 Windows IL2CPP build (Development build off for the timing run) on a GTX 1060-class PC and record the full route with the same counters, p95/p99/max per scene and per cutscene. Owner: devops-engineer builds, performance-analyst reads. Closes AC-04.
2. Confirm the Standalone backend is IL2CPP before that build (F5). Owner: devops-engineer.
3. Ask technical-director to set draw call, memory, GC and load-time budgets, or state they stay unset for the demo. Owner: technical-director.
4. If the 1060 run is over or close to 16.67 ms at p99: apply F1 (opaque texture, cascades, SSAO downsample) one change at a time with before/after captures. Owner: technical-artist.
5. If GC in the player is above zero in steady state, take a Deep Profile capture to attribute it (F4). Owner: engine-programmer.
