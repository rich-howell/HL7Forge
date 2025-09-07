# HL7 v2.5.1 Segment Map Cheat Sheet (Selected Fields)

This summarizes common fields you just got maps for, with typical **datatypes** and example values.

## MSH — Message Header
- MSH-1 **ST** field separator `|`
- MSH-2 **ST** encoding chars `^~\&`
- MSH-3 **HD** Sending Application
- MSH-4 **HD** Sending Facility
- MSH-5 **HD** Receiving Application
- MSH-6 **HD** Receiving Facility
- MSH-7 **TS** Message Date/Time `yyyyMMddHHmmss`
- MSH-9 **MSG** Message Type (e.g., `ORU^R01`)
- MSH-10 **ST** Control ID (unique per message)
- MSH-11 **PT** Processing ID (`P`/`T`)
- MSH-12 **VID** Version ID (`2.5.1`)

## EVN — Event Type
- EVN-1 **ID** Event type code (e.g., `A01`)
- EVN-2 **TS** Recorded date/time

## PID — Patient Identification
- PID-3 **CX** Identifier list (`ID^^^AssigningAuthority^Type`)
- PID-5 **XPN** Patient Name (`Family^Given`)
- PID-7 **DT** DOB `yyyyMMdd`
- PID-8 **IS** Sex
- PID-10 **CE** Race
- PID-11 **XAD** Address
- PID-13 **XTN** Phone
- PID-15 **CE** Primary Language
- PID-18 **CX** Patient Account Number

## PD1 — Patient Additional Demographics
- PD1-3 **XON** Patient Primary Care Provider Name & ID No.
- PD1-4 **XON** Patient’s institution

## NK1 — Next of Kin
- NK1-2 **XPN** Name
- NK1-3 **CE** Relationship
- NK1-5 **XTN** Phone

## PV1 — Patient Visit
- PV1-2 **IS** Patient Class
- PV1-3 **PL** Assigned Patient Location
- PV1-7 **XCN** Attending Doctor
- PV1-10 **IS** Hospital Service
- PV1-18 **CX** Patient Type/Payor class (implementation specific)
- PV1-19 **CX** Visit Number
- PV1-44 **TS** Admit Date/Time

## PV2 — Patient Visit - Additional Info
- PV2-3 **IS** Admit reason
- PV2-9 **IS** Expected admit length
- PV2-13 **IS** Readmission indicator

## DG1 — Diagnosis
- DG1-2 **IS** Diagnosis Type (`I`)
- DG1-3 **CE** Diagnosis Code (e.g., `Z999^...^I10`)
- DG1-5 **IS** Diagnosis Priority
- DG1-6 **DT** Diagnosis Date
- DG1-15 **IS** DRG Grouping reason

## AL1 — Allergy
- AL1-2 **IS** Allergen Type Code (`FA`)
- AL1-3 **CE** Allergen Code
- AL1-4 **CE** Severity (`SV`)
- AL1-5 **ST** Reaction
- AL1-6 **DT** Identification Date

## PR1 — Procedures
- PR1-3 **CE** Procedure Code
- PR1-6 **DT** Procedure Date
- PR1-7 **NM** Procedure Minutes
- PR1-11 **IS** Anesthesia Code

## PRD — Practitioner Detail
- PRD-1 **CE** Role
- PRD-2 **XCN** Practitioner
- PRD-7 **XTN** Phone

## ROL — Role
- ROL-2 **CE** Action Code
- ROL-3 **CE** Role
- ROL-4 **XCN** Person

## RF1 — Referral
- RF1-1 **CE** Referral Status
- RF1-2 **CE** Referral Priority
- RF1-3 **EI** Referral Number
- RF1-6 **DT** Referral Date
- RF1-8 **IS** Referral Type

## ORC — Common Order
- ORC-1 **ID** Order Control (`NW` new)
- ORC-2 **EI** Placer Order Number
- ORC-3 **EI** Filler Order Number
- ORC-9 **TS** Date/Time of Transaction
- ORC-12 **XCN** Ordering Provider
- ORC-21 **CE** Action By

## OBR — Observation Request
- OBR-2 **EI** Placer Order Number
- OBR-3 **EI** Filler Order Number
- OBR-4 **CE** Universal Service ID
- OBR-7 **TS** Observation Date/Time
- OBR-13 **ST** Relevant Clinical Information
- OBR-16 **XCN** Ordering Provider
- OBR-18 **XTN/HD** Placer Field 1 / Facility (varies by impl.)
- OBR-24 **ID** Diagnostic Service Section ID

## OBX — Observation/Result
- OBX-2 **ID** Value Type (`TX`, `NM`, etc.)
- OBX-3 **CE** Observation Identifier
- OBX-5 **var** Observation Value (varies by OBX-2)
- OBX-11 **ID** Result Status (`F` final)

## SPM — Specimen
- SPM-2 **EIP/EI** Specimen ID
- SPM-4 **CE** Specimen Type
- SPM-7 **TS** Specimen Collection Date/Time
- SPM-11 **CWE/HD** Specimen Site / Facility
- SPM-17 **CWE** Specimen Quality

## NTE — Notes & Comments
- NTE-3 **FT** Comment

## MSA — Acknowledgment
- MSA-1 **ID** Acknowledgment Code (`AA`,`AE`)
- MSA-2 **ST** Message Control ID (echo from MSH-10)

## ERR — Error
- ERR-2 **ERL** Error Location
- ERR-4 **ELD** Severity/Code

## LOC — Location Identification
- LOC-1 **PL** Primary Key Value - LOC
- LOC-2 **CE** Location Description
- LOC-3 **CE** Location Type
