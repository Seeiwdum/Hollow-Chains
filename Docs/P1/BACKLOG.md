# Backlog — Hollow Chains Demo (v0.1)

> เจ้าของ: ปุณ (Producer) · อ้างอิง: GDD v1.0 (FT-xx, SC-xx, AC-xx), Tech Spec, `09` §3 (sub-phase = จุด commit+push)
> ขนาด: S ≤ 1 session · M 1–3 sessions · L > 3 sessions · ลำดับ Phase ตาม D-001/D-011: P2 → P3 → P5 → P6 → P4

## P2 — Tech Prototype (Greybox) · จบเมื่อ: เดินจากห้อง → หน้าผาได้ลื่น
| ID | Sub | งาน | อ้างอิง | เจ้าของ | ขนาด | Definition of Done |
|---|---|---|---|---|---|---|
| B-201 | P2.0 | Core: EventChannelSO, StateMachine, ServiceLocator + EditMode tests | Class diagram §1 | ธีร์ | M | tests ผ่าน, diagram ตรง |
| B-202 | P2.0 | `_Boot` scene + SceneFlowService (additive + fade) | FT-14 | เอก | M | Boot → โหลด scene ทดสอบได้ |
| B-203 | P2.1 | Input actions (Player/UI/Cutscene) + PlayerInputReader | Tech §6 | กานต์ | S | KB/M + gamepad ขยับได้ |
| B-204 | P2.1 | PlayerController + Motor + states (Locomotion/Carry/Sprint/Locked) | FT-01 | กานต์ | L | เดิน/วิ่ง/sprint, ขึ้นลงทางลาด, ไม่ทะลุ |
| B-205 | P2.2 | Cinemachine third-person + sprint cam + กันทะลุกำแพง | FT-02 | กานต์ | M | ห้องแคบไม่ทะลุ |
| B-206 | P2.3 | IInteractable + Interactor + InteractPrompt (world-space) | FT-03 | กานต์/ณิชา | M | prompt โผล่/หายถูกระยะ |
| B-207 | P2.4 | Greybox S01 ห้องนอน | SC-01 | ฟ้าใส | S | ขนาดจริง, props สีเหลือง |
| B-208 | P2.4 | Greybox S02 หมู่บ้าน (เส้นหลัก + ซอย 2–3, landmark) | SC-02 | ฟ้าใส | L | เดินผ่าน 5–7 นาที, POI ทุก 30–60 วิ |
| B-209 | P2.4 | Greybox S03 ป่า + S04 ลานหน้าผา | SC-03/04 | ฟ้าใส | M | sprint path ชัด, ลานกึ่งวงกลม |
| B-210 | P2.4 | URP Volume ต่อ scene (cold/warm/forest) ตาม Art Bible §4 | FT-13 | พลอย | S | เห็นต่างกันชัด |

## P3 — Core Systems · จบเมื่อ: Q001 รันครบทุก Phase
| ID | Sub | งาน | อ้างอิง | เจ้าของ | ขนาด | DoD |
|---|---|---|---|---|---|---|
| B-301 | P3.1 | QuestSO/PhaseSO/Conditions + QuestService + tracker | FT-04 | นนท์/กานต์ | L | Q001-P1..P5 ผ่าน, AC-05 |
| B-302 | P3.2 | DialogueLineSO + DialogueRunner + SubtitleView + barks | FT-05 | อ.รัตติกาล/ณิชา | M | ซับซิงก์, ข้อความจาก string table |
| B-303 | P3.2 | เขียนบทอังกฤษทั้งเดโม + แปลบท CANON 3 ชิ้น → ขออนุมัติ (Q-009) | D-017 | อ.รัตติกาล | M | เจ้าของอนุมัติ |
| B-304 | P3.3 | UI framework (UIView, MenuView) + Title/Pause/Settings/End | FT-06 | มีนา/ณิชา | M | นำทางด้วย gamepad ได้ |
| B-305 | P3.3 | HUD: HP bar + Critical vignette (ไม่มี Humanity UI) | FT-07, D-002 | ณิชา | S | AC-07 |
| B-306 | P3.3 | Health + HumanityState (data only) | FT-08 | นนท์ | S | tests clamp ผ่าน |
| B-307 | P3.4 | SaveService (JSON, version, temp+rename) + CheckpointTrigger + Continue | FT-09 | เอก | M | round-trip test, AC-09 |

