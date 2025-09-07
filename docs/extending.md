# Extending

## Add new segment builder
1. Add a method to `SegmentFactory`.
2. Use `Hl7Composer.Encode(...)` to build the line.
3. Wire it into the `BuildByName` switch.

## Add a new profile
1. Copy an existing profile JSON.
2. Adjust the `order` array.
3. Save under `Profiles/<version>/<TRIGGER>.json`.

## Add new maps
1. Create `<SEG>.map.json` or `<TRIGGER>.<SEG>.map.json`.
2. Define field overrides with tokens.
