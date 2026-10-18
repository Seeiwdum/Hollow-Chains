# Tech Spec — Hollow Chains Demo (v0.9)

> **สถานะ: รอ NAMOE อนุมัติ** (เงื่อนไขจบ P1 = อนุมัติ GDD + Tech) · เจ้าของ: ธีร์ (Technical Director)
> ร่วม: เอก (Build/Git), กานต์ (Gameplay), ณิชา (UI), พลอย (Tech Art) · อ้างอิง: GDD v1.0 (D-018), `00` §8, `05` §5, `09`
> หมายเหตุเวอร์ชัน: ทุกเวอร์ชันด้านล่าง**ดึงจาก Unity Package Manager registry จริง** ผ่าน Unity MCP (`Client.SearchAll`) บน Editor 6000.3.10f1 เมื่อ 2026-10-18 — คอลัมน์ "recommended" = เวอร์ชันที่ Unity รับรองสำหรับ Editor นี้

## 1. Engine
| หัวข้อ | ค่า | เหตุผล |
|---|---|---|
| Editor | **Unity 6000.3.10f1** (สาย Unity 6.3 LTS) | D-007; ไม่ย้ายเวอร์ชันระหว่างเดโม (patch ภายในสาย 6000.3 อัปได้ถ้าแก้บั๊กที่เจอ) |
| Render pipeline | URP 17.3.0 | มีอยู่แล้ว, เหมาะกับ PC กลาง ๆ |
| Color space | Linear | ค่ามาตรฐาน URP สำหรับแสง/โพสต์ที่ถูกต้อง (ตรวจและตั้งใน P1.3) |
| Scripting backend | Mono ระหว่างพัฒนา · **IL2CPP** สำหรับ demo build | build iteration เร็ว / runtime เร็วกว่าในของจริง |
| Platform | Windows x64 (Steam ภายหลัง) | `01` §1 PROPOSAL |
| Input | KB/M + Gamepad | `01` §1 |

