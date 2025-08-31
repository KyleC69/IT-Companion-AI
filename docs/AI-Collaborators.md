# =====================================================================
# Document Title   : Model Training How-To (PowerShell & CLI Only)
# Version          : v0.1.0
# Date Created     : 2025-08-28
# Last Modified    : 2025-08-28
# Author(s)        : Unknown
# Reviewed By      : 
# Status           : Draft
# Purpose          : End-to-end PowerShell-first workflow for model training and deployment.
# Related Docs     : AI-TrainingDataPresets.md; AI-Dev-Quick-Ref.md; AI-FusionModel.md
# Change Log       :
#   - 2025-08-28 | System | Initial header applied
# =====================================================================
---
Title: Model Training How-To (PowerShell & CLI Only)
File: AI-Collaborators.md
Description: PowerShell-first workflow for preparing, training, evaluating, and deploying LightweightAI models.
Version: 0.1.0
LastUpdated: 2025-08-28
Status: Draft
Tags: [training, workflow, powershell, models]
---



# **LightweightAI – Model Training How‑To (PowerShell & CLI Only)**

## 1️⃣ Environment Prep

**Requirements**  
- Windows 10/11  
- .NET 6 SDK (ML.NET CLI included)  
- PowerShell 7+  
- Git  
- Local or staged telemetry datasets

**Setup**
```powershell
git clone https://<repo-url>/LightweightAI.git
cd LightweightAI
```
No extra language runtimes needed — everything here assumes you’ll run commands via `.ps1` scripts or `mlnet` CLI.

---

## 2️⃣ Data Preparation

**Directory Layout**
```
/data
   /raw
      /fast      # short-interval streams for fast-twitch model
      /slow      # long-horizon telemetry for slow-burn model
   /processed
      /fast
      /slow
```

**Run Preprocessing (PowerShell scripts)**  
```powershell
# Fast-twitch
.\tools\Preprocess-Fast.ps1 -InputPath .\data\raw\fast -OutputPath .\data\processed\fast -WindowSizeSec 300

# Slow-burn
.\tools\Preprocess-Slow.ps1 -InputPath .\data\raw\slow -OutputPath .\data\processed\slow -WindowSizeSec 86400
```

**What happens here:**  
- Timestamp normalization, categorical ID mapping, and feature vector creation.  
- Rolling window buffers set independently for each model.

---

## 3️⃣ Training the Models

### **Fast‑Twitch Anomaly Detector**
```powershell
mlnet classification --training-data .\data\processed\fast\features.tsv `
    --label-column-name "AnomalyLabel" `
    --output-model .\models\FastTwitch.zip `
    --train-test-split 0.8
```
- **Model Type:** EWMA + z‑score hybrid wrapped in an ML.NET binary classifier  
- **Target:** Spike precision ≥ 95%, recall ≥ 90%

---

### **Slow‑Burn Trend Detector**
```powershell
mlnet regression --training-data .\data\processed\slow\features.tsv `
    --label-column-name "DriftScore" `
    --output-model .\models\SlowBurn.zip `
    --train-test-split 0.8
```
- **Model Type:** Incremental drift detection w/ statistical baselines  
- **Target:** Detect long‑term deviations w/ F1 ≥ 0.90 and <1% FPR

---

## 4️⃣ Evaluation

Scripts below run baseline metric checks — no code editing needed.

```powershell
.\tools\Eval-Fast.ps1 -ModelPath .\models\FastTwitch.zip -TestData .\data\test\fast
.\tools\Eval-Slow.ps1 -ModelPath .\models\SlowBurn.zip -TestData .\data\test\slow
```

Outputs:  
- Confusion matrices  
- Precision/Recall/F1  
- Drift lag metrics (slow‑burn)  
- Oscillation rate (fusion stress‑test)

---

## 5️⃣ Deployment

```powershell
Copy-Item .\models\*.zip .\deploy\models\
```
The **Decision Fusion Engine** will auto‑load these on service start.

---

## 6️⃣ “Q&” Training Placeholder (Future)

Not yet implemented — when built, this stage will:  
- Identify borderline classification zones in both models  
- Run targeted micro‑training jobs on these cases  
- Harmonize threshold boundaries before fusion

