# Asset Tracker — Hollow Chains

> เจ้าของ: ปุณ (dedupe/ลำดับ) · ตรวจรับ: พลอย (import/scale/URP/LOD) · อนุมัติภาพ: อ.ชัย · รูปแบบตาม `08`
> ความสำคัญ: **P0** = ต้องมีก่อนส่งเดโม · **P1** = ควรมี · **P2** = มีก็ดี
> สถานะ: Requested → Received → Integrated (หรือ Rejected) · **License check บังคับก่อนนำเข้า (D-004)**: บันทึก URL แหล่ง, ชื่อ license, ใช้เชิงพาณิชย์/แจกใน build ได้ไหม, ข้อความ credit, วันที่ตรวจ, ผู้ตรวจ — อ่านจากหน้า license จริงของแหล่งเสมอ
> สเกล 1 unit = 1 m · สไตล์ตาม Art Bible (รอ Q-011; ค่าเริ่ม semi-stylized painterly) · อ้างอิงคือคำบรรยาย/keyword เท่านั้น ห้ามคัดลอก asset จากเกมอื่น

## ชุดที่ 1 — ส่งเมื่อจบ P1 (2026-10-18)

### 3D Character / Animation
| ID | ประเภท | ชื่อ | คำอธิบาย / อารมณ์ | Reference | สเปกเทคนิค | ความสำคัญ | ใช้ใน | Placeholder | สถานะ | License check |
|---|---|---|---|---|---|---|---|---|---|---|
| AST-001 | 3D Character | Phineas (Base) | ผอม ไหล่ห่อ ผมยาวปิดตา เสื้อผ้าเก่าหม่น; แขนขวาเป็นจุดพลัง | keyword: "outcast village boy, long fringe, ragged clothes" | Humanoid rig, FBX, ≤ 30k tris, 2–4K tex, blendshape หน้า (ถ้ามี) | P0 | SC-01–06 | Capsule เทาเข้ม | Requested | — |
| AST-002 | 3D Character | Elena | เด็กหญิงสดใส ชุดสีอุ่น ตั้งตรง | keyword: "kind village girl, warm dress" | Humanoid rig, FBX, ≤ 30k tris | P0 | SC-04–06 | Capsule ครีม | Requested | — |
| AST-003 | 3D Character | Villagers ×4–6 | ชาวบ้านโทนเย็น หน้าไม่เด่น (modular ได้ยิ่งดี) | keyword: "fantasy peasant modular" | Humanoid rig, ≤ 15k tris/ตัว | P1 | SC-02 | Capsule เทาฟ้า | Requested | — |
| AST-004 | 3D Creature | Dark Wolf | หมาป่าใหญ่ 2–3 เท่าคน ไอดำ ตาสว่างจุดเดียว + สถานะมัมมี่แห้ง | keyword: "giant dire wolf, corrupted" | Generic/quadruped rig, FBX, ≤ 40k tris, 2 material states (ปกติ/แห้ง) | P0 | SC-04–05 | Cube ยาว + particle ดำ | Requested | — |
| AST-010 | Animation | Phineas set | Idle, Walk, Run, Sprint, Carry-bucket, Interact, Hit-from-behind, Collapse-protect, Awakening-pain, Kneel-scream, Reach-hand | Mixamo ได้ (D-004) | Humanoid, FBX, 30 fps, in-place + root motion (sprint/collapse) | P0 | SC-01–06 | Mixamo/T-pose | Requested | — |
| AST-011 | Animation | Elena set | Idle, Cry-cornered, Cast-water (D-003), Knocked-down, Push-hand-away, Run-away | Mixamo ได้ | Humanoid, FBX | P0 | SC-04–06 | Mixamo | Requested | — |
| AST-012 | Animation | Dark Wolf set | Idle-prowl, Growl, Pounce, Bite-hold, Shrug-off-water, Death-wither | — | Quadruped, FBX | P0 | SC-04–05 | ไม่มี (slide) | Requested | — |

