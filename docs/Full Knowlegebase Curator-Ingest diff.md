Version:1.0StartHTML:00000097EndHTML:00009184StartFragment:00000153EndFragment:00009151

Great — SQL Server’s new vector type actually makes your life easier, because the RAG layer can now live in the same database as the ingestion, diff, truth, samples, execution, and review layers. No architectural changes are needed, but we _can_ tighten the schema to take advantage of SQL Server’s vector indexing.

Let’s continue exactly where we left off:  
**Diff Mapper → Truth Synthesizer → Sample Engineer → Executor → Reviewer → RAG Librarian**, but now with SQL Server–friendly schemas and DTOs.

Below is the next layer in the pipeline: **Diff Mapper**.

* * *

🔷 1. Diff Mapper — C# DTOs (Artifact Contract)
===============================================

This is the JSON your Diff Mapper agent emits. It compares two snapshots and produces a structured change set.
    public sealed class DiffArtifact
    {
        public DiffRunInfo DiffRun { get; set; } = default!;
        public List<ApiTypeDiff> TypeChanges { get; set; } = new();
        public List<ApiMemberDiff> MemberChanges { get; set; } = new();
        public List<DocPageDiff> DocChanges { get; set; } = new();
    }

    public sealed class DiffRunInfo
    {
        public string DiffId { get; set; } = default!;
        public DateTime TimestampUtc { get; set; }
        public string OldSnapshotId { get; set; } = default!;
        public string NewSnapshotId { get; set; } = default!;
        public string SchemaVersion { get; set; } = "1.0.0";
    }

### Type-level diffs

    public sealed class ApiTypeDiff
    {
        public string TypeUid { get; set; } = default!;
        public string ChangeKind { get; set; } = default!; // "added", "removed", "modified"
        public bool OldPresence { get; set; }
        public bool NewPresence { get; set; }
        public Dictionary<string, object>? Detail { get; set; }
    }

### Member-level diffs

    public sealed class ApiMemberDiff
    {
        public string MemberUid { get; set; } = default!;
        public string ChangeKind { get; set; } = default!;
        public string? OldSignature { get; set; }
        public string? NewSignature { get; set; }
        public bool Breaking { get; set; }
        public Dictionary<string, object>? Detail { get; set; }
    }

### Doc-level diffs

    public sealed class DocPageDiff
    {
        public string DocUid { get; set; } = default!;
        public string ChangeKind { get; set; } = default!;
        public Dictionary<string, object>? Detail { get; set; }
    }

* * *

🔷 2. Diff Mapper — SQL Server Schema
=====================================

SQL Server schema mirrors the DTOs.  
All JSON fields use `NVARCHAR(MAX)` or `JSON` (if you’re on SQL Server 2022+ with JSON enhancements).

### `snapshot_diff`

    CREATE TABLE snapshot_diff (
        id UNIQUEIDENTIFIER PRIMARY KEY,
        old_snapshot_id UNIQUEIDENTIFIER NOT NULL,
        new_snapshot_id UNIQUEIDENTIFIER NOT NULL,
        timestamp_utc DATETIME2 NOT NULL,
        schema_version NVARCHAR(50) NOT NULL
    );

### `api_type_diff`

    CREATE TABLE api_type_diff (
        id UNIQUEIDENTIFIER PRIMARY KEY,
        snapshot_diff_id UNIQUEIDENTIFIER NOT NULL,
        type_uid NVARCHAR(400) NOT NULL,
        change_kind NVARCHAR(50) NOT NULL,
        detail_json NVARCHAR(MAX) NULL
    );

### `api_member_diff`

    CREATE TABLE api_member_diff (
        id UNIQUEIDENTIFIER PRIMARY KEY,
        snapshot_diff_id UNIQUEIDENTIFIER NOT NULL,
        member_uid NVARCHAR(400) NOT NULL,
        change_kind NVARCHAR(50) NOT NULL,
        old_signature NVARCHAR(MAX) NULL,
        new_signature NVARCHAR(MAX) NULL,
        breaking BIT NOT NULL,
        detail_json NVARCHAR(MAX) NULL
    );

### `doc_page_diff`

    CREATE TABLE doc_page_diff (
        id UNIQUEIDENTIFIER PRIMARY KEY,
        snapshot_diff_id UNIQUEIDENTIFIER NOT NULL,
        doc_uid NVARCHAR(400) NOT NULL,
        change_kind NVARCHAR(50) NOT NULL,
        detail_json NVARCHAR(MAX) NULL
    );

* * *

🔷 3. Diff Mapper — Pipeline Logic
==================================

