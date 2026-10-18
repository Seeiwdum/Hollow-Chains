---
name: game-design-document
description: Write or update Hollow Chains design documents — the demo GDD, one-pagers and feature specs under Docs/. Use this whenever the task is to freeze, revise or extend game design (scenes, quests, characters, systems, acceptance criteria) for Hollow Chains, or when a design decision from the owner (NAMOE) must be reflected in a spec, even if the user only says "update the design" or "write the spec for X".
---

# Game Design Document (Hollow Chains, skill #43)

Design docs are the contract between the owner (NAMOE, Creative Director) and the studio. Their main job is to make
it obvious what is **CANON** (owner-confirmed, never change on your own) and what is a **PROPOSAL** (team idea,
needs approval). A spec that blurs the two causes the team to build things the owner never agreed to.

## Sources of truth (read before writing)
1. `Docs/DECISION_LOG.md` — OWNER decisions override the source docs.
2. `Docs/STUDIO_LOG.md` — current phase and scope.
3. Source design docs `00`–`09` (outside the repo, at `D:\DL\`; ask for them if missing).

If a decision log entry and a source doc disagree, follow the decision log and note the conflict in the spec.

## Writing rules
- Language: Thai prose, English for code names, IDs and Unity terms.
- Tag every design statement: `[CANON]`, `[OWNER D-xxx]`, `[PROPOSAL]`, `[IMPL]` (team implementation detail).
- Never invent lore the owner has reserved (father's truth, Elena's later arc). Write `❓ ต้องถามเจ้าของ` instead.
- Keep scope tight: the demo only needs what `06` acceptance criteria require. Anything else goes in an
  "Out of scope / หลังเดโม" section rather than being silently dropped.
- Every feature gets: purpose (what the player should feel), player-facing behaviour, data it needs, acceptance
  check (testable sentence), and owning role.
- Use stable IDs so code, backlog and asset requests can reference them: `SC-01` scenes, `Q001-P1` quest phases,
  `FT-xx` features, `AC-xx` acceptance criteria.

## Demo GDD structure
```
# GDD — Demo Prologue (vX.Y, freeze status)
0. Change log (version, date, what changed, decision IDs)
1. Experience goal (1 paragraph) + pillars that apply to the demo
2. Scope: in / out
3. Scene breakdown SC-00..SC-06 — beat, player verbs, camera, audio mood, systems used, assets needed
4. Quest Q001 phase table
5. Systems needed by the demo (FT-xx) with acceptance checks
6. HUD / UI for the demo
7. Text & localisation notes (Thai first)
8. Acceptance criteria AC-xx (from 06 + additions)
9. Open questions (L1/L2/L3 per 00 §5) with team default
```

## Freezing
"Freeze" means changes after this point need a new DECISION_LOG entry. Mark the header
`สถานะ: FREEZE CANDIDATE` until the owner approves, then `FROZEN (D-xxx)`.
