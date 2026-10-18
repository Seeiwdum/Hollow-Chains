# STUDIO_LOG — Hollow Chains Studio

> ดูแลโดย: ใบเตย 🐱 · อัปเดตทุกครั้งที่จบ Phase
> วิธีใช้: คัดลอกไฟล์นี้ (+ `DECISION_LOG.md`) ไปวางในแชทใหม่ พร้อมไฟล์ `00–09` เพื่อทำงานต่อโดยไม่เสียบริบท

---

## สถานะปัจจุบัน
| หัวข้อ | ค่า |
|---|---|
| Phase | **P3 Core Systems จบ** — รออนุมัติเข้า P5 (tag `p3-done`) |
| วันที่ | 2026-10-18 |
| Unity | 6000.3.10f1 · URP 17.3.0 · Input System 1.18.0 (template เริ่มต้นยังอยู่: Scenes, Settings, TutorialInfo, Readme) |
| Repo | `github.com/Seeiwdum/Hollow-Chains` · main = `0d7e2a7 Initial commit` · ชื่อเกม = **Hollow Chains** (D-013) |
| Branch งาน | `phase/P3-core-systems` (แตกจาก `p2-done`) |
| Unity MCP | ✅ ต่อ repo แล้ว (`D:/Git/Hollow-Chains`, D-021) |
| เอกสารต้นทาง | `00_MASTER_PROMPT` + `01–09` (อยู่นอก repo ที่ `D:\DL\`) |

---

## P0 — สรุป Kickoff (2026-10-18)

### เกมคืออะไร (1 บรรทัด)
Story-driven 3D Action-Adventure RPG แบบ Open-Hub (Unity) — เด็กหนุ่มไร้เวท "0" ในโลกที่ทุกคนมีเวท ร่างกายคือภาชนะของปีศาจ 3 ตน; เดโม = Prologue ~20–30 นาที จบด้วยเอเลนาปฏิเสธเขา

### CANON (ห้ามเปลี่ยนเอง)
- โลก: ทุกคนมีเวท ≥ 1 แขนง (ราชวงศ์ ≥ 2) · หมู่บ้าน **Astraon** · ป่าลึก · หน้าผาลำธาร
- เวท: Aethel / Soma / Vecta / Aeterna
- Phineas = 0 → Vessel ของปีศาจ 3 ตน · ฟอร์ม 1 Inversus (ขั้วตรงข้าม) · ฟอร์ม 2 Radix (รากดำดูดวิญญาณ + Grapple)
- Elena: เพื่อนคนเดียว → ปฏิเสธเขาในฉากจบ (บทพูดตาม `06`) · พ่อ = ฆาตกรที่หายตัว + ข้อความ "จงหนีไป... ผนึกกำลังจะแตกแล้ว"
- หมาป่าทมิฬ (ไอดำ) · Beat sheet เดโม Scene 1–6 · บทบรรยาย Comic Intro
- Comic Intro อยู่ในดีไซน์แต่ **เลื่อนทำทีหลัง** (ต้องเว้นช่องให้เสียบได้)
- Gameplay: เควสเป็น Phase, เก็บเลเวล, บอสเรื่อย ๆ, ต่อสู้สไตล์ Persona, Open World เทียม (แมพเล็กแต่แน่น)
- **เพิ่มจาก Owner (ดู DECISION_LOG):** D-001 ลำดับ Phase · D-002 HUD = HP อย่างเดียว · D-003 เอเลนาใช้เวทน้ำสู้หมาป่าแล้วล้มเหลว · D-004 Mixamo · D-005 Skills ทยอยสร้าง

### PROPOSAL (รออนุมัติ — ไม่ block เดโม เว้นแต่ระบุ)
- ชื่อเกม (Q-002) · แพลตฟอร์ม PC/Steam · ไม่มีเสียงพากย์ในเดโม
- Mentis / Fatum · ฟอร์ม 3 Nihil + ชื่อ Radix · Affinity Wheel · Humanity/Polarity รายละเอียดตัวเลข
- ระบบต่อสู้ทั้งหมดใน `04` §4 · บอสหลังเดโม · Chain Tree · Soul Fragment / Memory Echo
- โซนหลังเดโม (Blackthorn Hollow, The Pit, Royal Border) · Performance target
- ความลึกของเอเลนา, ความจริงของพ่อ (**ห้ามเดา — ต้องถามเจ้าของ แต่ไม่จำเป็นสำหรับเดโม**)
- `[IMPL]` ทั้งหมดใน `06` (Interactive beat, Ambient barks, ฯลฯ)

### ความเสี่ยงที่ทีมเห็น (Risk Register ฉบับเต็มทำใน P1)
| # | ความเสี่ยง | เจ้าของ | ทางรับมือ |
|---|---|---|---|
| R1 | ฟอนต์ไทย TMP วรรณยุกต์/สระซ้อน | ณิชา | ทดสอบตั้งแต่ P1.3 (spike), ฟอนต์ OFL (เช่น ตระกูล Noto/Sarabun — ตรวจ license) |
| R2 | ฉาก Awakening (Scene 4–6) ต้องใช้ Asset/Anim/VFX หนักที่สุด | อ.ชัย / พลอย | ขอ Asset ตั้งแต่จบ P1; Placeholder สื่ออารมณ์ด้วยแสง/เสียง/กล้องก่อน |
| R3 | Scope บาน (ระบบ RPG เต็มก่อนเดโม) | ปุณ / พี่พิม | D-001 + D-002 ตัดแล้ว; P3 ทำเฉพาะระบบที่เดโมใช้ |
| R4 | ไม่มีตัวละคร rigged จริง | ต้น / เบนซ์ | Mixamo (D-004) ตรวจ license ก่อน |
| R5 | Git LFS quota ฟรีจำกัด | เอก | Asset หนักทยอยเพิ่ม, ไม่ commit ไฟล์ดิบที่ไม่ใช้ |
| R6 | Contribution graph ไม่นับ ถ้า `user.email` ไม่ตรงบัญชี GitHub | เอก | Q-003 ให้เจ้าของตรวจ |
| R7 | "รู้สึก" ของฉากจบ (เกณฑ์ข้อ 3 ใน `06`) วัดได้แค่จาก playtest | บอล / จ๊ะจ๋า | Playtest ภายในตั้งแต่ P5 greybox ไม่รอ Asset จริง |

### Phase Plan (ฉบับแก้ — ✅ อนุมัติแล้ว D-010)
ลำดับ: **P0 → P1 → P2 → P3 → P5 → P6 → P4 → P7 → P8+** (P6 ก่อน P4 = D-011)

| ลำดับ | Phase | ผลลัพธ์หลัก | Skills (สร้างตอนเริ่ม Phase) | เงื่อนไขจบ |
|---|---|---|---|---|
| 1 | **P0** Kickoff | ประชุมเปิด, สรุป CANON/PROPOSAL, Phase Plan, STUDIO_LOG/DECISION_LOG | — | เจ้าของอนุมัติแผน |
| 2 | **P1** Pre-production | P1.1 GDD เดโม freeze (รวม D-003 ฉากเอเลนา) · P1.2 Tech Spec (Package, โฟลเดอร์, asmdef, Event Bus) · P1.3 Unity init + `.gitattributes`/LFS + Force Text + `tools/gitbatch.sh` + ลบ template + spike ฟอนต์ไทย · Art Bible สั้น · Backlog · Risk Register · Asset Request ชุดแรก | #43, #49, #1, #2, #47 | อนุมัติ GDD/Tech |
| 3 | **P2** Tech Prototype | Player controller, กล้อง, Interact, Greybox ห้อง→หมู่บ้าน→ป่า→หน้าผา | #3, #4, #5, #8, #27, #36 | เดินจากห้อง → หน้าผาได้ลื่น |
| 4 | **P3** Core Systems (ตัดให้พอดีเดโม) | Quest Phase (Q001), Dialogue/Subtitle, UI framework + TMP ไทย, **HUD = HP**, Humanity (ซ่อน, data only), Checkpoint save. Inventory = key item (ถังน้ำ) เท่านั้น | #15, #16, #10, #26, #28, #29, #31 (#20 แบบเบา) | Q001 รันครบทุก Phase |
| 5 | **P5** Vertical Slice (Demo) | Scene 0 placeholder + Scene 1–6, ฉากเอเลนาใช้เวทน้ำ (D-003), Awakening, Cliffhanger, Asset จริง/Placeholder | #6, #7, #24, #25, #37, #38, #40, #41, #42 | เล่นจบเดโม ~20–30 นาที |
| 6 | **P6** Polish, QA & Demo Build | Bug fix, Perf, Accessibility, Build, Playtest ≥ 3 คน | #12, #13, #14, #33, #34, #48 | ผ่านเกณฑ์ใน `06` → tag `p6-demo-v0.1` |
| 7 | **P4** Combat Prototype | Turn-based Persona-style, Form Swap stub, Weakness/1 More/All-Out, ศัตรู 1–2, Polarity เริ่มใช้เมื่อ Inversus ปลดล็อก | #17, #18, #19, #21, #23, #30, #32 | ต่อสู้ 1 encounter ได้ |
| 8 | **P7** Comic Intro | ตัวเล่น Comic + Panels | #35 | เจ้าของสั่งเริ่ม |
| 9 | **P8+** Full Game | โซนใหม่, ฟอร์ม 2–3, บอส, Chain Tree | ตามแผนใหม่ | ตามแผนใหม่ |

> หมายเหตุ: เลข Phase คงเดิมตาม `00`/`09` (ไม่เปลี่ยนชื่อ branch/tag) — เปลี่ยนแค่ลำดับ. VFX #38 ย้ายเข้า P5 เพราะฉากตื่นรู้/เวทน้ำใช้

### ✅ ไฟล์ที่ส่งใน P0
- `Docs/STUDIO_LOG.md` (ไฟล์นี้) · `Docs/DECISION_LOG.md`

### ❓ คำถามค้าง
- ~~Q-001~~ → D-011 (P6 ก่อน P4) · ~~Q-003~~ → D-012 (อีเมลตรงแล้ว)
- **Q-002 ชื่อเกม** — รอเจ้าของเลือก (ตัวเลือกใน DECISION_LOG); ระหว่างนี้ใช้ `Hollow Chains`

### 🔀 Git
- Branch: `phase/P0-kickoff` · commits: `36a00f4` + commit ปิด P0 · tag: `p0-done` · push: branch + tag ขึ้น origin (ไม่แตะ `main`)
- Merge เข้า `main`: **ยังไม่ทำ** — เสนอพร้อมจบ P1 (ตอนนั้น main จะเปิดใน Unity ได้ครบ เพราะ P1.3 track `Assets/` `Packages/` `ProjectSettings/`)

---

## P1 — Pre-production (เริ่ม 2026-10-18)
Branch: `phase/P1-preproduction` (แตกจาก `phase/P0-kickoff`)

| Sub-phase | ผลลัพธ์ | สถานะ |
|---|---|---|
| P1.0 | Skills #43, #49, #1, #2, #47 ใน `.claude/skills/` | ✅ |
| P1.1 | `Docs/P1/GDD_DEMO.md` **v1.0 FROZEN** (D-018) | ✅ |
| P1.2 | `Docs/P1/TECH_SPEC.md` v0.9 (Package จาก registry จริง, โฟลเดอร์, asmdef, Event Bus) | ✅ อนุมัติ (D-019) |
| P1.2b | `Docs/P1/CLASS_DIAGRAM.md` v0.1 (แผนคลาส OOP — D-020) · `com.unity.ai.assistant` ใน manifest | ✅ |
| P1.3 | Unity init: `.gitattributes`+LFS, `tools/gitbatch.sh`, Linear/Force Text/Input System only, packages ตาม Tech Spec, ลบ template, โฟลเดอร์ + asmdef 8 ตัว, Localization `en` + table `UI` (ทำผ่าน Unity batchmode) | ✅ |
| P1.4 | `Docs/P1/ART_BIBLE.md` · `Docs/P1/BACKLOG.md` · `Docs/P1/RISK_REGISTER.md` · `Docs/ASSET_TRACKER.md` (ชุดที่ 1) | ✅ |

### ⚠️ P1 — สิ่งที่พบ
- ~~R8~~ MCP ต่อผิดโปรเจกต์ → ปิดแล้ว (D-021): MCP ชี้ `D:/Git/Hollow-Chains/Assets`
- Q-004–Q-007 ตอบแล้ว (D-014–D-017) · R1 ฟอนต์ไทย เลื่อนไปตอนเพิ่มภาษาไทย (D-017) · ค้าง: Q-009 (P3.2), Q-011 สไตล์ภาพ, Q-008 mood ref

### 🔀 Git (P1)
- `phase/P1-preproduction` · P1.0 `84f63fe` skills · P1.1 `8b9d763` GDD v0.9 + freeze v1.0 · P1.2 Tech Spec — push แล้ว
- P1.3 `efa55c7`…`6f6d3a7` (10 commits) — push แล้ว · Unity log: ไม่มี error; warning "asmdef has no scripts" ×8 = ปกติจนกว่าจะมีโค้ดใน P2
- หมายเหตุ: Localization สร้าง `Assets/AddressableAssetsData/` เอง (Addressables เป็น dependency) — track ไว้ตามมาตรฐาน
- P1.4 docs + ProjectSettings ของ package (ai.assistant, probuilder) — push แล้ว · tag `p1-done`

## P1 — Phase Report
✅ **ทำเสร็จ:** Skills 5 ตัว · GDD v1.0 FROZEN · Tech Spec (อนุมัติ) · Class Diagram · Unity init (settings/packages/asmdef/Localization) · Art Bible · Backlog P2–P6 · Risk Register · Asset Request ชุดที่ 1 (P0 30 รายการ)
⚠️ **ความเสี่ยงค้าง:** R2 asset ฉาก Awakening · R4 ตัวละคร rigged · R7 วัดความรู้สึกฉากจบ · R9 คำแปลบท CANON (ดู `RISK_REGISTER.md`)
❓ **คำถาม L3:** Q-011 สไตล์ภาพ (default ก) · Q-008 mood reference · (Q-009 ถามใน P3.2)
📦 **Asset Request:** `Docs/ASSET_TRACKER.md` ชุดที่ 1
🔀 **Git:** `phase/P1-preproduction` · tag `p1-done` · เสนอ merge → `main` (merge commit, ไม่ squash) รออนุมัติ
▶️ **ถัดไป:** P2 Tech Prototype (B-201…B-210) — skills #3, #4, #5, #8, #27, #36

---

## P2 — Tech Prototype (เริ่ม 2026-10-18)
การตัดสินใจก่อนเริ่ม: D-023 (ProBuilder blocking + capsule, ไม่มี animation) · D-024 (เจ้าของ playtest เอง — ทีมแจ้งจุดทดสอบ) · D-025 (เดโมกลับเป็นภาษาไทย) · D-026 (สไตล์ ก) · D-027 (หมู่บ้านในกำแพงเมืองยุคกลาง)

| Sub-phase | ผลลัพธ์ | สถานะ |
|---|---|---|
| P2.0 | Skills #3 #4 #5 #8 #27 #36 · Core: event channels, StateMachine, ServiceLocator + EditMode tests 11/11 ผ่าน (B-201) | ✅ |
| P2.0b | `_Boot` (Main Camera + Brain, UI Root, Fade) + GameBootstrapper + SceneFlowService + BootSceneLoader (กด Play ฉากไหนก็ได้ _Boot ตามมาเอง) (B-202) | ✅ |
| P2.1 | IPlayerInput/PlayerInputReader, PlayerController + Motor + 4 states, config SO, Player capsule prefab, scene `P2_PlayerTest` — play mode smoke ผ่าน (B-203/204) | ✅ |
| P2.2 | PlayerRig prefab: CM_Explore (OrbitalFollow + RotationComposer + Deoccluder) + OrbitLookInput (mouse/stick) + SprintCameraEffect (B-205) | ✅ |
| P2.3 | IInteractable/InteractableBase → Pickup (ถัง) / Inspect (ความคิด); Interactor; PlayerTriggerZone → SceneTransition/Sprint/Event; UIView + prompt/thought text (B-206) | ✅ |
| P2.4 | GreyboxKit (ProBuilder) + S01 ห้องนอน · S02 ตรอกมืด→ย่านหมู่บ้าน→ถนน→ประตูกำแพง + เมืองฉากหลัง · S03 ป่า (ต้นไม้ตาย, จุดกรีดร้อง, sprint zone) · S04 ลานหน้าผา (Elena/หมาป่า capsule) + Volume ต่อฉาก (B-207–210) | ✅ ผ่าน (D-029) |

### 🔀 Git (P2)
- `phase/P2-tech-prototype` · docs `65f4e05`…`85ec697` · skills `2b8f5e9` · P2.0 `bbb67c8`…`3447630` · P2.1 `d1f6eb3`…`c663cad` — push แล้ว
- PR P1 → main: เปิดโดยเจ้าของผ่านลิงก์ compare (เครื่องนี้ไม่มี `gh`)
- P2.0b–P2.4 `…`→`06848d4` (22 commits) — push แล้ว · Play-mode smoke (step frames ผ่าน MCP): Boot adopt ✓, S01→S02→S03→S04 load ✓, ประตูห้องนอน trigger ✓, หยิบถัง → Carry ✓, กล้องตัวเดียว ✓, Console ไม่มี error

### 🎮 จุดเทสของเจ้าของ (D-024) — P2 "เดินจากห้อง → หน้าผา"
เปิด `Assets/_Project/Scenes/S01_Bedroom` → Play (หรือเปิด `_Boot` แล้ว Play) · WASD/สติ๊กซ้าย เดิน, เมาส์/สติ๊กขวา หมุนกล้อง, E/ปุ่ม Interact, Shift = sprint (ใช้ได้เฉพาะในป่าช่วงหลังเสียงกรีด)
สังเกต: (1) ความเร็ว/ความหนืดเดิน-วิ่ง (2) กล้องในห้องแคบ/ตรอก ทะลุหรือกระตุกไหม (3) ตรอกมืด→หมู่บ้าน รู้สึก "เมืองใหญ่ แต่ไม่มีใครมองเรา" ไหม (4) หลงทางไหม (มองเห็นต้นไม้ใหญ่นอกประตูไหม) (5) ระยะทางแต่ละฉากยาว/สั้นไป
- P2.5 ลำธารที่ S04 (น้ำตก → แอ่ง → ลำธารผ่านลาน) + ลำห้วยนำทางใน S03 ตาม feedback เจ้าของ (D-029)

## P2 — Phase Report
✅ **ทำเสร็จ:** Skills 6 ตัว · Core (events/FSM/services, tests 11/11) · Player controller + capsule · _Boot + SceneFlow · กล้อง Cinemachine 3 · Interaction + triggers + prompt UI · GreyboxKit + S01–S04 + Volume · ลำธาร (P2.5)
⚠️ **ค้าง:** ข้อความยัง `[PH]` อังกฤษ (ฟอนต์ไทย P3.3) · ถังไม่ข้ามฉาก (P3 Quest/Save) · ScriptedInteractable (กอด/ยื่นมือ) → P5
❓ **คำถาม:** Q-016 แนวเรื่องนาฬิกาพก
📦 **Asset Request:** ไม่มีรายการใหม่ — ยังรอ **ฟอนต์ไทย UI-001/002** (ใช้ใน P3.3)
🔀 **Git:** `phase/P2-tech-prototype` · tag `p2-done` · merge: รอ PR P1 ก่อน แล้วค่อยเปิด PR P2 (P2 มี P1 อยู่ในตัว — merge P2 อย่างเดียวก็ได้)
▶️ **ถัดไป:** P3 Core Systems

---

## P3 — Core Systems (เริ่ม 2026-10-18)
| Sub-phase | ผลลัพธ์ | สถานะ |
|---|---|---|
| P3.0 | Skills #15 #16 #10 #28 #29 #31 #20 · Lore proposal นาฬิกาพก (Q-016) | ✅ |
| P3.1 | Quest: GameDataSO, QuestSO/QuestPhaseSO, PhaseConditionSO + EventRaised, QuestRunner, QuestService (_Boot) + HUD tracker · Q001 data 5 phases (D-031) · tests 17/17 · play-test: tracker + prompt + หยิบถัง → P2 ✓ | ✅ |
| P3.1b | **แก้บั๊ก:** reference event channel หลุดเป็น null ใน _Boot/S03/S04 (โหลด asset ก่อนเปิดฉาก) → prompt/thought/tracker ไม่ขึ้น และเสียงกรีดใน S03 ไม่ยิง event — ต่อใหม่ครบ + บันทึกบทเรียนใน skill `unity-project-setup` | ✅ |
| P3.2 | DialogueLineSO + EVT_PlayLine, SubtitleQueue (tests), DialogueRunner (_Boot), BarkEmitter ×8 ใน S02, LineTriggerZone (ความคิดถึงเอเลนาตอนออกจากตรอก), นาฬิกาพกใน S01 (D-032) · tests 21/21 · play-test ✓ · บทยังเป็น `[PH]` อังกฤษจนถึง P3.3 (B-302) | ✅ |
| P3.3a | ฟอนต์ไทย Sarabun + Mali (+ Amatic) · TMP SDF · **ThaiMarkPositioner** แก้วรรณยุกต์ซ้อน (D-034, ภาพก่อน/หลัง) · Localization: locale `th` เริ่มต้น, tables UI/Dialogue/Quests, LocalizedString ใน Dialogue/Quest/Prompt · บทไทย 30 รายการ (D-035) · tests 27/27 · play-test ไทยขึ้นครบ | ✅ |
| P3.3b | GameFlowService (Title→Playing⇄Paused→End) · เมนู Title/Pause (แสดงภารกิจปัจจุบัน)/Settings (เสียง, ขนาดซับ, กลับแกนกล้อง)/End เป็นภาษาไทย · Pause = Esc/Start · HUD: แถบ HP (Health + HealthModel) + จอแดงตอน HP วิกฤต · tests 31/31 · play-test flow 9 ขั้นผ่าน (D-036) · เสียง CC0 ชุดที่ 1 (D-037) | ✅ |
| P3.4 | SaveFile/SaveService (ISaveable), PlayerStateService (HP, Humanity data-only, ของในมือ) + PlayerStateSync, ถังข้ามฉาก + หลุดมือ (D-015), CheckpointTrigger CP_01/CP_02 + ข้อความ "บันทึกแล้ว", ปุ่ม "เล่นต่อจากจุดบันทึก", ฝีเท้า Kenney + เสียงกลอนประตู · tests 35/35 · play-test 8 ขั้นผ่าน (D-038, D-039) | ✅ |

## P3 — Phase Report
✅ **ทำเสร็จ:** Quest (Q001 5 phase) · บทพูด/ซับ/เสียงกระซิบ · ภาษาไทย (Sarabun + Mali + ThaiMarkPositioner, Localization th) · เมนู Title/Pause/Settings/End · HUD HP + จอแดง · Save/Checkpoint/Continue · ถังข้ามฉาก/หลุดมือ · ฝีเท้า + เสียงประตู (Kenney) · นาฬิกาพก (D-032) · tests 35/35
⚠️ **ค้าง/ความเสี่ยง:** ไฟล์ฟอนต์ dynamic เปลี่ยนทุกครั้งที่เล่น (ทำ static ใน P6) · ยังไม่มี AudioMixer (P5) · เมนูยังเป็น UI greybox · ญ/ฐ + สระล่าง
❓ **คำถาม:** Q-017 (ตระกูลฝั่งแม่ — หลังเดโม) · Q-018 (ตรวจบทไทย)
📦 **Asset Request:** เสียง Freesound ชุดแนะนำ 4 ตัว (เจ้าของโหลด) · ฟอนต์ ✅ ครบ · โมเดลยังไม่ต้อง (D-023)
🔀 **Git:** `phase/P3-core-systems` · tag `p3-done` · `main` ยังเป็น Initial commit → แนะนำเปิด PR ครั้งเดียวจาก `phase/P3-core-systems` (มี P0–P3 ครบ) แบบ merge commit
▶️ **ถัดไป:** P5 Vertical Slice — Scene 0–6 (Comic placeholder, ฉากเวทน้ำ, Awakening, Cliffhanger) ตาม D-001