### 3D Environment / Props
| ID | ประเภท | ชื่อ | คำอธิบาย | สเปก | ความสำคัญ | ใช้ใน | Placeholder | สถานะ | License check |
|---|---|---|---|---|---|---|---|---|---|
| AST-020 | 3D Environment | Village kit | บ้านไม้ชนบทแฟนตาซี หลังคาสูงชัน, รั้ว, บ่อน้ำ, ลานกลาง, ประตู/หน้าต่างเปิด-ปิดได้ | Modular, FBX, LOD0–2, URP Lit | P0 | SC-02 | ProBuilder greybox | Requested | — |
| AST-021 | 3D Environment | Bedroom set | ห้องเล็กเก่า: เตียง, โต๊ะ, หน้าต่าง, ชั้น | FBX, URP | P0 | SC-01 | ProBuilder | Requested | — |
| AST-022 | 3D Environment | Forest kit | ต้นไม้สูงแน่น, พุ่ม, หิน, รากโผล่, ต้นไม้ตาย, ซากไม้ | LOD + GPU instancing ได้, foliage wind | P0 | SC-03–04 | ProBuilder + primitive | Requested | — |
| AST-023 | 3D Environment | Stream cliff | ลานกึ่งวงกลม, หน้าผา, ลำธาร, น้ำตกเล็ก | FBX + water shader URP | P0 | SC-04–06 | ProBuilder | Requested | — |
| AST-030 | 3D Prop | Wooden bucket | ถังน้ำไม้ (ถือได้ + หลุดมือ D-015) | ≤ 2k tris, collider | P0 | SC-01–03 | Cylinder เหลือง | Requested | — |
| AST-031 | 3D Prop | Story props | ภาพวาดพ่อถูกฉีก, ดอกไม้จากเอเลนา, โซ่เก่าใต้เตียง | ≤ 3k tris/ชิ้น | P1 | SC-01 | Cube เหลือง | Requested | — |
| AST-032 | Texture/Decal | Claw marks & dead-tree decals | รอยเล็บ, ไอดำจาง | Decal URP | P1 | SC-03 | — | Requested | — |

### VFX
| ID | ชื่อ | คำอธิบาย / อารมณ์ | สเปก | ความสำคัญ | ใช้ใน | Placeholder | สถานะ |
|---|---|---|---|---|---|---|---|
| VFX-001 | Black Roots Burst (Radix) | รากหนามดำปะทุจากเส้นเลือดแขนขวา พุ่งเสียบ | VFX Graph หรือ mesh+shader, ≤ 2 ms GPU | P0 | SC-05 | particle ดำ |
| VFX-002 | Dark Mist | ไอดำพวยรอบหมาป่า / แขนกลายพันธุ์ | VFX Graph, loop | P0 | SC-04–06 | particle ดำ |
| VFX-003 | Soul Drain | พลังชีวิตไหลจากหมาป่าเข้าราก | VFX Graph | P0 | SC-05 | trail ม่วง |
| VFX-004 | Mummify Dissolve | ร่างแห้งเหี่ยว (shader dissolve/shrink) | Shader Graph | P0 | SC-05 | scale ลด |
| VFX-005 | Elena Water Magic | สายน้ำจากลำธารพุ่ง → ถูกไอดำกลืน/ระเหย (D-003) | VFX Graph, สี Stream palette | P0 | SC-04 | particle ฟ้า |
| VFX-006 | Black Ichor Letters | น้ำเหลืองดำหยด → ก่อตัวอักษร "Flee... the seal is about to break" (ข้อความอังกฤษรอ Q-009) | Decal/shader reveal | P0 | SC-06 | ข้อความ UI |
| VFX-007 | Critical HP Overlay | vignette เลือด | Post/UI | P0 | SC-04 | Volume vignette |
| VFX-008 | Forest ambience | หมอก ฝุ่น ลำแสง | Particle + volumetric-ish | P2 | SC-03 | Fog URP |

