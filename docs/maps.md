# Maps

Maps override **specific fields** in a segment using tokens.

Located in:
```
src/Generator.Core/Profiles/<version>/maps/*.map.json
```

Example (`PID.map.json`):
```json
{
  "segment": "PID",
  "overrides": {
    "3": "${person.identifiers|cx}",
    "5": "${person.names[0].family}^${person.names[0].given}"
  }
}
```

- Field numbers are **1-based after segment ID**.
- Tokens let you pull values from person, constants, profile, or generate dynamically.

Trigger-specific maps override generic ones:
```
ADT_A01.PID.map.json  >  PID.map.json
```
