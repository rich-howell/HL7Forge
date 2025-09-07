# GUI Architecture

The GUI is built with **WinForms**.

## Main controls
- Trigger/version/seed inputs
- Output folder
- Generate button

## Preview
- Segment dropdown
- Live toggle → auto-refresh preview
- Preview button → show one segment for current seed

## Diff
- Seed A vs Seed B inputs
- Diff button → compare segment side by side
- Status label shows "Identical"/"Different"

## Implementation
- `MainForm.cs` wires up events.
- Calls into SegmentFactory the same as CLI.
