**Do not add to manifest**; this is a documentation is temporary only.

Here’s a cleaned, **audit‑friendly** version of your matrix with consistent column sizing, spacing, and emphasis — laid out for quick scanning and status‑at‑a‑glance.  
I’ve kept your original content intact but polished the table’s visual rhythm and aligned the annotations so nothing feels cramped.

---

### **Capability Verification & Gaps Tracker**  

| **Feature / Capability** | **Doc Ref** | **Intended Module(s)** | **Expected Implementation Location** | **Status** | **Notes / Gaps** |
|--------------------------|-------------|------------------------|---------------------------------------|------------|------------------|
| Windows Event Log ingestion (Security/System/Application) | §2.1 | Input Layer | `Loaders/EventLogIngest.cs` | ⚠ Stub | Only `Loaders/Events/EventLogLoader.cs` present; returns empty DataTable. Needs real ingestion + backpressure. |
| Sysmon operational log ingestion | §2.1 | Input Layer | `Loaders/SysmonIngest.cs` | ❌ Missing | No Sysmon loader found. |
| JSON/CSV agent feed ingestion | §2.1 | Input Layer | `Loaders/GenericFeedIngest.cs` | ❌ Missing | Generic feed loader not found (only `ServiceLoader` for CSV). |
| Windows Perfmon file ingestion | §2.1 | Input Layer | `Loaders/PerfmonIngest.cs` | ❌ Missing | Not implemented. |
| 3rd‑party app log ingestion | §2.1 | Input Layer | `Loaders/AppLogIngest.cs` | ❌ Missing | Not implemented; need plug‑in contract. |
| Windows Trace log ingestion | §2.1 | Input Layer | `Loaders/WtraceIngest.cs` | ❌ Missing | Not implemented. |
| Windows health log ingestion | §2.1 | Input Layer | `Loaders/HealthLogIngest.cs` | ❌ Missing | Not implemented. |
| Ingestion latency < 50 ms @ 10k EPS | §2.1 | Input Layer | — | ⚠ Benchmark needed | No perf harness yet. |
| Stream normalization (timestamp, severity code mapping) | §2.2 | Preprocessing | `Refinery/Normalizer.cs` / `SeverityMapper.cs` | ⚠ Partial | `SeverityMapper` implemented; existing `Normalizer` is Q&A text‑focused, not event schema. Need event envelope normalization. |
| One‑hot encoding for categorical attributes | §2.2 | Feature Mapping | `Refinery/FeatureEncoder.cs` | ❌ Missing | No encoder present. |
| Dimensionality reduction (PCA/feature hashing) | §2.2 | Feature Mapping | `Refinery/FeatureReducer.cs` | ❌ Missing | No reducer implementation. |
| Rolling window buffer | §2.2 | Feature Mapping | `Refinery/WindowBuffer.cs` | ✅ Implemented | `Training/SlidingWindowBuffer.cs` satisfies rolling buffer requirement. |
| Transform + vectorization < 2 ms/event | §2.2 | Preproc./Feature Mapping | — | ⚠ Benchmark needed | Need timing around normalization + vector build. |
| Feature cache < 50 MB RAM | §2.2 | Feature Mapping | — | ⚠ Measure | No measurements captured. |
| EWMA spike detection | §2.3 | Anomaly Layer (fast) | `Engine/FastDetectors/Ewma.cs` | ❌ Missing | No EWMA detector class. |
| z‑score deviation detection | §2.3 | Anomaly Layer (fast) | `Engine/FastDetectors/ZScore.cs` | ❌ Missing | `AnomalySignal` carries ZScore field but no calculator/detector implementation. |
| Lightweight density estimator (incremental k‑NN) | §2.3 | Anomaly Layer (fast) | `Engine/FastDetectors/IncrementalKnn.cs` | ❌ Missing | Not implemented. |
| K‑S statistical drift detection | §2.3 | Anomaly Layer (slow) | `Engine/SlowDetectors/KsTest.cs` | ❌ Missing | Not implemented. |
| ADWIN/Page‑Hinkley drift detector | §2.3 | Anomaly Layer (slow) | `Engine/SlowDetectors/Adwin.cs` or `PageHinkley.cs` | ❌ Missing | Neither variant present. |
| Spike detection precision/recall meets spec | §2.3 | Anomaly Layer | — | ⚠ Benchmark needed | Requires evaluation harness. |
| Multi‑hour/day degradation FP rate < 1 % | §2.3 | Anomaly Layer | — | ⚠ Benchmark needed | Long‑duration test missing. |
| Weighted score aggregation | §2.4 | Fusion Engine | `Engine/Fusion/FusionEngine.cs` | ✅ Implemented | Aggregation via weighted field map. |
| Hysteresis state machine | §2.4 | Fusion Engine | `Engine/HysteresisDecider.cs` | ✅ Implemented | HysteresisDecider present (O(1)). |
| Rule precedence overrides | §2.4 | Fusion Engine | `Engine/RulesEngine.cs` | ⚠ Partial | Rules evaluate; explicit precedence/override logic not implemented. |
| Fusion latency < 5 ms/decision | §2.4 | Fusion Engine | — | ⚠ Benchmark needed | Need timing harness. |
| Async alert dispatch (SMTP, syslog, webhook) | §2.5 | Action Layer | `Engine/ActionDispatcher.cs` | ❌ Missing | No dispatcher implementation. |
| Automated remediation hooks | §2.5 | Action Layer | `Engine/RemediationHooks.cs` | ❌ Missing | Not implemented. |
| Alert dispatch < 1 sec from classification | §2.5 | Action Layer | — | ⚠ Benchmark needed | Blocked by missing dispatcher. |
| Signed log entries for actions | §2.5 | Audit Layer | `Audit/ActionLogger.cs` | ❌ Missing | Only generic console loggers; no hashing/signature. |
| Append‑only audit logs w/ integrity checks | §2.6 | Audit Layer | `Audit/AuditLogger.cs` | ❌ Missing | `ProvenanceLogger` writes to console; no append‑only storage or integrity. |
| Incremental model updates w/ confirmed labels | §2.6 | Learning Pipeline | `Training/IncrementalUpdater.cs` | ⚠ Partial | `AuditFriendlyTrainer` + buffer exist; needs confirmed label gating & update policy. |
| Rolling retraining windows (hrs → wks) | §2.6 | Learning Pipeline | `Training/RetrainingScheduler.cs` | ❌ Missing | No scheduler component. |
| Incremental update < 500 ms/batch | §2.6 | Learning Pipeline | — | ⚠ Benchmark needed | Need batch timing on trainer. |
| Audit retrieval O(log n) | §2.6 | Audit Layer | — | ❌ Missing | No indexed storage / retrieval API. |
| 4 rolling windows of telemetry before training | §3 | Training | — | ⚠ Policy TBD | Logic not enforced; buffer present only. |
| Balanced sampling for training | §3 | Training | `Training/DataSampler.cs` | ❌ Missing | No sampler class. |
| Fast model anomaly detect ≤ 1 s | §3 | Anomaly Layer | — | ⚠ Benchmark needed | Pending detector implementations. |
| Slow model drift detection F1 > 0.90 | §3 | Anomaly Layer | — | ⚠ Benchmark needed | Blocked by missing drift detectors. |
| Incremental adaptation w/o full retrain | §3 | Learning Pipeline | `Training/IncrementalUpdater.cs` | ⚠ Partial | Trainer scaffold only; adaptation policy absent. |
| Deployment footprint < 150 MB disk, < 200 MB RAM idle | §4 | All modules | — | ⚠ Measure | No profiling captured. |

---

**✅ Next Steps for a Green‑Lighted Training Phase**  
1. **Implement & flesh out detectors** → EWMA, Z‑score calculator, k‑NN, KS/ADWIN.  
2. **Build ingestion coverage** → Complete remaining loaders (Sysmon, Perfmon, Trace, Health, Generic).  
3. **Add feature mapping layer** → One‑hot encoder, dimensionality reducer, event normalizer (schema‑aware).  
4. **Action & Audit Hardening** → Implement dispatcher, action signing, append‑only audit store w/ hash chain.  
5. **Scheduling & Sampling** → Add retraining scheduler + balanced sampler.  
6. **Benchmarks & Profiling** → Create perf harness (ingest, transform, fusion, training batch).  
7. **Policy Enforcement** → Enforce 4‑window prerequisite & confirmed label gating before incremental updates.  

If you like, I can scaffold the detector set or the ingestion loaders next—just specify which track you want first.
