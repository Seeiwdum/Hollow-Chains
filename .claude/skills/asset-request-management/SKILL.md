---
name: asset-request-management
description: Build and maintain the Hollow Chains Asset Request List and Asset Tracker — models, animations, VFX, audio, UI, fonts and comic panels the owner must supply, plus placeholders and license checks. Use this at every phase end, whenever a scene or feature needs art/audio, when the owner delivers files, or when evaluating free/Mixamo assets.
---

# Asset Request Management (Hollow Chains, skill #47)

The owner sources assets personally, so requests must be precise enough to find or commission without follow-up
questions, and the game must keep running on placeholders while waiting.

## Request table (format from 08)
| ID | ประเภท | ชื่อ | คำอธิบาย / อารมณ์ | Reference | สเปกเทคนิค | ความสำคัญ | ใช้ใน Scene/Phase | Placeholder |

- ID prefixes: `AST-` 3D/2D/texture/animation, `VFX-`, `AUD-`, `UI-`. Never reuse an ID.
- Priority: P0 = needed before the demo ships, P1 = should have, P2 = nice to have.
- Spec: format (FBX/GLB/WAV/OGG/TTF), poly budget, rig (Humanoid for characters), texture size, loop/length for
  audio, and **required license** (CC0 / CC-BY / purchased / self-made).
- Reference: describe mood in words or keywords; don't paste copyrighted images. Other games are inspiration only.

## Process
1. Gather needs per scene from the GDD (`SC-xx`) — Art (อ.ชัย), VFX (ไฟ/พลอย), Audio (ภีม/แพรว).
2. Dedupe and prioritise (ปุณ). Only request what the current or next phase uses.
3. Send to the owner in the Phase Report.
4. On delivery: Tech Artist checks import settings, scale (1 unit = 1 m), materials (URP), LOD → integrate →
   close the row.

## License check (D-004 — mandatory)
Before any third-party asset (including Mixamo) enters `Assets/`, record in the tracker: source URL, license name,
whether commercial use and redistribution in a game build are allowed, attribution text if needed, date checked,
checker. Do not state license terms from memory; read the source's current license page. If unclear, keep the
placeholder and ask the owner.

## Tracker
Keep `Docs/ASSET_TRACKER.md` (or `.xlsx` if the owner asks): request table + columns
`Status (Requested/Received/Integrated/Rejected) | License check | Path in project`.
