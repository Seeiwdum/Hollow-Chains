---
name: unity-timeline-cutscene
description: Cutscenes for Hollow Chains with Unity Timeline 1.8 + Cinemachine 3 — PlayableDirector setup, custom tracks (channel events, subtitle lines, interactive beats), actor capsules, camera shots, volume/post animation, building timelines from editor scripts. Use for SC-04 water magic, the hug, SC-05 Awakening, SC-06 rejection/cliffhanger, the comic intro, or any scripted sequence.
---

# Timeline Cutscenes (Hollow Chains, skill #7)

## Pattern used in the demo
- **Actors, not the player:** at cutscene start the gameplay Player is locked/hidden (ActivationTrack) and
  cutscene actors (Phineas_Actor, Elena, DarkWolf capsules — D-023) are animated with AnimationTracks
  (transform curves). Avoids fighting the CharacterController.
- **Cameras:** `CinemachineTrack` + `CinemachineShot` clips referencing `CinemachineCamera`s in the scene
  (`director.SetReferenceValue(shot.VirtualCamera.exposedName, cam)`). Camera moves = AnimationTrack on the camera.
- **Custom tracks (Narrative assembly):**
  - `ChannelEventTrack` / `ChannelEventClip` — raises a `VoidEventChannelSO` at clip start (HP = 1, demo end,
    flash…). Gameplay reacts through `PlayerStoryHooks` etc. — the timeline never calls gameplay code.
  - `LineTrack` / `LineClip` — plays a `DialogueLineSO` via EVT_PlayLine (subtitles stay in sync).
  - `InteractiveBeatTrack` / `InteractiveBeatClip` — pauses the director and waits for the player
    (press or hold, prompt from a LocalizedString); result is fixed, cannot fail (`06` [IMPL]).
- **Post FX:** AnimationTrack on a Volume's `weight` (VP_Awakening, VP_Critical).
- **Audio:** built-in AudioTrack bound to an AudioSource (watch tick, chain drag).

## Rules
- Build complex timelines with an editor builder script (`HollowChains.Editor`) so they can be regenerated;
  after building, check bindings in the `.unity` file (`fileID: 0`).
- CANON lines (Elena SC-06, father's message, narration) must match `06`/`02` exactly — split across subtitle
  lines only at existing punctuation, never reword.
- Every cutscene must be skippable or short, and must end by raising an event (quest phase / next beat).
- Time in cutscenes uses the director's time; pausing the game (`Time.timeScale = 0`) also pauses the director
  when `DirectorUpdateMode.GameTime`.
