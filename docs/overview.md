# Overview

The HL7Forge is a tool to quickly create **fake but meaningful HL7 v2.x messages**.

## Key Features
- Supports many core HL7 segments (PID, PV1, ORC, OBR, OBX, etc.).
- Uses **profiles** (JSON) to define segment ordering and repetitions.
- Supports **people.json** for deterministic patient data.
- Supports **constants.json** for site-wide defaults.
- Field-level overrides with **map.json** using a flexible token engine.
- CLI and GUI modes, both running cross-platform (.NET 8).
- GUI includes **live preview**, **segment dropdown**, and **two-seed diff**.
