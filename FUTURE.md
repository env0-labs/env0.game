# FUTURE

This document describes the intended future shape of the project, its operating model,
and its design constraints.

Nothing in this file is guaranteed to be implemented.
If something described here becomes true in the repository, it should be reflected in
README.md.

This project is no longer described in terms of “Acts”.
It is structured as a single system experienced through different operational modes.

Each mode exposes a different surface of the same underlying work.

---

## High-Level Direction

The system is always doing work.

What changes over time is:
- how much of that work the player performs directly
- how much of it they delegate
- how closely they choose to observe the consequences

Movement between modes does not represent progress, success, or enlightenment.
It represents **distance**.

---

## Roadmap (Non-Binding)

- Finalise Maintenance Mode as a stable, background system.
- Build Records Mode as a navigable interpretive layer.
- Implement a CLI-based Automation Mode that can alter how maintenance is performed.
- Add a final Interpretation Mode once the system’s behaviour is stable.

---

## Maintenance Mode (In Progress)

### Purpose

Maintenance Mode establishes procedural compliance through repetition.

The player performs work correctly without understanding its meaning or scope.
Over time, the work becomes routine and attention shifts away from content and toward
completion.

This mode is designed to become ignorable, not satisfying.

### Player Belief

“I’m doing necessary work. Nothing is wrong.”

### Interface Constraints

- Terminal-only interface.
- Only two accepted commands:
  - `process`
  - `status`
- Any other input returns a flat error that reasserts the allowed commands.

### Work Model

- Work is represented as parent and child process containers.
- The player processes child items within the current parent container.
- Parent containers are finite.
- When a parent container completes, a new one is assigned automatically.
- The size of a new parent container is unknown until processing begins.
- Scope is revealed only through action, not planning.

Backlog must feel normal.
It must not feel like punishment or escalation.

### Status Presentation

- `status` shows only the current container.
- Output uses minimal ASCII structure.
- Processed and pending items are distinguished.
- No global totals.
- No completion percentages.
- No finish line.

### Language Constraints

- Output is procedural, flat, and indifferent.
- No moral framing.
- No ethical warnings.
- No narrative commentary.
- Only procedural confirmations are allowed.

Terms such as `kill-child` and `kill-parent` may appear as legitimate process-control
language:
- present early
- repeated often
- operationally dull
- never explained
- never escalated or dramatized

### Experience Constraints

- Maintenance Mode must be dull.
- Repetition is the mechanism.
- Do not add variety to relieve boredom.
- Output should become skimmable.
- The player should optimise attention away from reading.

There are no puzzles.
There is no exploration.
There is no help system.

### Transition Condition

After sufficient repetition, the system allows access to Records Mode.

This is not framed as success or completion.
It is framed as availability.

Maintenance Mode does not end.
It becomes background labour.

---

## Records Mode (Planned / Under Consolidation)

### Purpose

Records Mode provides access to institutional artifacts:
logs, policies, memos, terminals, and records.

It allows investigation without resolution.

Maintenance remains mandatory.
Interpretation remains optional.

### Structure

- The player can move between rooms or locations.
- Each location contains one or more terminals or records.
- Different terminals may perform similar work, framed differently.
- Records are locally coherent but globally incomplete.

No single record explains the system.
Contradictions are allowed.
Gaps are allowed.

### Constraints

- Records must not deliver answers.
- Records must not form a complete narrative.
- Records must not unlock truth.

The player may form theories.
Those theories should be plausible and incomplete.

If the player feels clever, too much has been revealed.

---

## Automation Mode (CLI / Configuration Mode)

### Purpose

Automation Mode allows the player to alter how work is performed.

It introduces automation, patching, and delegation.

This mode reduces direct labour and increases distance between action and outcome.

### Interface

- Linux-like CLI.
- Read access to system configuration.
- Indirect write access via patch scripts or automation definitions.

No direct editing.
All changes are applied procedurally.

### Constraints

- Automation is framed as relief, not mastery.
- No celebratory feedback.
- No efficiency praise.
- No moral framing.

Patch scripts provide only procedural confirmation, for example:
- “This will change X. Are you sure? (y/n)”
- later: `-y`

### Critical Rule

Automation does not eliminate maintenance.
It moves it out of sight.

The player must always be able to return to a Maintenance terminal and run:

'''Status''' 

and receive real, current, unsanitised output.

Maintenance remains the ground truth.

---

## Interpretation Mode (Planned)

### Purpose

Interpretation Mode provides fluent, persuasive explanations.

These explanations are plausible but not authoritative.

### Asymmetry

- Maintenance and Automation provide truth without meaning.
- Interpretation provides meaning without guaranteed truth.

### Constraints

- Interpretation must tempt belief.
- It must not demand trust.
- It must not resolve ambiguity.

Ignorance must become a choice.

---

## Cross-Mode Invariants

- The system never lies.
- The system never cares.
- No mode invalidates another.
- Maintenance is always the ground truth.
- Optimisation creates distance.
- Understanding arrives after action.
- Horror is recognition, not malfunction.

---

## Non-Goals

This project explicitly avoids:

- Power or rebellion fantasies.
- Cyberpunk spectacle.
- Apocalypse framing.
- Punishment for curiosity.
- Lore dumps that explain intent instead of embodying it.