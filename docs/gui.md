# GUI Guide

## Main controls
- **Trigger**: pick message type
- **Version**: profile version
- **Seed**: base seed for determinism
- **Count**: number of messages
- **Output**: destination folder

## Preview
- **Segment dropdown**: choose PID, PV1, OBR, etc.
- **Preview**: generate one message and show the chosen segment.
- **Live**: auto-refresh preview when changing trigger/seed/etc.

## Diff view
- **Seed A / Seed B**: pick two seeds
- **Diff A vs B**: compare chosen segment across two seeds
- Status label shows “Identical” or “Different”.


## Cohort extras
- **Reset seq for each patient**: If checked, `${seq}` restarts at the Seq Start value for each patient.
- **Per-trigger count**: Emit `N` messages per trigger per patient.
- **Filename pattern**: Tokens allowed, including `${patient}`, `${seq}`, `${seed}`, `${profile.*}`, `${const.*}`. Click **Preview Names** to preview a sample list.
- **Open Output Folder**: Opens the current output directory in Explorer.
- **Log panel**: Shows paths of files that were written.


## Progress bar & colored log
- A **progress bar** shows completion for both single-run generation and cohort generation.
- The **log panel** now uses colors: gray for info, black for writes, green for success, red for errors.
