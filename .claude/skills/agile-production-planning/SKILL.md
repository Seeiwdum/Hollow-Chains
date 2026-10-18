---
name: agile-production-planning
description: Plan and track Hollow Chains production — phases, sub-phases, backlog, risk register, Definition of Done, phase reports and STUDIO_LOG/DECISION_LOG upkeep. Use this whenever a phase starts or ends, when the owner approves or changes plans, when writing a Phase Report, backlog or risk list, or when deciding what to cut to protect scope.
---

# Agile Production Planning (Hollow Chains, skill #49)

The owner wants a studio that ships a small, dense demo — not a pile of half-built systems. Planning exists to
protect scope and to make every hand-off copy-pasteable into a new chat without losing context.

## Phase rhythm (from 00 §6 and 09)
1. **Phase start** — short meeting (5–12 lines, only relevant staff, D-006): who does what, Definition of Done.
2. **Work** — deliver real files, not talk. Each sub-phase ends with commit + push on `phase/Px-name`.
3. **Phase end** — Phase Report, tag `pX-done`, propose merge to `main` (owner approves; merge commit or rebase,
   never squash).
4. Never start the next phase without the owner's go-ahead.

## Phase Report template
```
## Px — <name> · Phase Report
✅ ทำเสร็จ (+ ไฟล์)
⚠️ ปัญหา/ความเสี่ยงค้าง
❓ คำถาม L2/L3 (≤ 8 ข้อ, มี default)
📦 Asset Request List (ตาม 08 — ใช้ skill asset-request-management)
🔀 Git: branch / commits / tag / push
▶️ Phase ถัดไป + ขออนุมัติ
```

## Logs
- `Docs/DECISION_LOG.md`: append-only. New entry `D-0xx` with date, phase, decision, level (OWNER / L4 / L4 มอบอำนาจ),
  who decided, docs affected. To change a decision, add a new entry that supersedes the old one.
- `Docs/STUDIO_LOG.md`: current status table, per-phase section with sub-phase table, open questions, Git, next steps.
  Update at every sub-phase end so it is always a valid hand-off.

## Question levels (00 §5)
L1 blocker → ask now with options + recommendation. L2 → batch at phase end. L3 → 1–3 taste questions per phase.
L4 → decide, log, don't ask. Never ask non-game questions.

## Backlog & risk
- Backlog rows: `ID | Phase | Feature (FT-xx) | Owner role | Estimate (S/M/L) | DoD | Status`.
  Deliver as `.md` table; use the xlsx skill when the owner wants a spreadsheet.
- Risk rows: `R# | Risk | Likelihood | Impact | Owner | Mitigation | Trigger`.
- Scope check for every new feature: "ผู้เล่นจะรู้สึกอะไร?" and "เดโมต้องใช้ไหม?" — if not needed by `06`
  acceptance criteria, move it to a later phase.