In current state, archive borderline eval samples in:
```
/data/qand_pending/
```
…so they’re ready when Q& comes online.

---

## 7️⃣ Collaborator Rules of Thumb
1. **Version Everything** — model file names should contain `YYYYMMDD` + Git commit.  
2. **Audit Trail** — preprocessing configs must be under source control; they define feature space.  
3. **Fusion‑First Validation** — always check combined output stability, not just per‑model stats.

---

Here’s a clean “Spec Sheet” style section you can drop straight into the README or onboarding docs, so any collaborator knows exactly what the **Aggregator** consumes — no reverse‑engineering required.

---

## 📊 **Aggregator – Expected Input Schema**

| **Col #** | **Field Name**       | **Data Type**          | **Max Size / Format**                      | **Limits / Constraints**                                                                 | **Context Type**                                                                 |
|-----------|----------------------|------------------------|---------------------------------------------|-------------------------------------------------------------------------------------------|-----------------------------------------------------------------------------------|
| 1         | `EventTimestamp`     | `DateTime` (UTC)       | `yyyy-MM-ddTHH:mm:ss.fffZ` (ISO‑8601)       | Must be monotonic within stream; ±2s tolerance to source clock drift                      | Temporal ordering anchor for fusion; drives rolling window alignment             |
| 2         | `SourceID`           | `string`               | 64 chars max                                | Alphanumeric + underscores; unique per telemetry source                                   | Origin identifier (agent ID, hostname, etc.)                                     |
| 3         | `EventID`            | `int` (32‑bit)         | 0–2,147,483,647                             | Maps to normalized event taxonomy                                                         | Categorical event type                                                            |
| 4         | `Severity`           | `byte`                 | 0–10                                        | 0=Info, 10=Critical                                                                       | Priority weighting in fusion                                                      |
| 5         | `Category`           | `string`               | 32 chars max                                | Must match predefined taxonomy list                                                       | Logical grouping (auth, process, network, etc.)                                   |
| 6         | `FeatureVector`      | `float[]`              | Length: fixed per model config (e.g., 64)   | Normalized 0.0–1.0; NaN not allowed                                                        | Numerical features for model input                                                |
| 7         | `AnomalyScore`       | `float` (single)       | 0.0–1.0                                     | Output from upstream detector; nullable if pre‑fusion                                      | Continuous risk measure                                                            |
| 8         | `DriftScore`         | `float` (single)       | 0.0–1.0                                     | Output from trend detector; nullable if pre‑fusion                                         | Long‑term deviation measure                                                        |
| 9         | `Label`              | `string`               | 32 chars max                                | Optional during inference; required during supervised training                            | Ground truth for model tuning                                                      |
| 10        | `ContextHash`        | `string`               | 64 chars (SHA‑256 hex)                      | Calculated from key contextual fields (EventID, SourceID, etc.)                            | Deduplication & correlation handle                                                 |
| 11        | `ContextType`        | `string`               | 16 chars max                                | Enum: {FAST, SLOW}                                                                         | Routes record to correct processing lane                                           |
| 12        | `MetaFlags`          | `uint16` (bitmask)     | 0–65535                                     | Custom operational flags (e.g., replayed, synthetic, Q& candidate)                        | Auxiliary control bits                                                             |

---

### **Notes for Collaborators**
- **Number of Columns:** 12 — all columns must be present in order, even if nullables are empty.  
- **Nullability:** Only `AnomalyScore`, `DriftScore`, and `Label` can be null during live inference; all others are required.  
- **Encoding:** UTF‑8 for all string fields.  
- **Row Size:** ~1–2 KB typical; hard limit 8 KB per row.  
- **Ordering:** `EventTimestamp` is the primary sort key; ingestion assumes time‑ascending batches.  
- **Validation:** The preprocessor will reject rows violating type/size constraints before hitting the fusion engine.  

---

If you want, I can also give you a **field‑level lineage map** showing exactly where each of these columns originates in the ingestion pipeline and which modules consume them — makes it far easier for a new dev to trace data from log to model to action. Would that be useful to add next?


