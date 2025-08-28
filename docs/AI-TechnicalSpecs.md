# =====================================================================
# Document Title   : Detailed Architecture Specification
# Version          : v0.1.0
# Date Created     : 2025-08-28
# Last Modified    : 2025-08-28
# Author(s)        : Unknown
# Reviewed By      : 
# Status           : Draft
# Purpose          : Deep technical specification of components, targets, and performance.
# Related Docs     : AI-Overview.md; AI-FusionModel.md
# Change Log       :
#   - 2025-08-28 | System | Initial header applied
# =====================================================================

---

# **LightweightAI – Detailed Architecture Specification**

## 1. **System Context**
LightweightAI is a modular, low‑latency anomaly detection and decision fusion platform for Windows systems. It operates in **streaming mode** with rolling retention, designed for **audit‑friendly forensic reconstruction** and **incremental model adaptation**. The architecture is **component‑decoupled** with defined contracts, so each block is hot‑swappable.

---

## 2. **Component Specifications**

### **1. Input Layer**
- **Sources:**  
  - Windows Event Logs (Security, System, Application)  
  - Sysmon operational logs  
  - Optional: JSON/CSV telemetry feeds from agents  
  - Windows Perfmon files
  - 3rd party app logs
  - Window Trace logs
  - Windows health logs
- **Interface Contract:**  
  - Push‑based event ingestion with bounded queue length  
  - Supports backpressure signaling for overload conditions  
- **Performance Targets:**  
  - Latency < 50 ms ingestion per event under 10k EPS (events/sec)  
  - Throughput scaling linear to available cores

---

### **2. Preprocessing & Feature Mapping**
- **Operations:**  
  - Stream normalization (timestamp alignment, severity code mapping)  
  - One‑hot encoding for categorical attributes (e.g., event IDs)  
  - Dimensionality reduction (PCA/feature hashing) – configurable  
  - Rolling window buffer for temporal context (configurable length)
- **Implementation Notes:**  
  - All transforms pure‑function, no hidden state except window buffer  
  - Feature mapping tables versioned for audit
- **Performance Targets:**  
  - Transform + feature vectorization < 2 ms per event on commodity hardware  
  - Memory footprint < 50 MB for feature cache

---

### **3. Anomaly Layer**
- **Fast‑Twitch Detectors:**  
  - EWMA (Exponentially Weighted Moving Average) spike detection  
  - z‑score deviation  
  - Lightweight density estimators (e.g., incremental k‑NN on last N events)
- **Slow‑Burn Trend Detectors:**  
  - Rolling statistical drift (e.g., K‑S test)  
  - Incremental concept drift detectors (ADWIN or Page‑Hinkley variants)
- **Expected Post‑Training Capability:**  
  - Detect sudden spikes (latency < 200 ms end‑to‑end) with 95% precision @ 90% recall baseline  
  - Detect multi‑hour/day degradations with < 1% false positive rate

---

### **4. Fusion Engine**
- **Inputs:** All detector outputs + business/rule‑based constraints  
- **Processing:**  
  - Weighted score aggregation  
  - Hysteresis state machine to suppress alert oscillation  
  - Rule precedence overrides for critical event classes
- **Outputs:**  
  - Unified risk score [0.0, 1.0]  
  - Decision classification: {NORMAL, WARNING, ALERT, CRITICAL}
- **Performance Targets:**  
  - Fusion latency < 5 ms per decision cycle  
  - State retention across rolling windows with O(1) hysteresis updates

---

### **5. Action Layer**
- **Capabilities:**  
  - Asynchronous dispatch to alerting channels (SMTP, syslog, webhook)  
  - Optional automated remediation hooks (PowerShell / custom scripts)
- **Auditability:**  
  - Every action has a signed log entry with hash of triggering features
- **Performance Targets:**  
  - Alert dispatch < 1 sec from anomaly classification

---

### **6. Audit & Learning Pipeline**
- **Functions:**  
  - Append‑only audit logs with SHA‑256 integrity checks  
  - Incremental model updates using confirmed labels  
  - Rolling retraining windows (configurable: hours → weeks)
- **Model Update Protocol:**  
  - No full retrain unless drift detection exceeds defined thresholds  
  - All model updates versioned with reproducible build scripts
- **Performance Targets:**  
  - Incremental update < 500 ms per batch  
  - Audit retrieval O(log n) for decision history

---

## 3. **Model Training & Capability Profile**

### **Training Data Requirements:**
- Minimum 4 rolling windows of full telemetry coverage for convergence  
- Balanced representation of normal vs. anomalous sequences (weighted sampling for imbalance)

### **Feature Space:**
- Numeric: event frequency, time deltas, severity levels  
- Categorical: process names, account IDs, event IDs  
- Derived: rolling averages, entropy scores, variance trends

### **Expected Capabilities After Training:**
- **Fast‑Twitch Models:**  
  - Detect anomalies in ≤ 1 sec from event occurrence  
  - Handle streaming volumes up to 50k EPS with < 5% CPU on mid‑tier CPU
- **Slow‑Burn Models:**  
  - Identify drift over weeks without flagging transients  
  - Maintain F1 score > 0.90 with low variance over rolling evaluations
- **Fusion Layer:**  
  - Blend detectors into coherent, explainable scoring output  
  - Hysteresis ensures < 2% oscillation rate in borderline cases
- **Incremental Adaptation:**  
  - Integrate new benign patterns without full retraining  
  - Preserve anomaly signatures for > 90 days unless explicitly pruned

---

## 4. **Deployment Footprint**
- **Local:** < 150 MB disk, < 200 MB RAM idle, spikes < 400 MB under load  
- **Enterprise Hybrid:** Aggregator node + distributed agents, same per‑node footprint

---

**performance characterization tables** with precision/recall curves, memory/CPU benchmarks under different loads, and drift detection sensitivity plots — useful if you’re prepping for stakeholder review or compliance submission.


