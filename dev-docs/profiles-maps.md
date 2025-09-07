# Profiles & Maps JSON

## Profiles
Define segment ordering for each trigger.

Example (`ORU_R01.json`):
```json
{
  "trigger": "ORU^R01",
  "version": "2.5.1",
  "order": [
    "MSH",
    "PID",
    { "segment": "OBX", "repeatMin": 1, "repeatMax": 5 }
  ]
}
```

Fields:
- `segment`: HL7 segment name
- `repeat`: fixed count
- `repeatMin`/`repeatMax`: random range (deterministic)
- `include: false`: skip
- `args`: optional metadata

## Maps
Override fields post-build.

Example:
```json
{
  "segment": "PID",
  "overrides": {
    "3": "${person.identifiers|cx}",
    "5": "${person.names[0].family}^${person.names[0].given}"
  }
}
```

