# Template Engine & Maps

## TemplateEngine
- Expands `${tokens}` inside strings.
- Supports person, constants, profile, random, date/time, string ops.

Example:
```
"${upper:person.names[0].family}^${person.names[0].given}"
```

## SegmentPatcher
- After a segment is built, SegmentPatcher applies map overrides.
- Maps live in `Profiles/<version>/maps/*.map.json`.

## Map precedence
- Trigger-specific maps: `ADT_A01.PID.map.json`
- Generic maps: `PID.map.json`
- Trigger-specific overrides win.

