Version:1.0StartHTML:00000097EndHTML:00010386StartFragment:00000153EndFragment:00010353

Alright Kyle — now we enter the **Adversarial Reviewer layer**, the part of the system that encodes _your_ engineering philosophy:  
**“Trust nothing unless it is proven. Attack every assumption. Mark every inconsistency.”**

This is the layer that prevents drift, hallucination, and silent breakage across the entire SK knowledge pipeline.  
It is also the layer that ensures your RAG system only serves **verified, version‑correct, evidence‑backed** information.

Let’s design it with the same rigor as the previous layers.

* * *

🔥 1. Adversarial Reviewer — Purpose
====================================

This agent is not a summarizer.  
It is not a validator.  
It is not a helper.

It is an **adversary**.

Its job is to:

* Cross‑examine:
  * Ingestion artifacts
  * Diff artifacts
  * Truth artifacts
  * Samples
  * Execution results
* Detect:
  * API/doc mismatches
  * Deprecated or removed APIs still referenced
  * Samples that compile but behave incorrectly
  * Docs that contradict runtime behavior
  * Features that are underspecified or ambiguous
  * Missing coverage (features with no samples, samples with no execution)
* Produce:
  * **Review items** (approved, disputed, risky, deprecated)
  * **Issue codes** with severity
  * **Evidence links** to artifacts
* Feed:
  * RAG Librarian (so retrieval is aware of risk/dispute status)

This is your “red team” inside the pipeline.

* * *

🔥 2. Reviewer Artifact — C# DTOs (Contract)
============================================

This is the JSON the Reviewer agent emits.
    public sealed class ReviewArtifact
    {
        public ReviewRunInfo ReviewRun { get; set; } = default!;
        public List<ReviewItemInfo> Reviews { get; set; } = new();
    }

    public sealed class ReviewRunInfo
    {
        public string ReviewRunId { get; set; } = default!;
        public DateTime TimestampUtc { get; set; }
        public string SnapshotId { get; set; } = default!;
        public string SchemaVersion { get; set; } = "1.0.0";
    }

### Review items

    public sealed class ReviewItemInfo
    {
        public string TargetKind { get; set; } = default!; // sample | feature | doc | api_member
        public string TargetId { get; set; } = default!;   // sample_uid, feature_uid, doc_uid, member_uid
        public string Status { get; set; } = default!;     // approved | disputed | risky | deprecated
        public string Summary { get; set; } = default!;
        public List<ReviewIssueInfo> Issues { get; set; } = new();
    }

### Issue details

    public sealed class ReviewIssueInfo
    {
        public string Code { get; set; } = default!;       // e.g., "USES_DEPRECATED_MEMBER"
        public string Severity { get; set; } = default!;   // info | warning | error
        public string? RelatedMemberUid { get; set; }
        public string Details { get; set; } = default!;
    }

* * *

🔥 3. SQL Server Schema — Reviewer Layer
========================================

### `review_run`

    CREATE TABLE review_run (
        id UNIQUEIDENTIFIER PRIMARY KEY,
        snapshot_id UNIQUEIDENTIFIER NOT NULL,
        timestamp_utc DATETIME2 NOT NULL,
        schema_version NVARCHAR(50) NOT NULL
    );

### `review_item`

    CREATE TABLE review_item (
        id UNIQUEIDENTIFIER PRIMARY KEY,
        review_run_id UNIQUEIDENTIFIER NOT NULL,
        target_kind NVARCHAR(50) NOT NULL,
        target_uid NVARCHAR(400) NOT NULL,
        status NVARCHAR(50) NOT NULL,
        summary NVARCHAR(MAX) NOT NULL
    );

### `review_issue`

    CREATE TABLE review_issue (
        id UNIQUEIDENTIFIER PRIMARY KEY,
        review_item_id UNIQUEIDENTIFIER NOT NULL,
        code NVARCHAR(100) NOT NULL,
        severity NVARCHAR(50) NOT NULL,
        related_member_uid NVARCHAR(400) NULL,
        details NVARCHAR(MAX) NOT NULL
    );

* * *

🔥 4. Reviewer Pipeline Logic
=============================

The Reviewer agent performs a **full adversarial pass** over all artifacts.

Below is the deterministic logic.

* * *

**Step 1 — Load all artifacts for the snapshot**
------------------------------------------------

* `api_type`, `api_member`, `api_parameter`
* `doc_page`, `doc_section`, `code_block`
* `feature`, `feature_type_link`, `feature_member_link`
* `sample`, `sample_api_member_link`
* `execution_result`
* `snapshot_diff`

