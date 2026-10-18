---
name: unity-object-pooling-performance
description: Performance for Hollow Chains — hitting 60 FPS at 1080p on a GTX 1060 (AC-04), Unity Profiler and Frame Debugger, GC-free Update patterns, object pooling, culling/LOD, URP settings and shadow cost. Use when profiling, when a scene drops frames or stutters, when adding something spawned repeatedly, or reviewing code for per-frame allocations.
---

# Performance (Hollow Chains, skill #12)

Target (AC-04): **60 FPS, 1080p, GTX 1060-class GPU**, no hitch > 50 ms on scene loads after the fade.

## Measure first
- Profile a **development build** (Autoconnect Profiler), not just the Editor — Editor overhead lies.
- Worst spots to check: S02 (city backdrop + barks), S03 (trees + mist particles), S04 climax (6 cameras, volumes, particles).
- Record CPU main thread, GPU, GC Alloc per frame. Anything allocating every frame in gameplay is a bug.
- Via MCP: `ProfilerRecorder` (`"Main Thread"`, `"GC Allocated In Frame"`) sampled while stepping play mode.

## Rules
- No `string` concat, LINQ, `GetComponent`, `Find*`, or lambdas in `Update`; cache in `Awake`.
- Event channels are fine (no alloc on Raise); don't subscribe new closures per frame.
- Pool anything spawned more than a few times (VFX bursts, footstep sources) — `UnityEngine.Pool.ObjectPool<T>`.
- Greybox: ProBuilder meshes are static → mark static (batching + occlusion). Trees are many draw calls — combine per
  area if the Frame Debugger shows > ~1500 batches.
- URP: one main light with shadows, shadow distance ≤ 40 m in the forest, soft particles off, MSAA 2x or FXAA,
  post-processing only what the Art Bible needs.
- Volumes with weight 0 still cost the Volume framework a little; disable the GameObject when unused for long.

## Done when
Profiler capture of each scene saved under `Docs/P6/perf/` with FPS numbers in the STUDIO_LOG.
