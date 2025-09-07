# People

Patient demographics come from:
```
src/Generator.Core/Profiles/<version>/people.json
```

Each person entry:
```json
{
  "mrn": "MRN100001",
  "identifiers": [{ "id": "MRN100001", "assigningAuthority": "DUMMY.FAC", "typeCode": "MR" }],
  "names": [{ "family": "RIVET", "given": "ROWAN", "use": "L" }],
  "addresses": [{ "line1": "12 Example Road", "city": "Testford", "use": "H" }],
  "telecoms": [{ "value": "07000 000001", "use": "H" }],
  "dob": "19900314",
  "sex": "M",
  "race": [{ "code": "W", "text": "White", "system": "LCL" }],
  "ethnicity": [{ "code": "N", "text": "Not Hispanic", "system": "LCL" }],
  "language": "en"
}
```

- Multiple names → PID-5 repetitions
- Multiple addresses → PID-11 repetitions
- Race → PID-10
- Ethnicity → PID-22
- Language → PID-15
