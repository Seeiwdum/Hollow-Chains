---
name: playtest-feedback
description: Playtests for the Hollow Chains demo — playtest plan, who/how many (≥ 3 outside the team, AC-03/AC-08), what to observe, a short Thai questionnaire, light local telemetry (timings, beat retries), and turning feedback into backlog items. Use when planning or running a playtest, writing a survey, or processing feedback from the owner or testers.
---

# Playtest & Feedback (Hollow Chains, skill #48)

- **Plan (Docs/P6/PLAYTEST_PLAN.md):** goal, build version, ≥ 3 testers who never saw the game, ~30 min each,
  observer notes without helping. The owner runs the sessions (D-024); the team prepares the build and forms.
- **Observe:** where they get lost (S02 route, finding the stream), whether they read the hints, reaction at the
  Awakening and Elena's rejection, time per scene.
- **Questionnaire (Thai, 1–5 scale + open):** เข้าใจเรื่องไหม / รู้สึกอะไรตอนเอเลน่าปฏิเสธ / อยากเล่นต่อไหม /
  ตรงไหนงง / ตัวหนังสือไทยอ่านง่ายไหม.
- **Telemetry (optional, local only):** JSON in persistentDataPath with scene durations and beat completion times —
  no personal data, nothing sent online.
- **After:** every finding → BUGS.md (bug) or BACKLOG (change) with the tester's words quoted; decisions go to
  DECISION_LOG.