## 2. Packages
| Package | ตอนนี้ | เป้าหมาย | ใช้ทำอะไร | Phase |
|---|---|---|---|---|
| `com.unity.render-pipelines.universal` | 17.3.0 | 17.3.0 | Rendering | — |
| `com.unity.inputsystem` | 1.18.0 | 1.18.0 (recommended) | Input; ไม่ขึ้น 1.20.x จนกว่าจะมีเหตุผล | P2 |
| `com.unity.timeline` | 1.8.10 | **1.8.13** | Cutscene SC-04–06 | P1.3 |
| `com.unity.ai.navigation` | 2.0.10 | **2.0.16** | NavMesh ชาวบ้าน/หมาป่า | P1.3 |
| `com.unity.test-framework` | 1.6.0 | 1.6.0 | EditMode/PlayMode tests | — |
| `com.unity.ugui` | 2.0.0 | 2.0.0 | uGUI + TextMeshPro (TMP รวมอยู่ใน ugui 2.0 ของ Unity 6) | — |
| `com.unity.cinemachine` | — | **3.1.7** | กล้อง third-person, sprint cam, cutscene, Impulse | P1.3 |
| `com.unity.localization` | — | **1.5.13** | String tables en (เดโม) / th (ภายหลัง) — D-017 | P1.3 |
| `com.unity.probuilder` | — | **6.0.9** | Greybox ห้อง/หมู่บ้าน/ป่า/หน้าผา | P1.3 |
| `com.unity.splines` | — | 2.8.4 | เส้นทางป่า, ลำธาร, ทางเดินกล้อง | P2 |
| `com.unity.visualeffectgraph` | — | 17.3.0 | ไอดำ, ราก, เวทน้ำ, Soul Drain | P5 |
| `com.unity.animation.rigging` | — | 1.4.1 | IK มือยื่น/กอด | P5 |
| `com.unity.recorder` | — | 5.1.7 | (ทางเลือก) อัดคลิป playtest/trailer | P6 |
| `com.unity.ai.assistant` | — | 2.20.0-pre.2 | **Unity MCP Server** (เครื่องมือ Editor เท่านั้น ไม่เข้า build) — ⚠️ เป็น pre-release | P1.3 |
| `com.unity.collab-proxy` | 2.11.3 | **ลบ** | Unity Version Control — เราใช้ Git | P1.3 |
| `com.unity.visualscripting` | 1.9.9 | **ลบ** | ไม่ใช้ (C# ล้วน) | P1.3 |
| `com.unity.multiplayer.center` | 1.0.1 | **ลบ** | ไม่ใช่เกมออนไลน์ | P1.3 |
| `com.unity.addressables` | — | ไม่ติดตั้งเอง | Localization ดึงเป็น dependency เอง; ไม่ใช้ Addressables กับ scene ในเดโม (ดู §5) | — |
| Ink / Yarn Spinner | — | **ไม่ใช้ในเดโม** | เดโมไม่มีบทแตกแขนง → ระบบบทพูดเบา ๆ ของเราเอง (§6); ทบทวนตอนมี choice/Polarity | — |
| DOTween / Feel | — | ไม่ใช้ตอนนี้ | ลด dependency; ใช้ coroutine/Animator ก่อน (ถ้าจำเป็นต้องตรวจ license ก่อน) | — |

## 3. โครงโฟลเดอร์
ตาม skill `unity-project-setup`: `Assets/_Project/{Art,Audio,Data,Prefabs,Scenes,Scripts,Settings,UI,VFX,Timeline}`, `Assets/ThirdParty/` (มี LICENSE note ต่อชิ้น), `Assets/Placeholders/`
- Template ของ Unity (`TutorialInfo/`, `Readme.asset`, `Scenes/SampleScene`) **ลบใน P1.3**; `Settings/` (URP assets) ย้ายเข้า `_Project/Settings/`; `InputSystem_Actions.inputactions` ย้ายเข้า `_Project/Settings/Input/` แล้วปรับ Action Map ใน P2

## 4. Assemblies & สถาปัตยกรรม
ตาม skill `unity-csharp-architecture`:
`HollowChains.Core` ← `Data` ← `Gameplay` / `Narrative` / `UI` / `Combat` (+ `Editor`, `Tests.*`); พึ่งพาเข้าหา Core เท่านั้น
- **Event Bus** = ScriptableObject event channels ใน `Data/Events/` (เช่น `OnQuestPhaseChanged`, `OnHealthChanged`, `OnInteract`, `OnCutsceneStarted/Ended`, `OnCheckpointReached`)
- **Services** = `ServiceLocator` เล็ก ๆ ลงทะเบียนจาก `_Boot`: `SaveService`, `SceneFlowService`, `AudioService`, `LocalizationService` (wrapper บาง ๆ) — ห้าม `static Instance` บน MonoBehaviour
- **State machine** ใน Core ใช้กับ Player (Locomotion/Carry/Sprint/Locked), Quest phase, Game flow (Title/Playing/Cutscene/Paused/End)
- **Combat hook (D-011):** assembly `HollowChains.Combat` สร้างว่างไว้ตั้งแต่ P1.3; `Gameplay` ไม่อ้างอิง Combat → เดโม build ได้โดยไม่มี combat, P4 เสียบผ่าน event channel `OnEncounterRequested`
- **Humanity (D-002):** `HumanityState` (0–100) อยู่ใน Gameplay เป็น data + event; ไม่มี UI
- **Comic hook:** `IComicPage` / `ComicPlayer` ใน Narrative; S00 ใช้ `SubtitleComicPage` (จอดำ + ซับ)

## 5. Scene flow
- `_Boot` (persistent: services, EventSystem, Audio, UI root) → โหลด scene เนื้อหาแบบ **Additive** ทีละตัว: `S00_ComicIntro_Placeholder`, `S01_Bedroom`, `S02_Astraon`, `S03_Forest`, `S04_StreamCliff`, `S05_EndCredit`
- เปลี่ยน scene ด้วย fade สั้น + `LoadSceneAsync` (โหลด S03 ล่วงหน้าตอนผู้เล่นใกล้ประตูหมู่บ้าน) → ไม่มีจอโหลดยาว (FT-14)
- Addressables สำหรับ scene streaming **ยังไม่ใช้** — เดโมเล็ก, Build Settings scene list พอ; จะทบทวนตอน P8+ (`05` §5)

## 6. ระบบหลักของเดโม (ภาพรวม — รายละเอียดใน spec ของแต่ละ Phase)
| ระบบ | การออกแบบ |
|---|---|
| Quest (FT-04) | `QuestSO` → `QuestPhaseSO[]` (ID `Q001-P1`…), เงื่อนไขจบแต่ละ phase = event channel ที่ฟัง; runtime `QuestState` serializable |
| Dialogue/Subtitle (FT-05) | `DialogueLineSO` อ้างอิง Localization key + speaker + duration; เล่นจาก Timeline (custom track/Signal) หรือจาก trigger (barks); ไม่มี branching ในเดโม |
| Localization (D-017) | Locale เริ่มต้น `en`; `th` เพิ่มทีหลัง; string table แยกตาม scene + `UI`; ห้ามข้อความในโค้ด/ใน prefab ตรง ๆ |
| UI (FT-06/07) | **uGUI + TMP** ทั้งเดโม (HUD, world-space prompt, เมนู) — เครื่องมือเดียว ลดความซับซ้อน; UI Toolkit พิจารณาใหม่หลังเดโม |
| Save (FT-09) | JSON ที่ `Application.persistentDataPath/save_0.json`; มี `saveVersion`; เก็บ checkpoint ID + QuestState + HP/Humanity; เขียนไฟล์ temp แล้ว rename กันไฟล์เสีย |
| Input | Action maps: `Player`, `UI`, `Cutscene` (สำหรับ interactive beats: hold/mash) — สลับ map ตาม game state |
| Camera | Cinemachine 3: `CinemachineCamera` + Orbital Follow/Rotation Composer สำหรับ third-person, กล้อง sprint แยก, กล้อง cutscene ผ่าน Timeline, Impulse สำหรับ shake |
| Cutscene (FT-10) | Timeline + Cinemachine track + Signal → event channel; interactive beat = Timeline หยุด (hold) รอ input แล้วเล่นต่อ (ผลลัพธ์คงที่) |
| Audio (FT-12) | Audio Mixer: Music / SFX / Ambience / UI + snapshots `Default`, `Awakening_Silence`, `Critical` |
| Post (FT-13) | Global Volume ต่อ scene + Volume "Warm" ที่เพิ่ม weight ตอน monologue ถึงเอเลนา |

## 7. Performance target `[PROPOSAL จาก 05 §5 — ใช้เป็นค่าเริ่ม]`
- **60 FPS ที่ 1080p** บน PC ระดับ GTX 1060 / 16 GB RAM; 30 FPS เป็น fallback
- งบประมาณเริ่มต้น: draw calls ≤ 1,500 ต่อเฟรม, VFX ชิ้นใหญ่ ≤ 2 ms GPU (`08`), GC alloc ต่อเฟรมในเกมเพลย์ ≈ 0
- เครื่องมือ: Profiler + Frame Debugger; Occlusion Culling, LOD, GPU Instancing สำหรับพืช (P5)

## 8. คุณภาพและการทดสอบ
- EditMode tests: Quest phase transitions, Save round-trip, Humanity clamp
- PlayMode smoke test: Boot → S01 โหลดได้, Q001-P1 สำเร็จด้วยการ simulate interact
- ก่อนทุก commit: Console ไม่มี error (เช็กผ่าน `Unity_GetConsoleLogs`), ไม่มี compile warning ใหม่
- **Unity MCP rule:** ก่อนเขียนอะไรผ่าน MCP ต้อง log `Application.dataPath` ให้ตรง `D:/Git/Hollow-Chains/Assets` ก่อน (Q-010)

## 9. Git / Build
- ตาม `09`: `.gitignore` (Unity template + `09` §6), `.gitattributes` LFS ตาม `09` §6, `git lfs install`, `tools/gitbatch.sh`
- Visible Meta Files + Force Text (ตรวจ/ตั้งใน P1.3)
- Build: Build Profile "Demo-Windows" → `Builds/` (ignored); tag demo ตาม D-011 = `p6-demo-v0.1`

## 10. P1.3 Checklist (Unity init — ทำหลัง Q-010 แก้แล้ว)
1. ยืนยัน MCP ชี้ repo (`Application.dataPath`)
2. `.gitattributes` + `git lfs install` + ปรับ `.gitignore` → commit **ก่อน**ไฟล์ไบนารีใด ๆ
3. `tools/gitbatch.sh`
4. ตั้ง Visible Meta / Force Text / Linear / Input System only / Company+Product name `Hollow Chains`
5. ลบ template, สร้างโครงโฟลเดอร์ + asmdef ว่าง 8 ตัว
6. ปรับ packages ตาม §2 (เพิ่ม/อัป/ลบ) → Console ต้องสะอาด
7. ตั้ง Localization: locale `en`, string table `UI` ว่าง
8. Track `Assets/` `Packages/` `ProjectSettings/` → commit แบบ Farm `normal` → push

## 11. ความเสี่ยงทางเทคนิค
| # | ความเสี่ยง | ทางรับมือ |
|---|---|---|
| T1 | `com.unity.ai.assistant` เป็น pre-release อาจทำ Editor ช้า/error | เป็น Editor-only; ถ้ามีปัญหาถอดออกได้โดยไม่กระทบเกม |
| T2 | Cinemachine 3 API ต่างจาก tutorial เก่า (v2) | อ้างอิง docs 3.1 เท่านั้น |
| T3 | Interactive beat ใน Timeline (hold/mash) ทำให้ sync ซับ/เสียงหลุด | ใช้ hold-point + Signal; ทดสอบใน P5 greybox |
| T4 | Localization เพิ่มไทยทีหลังแล้วฟอนต์พัง (R1) | String table ตั้งแต่วันแรก; spike ฟอนต์ไทยตอนเพิ่ม `th` |
