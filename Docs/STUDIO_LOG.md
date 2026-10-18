# STUDIO_LOG — Hollow Chains Studio

> ดูแลโดย: ใบเตย 🐱 · อัปเดตทุกครั้งที่จบ Phase
> วิธีใช้: คัดลอกไฟล์นี้ (+ `DECISION_LOG.md`) ไปวางในแชทใหม่ พร้อมไฟล์ `00–09` เพื่อทำงานต่อโดยไม่เสียบริบท

---

## สถานะปัจจุบัน
| หัวข้อ | ค่า |
|---|---|
| Phase | **P0 ปิดแล้ว (D-010)** → **P1 Pre-production กำลังทำ** |
| วันที่ | 2026-10-18 |
| Unity | 6000.3.10f1 · URP 17.3.0 · Input System 1.18.0 (template เริ่มต้นยังอยู่: Scenes, Settings, TutorialInfo, Readme) |
| Repo | `github.com/Seeiwdum/Hollow-Chains` · main = `0d7e2a7 Initial commit` · ชื่อเกม = **Hollow Chains** (D-013) |
| Branch งาน | `phase/P1-preproduction` (แตกจาก `phase/P0-kickoff` เพราะ main ยังไม่มี Docs) |
| Unity MCP | ⚠️ Editor ยังเปิด `D:/project for unity/Hollow Chains` (Q-010) — P1.3 ทำผ่าน batchmode แทน |
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
| P1.4 | Art Bible สั้น · Backlog · Risk Register · Asset Request ชุดแรก | ⏳ |

### ⚠️ P1 — สิ่งที่พบ
- **R8 (L1 ก่อน P1.3):** Unity Editor ที่ MCP ต่ออยู่เปิดโปรเจกต์ `D:/project for unity/Hollow Chains` (ไม่มี git, ไม่มี URP/Input System, มี `com.unity.ai.assistant`) — **ไม่ใช่** repo `D:\Git\Hollow-Chains` (ซึ่งไม่มี `com.unity.ai.assistant`). ก่อน P1.3 ต้องให้เจ้าของเปิด repo ใน Unity และเพิ่ม package `com.unity.ai.assistant` เพื่อเปิด MCP Server ในโปรเจกต์นี้
- Q-004–Q-007 ตอบแล้ว (D-014–D-017) · R1 ฟอนต์ไทย เลื่อนไปตอนเพิ่มภาษาไทย (D-017) · ค้าง: Q-009 คำแปลบท CANON, Q-010 โปรเจกต์ Unity

### 🔀 Git (P1)
- `phase/P1-preproduction` · P1.0 `84f63fe` skills · P1.1 `8b9d763` GDD v0.9 + freeze v1.0 · P1.2 Tech Spec — push แล้ว
- P1.3 `efa55c7`…`6f6d3a7` (10 commits) — push แล้ว · Unity log: ไม่มี error; warning "asmdef has no scripts" ×8 = ปกติจนกว่าจะมีโค้ดใน P2
- หมายเหตุ: Localization สร้าง `Assets/AddressableAssetsData/` เอง (Addressables เป็น dependency) — track ไว้ตามมาตรฐาน
