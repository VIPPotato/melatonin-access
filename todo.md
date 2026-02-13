# Accessibility Todo List

This is the trimmed active backlog for release work.

## v1.1 Remaining
- [ ] **Calibration Feedback Accuracy Validation**
  - Validate early/late millisecond callouts against intentional early/late taps.
  - Confirm the spoken direction and values feel reliable in live play.

- [ ] **Map POI Announcements**
  - Announce non-level map points of interest (for example shop/pool) when focused, if present in scene flow.

- [ ] **Contextual Cues for Remaining Practice Modes**
  - Add scene-specific low-noise guidance for practice modes not covered yet.
  - Prefer one-shot section prompts over per-note spam.
  - Validate each addition through playtest logs.

- [ ] **Tutorial State-Change Audit**
  - Verify tutorial state transitions that change instructions are always spoken.
  - Ensure “wait for input” moments are distinguishable from demonstration moments.

- [ ] **Per-Level Completion Briefing (Optional)**
  - On level/mode entry, speak one concise objective/mechanic line.
  - Keep it single-utterance to avoid overlap with menu narration.

## Clarified / Not Needed
- [x] **Practice Mode Start Announcements**
  - Practice and score-mode start prompts are implemented.

- [x] **Dream_food Jump/Shoot Split**
  - Marked not applicable for current cue layer: decompiled `Dream_food` tutorial queues expose generic `QueueHitWindow(...)` timing windows, not separate jump/shoot input queue types.

- [x] **Transient Label Coverage**
  - Title screen, language menu prompt flow, and side-label tutorial prompts are already handled.

## Deferred to v1.2
- [ ] **Live Audio Description for Intro/Outro Scenes**
  - Build and author cutscene narration content on top of the existing timing pipeline.

- [ ] **Per-Level Audio Descriptions During Gameplay**
  - Optional descriptive callouts for key visual events, with feature toggle.
