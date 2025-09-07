# Profiles

Profiles control **segment order and repetition** for each trigger.

Located in:
```
src/Generator.Core/Profiles/<version>/*.json
```

Example (`ORU_R01.json`):
```json
{
  "trigger": "ORU^R01",
  "version": "2.5.1",
  "order": [
    "MSH",
    "PID",
    "PV1",
    { "segment": "OBX", "repeatMin": 1, "repeatMax": 5 }
  ]
}
```

- `segment`: HL7 segment name
- `repeat`: fixed number of repeats
- `repeatMin`/`repeatMax`: random range (deterministic by seed)
- `include: false`: skip segment
- `args`: extra metadata for segment builders
