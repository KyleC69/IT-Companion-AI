# =====================================================================
# Document Title   : Data Flow Overview
# Version          : v0.1.0
# Date Created     : 2025-08-28
# Last Modified    : 2025-08-28
# Author(s)        : Unknown
# Reviewed By      : 
# Status           : Draft
# Purpose          : High-level visual description of end-to-end data movement.
# Related Docs     : AI-DataKeyMap.md; AI-DataSourceMap.txt
# Change Log       :
#   - 2025-08-28 | System | Initial header applied
# =====================================================================
---
Title: Data Flow Overview
File: ai-dataflow.md
Description: Visual high-level data flow from raw inputs through loaders, refinery, and models.
Version: 0.1.0
LastUpdated: 2025-08-28
Status: Draft
Tags: [dataflow, pipeline]
---

[Raw Inputs]

&nbsp;  ├── Drivers dump (Name, Version, StatusCode)

&nbsp;  ├── Services list (Name, StartupType, SecurityDescriptor)

&nbsp;  ├── Event Logs (various formats: EVTX, text exports)

&nbsp;  ├── Registry keys

&nbsp;  ├── WMI queries

&nbsp;  └── Custom log/text files

&nbsp;       ↓

[Source-specific Loaders]

&nbsp;  - Parse each source type

&nbsp;  - Detect schema (header names, value types)

&nbsp;  - Handle quirks (encodings, delimiters, corrupt rows)

&nbsp;       ↓

[Transformation Layer — "The Refinery"]

&nbsp;  ├── **Canonical Naming**  

&nbsp;  │      e.g., "drv_status", "STATE", "0x4" → `Status` (enum)

&nbsp;  ├── **Type Coercion**  

&nbsp;  │      Timestamps → ISO8601; Status codes → enums; Yes/No → bools

&nbsp;  ├── **Derived Features**  

&nbsp;  │      StartupType=2 → "Auto (Delayed Start)"

&nbsp;  ├── **Context Tagging**  

&nbsp;  │      Add `SourceType` = driver / service / log / registry / wmi / custom

&nbsp;  ├── **Temporal Alignment**  

&nbsp;  │      Normalize to UTC, fill gaps, prep rolling windows

&nbsp;  └── **Audit Trail Hooks**  

&nbsp;         Keep raw + transformed side-by-side for review

&nbsp;       ↓

[Clean, Unified Feature Set]

&nbsp;  ├── Columns: Name | Version | Status | SourceType | Timestamp | ...

&nbsp;  └── Predictable formats & semantics

&nbsp;       ↓

[Models]

&nbsp;  ├── Conversational Classifier  

&nbsp;  │      - Handles NL queries about system state/security

&nbsp;  └── Telemetry Anomaly Detector  

&nbsp;         - Monitors unified stream for out-of-pattern behavior