Everything is available.

* * *

**Step 2 — Check for API/doc inconsistencies**
----------------------------------------------

Examples:

* API member exists but no doc references
* Doc references a member that no longer exists
* Doc describes a parameter that is not in the signature
* Doc shows a sample using a removed API

Reviewer emits:
    Status: disputed
    Issue: DOC_API_MISMATCH
    Severity: error

* * *

**Step 3 — Check for deprecated or removed API usage**
------------------------------------------------------

If a sample uses:

* A member marked `[Obsolete]`
* A member removed in the diff
* A member whose signature changed

Reviewer emits:
    Status: risky
    Issue: USES_DEPRECATED_MEMBER
    Severity: warning

Or:
    Status: disputed
    Issue: USES_REMOVED_MEMBER
    Severity: error

* * *

**Step 4 — Check execution results**
------------------------------------

If a sample:

* Fails to build → `build_failed`
* Fails at runtime → `failed`
* Throws exceptions → captured in execution logs

Reviewer emits:
    Status: disputed
    Issue: SAMPLE_RUNTIME_FAILURE
    Severity: error

* * *

**Step 5 — Check feature coverage**
-----------------------------------

If a feature has:

* No samples
* No execution results
* No doc references
* No primary members

Reviewer emits:
    Status: risky
    Issue: FEATURE_UNDER_SPECIFIED
    Severity: warning

* * *

**Step 6 — Check for ambiguous or underspecified features**
-----------------------------------------------------------

If a feature:

* Has no clear primary type
* Has no clear primary member
* Has contradictory doc references
* Has multiple samples with conflicting behavior

Reviewer emits:
    Status: disputed
    Issue: FEATURE_AMBIGUOUS
    Severity: error

* * *

**Step 7 — Check for drift between truth model and API surface**
----------------------------------------------------------------

If the Truth Synthesizer:

* Missed a type
* Missed a member
* Misclassified a feature
* Used outdated version metadata

Reviewer emits:
    Status: disputed
    Issue: TRUTH_MODEL_DRIFT
    Severity: error

* * *

**Step 8 — Emit Review Artifact JSON**
--------------------------------------

One artifact per run.

* * *

**Step 9 — Persist into SQL Server**
------------------------------------

Insert into:

* `review_run`
* `review_item`
* `review_issue`

* * *

🔥 5. Example Review Artifact (Realistic)
=========================================

    {
      "reviewRun": {
        "reviewRunId": "01JH8Z8X9Z5T3C2Y8M9K4WQ9D1",
        "timestampUtc": "2025-01-02T15:00:00Z",
        "snapshotId": "01JH8Z8X9Z5T3C2Y8M9K4WQ9B6",
        "schemaVersion": "1.0.0"
      },
      "reviews": [
        {
          "targetKind": "sample",
          "targetId": "sample:csharp:kernel-builder:minimal",
          "status": "approved",
          "summary": "Sample compiles and runs successfully.",
          "issues": []
        },
        {
          "targetKind": "sample",
          "targetId": "sample:csharp:planner:legacy",
          "status": "disputed",
          "summary": "Sample uses a removed planner API.",
          "issues": [
            {
              "code": "USES_REMOVED_MEMBER",
              "severity": "error",
              "relatedMemberUid": "csharp:Microsoft.SemanticKernel.Planners.LegacyPlanner::Plan()",
              "details": "Member removed in SK 1.23.0."
            }
          ]
        }
      ]
    }

* * *

🔥 6. Why this layer matters
============================

This is the layer that:

* Protects your RAG system from serving outdated or incorrect SK information
* Ensures every sample is grounded in runtime truth
* Flags drift between SK versions
* Provides metadata for retrieval filtering
* Encodes your adversarial engineering philosophy directly into the pipeline

This is the “Kyle filter.”

* * *

⭐ 7. Next Layer: RAG Librarian
==============================

Now that we have:

* Ingestion
* Diff
* Truth
* Samples
* Execution
* Review

…the final layer is the **RAG Librarian**, which:

* Chunks all artifacts
* Embeds them using SQL Server’s vector type
* Stores metadata (version, status, tags, risk level)
* Provides the retrieval view your assistant will use
* Ensures answers are always:
  * version‑correct
  * evidence‑backed
  * filtered by review status

If you’re ready, I’ll continue with:
⭐ **RAG Librarian Layer**
=========================

DTOs, SQL schema (with SQL Server vector columns), and retrieval logic.

Just say **continue**.
