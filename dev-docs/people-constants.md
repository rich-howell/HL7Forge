# People & Constants

## PeopleStore
- Loads `people.json` if present.
- Deterministically selects a person by seed.
- Supports multiple names, addresses, race, ethnicity, language.

## ConstantsStore
- Loads `constants.json` if present.
- Provides site-wide defaults (assigning authority, facility, providers, locations).
- SafePiPolicy and DataFaker both consume constants.

