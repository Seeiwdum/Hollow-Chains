---
name: unity-cinemachine-camera
description: Cameras for Hollow Chains with Cinemachine 3.1 — third-person follow camera, sprint camera, cutscene cameras via Timeline, impulse shake, collision handling in tight rooms. Use when creating or tuning any camera, blending between cameras, or fixing camera clipping or jitter.
---

# Cinemachine Camera (Hollow Chains, skill #5)

Package `com.unity.cinemachine` 3.1.7. Cinemachine 3 renamed most components (`CinemachineCamera` replaces
`CinemachineVirtualCamera`; behaviours such as Orbital Follow, Rotation Composer, Deoccluder, Input Axis
Controller). Most v2 tutorials are wrong now — confirm type names in `Library/PackageCache/com.unity.cinemachine@*`
before writing code.

## Setup
- Main Camera + `CinemachineBrain` live in `_Boot` (persistent).
- `CM_Explore`: third-person orbit around a `CameraTarget` child of the player (~1.5 m up), driven by the Look
  action; deoccluder/decollider for the bedroom and alleys.
- `CM_Sprint`: higher priority while sprinting — wider FOV, closer, slight noise.
- Cutscene cameras are driven by Timeline Cinemachine tracks.
- Impulse sources on dramatic events + listeners on gameplay cameras.

## Rules
- Code never moves the Main Camera directly; switch by priority or enabling cameras.
- Camera-relative movement reads the Main Camera transform (brain output).
- Short blends (0.3–0.6 s) except emotional beats, which are authored shots in Timeline.
