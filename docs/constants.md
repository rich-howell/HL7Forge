# Constants

`constants.json` controls **site-wide defaults**.

Located in:
```
src/Generator.Core/Profiles/<version>/constants.json
```

Example:
```json
{
  "assigningAuthority": "INSTITUTE.TEST",
  "sendingApplication": "DummyGen",
  "sendingFacility": "INSTITUTE.TEST",
  "receivingApplication": "Rhapsody",
  "receivingFacility": "TEST",
  "locations": ["ED","WARD1","ICU"],
  "providers": [
    { "id": "PROV1001", "name": "SMITH^ALEX" }
  ]
}
```

- SafePiPolicy pulls facility/application defaults.
- Faker uses `locations` if present.
- Tokens `${const.*}` available in maps.
