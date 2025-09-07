# Architecture

The generator has three main layers:

```
CLI / GUI
   ↓
Generator.Core
   ↓
SegmentFactory + supporting stores (People, Constants)
   ↓
Segment builders → SegmentPatcher → HL7 message string
```

## Core (`Generator.Core`)
- **SegmentFactory**: builds messages using profiles + data sources.
- **TemplateEngine**: expands tokens inside map overrides.
- **SegmentPatcher**: applies map overrides after a segment is built.
- **PeopleStore**: loads deterministic patient data.
- **ConstantsStore**: loads site-wide defaults.
- **DataFaker**: generates synthetic but safe demographics, identifiers, etc.

## CLI (`Generator.Cli`)
- Console entry point.
- Accepts trigger, version, count, seed, output folder.
- Streams messages via SegmentFactory.

## GUI (`Generator.Gui`)
- WinForms app.
- Lets you choose trigger/version/seed visually.
- Includes live preview + diff of segments.

