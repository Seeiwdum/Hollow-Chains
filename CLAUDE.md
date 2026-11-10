# Hollow Chains -- Game Studio Agent Architecture

Indie game development managed through 49 coordinated Claude Code subagents.
Each agent owns a specific domain, enforcing separation of concerns and quality.

## Technology Stack

- **Engine**: Unity 6000.3.10f1 (URP 17.3, Input System, Cinemachine 3, Timeline, Localization th/en, TMP)
- **Language**: C# (assemblies `HollowChains.*`, code root `Assets/_Project/Scripts`)
- **Version Control**: Git on `phase/*` branches (see "Hollow Chains Studio Rules"), not trunk-based
- **Build System**: Unity build pipeline, Windows IL2CPP (`.claude/skills/unity-build-pipeline`)
- **Asset Pipeline**: Unity importers; greybox with ProBuilder; every imported asset needs a verified license (D-004)

> **Note**: Engine-specialist agents exist for Godot, Unity, and Unreal with
> dedicated sub-specialists. Use the set matching your engine.

## Project Structure

@.claude/docs/directory-structure.md

## Engine Version Reference

<!-- ENGINE-REFERENCE-IMPORT: the line under this comment is engine-specific,
     and there is none until an engine is set. /setup-engine, the grow's record
     step and /project-settings (engine.name) write
     @docs/engine-reference/<engine>/VERSION.md here for the chosen engine. Skills
     that need the pinned version read that file on demand either way. -->
@docs/engine-reference/unity/VERSION.md


## Technical Preferences

`project.yaml` at the repo root is the primary config store — engine, specialists,
naming, platform, performance, modes. Skills resolve it via `resolve_config`
(see `.claude/docs/config-resolution.md`).

`.claude/docs/technical-preferences.md` is the **legacy fallback**, read on demand
when a key is absent from `project.yaml`. It is not imported here: before
`/setup-engine` runs it is almost entirely `[TO BE CONFIGURED]` placeholders, and
after it runs `project.yaml` holds the real values.

## Coordination Rules

@.claude/docs/coordination-rules.md

## Collaboration Protocol

**User-driven collaboration, not autonomous execution.**
Every task follows: **Question -> Options -> Decision -> Draft -> Approval**

- Agents MUST ask "May I write this to [filepath]?" before using Write/Edit tools
  (exempt: `active.md` appends, agent memory, a subagent's new file at the path
  its orchestrating skill named; see `.claude/rules/agent-memory.md`)
- Agents MUST show drafts or summaries before requesting approval
- Multi-file changes require explicit approval for the full changeset
- No commits without user instruction
- A skipped or dismissed question is not an answer: act on nothing, re-ask in
  plain text and wait
- Started outside this file's folder? Warn first: hooks, deny rules and the
  ask-first mode load only when Claude Code starts here

See `Docs/COLLABORATIVE-DESIGN-PRINCIPLE.md` for full protocol and examples.

> This project runs in `guided` automation (`project.yaml`): the "May I write" question
> is asked for major decisions only; a sub-phase the owner approved is worked to its end.

## Coding Standards

@.claude/docs/coding-standards.md

## Context Management

Read `.claude/docs/context-management.md` on demand — it is a reference, not
session context. Two of its conventions are load-bearing and cited by name
elsewhere in the repo, so they are restated here rather than lost:

- **`production/session-state/active.md` is the session checkpoint.** The file is
  the memory, not the conversation. Read it first after any compaction, crash, or
  `/clear`.
- **Helpers in `.claude/scripts/` emit observations, never verdicts.** A script
  that scores or judges will eventually contradict a mode or override it cannot
  see. (Cited by `artifact-check.sh` and `adr-dep-graph.sh`.)

## Hollow Chains Studio Rules

These rules are this project's own and win over the template text above where
they differ.

### The owner talks to the Manager only

- The owner is NAMOE, the Creative Director, and holds every final decision. The
  `creative-director` agent advises; it never decides for him.
- The main session is the **Manager** (the `producer` role). It is the only one
  that talks to the owner, always **in Thai**. It plans, delegates each piece of
  work to the agent that owns it, checks what comes back, and reports.
- The Manager does not do a specialist's work itself when an agent owns it. It
  briefs the agent with the files, the goal and what done looks like.
- Meetings stay short (D-006). The owner tests in Unity himself (D-024), so every
  report ends with test points. Bugs found on the way are reported, not hidden.

### One model per role

The `model:` line in each `.claude/agents/*.md` is what runs. The table and the
reasons are in `.claude/docs/model-tiers.md` ("Hollow Chains assignment").

| Tier | Model | Who |
|---|---|---|
| Manager | the session's model | main session only |
| Direction | `opus` | creative-director, technical-director, producer, lead-programmer, qa-lead, narrative-director |
| Build and design | `sonnet` | programmers, Unity specialists, designers, art, audio, UX, release, performance |
| Checks and lists | `haiku` | qa-tester, analytics-engineer, community-manager |

Pass no `model` when spawning an agent, so the agent's own pin applies.

### When the context is nearly full

At about 70 % context (`features.token_budget_warn_at`) the Manager stops taking
new work and hands over:

1. Update `production/session-state/active.md` (checkpoint block: branch, current
   task, next step, blocked on) and the phase table in `Docs/STUDIO_LOG.md`.
2. Commit and push finished work on the phase branch.
3. Start a fresh Manager session with a brief that names those two files, and
   tell the owner in one line to continue there. The owner keeps talking to a
   Manager; he never has to brief a specialist.

### Git workflow

- Work on `phase/*` branches. Never force push. Never push to `main` without the
  owner's approval. Merge with a merge commit, never squash.
- Commit and push at the end of each sub-phase, 3-6 atomic commits. Tag at phase
  end.
- Never commit `Library/`, `Temp/`, builds or secrets.

### Code and content

- OOP and SOLID, composition first; keep `Docs/P1/CLASS_DIAGRAM.md` in sync in the
  same commit as any new public type (D-020).
- The UI assembly never references Gameplay; systems talk through ScriptableObject
  event channels.
- The demo is in Thai (D-025). No Thai text hard-coded in C#.
- CANON lore is the owner's. Lines the team writes are marked `[PROPOSAL]`.
- Greybox only: ProBuilder, capsules for characters, no animation yet (D-023).
- The 26 `unity-*` and production skills in `.claude/skills/` that predate the
  template stay in force for Unity work.

### Where things are

| What | Where |
|---|---|
| Phase plan and reports | `Docs/STUDIO_LOG.md` |
| Decisions and open questions | `Docs/DECISION_LOG.md` |
| Backlog | `Docs/P1/BACKLOG.md` |
| Bugs | `Docs/P6/BUGS.md` |
| Class diagram | `Docs/P1/CLASS_DIAGRAM.md` |
| Session checkpoint | `production/session-state/active.md` |
