# Tokens

Tokens can be used in maps (and eventually elsewhere).

## Built-in
- `${now:yyyyMMddHHmmss}` — current UTC time
- `${rand:100-999}` — deterministic random
- `${seed}` — message seed

## Person
- `${person.names[0].family}`
- `${person.addresses[0].city}`
- `${person.identifiers|cx}`

## Constants
- `${const.assigningAuthority}`
- `${const.providers[0].name}`

## Profile
- `${profile.trigger}`
- `${profile.version}`

## String transforms
- `upper:...`
- `lower:...`
- `padleft:n:<expr>`
- `padright:n:<expr>`
- `substr:start:len:<expr>`
