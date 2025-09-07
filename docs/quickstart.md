# Quick Start

## CLI
```powershell
cd src/Generator.Cli
dotnet build
dotnet run -- ADT^A01 2.5.1 10 1234 .\out
```
This generates 10 ADT^A01 messages using HL7 version 2.5.1 with seed 1234.

Arguments:
- Trigger: e.g., `ADT^A01`, `ORU^R01`
- Version: profile version folder (e.g., `2.5.1`)
- Count: number of messages
- Seed: base seed for deterministic fake data
- Output folder

## GUI
```powershell
cd src/Generator.Gui
dotnet build
dotnet run
```

Features:
- Pick trigger/version/seed visually.
- Generate messages to a folder.
- Live preview a segment (default PID).
- Compare segments for two seeds side by side.
