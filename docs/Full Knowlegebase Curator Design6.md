Version:1.0StartHTML:00000097EndHTML:00008334StartFragment:00000153EndFragment:00008301

Excellent — now we move into the **Executor & Verifier layer**, which is the first point in your pipeline where _reality_ is enforced. This is the layer that prevents hallucinated samples, incorrect assumptions about SK APIs, or drift between documentation and actual behavior.

This is also the layer that gives your RAG system **ground truth**: if a sample compiles and runs successfully against the exact SK version in the snapshot, it becomes a “verified” artifact. If not, it becomes a “disputed” or “invalid” artifact for the Reviewer to analyze.

Let’s design this layer with the same rigor as the ingestion and sample layers.

* * *

⭐ 1. Executor & Verifier — Purpose
==================================

This agent:

* Takes each generated sample from the Sample Engineer
* Creates a temporary build environment (e.g., .NET SDK container or local sandbox)
* Writes the sample code to disk
* Restores packages for the exact SK version in the snapshot
* Compiles the sample
* Runs the sample
* Captures:
  * Build logs
  * Runtime logs
  * Exceptions
  * Exit codes
  * Duration
* Emits a **Execution Artifact JSON**
* Persists results into SQL Server

This is the “truth anchor” of your entire system.

* * *

⭐ 2. Execution Artifact — C# DTOs (Contract)
============================================

This is the JSON your Executor & Verifier agent emits.
    public sealed class ExecutionArtifact
    {
        public ExecutionRunInfo ExecutionRun { get; set; } = default!;
        public List<ExecutionResultInfo> Results { get; set; } = new();
    }

    public sealed class ExecutionRunInfo
    {
        public string ExecutionRunId { get; set; } = default!;
        public DateTime TimestampUtc { get; set; }
        public string SnapshotId { get; set; } = default!;
        public string SampleRunId { get; set; } = default!;
        public ExecutionEnvironmentInfo Environment { get; set; } = default!;
        public string SchemaVersion { get; set; } = "1.0.0";
    }

    public sealed class ExecutionEnvironmentInfo
    {
        public string OS { get; set; } = default!;
        public string DotnetVersion { get; set; } = default!;
        public string SkPackageVersion { get; set; } = default!;
    }

### Execution result

    public sealed class ExecutionResultInfo
    {
        public string SampleId { get; set; } = default!;
        public string Status { get; set; } = default!; // passed | failed | build_failed | skipped
        public string BuildLog { get; set; } = default!;
        public string RunLog { get; set; } = default!;
        public ExecutionExceptionInfo? Exception { get; set; }
        public int DurationMs { get; set; }
    }

### Exception details

    public sealed class ExecutionExceptionInfo
    {
        public string Type { get; set; } = default!;
        public string Message { get; set; } = default!;
        public string? StackTrace { get; set; }
    }

* * *

⭐ 3. SQL Server Schema — Execution Layer
========================================

### `execution_run`

    CREATE TABLE execution_run (
        id UNIQUEIDENTIFIER PRIMARY KEY,
        snapshot_id UNIQUEIDENTIFIER NOT NULL,
        sample_run_id UNIQUEIDENTIFIER NOT NULL,
        timestamp_utc DATETIME2 NOT NULL,
        environment_json NVARCHAR(MAX) NOT NULL,
        schema_version NVARCHAR(50) NOT NULL
    );

### `execution_result`

    CREATE TABLE execution_result (
        id UNIQUEIDENTIFIER PRIMARY KEY,
        execution_run_id UNIQUEIDENTIFIER NOT NULL,
        sample_uid NVARCHAR(400) NOT NULL,
        status NVARCHAR(50) NOT NULL,
        build_log NVARCHAR(MAX) NULL,
        run_log NVARCHAR(MAX) NULL,
        exception_json NVARCHAR(MAX) NULL,
        duration_ms INT NOT NULL
    );

* * *

⭐ 4. Executor & Verifier — Pipeline Logic
=========================================

This is the deterministic process your agent follows.

* * *

**Step 1 — Load samples**
-------------------------

Query:
    SELECT * FROM sample WHERE sample_run_id = @SampleRunId

* * *

**Step 2 — Create isolated build environment**
----------------------------------------------

Options:

* Docker container with .NET SDK
* Local temp directory with restricted permissions
* Windows sandbox

The environment must be:

* Clean
* Deterministic
* Versioned

* * *

**Step 3 — Write sample code to disk**
--------------------------------------

For each sample:
    /tmp/sk-samples/{sampleId}/Program.cs
    /tmp/sk-samples/{sampleId}/sk-sample.csproj

The `.csproj` must include:
    <ItemGroup>
      <PackageReference Include="Microsoft.SemanticKernel" Version="{snapshot.PackageVersion}" />
    </ItemGroup>

* * *

**Step 4 — Restore packages**
-----------------------------

Run:
    dotnet restore

Capture stdout/stderr.

If restore fails → `status = build_failed`.

* * *

**Step 5 — Build**
------------------

Run:
    dotnet build --configuration Release

Capture logs.

If build fails → `status = build_failed`.

* * *

**Step 6 — Run**
----------------

Run:
    dotnet run --configuration Release

Capture:

* stdout
* stderr
* exit code
* duration

If exit code != 0 → `status = failed`.

If success → `status = passed`.

* * *

**Step 7 — Emit Execution Artifact JSON**
-----------------------------------------

One artifact per run.

* * *

**Step 8 — Persist into SQL Server**
------------------------------------

Insert into:

* `execution_run`
* `execution_result`

* * *

⭐ 5. Example Execution Artifact (Realistic)
===========================================

    {
      "executionRun": {
        "executionRunId": "01JH8Z8X9Z5T3C2Y8M9K4WQ9C1",
        "timestampUtc": "2025-01-02T14:00:00Z",
        "snapshotId": "01JH8Z8X9Z5T3C2Y8M9K4WQ9B6",
        "sampleRunId": "01JH8Z8X9Z5T3C2Y8M9K4WQ9B7",
        "environment": {
          "os": "linux",
          "dotnetVersion": "8.0.100",
          "skPackageVersion": "1.23.4"
        },
        "schemaVersion": "1.0.0"
      },
      "results": [
        {
          "sampleId": "sample:csharp:kernel-builder:minimal",
          "status": "passed",
          "buildLog": "Build succeeded.",
          "runLog": "Kernel built successfully.",
          "exception": null,
          "durationMs": 1234
        }
      ]
    }

* * *

⭐ 6. Why this layer is essential
================================

This is the layer that:

* Prevents hallucinated SK usage
* Detects breaking changes in SK versions
* Validates that samples match the real API surface
* Produces evidence for the Reviewer
* Feeds verified artifacts into RAG

This is the **reality anchor** of your entire system.

* * *

⭐ 7. Next Layer: Adversarial Reviewer
=====================================

Now that we have:

* Ingestion
* Diff
* Truth
* Samples
* Execution

…the next agent is the **Adversarial Reviewer**, which:

* Cross-examines all artifacts
* Flags inconsistencies
* Marks samples as disputed, risky, deprecated, or approved
* Produces issue codes and severity levels
* Feeds metadata into RAG

If you’re ready, I’ll continue with:
🔥 **Adversarial Reviewer Layer**
=================================

DTOs, SQL schema, and pipeline logic.

Just say **continue**.
