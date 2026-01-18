Version:1.0StartHTML:00000097EndHTML:00007965StartFragment:00000153EndFragment:00007932

Perfect — let’s continue the full-stack design with the next major layer:
⭐ **Sample Engineer Layer**
===========================

This is where your system begins to _generate executable, version‑aware, feature‑aligned code artifacts_ that later agents will compile, run, verify, and adversarially review.

This layer is the bridge between **semantic truth** (features) and **runtime truth** (execution).  
It must be deterministic, reproducible, and fully grounded in the ingestion + truth layers.

Below is the complete design: DTOs, SQL Server schema, and pipeline logic.

* * *

🔷 1. Sample Engineer — Purpose & Responsibilities
==================================================

The Sample Engineer agent:

* Reads:
  * `feature` records (semantic SK features)
  * `ApiMember` records (actual API surface)
  * `code_block` records (docs)
* Generates:
  * Minimal, idiomatic, compilable C# samples
  * One sample per feature, or multiple samples per feature if needed
  * Samples that explicitly reference the SK version from the snapshot
  * Samples that include package references, using statements, and entry points
* Emits:
  * A **Sample Artifact JSON**
  * Which is then persisted into SQL Server

These samples are the _canonical examples_ that the Executor & Verifier will test.

* * *

🔷 2. Sample Artifact — C# DTOs (Contract)
==========================================

This is the JSON your Sample Engineer agent emits.
    public sealed class SampleArtifact
    {
        public SampleRunInfo SampleRun { get; set; } = default!;
        public List<SampleInfo> Samples { get; set; } = new();
    }

    public sealed class SampleRunInfo
    {
        public string SampleRunId { get; set; } = default!;
        public DateTime TimestampUtc { get; set; }
        public string SnapshotId { get; set; } = default!;
        public string SchemaVersion { get; set; } = "1.0.0";
    }

### Sample definition

    public sealed class SampleInfo
    {
        public string SampleId { get; set; } = default!; // "sample:csharp:kernel-builder:minimal"
        public string FeatureId { get; set; } = default!;
        public string Language { get; set; } = "csharp";
        public string Code { get; set; } = default!;
        public string EntryPoint { get; set; } = "Program.cs";
        public string TargetFramework { get; set; } = "net8.0";
    
        public List<PackageReferenceInfo> PackageReferences { get; set; } = new();
        public List<string> RelatedApiMembers { get; set; } = new();
        public string? DerivedFromCodeUid { get; set; }
        public List<string> Tags { get; set; } = new();
    }

### Package reference

    public sealed class PackageReferenceInfo
    {
        public string Name { get; set; } = default!;
        public string Version { get; set; } = default!;
    }

* * *

🔷 3. SQL Server Schema — Sample Layer
======================================

### `sample_run`

    CREATE TABLE sample_run (
        id UNIQUEIDENTIFIER PRIMARY KEY,
        snapshot_id UNIQUEIDENTIFIER NOT NULL,
        timestamp_utc DATETIME2 NOT NULL,
        schema_version NVARCHAR(50) NOT NULL
    );

### `sample`

    CREATE TABLE sample (
        id UNIQUEIDENTIFIER PRIMARY KEY,
        sample_run_id UNIQUEIDENTIFIER NOT NULL,
        sample_uid NVARCHAR(400) NOT NULL,
        feature_uid NVARCHAR(400) NOT NULL,
        language NVARCHAR(50) NOT NULL,
        code NVARCHAR(MAX) NOT NULL,
        entry_point NVARCHAR(200) NOT NULL,
        target_framework NVARCHAR(50) NOT NULL,
        package_references_json NVARCHAR(MAX) NOT NULL,
        derived_from_code_uid NVARCHAR(400) NULL,
        tags NVARCHAR(MAX) NULL
    );

### `sample_ApiMember_link`

    CREATE TABLE sample_ApiMember_link (
        id UNIQUEIDENTIFIER PRIMARY KEY,
        sample_id UNIQUEIDENTIFIER NOT NULL,
        member_uid NVARCHAR(400) NOT NULL
    );

* * *

🔷 4. Sample Engineer — Pipeline Logic
======================================

The Sample Engineer agent performs:

### **Step 1 — Read semantic features**

From `feature` table:

* `feature_uid`
* primary types
* primary members
* doc references
* tags

### **Step 2 — Read API members**

From `ApiMember` table:

* signatures
* parameters
* return types
* attributes

### **Step 3 — Read doc code blocks**

From `code_block` table:

* content
* declared packages
* tags

### **Step 4 — Synthesize minimal samples**

Rules:

* Must compile under the SK version in the snapshot
* Must use the _current_ API surface (no deprecated members unless explicitly tagged)
* Must include:
  * `using` statements
  * package references
  * a minimal `Main` method
  * deterministic behavior (no randomness, no external calls)
* Must be **single-file** unless the feature requires multiple files

### **Step 5 — Emit Sample Artifact JSON**

One artifact per run.

### **Step 6 — Persist into SQL Server**

Using the schema above.

* * *

🔷 5. Example Sample Artifact (Realistic)
=========================================

    {
      "sampleRun": {
        "sampleRunId": "01JH8Z8X9Z5T3C2Y8M9K4WQ9B7",
        "timestampUtc": "2025-01-02T13:00:00Z",
        "snapshotId": "01JH8Z8X9Z5T3C2Y8M9K4WQ9B6",
        "schemaVersion": "1.0.0"
      },
      "samples": [
        {
          "sampleId": "sample:csharp:kernel-builder:minimal",
          "featureId": "feature:kernel-building",
          "language": "csharp",
          "entryPoint": "Program.cs",
          "targetFramework": "net8.0",
          "code": "using Microsoft.SemanticKernel;\n\nvar builder = Kernel.CreateBuilder();\nbuilder.AddOpenAIChatCompletion(\"gpt-4o-mini\", \"YOUR_KEY\");\nvar kernel = builder.Build();\n\nConsole.WriteLine(\"Kernel built successfully.\");",
          "packageReferences": [
            {
              "name": "Microsoft.SemanticKernel",
              "version": "1.23.4"
            }
          ],
          "relatedApiMembers": [
            "csharp:Microsoft.SemanticKernel.KernelBuilder::Build()"
          ],
          "derivedFromCodeUid": "code:kernel-builder:minimal-example",
          "tags": [ "kernel", "builder", "minimal" ]
        }
      ]
    }

* * *

🔷 6. Next Layer: Executor & Verifier
=====================================

Now that samples exist, the next agent:

* Compiles each sample
* Runs it in a controlled environment
* Captures:
  * build logs
  * runtime logs
  * exceptions
  * exit codes
  * duration
* Emits an **Execution Artifact JSON**
* Persists into SQL Server

If you’re ready, I’ll continue with:
⭐ **Executor & Verifier Layer**
===============================

DTOs, SQL schema, and pipeline logic.

Just say **continue**.