## P5 — Vertical Slice (Demo) · จบเมื่อ: เล่นจบเดโม ~20–30 นาที
| ID | Sub | งาน | อ้างอิง | เจ้าของ | ขนาด | DoD |
|---|---|---|---|---|---|---|
| B-501 | P5.0 | S00 placeholder: ComicPlayer + SubtitleComicPage + Skip | SC-00 | กานต์ | S | Skip ได้ |
| B-502 | P5.1 | SC-01 ครบ (สอนควบคุม, story props, ถัง) | SC-01 | กานต์/ฟ้าใส | M | Q001-P1 |
| B-503 | P5.2 | SC-02 ambience: barks spatial, ประตูปิด, monologue + warm volume | SC-02, FT-11 | กานต์/แพรว | L | ไม่มีชาวบ้านใจดี (D-016) |
| B-504 | P5.3 | SC-03 environmental storytelling + sprint sequence + ถังหลุดมือ | SC-03, D-015 | ฟ้าใส/กานต์ | M | — |
| B-505 | P5.4 | SC-04 cutscene เวทน้ำล้มเหลว + prompt กอด + HP = 1 | SC-04, D-003/D-014 | กานต์/ไฟ | L | AC-06 |
| B-506 | P5.5 | SC-05 Awakening: Timeline + HoldBeat + ราก + มัมมี่ + flashback + audio snapshot | SC-05 | กานต์/ไฟ/ภีม | L | ล้มเหลวไม่ได้ |
| B-507 | P5.6 | SC-06 ยื่นมือ + ปฏิเสธ + ข้อความพ่อ + zoom out + End | SC-06 | กานต์/เบนซ์ | M | บท CANON ตรงตัว |
| B-508 | P5.x | แทน placeholder ด้วย asset จริงที่เจ้าของส่ง (ตาม Asset Tracker) | `08` | พลอย | L | license check ครบ |
| B-509 | P5.x | Playtest ภายในรอบแรก (greybox ไม่รอ asset) | R7 | บอล/จ๊ะจ๋า | S | บันทึกความรู้สึกผู้เล่น |

## P6 — Polish, QA & Demo Build · จบเมื่อ: ผ่านเกณฑ์ใน `06` → tag `p6-demo-v0.1`
| ID | งาน | อ้างอิง | เจ้าของ | ขนาด |
|---|---|---|---|---|
| B-601 | Bug bash + triage | AC-01 | จ๊ะจ๋า | M |
| B-602 | Profiling → 60 FPS 1080p GTX 1060 | AC-04, Tech §7 | ธีร์/พลอย | M |
| B-603 | Accessibility ขั้นต่ำ (ขนาดซับ, remap, invert) | `04` §6 #9 | มีนา/ณิชา | M |
| B-604 | Controller glyphs (KB/M ↔ gamepad) | skill #34 | ณิชา | S |
| B-605 | Build profile Windows IL2CPP + smoke test เครื่องอื่น | AC-04 | เอก | S |
| B-606 | Playtest ≥ 3 คน + แบบสอบถาม (AC-03, AC-08) | `06` #3 | บอล | M |

## P4 — Combat Prototype (หลังเดโม) — รายละเอียดแตกตอนเริ่ม P4
Turn engine · Weakness/Down/1 More/All-Out · Form Swap stub · Battle UI · ศัตรู 1–2 · Polarity (เมื่อ Inversus ปลดล็อก)

## ไม่อยู่ใน backlog เดโม (ตั้งใจตัด)
Comic จริง (P7) · ภาษาไทย (เพิ่มหลังเดโม, D-017) · ชาวบ้านใจดี (D-016) · Inventory เต็ม · Soul Fragment เป็นระบบ · Fast travel