Your Diff Mapper agent should:

1. Load the **old** and **new** snapshots from DB (or from the ingestion JSON).
2. Compare:
   * `api_type` rows by `type_uid`
   * `api_member` rows by `member_uid`
   * `doc_page` rows by `doc_uid`
3. Emit a `DiffArtifact`.
4. Persist it into the SQL Server tables above.

This is deterministic and reproducible.

* * *

🔷 4. Truth Synthesizer — Next Layer
====================================

Now that diffs exist, the Truth Synthesizer can build a semantic model of SK.

Here are the DTOs for the Truth Synthesizer artifact.

* * *

🔷 5. Truth Synthesizer — C# DTOs (Artifact Contract)
=====================================================

    public sealed class TruthArtifact
    {
        public TruthRunInfo TruthRun { get; set; } = default!;
        public List<FeatureInfo> Features { get; set; } = new();
    }
    
    public sealed class TruthRunInfo
    {
        public string TruthId { get; set; } = default!;
        public DateTime TimestampUtc { get; set; }
        public string SnapshotId { get; set; } = default!;
        public string SchemaVersion { get; set; } = "1.0.0";
    }

### Feature model

    public sealed class FeatureInfo
    {
        public string FeatureId { get; set; } = default!; // "feature:kernel-building"
        public string Name { get; set; } = default!;
        public string Language { get; set; } = default!;
        public string Description { get; set; } = default!;
        public List<string> PrimaryTypes { get; set; } = new();
        public List<string> RelatedTypes { get; set; } = new();
        public List<string> PrimaryMembers { get; set; } = new();
        public List<FeatureDocRef> DocRefs { get; set; } = new();
        public List<string> Tags { get; set; } = new();
        public string IntroducedInVersion { get; set; } = default!;
        public string LastSeenVersion { get; set; } = default!;
    }
    
    public sealed class FeatureDocRef
    {
        public string DocUid { get; set; } = default!;
        public string SectionUid { get; set; } = default!;
    }

* * *

🔷 6. Truth Synthesizer — SQL Server Schema
===========================================

### `truth_run`

    CREATE TABLE truth_run (
        id UNIQUEIDENTIFIER PRIMARY KEY,
        snapshot_id UNIQUEIDENTIFIER NOT NULL,
        timestamp_utc DATETIME2 NOT NULL,
        schema_version NVARCHAR(50) NOT NULL
    );

### `feature`

    CREATE TABLE feature (
        id UNIQUEIDENTIFIER PRIMARY KEY,
        truth_run_id UNIQUEIDENTIFIER NOT NULL,
        feature_uid NVARCHAR(400) NOT NULL,
        name NVARCHAR(200) NOT NULL,
        language NVARCHAR(50) NOT NULL,
        description NVARCHAR(MAX) NOT NULL,
        tags NVARCHAR(MAX) NULL,
        introduced_in_version NVARCHAR(50) NOT NULL,
        last_seen_version NVARCHAR(50) NOT NULL
    );

### `feature_type_link`

    CREATE TABLE feature_type_link (
        id UNIQUEIDENTIFIER PRIMARY KEY,
        feature_id UNIQUEIDENTIFIER NOT NULL,
        type_uid NVARCHAR(400) NOT NULL,
        role NVARCHAR(50) NOT NULL
    );

### `feature_member_link`

    CREATE TABLE feature_member_link (
        id UNIQUEIDENTIFIER PRIMARY KEY,
        feature_id UNIQUEIDENTIFIER NOT NULL,
        member_uid NVARCHAR(400) NOT NULL,
        role NVARCHAR(50) NOT NULL
    );

### `feature_doc_link`

    CREATE TABLE feature_doc_link (
        id UNIQUEIDENTIFIER PRIMARY KEY,
        feature_id UNIQUEIDENTIFIER NOT NULL,
        doc_uid NVARCHAR(400) NOT NULL,
        section_uid NVARCHAR(400) NOT NULL
    );

* * *

🔷 7. Next Step
===============

If you’re ready, the next layer is:

### **Sample Engineer**

* DTOs for generated samples
* SQL Server schema
* How samples link to features, API members, and code blocks

After that:

### **Executor & Verifier**

* Execution logs
* Build/run results
* Exception capture

Then:

### **Adversarial Reviewer**

* Dispute flags
* Issue codes
* Severity levels

Finally:

### **RAG Librarian**

* Chunk schema
* Vector column definition (`VECTOR(1536)` or whatever your embedding size is)
* Indexing strategy (HNSW or IVF)
* Retrieval metadata

If you want, I can continue with the Sample Engineer layer next.
