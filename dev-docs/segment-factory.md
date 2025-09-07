# Segment Factory

`SegmentFactory` is the heart of message building.

## Flow
1. Loads a profile (`Profiles/<version>/<TRIGGER>.json`).
2. Iterates over the `order` array.
3. Calls `BuildByName(segment, trigger, version, patientSeed, visitSeed, args)`.
4. Segment builder returns a line like `PID|...`.
5. SegmentPatcher applies map overrides.

## Adding a new segment
1. Add a `BuildXXX(int seed, ...)` method.
2. Use `Hl7Composer.Encode("SEG", fields...)` to build it.
3. Add a case to `BuildByName` dispatch.
4. Update profiles to include `"SEG"` where you want.

## Seeds
- `patientSeed` and `visitSeed` ensure deterministic data per run.
- Incremented by index so multiple repetitions differ but stay repeatable.