### Audio
| ID | ประเภท | ชื่อ | คำอธิบาย | สเปก | ความสำคัญ | ใช้ใน |
|---|---|---|---|---|---|---|
| AUD-001 | Music | Village theme | หม่น เปียโน/สตริงเบา | OGG/WAV loop 48 kHz | P0 | SC-02 |
| AUD-002 | Music | Forest omen | ลางร้าย ค่อย ๆ หนาขึ้น | loop | P1 | SC-03 |
| AUD-003 | Music | Chase / run | เร่ง | 1–2 นาที | P1 | SC-03 |
| AUD-004 | Music | Ending sorrow | เศร้า จุกอก | 2–3 นาที | P0 | SC-06 |
| AUD-005 | Music | Intro narration bed | เบา ใต้บทบรรยาย | loop | P1 | SC-00 |
| AUD-010 | SFX | Giant chain drag | โซ่ยักษ์ลากครูดพื้น ก้องในหัว | WAV, หลาย take | P0 | SC-05 |
| AUD-011 | SFX | Roots burst / flesh tear | รากโผล่ทะลุ | WAV | P0 | SC-05 |
| AUD-012 | SFX | Wolf set | ขู่, กัด, โหยหวน, แห้งเหี่ยว | WAV | P0 | SC-04–05 |
| AUD-013 | SFX | Human set | ลมหายใจหอบ, หัวใจเต้น, กรีดร้องเอเลนา, ร้องไห้ | WAV | P0 | SC-03–06 |
| AUD-014 | SFX | Footsteps | ดิน/หญ้า/หิน/ไม้ (เดิน/วิ่ง) | WAV ชุดละ 4–6 | P0 | ทุก scene |
| AUD-015 | SFX | Village whispers | กระซิบ/บ่นไม่ชัดคำ (ไม่มีภาษา = ไม่ต้องแปล) | WAV หลายไฟล์ | P0 | SC-02 |
| AUD-016 | SFX | Water magic + steam hiss | เวทน้ำพุ่ง → ระเหย | WAV | P0 | SC-04 |
| AUD-017 | Ambience | Village / forest / stream | ลม นก ลำธาร น้ำตก | loop | P0 | SC-01–06 |

### UI / Font
| ID | ประเภท | ชื่อ | คำอธิบาย | สเปก | ความสำคัญ | สถานะ | License check |
|---|---|---|---|---|---|---|---|
| UI-001 | Font | Title/heading serif (EN) | serif คลาสสิกดาร์ก (เช่น ตระกูล Cormorant) | TTF/OTF, ต้องเป็น OFL หรือเทียบเท่า | P0 | Requested | ทีมตรวจ license ให้ได้ถ้าเจ้าของชี้แหล่ง |
| UI-002 | Font | Body/subtitle sans (EN) | อ่านง่ายขนาดเล็ก (เช่น Source Sans 3) | TTF/OTF, OFL | P0 | Requested | 〃 |
| UI-003 | UI Art | Title logo "Hollow Chains" | โลโก้ + โซ่/ช่องว่าง | PNG โปร่ง 4K + SVG | P1 | Requested | — |
| UI-004 | UI Art | Icons | quest, interact, ปุ่ม KB/M + gamepad | PNG/SVG 256 | P1 | Requested | — |
| UI-005 | Font | Thai font (ภายหลัง — D-017) | — | — | P2 | ไม่ขอตอนนี้ | — |

### Comic (P7 — ยังไม่ขอ)
Panel 2D ชุดหม่น 6–10 ภาพ + ชุดสีนวล (เอเลนา)

## สรุปชุดที่ 1
- **P0 ทั้งหมด 30 รายการ** — ลำดับที่อยากได้ก่อน: AST-001/002/004 + AST-010/011 (Mixamo) → AST-020–023 kit → VFX-001–005 → AUD-010/012/013
- ทีมพร้อมช่วย **หาแหล่งฟรีที่ license ชัด** (เช่น ฟอนต์ OFL, CC0) และส่งลิงก์ให้เจ้าของเลือก — บอกได้ตลอด
