# DECISION LOG — Hollow Chains Studio

> ดูแลโดย: ใบเตย (Documentation Lead) · ผู้ตัดสินสุดท้าย: NAMOE (Owner / Creative Director)
> ระดับ: **OWNER** = เจ้าของตัดสิน (มีผลเท่า CANON) · **L4** = ทีมตัดสินเอง (แจ้งเพื่อทราบ) · **PENDING** = รอเจ้าของ
> ห้ามแก้ย้อนหลัง — ถ้าเปลี่ยนใจ ให้เพิ่มรายการใหม่ที่ "แทนที่" รายการเดิม

| ID | วันที่ | Phase | การตัดสินใจ | ระดับ | ผู้ตัดสิน | กระทบเอกสาร |
|---|---|---|---|---|---|---|
| D-001 | 2026-10-18 | P0 | **ลำดับ Phase ใหม่:** P3 Core Systems → **P5 Vertical Slice (Demo)** → **P4 Combat**. เหตุผล: หมาป่าทมิฬในเดโมเป็น Awakening Sequence แบบ Scripted และเกณฑ์ผ่านเดโมใน `06` ไม่ต้องใช้ Turn-based combat | OWNER | NAMOE | 00 §6, 09 §3 |
| D-002 | 2026-10-18 | P0 | **HUD เดโมแสดง HP อย่างเดียว.** Humanity ทำงานเบื้องหลัง (ซ่อน ไม่แสดง UI). Polarity เลื่อนไปจนกว่าฟอร์ม Inversus จะปลดล็อก | OWNER | NAMOE | 03 §2, 04 §3, 07 #21/#29 |
| D-003 | 2026-10-18 | P0 | **ฉากใหม่ใน Scene 4:** ก่อน Phineas มาถึง เอเลนาใช้เวท **Aethel-Water** สู้หมาป่าทมิฬแล้ว **ล้มเหลว** — ตอกย้ำว่า "ทุกคนมีเวท ยกเว้นเขา" (owner ref: เดกุวิ่งเข้าไปช่วยบาคุโก). ผลพลอยได้: เวทน้ำของเอเลนา (เดิม PROPOSAL ใน `02`) ถือว่าได้รับการยืนยัน. เพิ่ม VFX ~1 ชิ้น | OWNER | NAMOE | 02 §2 (Elena), 06 Scene 4, 08 |
| D-004 | 2026-10-18 | P0 | **Placeholder ตัวละคร/แอนิเมชัน:** ใช้ Mixamo ได้ — แต่ต้องตรวจ License ก่อนใช้ทุกครั้ง และบันทึกผลตรวจใน Asset Tracker | OWNER | NAMOE | 08 |
| D-005 | 2026-10-18 | P0 | **Unity Skills:** Skill `unity-*` ตาม `07` ไม่มีใน catalog → ทีมสร้างเองด้วย `skill-creator` แบบ **ทยอยตาม Phase** (P1 = #43, #49, #1, #2, #47) เก็บที่ `.claude/skills/` ใน repo นี้ — ไม่สร้างทั้ง 49 ตัวล่วงหน้า | OWNER | NAMOE | 07 |
| D-006 | 2026-10-18 | P0 | **ประชุมสั้น:** 5–12 บรรทัด เรียกเฉพาะพนักงานที่เกี่ยวกับหัวข้อ (ประหยัด token) | OWNER | NAMOE | 00 §4 |
| D-007 | 2026-10-18 | P0 | Engine = Unity **6000.3.10f1** (โปรเจกต์ที่เจ้าของสร้างไว้แล้ว) + URP 17.3 + Input System 1.18 — ไม่ย้ายเวอร์ชัน; ธีร์ยืนยันสถานะ LTS และชุด Package ใน P1.2 | L4 | ธีร์ | 01 §1, 00 §8 |
| D-008 | 2026-10-18 | P0 | เอกสารสตูดิโอเก็บที่ `Docs/` (นอก `Assets/` เพื่อไม่ให้ Unity import) | L4 | ใบเตย/เอก | — |
| D-009 | 2026-10-18 | P0 | `main` มีแค่ Initial commit; `Assets/`, `Packages/`, `ProjectSettings/` ยังไม่ถูก track → จะ track ใน **P1.3** พร้อม `.gitattributes` (LFS) + ตั้ง Force Text / Visible Meta. ระหว่างนี้งาน P0 อยู่บน branch `phase/P0-kickoff` (local, ยังไม่ push) | L4 | เอก | 09 §6 |
| D-010 | 2026-10-18 | P0 | **อนุมัติ Phase Plan ฉบับแก้** (STUDIO_LOG › Phase Plan) → ปิด P0, เริ่ม P1 | OWNER | NAMOE | 00 §6 |
| D-011 | 2026-10-18 | P0 | **Q-001 → P6 อยู่ก่อน P4:** ลำดับสุดท้าย P5 → P6 → P4. เจ้าของมอบให้ทีมตัดสิน. เหตุผล: ได้ demo build ที่ playtest ได้จริงเร็วสุด (เกณฑ์ `06` ข้อ 3 วัดได้จาก playtest เท่านั้น), feedback ฉากจบไหลกลับมาก่อนลงทุนระบบ combat, และ P4 ไม่ถูกใช้ในเดโม (D-001). ความเสี่ยง: ถ้า P4 ต้องแก้สถาปัตยกรรม → ธีร์วาง hook combat/Form ไว้ตั้งแต่ P1.2 | L4 (มอบอำนาจ) | พี่พิม/ภูผา/ธีร์ | 00 §6, 09 §5 |
| D-012 | 2026-10-18 | P0 | **Q-003:** เจ้าของยืนยันอีเมลบัญชี GitHub; ตรวจแล้ว `git config user.email` (global) ตรงกัน และ commit เดิมทั้ง 2 ใช้อีเมลนี้ — ไม่ต้องแก้ | OWNER | NAMOE / เอก | 09 §9 |
| D-013 | 2026-10-18 | P1 | **Q-002 ชื่อเกม = `Hollow Chains`** (แทน working title `Chains of the Hollow` ใน `01`). ชื่อไทยยังไม่กำหนด | OWNER | NAMOE | 01 §1 |
| D-014 | 2026-10-18 | P1 | **Q-004:** ผู้เล่นเห็นฉากเอเลนาใช้เวทน้ำแล้วล้มเหลวเอง (cutscene สั้นตอนวิ่งถึงลาน) | OWNER | NAMOE | GDD SC-04 |
| D-015 | 2026-10-18 | P1 | **Q-005:** ถังน้ำหลุดมือระหว่าง Sprint ใน SC-03 | OWNER | NAMOE | GDD SC-03 |
| D-016 | 2026-10-18 | P1 | **Q-006:** เจ้าของต้องการให้ **เอเลนาเป็นคนแรกที่ไม่เกลียด Phineas**. ทีมตัดสิน (มอบอำนาจ): ไม่มีชาวบ้านใจดีในเดโม; ชาวบ้านที่ "ไม่ได้เกลียดเต็มที่" เลื่อนไปหลังเดโม | OWNER + L4 (อ.รัตติกาล/ภูผา) | NAMOE | 02 §2, GDD SC-02 |
| D-017 | 2026-10-18 | P1 | **Q-007: เดโมเป็นภาษาอังกฤษ ภาษาไทยทำภายหลัง** — แทนที่ "ไทยหลัก" ของ `01` §1 สำหรับเดโม. ผลกระทบ: `06` เกณฑ์ #2 (ฟอนต์ไทย) ใช้เมื่อเพิ่มไทย; spike ฟอนต์ไทย (R1) เลื่อน; ข้อความทั้งหมดผ่าน Localization string table; บท CANON ต้องแปลอังกฤษ (Q-009) | OWNER | NAMOE | 00 §8, 01 §1, 06, GDD §7 |
| D-018 | 2026-10-18 | P1 | **GDD เดโม v1.0 FROZEN** (`Docs/P1/GDD_DEMO.md`) | OWNER | NAMOE | GDD |
| D-019 | 2026-10-18 | P1 | **อนุมัติ Tech Spec v0.9** (`Docs/P1/TECH_SPEC.md`) + ให้ทีมเพิ่ม `com.unity.ai.assistant` 2.20.0-pre.2 ใน `Packages/manifest.json` ของ repo (Unity MCP) | OWNER | NAMOE | TECH_SPEC §2 |
| D-020 | 2026-10-18 | P1 | **โค้ดต้องยึดหลัก OOP** (Encapsulation / Abstraction / Inheritance / Polymorphism) + SOLID — สะอาด ไม่สปาเกตตี้. ทีมตีความ: Composition ก่อน, inheritance ตื้นเฉพาะ is-a จริง. ดูแล **Class Diagram** (`Docs/P1/CLASS_DIAGRAM.md`, Mermaid) ให้ตรงกับโค้ดทุก commit | OWNER | NAMOE | 00 §8, skill #2 |

## PENDING — รอเจ้าของตอบ (ดู STUDIO_LOG › คำถามค้าง)
| ID | หัวข้อ | ค่า default ของทีม |
|---|---|---|
| Q-009 | อนุมัติคำแปลอังกฤษของบท CANON 3 ชิ้น (บรรยาย SC-00, บทเอเลนา SC-06, ข้อความพ่อ) — ส่งใน P3.2 | อ.รัตติกาลแปลรักษาน้ำเสียง |
| Q-010 | 🔴 L1: Unity Editor ที่ MCP ต่ออยู่เปิด `D:/project for unity/Hollow Chains` ไม่ใช่ repo `D:\Git\Hollow-Chains` | เปิด repo ใน Unity + ติดตั้ง `com.unity.ai.assistant` ในโปรเจกต์ repo |
