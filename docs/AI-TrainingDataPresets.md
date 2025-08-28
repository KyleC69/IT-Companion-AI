Perfect — here’s a ready‑to‑reference **pre‑defined value set** for the schema fields we flagged as needing strict, known lists. These should live in the repo as version‑controlled config files so they can be centrally updated without touching code.

---

## **Pre‑Defined Field Value Sets**

### **`Category` Field** (Col #5)
> Logical grouping of events for routing and feature weighting.  
> **Source:** `/config/taxonomy/category_map.json`

| **Value**     | **Description**                                          |
|---------------|----------------------------------------------------------|
| `auth`        | Authentication & authorization events                    |
| `process`     | Process creation, termination, and exec chain changes    |
| `network`     | Network connection open/close, port activity             |
| `filesystem`  | File create/delete/modify, permission changes            |
| `registry`    | Windows registry reads/writes                            |
| `service`     | Service start/stop/install/uninstall                     |
| `driver`      | Driver load/unload                                       |
| `schedule`    | Scheduled task creation/modification                     |
| `policy`      | Group policy or configuration changes                    |
| `alert`       | Security appliance or IDS/IPS generated alerts           |
| `other`       | Miscellaneous, non‑mapped events                         |

---

### **`ContextType` Field** (Col #11)
> Used by aggregator to route records into the correct processing lane.

| **Value** | **Meaning**                                   |
|-----------|-----------------------------------------------|
| `FAST`    | Short‑interval, high‑resolution anomaly lane  |
| `SLOW`    | Long‑term drift/trend analysis lane           |

---

### **`Severity` Field** (Col #4)
> Numeric scale for prioritization in fusion scoring.

| **Value** | **Label**         | **Guideline**                                                |
|-----------|-------------------|--------------------------------------------------------------|
| 0         | Info              | Routine operational info; no action                         |
| 1–3       | Low               | Minor anomalies or policy violations                        |
| 4–6       | Medium            | Potentially impactful; monitor                              |
| 7–8       | High              | Significant threat indicators                               |
| 9         | Severe            | Confirmed or likely incident; immediate attention           |
| 10        | Critical          | System compromise, major outage, or direct attack detected  |

---

### **`MetaFlags` Bitmask** (Col #12)
> Operational control bits (uint16). Multiple flags can be OR’d together.

| **Bit** | **Hex**   | **Name**           | **Meaning**                                           |
|---------|-----------|--------------------|-------------------------------------------------------|
| 0       | `0x0001`  | `IS_REPLAY`         | Data is replayed from archive                        |
| 1       | `0x0002`  | `IS_SYNTHETIC`      | Synthetic/test event                                 |
| 2       | `0x0004`  | `IS_QCANDIDATE`     | Candidate for future Q& training                     |
| 3       | `0x0008`  | `NO_ALERT`          | Suppress alert dispatch                              |
| 4       | `0x0010`  | `FORCE_ALERT`       | Override normal scoring; force alert                 |
| 5       | `0x0020`  | `EXTERNAL_FEED`     | Originated from third‑party feed                     |
| 6       | `0x0040`  | `DEPRIORITIZE`      | Lower fusion priority                                |
| 7–15    | —         | Reserved            | Keep free for future use                             |

---

### **`EventID` Normalization Table**
> Maps raw platform‑specific IDs to unified taxonomy.  
> **Source:** `/config/taxonomy/eventid_map.json`  
*(Example snippet)*

| **Raw EventID** | **UnifiedID** | **Category** | **Notes**                       |
|-----------------|---------------|--------------|----------------------------------|
| 4624            | `AUTH_LOGIN`  | auth         | Successful logon                 |
| 4625            | `AUTH_FAIL`   | auth         | Failed logon attempt             |
| 4688            | `PROC_START`  | process      | New process created               |
| 4689            | `PROC_END`    | process      | Process terminated                |
| …               | …             | …            | …                                |

---

If you like, I can follow this with a **lineage map** showing **exactly** where each column originates, which pre‑defined set it’s validated against, and which downstream modules consume it. That way, any collaborator can trace “Category → Fusion Engine weight table” in one glance without digging through source. Would you like me to build that next?
