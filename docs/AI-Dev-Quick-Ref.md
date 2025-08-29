# =====================================================================
# Document Title   : 12-Column Event Schema – Dev Quick Reference
# Version          : v0.1.0
# Date Created     : 2025-08-28
# Last Modified    : 2025-08-28
# Author(s)        : Unknown
# Reviewed By      : 
# Status           : Draft
# Purpose          : Quick reference for full unified 12-column event schema.
# Related Docs     : AI-DataKeyMap.md; AI-TrainingDataPresets.md
# Change Log       :
#   - 2025-08-28 | System | Initial header applied
# =====================================================================
Right — the quick‑ref I just gave you was a **pared‑down “core set”** for at‑a‑glance work. The full schema you’ve been shaping is 12 columns wide, so here’s an **expanded enterprise quick‑reference** that restores all of them, including the support/meta fields that didn’t make it into the 8‑col snapshot.

---

## **12‑Column Event Schema – Dev Quick Reference**

| **Col #** | **Field**        | **Origin**                  | **Validation / Lookup**              | **Purpose / Notes**                                                                 |
|-----------|------------------|-----------------------------|---------------------------------------|-------------------------------------------------------------------------------------|
| 1         | `Timestamp`      | Emitter / Collector         | ISO‑8601 check                        | Event time (UTC or normalized), used for ordering and windowing                    |
| 2         | `HostID`         | Ingest Agent                | `/config/host_registry.json`          | Uniquely identify emitting machine/appliance                                       |
| 3         | `SourceID`       | Source Tagger               | `/config/source_registry.json`        | Namespace per facility/module/vendor                                               |
| 4         | `Severity`       | Scoring Engine              | `/config/severity_scale.json`         | Priority scale 0–10 for alerts and SLAs                                            |
| 5         | `Category`       | Pre‑Classifier              | `/config/taxonomy/category_map.json`  | Logical grouping for routing & scoring weights                                     |
| 6         | `EventName`      | Module Emitter              | `/config/eventid_map.json`            | Human‑readable short name, from normalized ID                                      |
| 7         | `RawEventID`     | Module Emitter              | `/config/eventid_collision_map.json`  | Native code from source, preserved for audit parity                                |
| 8         | `ContextType`    | Emitter Header              | Enum in schema                        | FAST vs SLOW processing lanes                                                      |
| 9         | `MetaFlags`      | Module Decorators           | `/config/meta_flags.json`             | Bitmask for replay/test/suppress/force/etc.                                        |
| 10        | `PayloadHash`    | Normalizer / Hasher         | SHA‑256/MD5 format check               | Content fingerprint for dedupe and tamper detection                                |
| 11        | `UnifiedKey`     | Normalizer                  | —                                     | Bit‑packed composite: HostID + SourceID + RawEventID                               |
| 12        | `FreeText`       | Module Emitter              | None / size limit check                | Unstructured payload details, query‑indexed but not part of key                    |

---

### **Packing Recap**
- **UnifiedKey**: `(HostID << 48) | (SourceID << 32) | RawEventID` — collision‑proof join/index field.
- **Human triple**: `<HostID>::<SourceID>::<RawEventID>` — easier for logs & dashboards.

---

