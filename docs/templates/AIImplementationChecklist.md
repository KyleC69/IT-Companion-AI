Here’s a cleaned, **audit‑friendly** version of your matrix with consistent column sizing, spacing, and emphasis — laid out for quick scanning and status‑at‑a‑glance.  
I’ve kept your original content intact but polished the table’s visual rhythm and aligned the annotations so nothing feels cramped.

---

### **Capability Verification & Gaps Tracker**  

| **Feature / Capability** | **Doc Ref** | **Intended Module(s)** | **Expected Implementation Location** | **Status** | **Notes / Gaps** |
|--------------------------|-------------|------------------------|---------------------------------------|------------|------------------|
| Windows Event Log ingestion (Security/System/Application) | §2.1 | Input Layer | `Loaders/EventLogIngest.cs` | ⚠ Partial | Multi-channel loader added; needs real-time subscription & batching. |
| Sysmon operational log ingestion | §2.1 | Input Layer | `Loaders/SysmonIngest.cs` | ⚠ Partial | Sysmon loader present; add resilience & perf metrics. |
| JSON/CSV agent feed ingestion | §2.1 | Input Layer | `Loaders/GenericFeedIngest.cs` | ⚠ Partial | Generic loader + parser; add schema & retry logic. |
| Windows Perfmon file ingestion | §2.1 | Input Layer | `Loaders/PerfmonIngest.cs` | ⚠ Partial | CSV Perfmon loader; add type coercion + host metadata. |
| 3rd‑party app log ingestion | §2.1 | Input Layer | `Loaders/AppLogIngest.cs` | ❌ Missing | Provide plug-in parser contract. |
| Windows Trace log ingestion | §2.1 | Input Layer | `Loaders/WtraceIngest.cs` | ⚠ Partial | ETW real-time loader stub using TraceEvent (needs provider config & error handling). |
| Windows health log ingestion | §2.1 | Input Layer | `Loaders/HealthLogIngest.cs` | ⚠ Stub | Synthetic health snapshots; replace with real WMI/perf polling. |
| WMI diagnostic queries ingestion | §2.1 | Input Layer | `Loaders/WmiQueryLoader.cs` | ⚠ Partial | Periodic WMI query loader; add query set management + filtering. |
| Performance counter streaming | §2.1 | Input Layer | `Loaders/PerfCounterStreamLoader.cs` | ⚠ Stub | Synthetic counters; implement PDH/PerformanceCounter access. |
| Ingestion latency < 50 ms @ 10k EPS | §2.1 | Input Layer | — | ⚠ Benchmark needed | Perf harness pending. |
| Stream normalization (timestamp, severity code mapping) | §2.2 | Preprocessing | `Refinery/Normalizer.cs` / `SeverityMapper.cs` | ⚠ Partial | Default normalizer; extend to domain-specific fields. |
| One‑hot encoding for categorical attributes | §2.2 | Feature Mapping | `Refinery/FeatureEncoder.cs` | ✅ Implemented | OneHotEncoder + wrapper added. |
| Dimensionality reduction (PCA/feature hashing) | §2.2 | Feature Mapping | `Refinery/FeatureReducer.cs` | ⚠ Stub | Pass‑through reducer placeholder. |
| Rolling window buffer | §2.2 | Feature Mapping | `Refinery/WindowBuffer.cs` | ✅ Implemented | SlidingWindowBuffer present. |
| Transform + vectorization < 2 ms/event | §2.2 | Preproc./Feature Mapping | — | ⚠ Benchmark needed | Need timing tests. |
| Feature cache < 50 MB RAM | §2.2 | Feature Mapping | — | ⚠ Measure | No profiling yet. |
| EWMA spike detection | §2.3 | Anomaly Layer (fast) | `Engine/FastDetectors/Ewma.cs` | ✅ Implemented | Adapter over EwmaDetector. |
| z‑score deviation detection | §2.3 | Anomaly Layer (fast) | `Engine/FastDetectors/ZScore.cs` | ✅ Implemented | Adapter over ZScoreDetector. |
| Lightweight density estimator (incremental k‑NN) | §2.3 | Anomaly Layer (fast) | `Engine/FastDetectors/IncrementalKnn.cs` | ✅ Implemented | Adapter over WindowedKnnDensity. |
| K‑S statistical drift detection | §2.3 | Anomaly Layer (slow) | `Engine/SlowDetectors/KsTest.cs` | ⚠ Partial | Streaming KS drift implemented (KsStreamingDrift); ADWIN/Page-Hinkley still stubs. |
| ADWIN/Page‑Hinkley drift detector | §2.3 | Anomaly Layer (slow) | `Engine/SlowDetectors/Adwin.cs` or `PageHinkley.cs` | ⚠ Stub | Placeholders added. |
| Spike detection precision/recall meets spec | §2.3 | Anomaly Layer | — | ⚠ Benchmark needed | Evaluation harness pending. |
| Multi‑hour/day degradation FP rate < 1 % | §2.3 | Anomaly Layer | — | ⚠ Benchmark needed | Long run tests needed. |
| Weighted score aggregation | §2.4 | Fusion Engine | `Engine/Fusion/FusionEngine.cs` | ✅ Implemented | Weighted map aggregation. |
| Hysteresis state machine | §2.4 | Fusion Engine | `Engine/HysteresisDecider.cs` | ✅ Implemented | Present. |
| Rule precedence overrides | §2.4 | Fusion Engine | `Engine/RulesEngine.cs` | ✅ Implemented | Hard floor override logic applied pre-hysteresis. |
| Fusion latency < 5 ms/decision | §2.4 | Fusion Engine | — | ⚠ Benchmark needed | Need perf test. |
| Async alert dispatch (SMTP, syslog, webhook) | §2.5 | Action Layer | `Engine/ActionDispatcher.cs` | ✅ Implemented | Dispatcher + SMTP, Syslog, Webhook sinks wired. Metrics & retries TBD. |
| Automated remediation hooks | §2.5 | Action Layer | `Engine/RemediationHooks.cs` | ❌ Missing | Not implemented. |
| Alert dispatch < 1 sec from classification | §2.5 | Action Layer | — | ⚠ Benchmark needed | Measure end-to-end once metrics added. |
| Signed log entries for actions | §2.5 | Audit Layer | `Audit/ActionLogger.cs` | ❌ Missing | Need hashing + chain verification. |
| Append‑only audit logs w/ integrity checks | §2.6 | Audit Layer | `Audit/AuditLogger.cs` | ⚠ Partial | FileAuditLog with hash chain; needs tamper verification & indexing. |
| Incremental model updates w/ confirmed labels | §2.6 | Learning Pipeline | `Training/IncrementalUpdater.cs` | ⚠ Partial | Trainer + buffer present; gating logic missing. |
| Rolling retraining windows (hrs → wks) | §2.6 | Learning Pipeline | `Training/RetrainingScheduler.cs` | ✅ Implemented | Basic interval scheduler. |
| Incremental update < 500 ms/batch | §2.6 | Learning Pipeline | — | ⚠ Benchmark needed | Batch timing missing. |
| Audit retrieval O(log n) | §2.6 | Audit Layer | — | ❌ Missing | Current file log is linear scan. |
| 4 rolling windows of telemetry before training | §3 | Training | — | ⚠ Policy TBD | Enforcement missing. |
| Balanced sampling for training | §3 | Training | `Training/DataSampler.cs` | ⚠ Partial | Sampler present; needs weighting validation. |
| Fast model anomaly detect ≤ 1 s | §3 | Anomaly Layer | — | ⚠ Benchmark needed | Performance tests missing. |
| Slow model drift detection F1 > 0.90 | §3 | Anomaly Layer | — | ⚠ Benchmark needed | Requires enhanced drift + eval harness. |
| Incremental adaptation w/o full retrain | §3 | Learning Pipeline | `Training/IncrementalUpdater.cs` | ⚠ Partial | Policy logic pending. |
| Deployment footprint < 150 MB disk, < 200 MB RAM idle | §4 | All modules | — | ⚠ Measure | Profiling not yet run. |

---

**✅ Next Steps (Alerts & Audit Hardening)**
1. Add retry / circuit breaker + metrics (success, latency) to sinks.
2. Implement remediation hook scaffold + execution sandbox.
3. Add SHA-256 action signature & integrity verification log chain.
4. Introduce structured alert payload schema v1 (JSON contract) and version header.
5. Wire benchmark harness to record dispatch latency and fusion timing.
