# Contributing

## Adding a trigger
1. Create a new profile JSON in `Profiles/<version>/<TRIGGER>.json`.
2. Define the segment order.

## Adding a segment
1. Add `BuildSEG` to `SegmentFactory`.
2. Add a case in `BuildByName`.

## Adding tokens
1. Extend `TemplateEngine.Eval` with new case.
2. Document in `docs/tokens.md`.

## Adding maps
1. Add `<SEG>.map.json` in maps folder.
2. Or trigger-specific `<TRIGGER>.<SEG>.map.json`.

## Style
- Use `Hl7Composer.Encode` for segment lines.
- Keep defaults safe (no real PI).
