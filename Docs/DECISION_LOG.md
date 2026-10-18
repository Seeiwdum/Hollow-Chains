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

## PENDING — รอเจ้าของตอบ (ดู STUDIO_LOG › คำถามค้าง)
| ID | หัวข้อ | ค่า default ของทีม |
|---|---|---|
| Q-001 | ตำแหน่ง P6 (Polish & Demo Build) อยู่ก่อนหรือหลัง P4 Combat | **ก่อน** (P5 → P6 → P4) — ได้เดโมจริงเร็วสุด |
| Q-002 | ชื่อเกม: `Hollow Chains` (ชื่อ repo) vs `Chains of the Hollow` (ใน `01`) | `Hollow Chains` |
| Q-003 | git `user.email` ในเครื่องตรงกับอีเมลบัญชี GitHub ที่ถือ repo หรือไม่ (มีผลกับ Contribution Graph / Farm Mode) | เจ้าของตรวจเอง |
