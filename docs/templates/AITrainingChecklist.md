
| Feature / Capability | Doc Ref | Intended Module(s) | Implementation Location (expected) | Status | Notes / Gaps |
|----------------------|---------|--------------------|--------------------------------------|--------|--------------|
| Windows Event Log ingestion (Security/System/Application) | §2.1 | Input Layer | `Loaders/EventLogIngest.cs` | ⬜ To‑verify | Must support push‑based, bounded queue, backpressure. |
| Sysmon operational log ingestion | §2.1 | Input Layer | `Loaders/SysmonIngest.cs` | ⬜ To‑verify | Ensure Sysmon schema handled; push contract respected. |
| JSON/CSV agent feed ingestion | §2.1 | Input Layer | `Loaders/GenericFeedIngest.cs` | ⬜ To‑verify | Parsing error handling; config‑driven schema map. |
| Windows Perfmon file ingestion | §2.1 | Input Layer | `Loaders/PerfmonIngest.cs` | ⬜ To‑verify | Batch load & stream tailing modes. |
| 3rd‑party app log ingestion | §2.1 | Input Layer | `Loaders/AppLogIngest.cs` | ⬜ To‑verify | Plug‑in contract for custom parsers. |
| Windows Trace log ingestion | §2.1 | Input Layer | `Loaders/WtraceIngest.cs` | ⬜ To‑verify | Event filtering at source. |
| Windows health log ingestion | §2.1 | Input Layer | `Loaders/HealthLogIngest.cs` | ⬜ To‑verify | Normalize to common event envelope. |
| Ingestion latency < 50 ms @ 10k EPS | §2.1 | Input Layer | — | ⚠ Benchmark needed | Measure under load; verify linear scaling to cores. |
| Stream normalization (timestamp, severity code mapping) | §2.2 | Preprocessing | `Refinery/Normalizer.cs` / `SeverityMapper.cs` | ⬜ To‑verify | Pure‑function; config‑driven severity scale. |
| One‑hot encoding for categorical attributes | §2.2 | Feature Mapping | `Refinery/FeatureEncoder.cs` | ⬜ To‑verify | Check versioned mapping tables. |
| Dimensionality reduction (PCA/feature hashing) | §2.2 | Feature Mapping | `Refinery/FeatureReducer.cs` | ⚠ Stub? | Ensure configurable algorithm choice. |
| Rolling window buffer | §2.2 | Feature Mapping | `Refinery/WindowBuffer.cs` | ⬜ To‑verify | Configurable length; no hidden state outside buffer. |
| Transform + vectorization < 2 ms/event | §2.2 | Preprocessing/Feature Mapping | — | ⚠ Benchmark needed | Verify on commodity hardware. |
| Feature cache < 50 MB RAM | §2.2 | Feature Mapping | — | ⚠ Measure | Memory profile under load. |
| EWMA spike detection | §2.3 | Anomaly Layer (fast) | `Engine/FastDetectors/Ewma.cs` | ⬜ To‑verify | Confirm latency budget < 200 ms e2e. |
| z‑score deviation detection | §2.3 | Anomaly Layer (fast) | `Engine/FastDetectors/ZScore.cs` | ⬜ To‑verify | Threshold configurable. |
| Lightweight density estimator (incremental k‑NN) | §2.3 | Anomaly Layer (fast) | `Engine/FastDetectors/IncrementalKnn.cs` | ⚠ Partial? | Verify neighbor count & window length. |
| K‑S statistical drift detection | §2.3 | Anomaly Layer (slow) | `Engine/SlowDetectors/KsTest.cs` | ⬜ To‑verify | Supports streaming calc. |
| ADWIN/Page‑Hinkley drift detector | §2.3 | Anomaly Layer (slow) | `Engine/SlowDetectors/Adwin.cs` or `PageHinkley.cs` | ⚠ Optional | At least one variant implemented. |
| Spike detection precision/recall meets spec | §2.3 | Anomaly Layer | — | ⚠ Benchmark needed | Validate 95% precision @ 90% recall baseline. |
| Multi‑hour/day degradation FP rate < 1% | §2.3 | Anomaly Layer | — | ⚠ Benchmark needed | Long‑term eval harness. |
| Weighted score aggregation | §2.4 | Fusion Engine | `Orchestrator/FusionEngine.cs` | ✅ Implemented | Calibrated logit stacking. |
| Hysteresis state machine | §2.4 | Fusion Engine | `Orchestrator/FusionEngine.cs` | ✅ Implemented | O(1) updates; dwell timers per spec. |
| Rule precedence overrides | §2.4 | Fusion Engine | `Orchestrator/FusionEngine.cs` | ✅ Implemented | Critical events enforce risk floor. |
| Fusion latency < 5 ms/decision | §2.4 | Fusion Engine | — | ⚠ Benchmark needed | Measure in decision loop. |
| Async alert dispatch (SMTP, syslog, webhook) | §2.5 | Action Layer | `Engine/ActionDispatcher.cs` | ⬜ To‑verify | Non‑blocking; all channels present. |
| Automated remediation hooks | §2.5 | Action Layer | `Engine/RemediationHooks.cs` | ⚠ Optional | Sandboxed; execution logged. |
| Alert dispatch < 1 sec from classification | §2.5 | Action Layer | — | ⚠ Benchmark needed | Measure across channels. |
| Signed log entries for actions | §2.5 | Audit Layer | `Audit/ActionLogger.cs` | ⬜ To‑verify | SHA‑256 of triggering features stored. |
| Append‑only audit logs with integrity checks | §2.6 | Audit Layer | `Audit/AuditLogger.cs` | ⬜ To‑verify | No mutation paths. |
| Incremental model updates with confirmed labels | §2.6 | Learning Pipeline | `Training/IncrementalUpdater.cs` | ⬜ To‑verify | Honors drift thresholds before retrain. |
| Rolling retraining windows (hrs→wks) | §2.6 | Learning Pipeline | `Training/RetrainingScheduler.cs` | ⬜ To‑verify | Configurable; reproducible build scripts. |
| Incremental update < 500 ms/batch | §2.6 | Learning Pipeline | — | ⚠ Benchmark needed | Measure update throughput. |
| Audit retrieval O(log n) | §2.6 | Audit Layer | — | ⚠ Benchmark needed | Test with large history. |
| 4 rolling windows of telemetry before training | §3 | Training | — | ⬜ To‑verify | Confirm data coverage. |
| Balanced sampling for training | §3 | Training | `Training/DataSampler.cs` | ⚠ Partial? | Weighting logic present. |
| Fast model anomaly detect ≤ 1 s | §3 | Anomaly Layer | — | ⚠ Benchmark needed | Validate CPU < 5% mid‑tier hardware. |
| Slow model drift detection F1 > 0.90 | §3 | Anomaly Layer | — | ⚠ Benchmark needed | Rolling evaluation framework. |
| Incremental adaptation without full retrain | §3 | Learning Pipeline | `Training/IncrementalUpdater.cs` | ⬜ To‑verify | Preserves anomaly sigs ≥ 90 days. |
| Deployment footprint < 150 MB disk, < 200 MB RAM idle | §4 | All modules | — | ⚠ Measure | Profile idle and load. |

---

**Next moves if you want to green‑light training**:  
1. Open up `Refinery/`, `Engine/FastDetectors/`, `Engine/SlowDetectors/`, and `Audit/` to tick the ⬜ rows to either ✅ or ⚠/❌.  
2. Wire in or stub any missing ingest sources, detectors, and action channels so your model sees the full range of telemetry described.  
3. Run perf harnesses to validate ingestion, detection, and footprint specs — that knocks out the “⚠ Benchmark needed” rows.  
4. Once the table’s all green for core capabilities, we lock a tag and move on to the training pipeline with no architectural debt.  

I can start drilling into the Input Layer files in `Loaders/` and `Orchestrator/` next, marking off each ingestion and preprocessing feature as verified or missing, so the top of the table turns green first. Want me to do that pass now?
