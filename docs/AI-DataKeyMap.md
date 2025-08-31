# =====================================================================
# Document Title   : 12-Column Schema Key
# Version          : v0.1.0
# Date Created     : 2025-08-28
# Last Modified    : 2025-08-28
# Author(s)        : Unknown
# Reviewed By      : 
# Status           : Draft
# Purpose          : Defines lineage, validation, and use of unified schema columns.
# Related Docs     : AI-DataSourceMap.txt; AI-Dev-Quick-Ref.md
# Change Log       :
#   - 2025-08-28 | System | Initial header applied
# =====================================================================
---
Title: 12-Column Schema Key
File: AI-DataKeyMap.md
Description: Source, validation, transforms, and consumers for unified 12-column schema.
Version: 0.1.0
LastUpdated: 2025-08-28
Status: Draft
Tags: [schema, lineage, reference]
---

## **12‑Column Schema – Column Key**

| **Col #** | **Field**       | **Origin**                  | **Validation Source**                 | **Transform(s)**                                     | **Consumed By**                                |
|-----------|-----------------|-----------------------------|----------------------------------------|------------------------------------------------------|------------------------------------------------|
| 1         | `Timestamp`     | Emitter / Collector         | ISO‑8601 check                         | Normalize to UTC, clamp drift ±2s                    | All time‑based windows & ordering              |
| 2         | `HostID`        | Ingest Agent                | `/config/host_registry.json`           | Normalize to FQDN or 16‑bit hash                     | Partitioning, index key, UnifiedKey            |
| 3         | `SourceID`      | Source Tagger               | `/config/source_registry.json`         | None                                                 | Namespace & UnifiedKey                         |
| 4         | `Severity`      | Scoring Engine              | `/config/severity_scale.json`          | Clamp 0–10                                           | Alert priority, SLA timers                     |
| 5         | `Category`      | Pre‑Classifier              | `/config/taxonomy/category_map.json`   | Lowercase normalize                                  | Fusion Engine weight mapping                   |
| 6         | `EventName`     | Module Emitter              | `/config/eventid_map.json`             | Map from RawEventID                                 | Dashboards, human‑readable logs                |
| 7         | `RawEventID`    | Module Emitter              | `/config/eventid_collision_map.json`   | Offset/pack with SourceID for UnifiedKey             | Audit Logger, reverse lookups                   |
| 8         | `ContextType`   | Emitter Header              | Enum in header schema                  | None                                                 | FAST/SLOW lane routing                         |
| 9         | `MetaFlags`     | Module Decorators           | `/config/meta_flags.json`              | Bitwise OR merge from multiple decorators            | Suppression/force rules                         |
| 10        | `PayloadHash`   | Normalizer / Hasher         | SHA‑256/MD5 format check               | None                                                 | Deduplication, tamper detection                |
| 11        | `UnifiedKey`    | Normalizer                  | —                                      | `(HostID << 48) | (SourceID << 32) | RawEventID`     | Collision‑proof primary key                    |
| 12        | `FreeText`      | Module Emitter              | Size limit check (≤8 KB/row)           | None                                                 | Indexed search, human investigation context    |

---

**Best Practice:** Keep this key and the diagram in the same repo folder so they always version together. If a column changes, the lineage visual and this table get updated in the same PR — no drift, no stale onboarding.  

When you’re back from stitching that last bit of the quilt, we can also layer in **flow‑specific examples** (real column sample values flowing through validation → transform → consumer) for one FAST event and one SLOW event, so new eyes see it in action, not just in theory. That would make this the Cadillac of quick‑reference packs.
