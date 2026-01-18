

* * *

1. Global principles and shared identifiers

-------------------------------------------

These apply to **all layers**:

* **Global IDs:**
  * **`run_id`** – a single ingestion run (per repo/language/commit/version).
  * **`snapshot_id`** – a specific source snapshot (e.g., C# SK at commit X).
  * **`type_uid`** – language-prefixed type ID (e.g., `csharp:Microsoft.SemanticKernel.KernelBuilder`).
  * **`member_uid`** – language-prefixed member ID (e.g., `csharp:Microsoft.SemanticKernel.KernelBuilder::Build()`).
  * **`doc_uid`** – document page ID (e.g., `doc:csharp:getting-started-kernel-builder`).
  * **`section_uid`** – section anchor ID.
  * **`code_uid`** – code block ID (e.g., `code:kernel-builder:minimal-example`).
* **Immutability:**
  * Snapshots, diffs, execution results, and reviews are **append-only**. Never mutate history.
* **Agents communicate via artifacts only:**
  * Each agent reads/writes JSON artifacts persisted to DB.
  * No agent trusts another’s “reasoning,” only its artifacts.

* * *

2. Ingestion layer (Spec Harvester)

-----------------------------------

### 2.1 Ingestion artifact JSON

One JSON per ingestion run (per language/source):
    {
      "ingestionRun": {
        "runId": "uuid-or-ulid",
        "timestampUtc": "2025-01-01T12:34:56Z",
        "schemaVersion": "1.0.0",
        "sourceSnapshot": {
          "snapshotId": "uuid-or-ulid",
          "repoUrl": "https://github.com/microsoft/semantic-kernel",
          "branch": "main",
          "commit": "abcdef1234567890",
          "language": "csharp",
          "packageName": "Microsoft.SemanticKernel",
          "packageVersion": "1.23.4",
          "config": {
            "includePrivate": false,
            "docPaths": [ "docs/", "samples/" ],
            "apiRoots": [ "dotnet/src/", "dotnet/samples/" ]
          }
        }
      },
      "apiSurface": {
        "types": [
          {
            "typeUid": "csharp:Microsoft.SemanticKernel.KernelBuilder",
            "name": "KernelBuilder",
            "namespace": "Microsoft.SemanticKernel",
            "kind": "class",
            "accessibility": "public",
            "isStatic": false,
            "isGeneric": false,
            "genericParameters": [],
            "summary": "Short description, if available.",
            "remarks": null,
            "attributes": [
              {
                "name": "Obsolete",
                "ctorArguments": [ "Use NewKernelBuilder instead." ]
              }
            ],
            "members": [
              {
                "memberUid": "csharp:Microsoft.SemanticKernel.KernelBuilder::Build()",
                "name": "Build",
                "kind": "method",
                "accessibility": "public",
                "isStatic": false,
                "isExtensionMethod": false,
                "isAsync": false,
                "returnType": "Microsoft.SemanticKernel.Kernel",
                "summary": "Builds a kernel instance.",
                "remarks": null,
                "parameters": [
                  {
                    "name": "cancellationToken",
                    "type": "System.Threading.CancellationToken",
                    "position": 0,
                    "hasDefaultValue": true,
                    "defaultValueLiteral": "default"
                  }
                ],
                "genericParameters": [],
                "attributes": [],
                "sourceLocation": {
                  "filePath": "dotnet/src/SemanticKernel/KernelBuilder.cs",
                  "startLine": 42,
                  "endLine": 88
                },
                "docLinks": [
                  {
                    "docUid": "doc:csharp:getting-started-kernel-builder",
                    "sectionUid": "sec:kernel-builder:build-method"
                  }
                ]
              }
            ]
          }
        ]
      },
      "docs": {
        "pages": [
          {
            "docUid": "doc:csharp:getting-started-kernel-builder",
            "sourcePath": "docs/getting-started/kernel-builder.md",
            "title": "Getting Started with KernelBuilder",
            "language": "csharp",
            "url": "https://learn.microsoft.com/.../kernel-builder",
            "rawMarkdown": "Full markdown here if you want.",
            "sections": [
              {
                "sectionUid": "sec:kernel-builder:overview",
                "heading": "Overview",
                "level": 1,
                "orderIndex": 0,
                "contentMarkdown": "Overview text...",
                "codeBlocks": [
                  {
                    "codeUid": "code:kernel-builder:minimal-example",
                    "language": "csharp",
                    "content": "var builder = Kernel.CreateBuilder();\n...",
                    "declaredPackages": [
                      {
                        "name": "Microsoft.SemanticKernel",
                        "version": "1.23.4"
                      }
                    ],
                    "tags": [ "kernel", "builder", "getting-started" ],
                    "inlineComments": null
                  }
                ]
              }
            ]
          }
        ]
      }
    }

### 2.2 Ingestion DB schema

**Core:**

* **`ingestion_run`**
  
  * **`id`** (PK, UUID)
  * **`timestamp_utc`** (timestamptz)
  * **`schema_version`** (text)
  * **`notes`** (text)

* **`source_snapshot`**
  
  * **`id`** (PK, UUID)
  * **`ingestion_run_id`** (FK → `ingestion_run.id`)
  * **`snapshot_uid`** (text, unique) // equals `snapshotId`
  * **`repo_url`** (text)
  * **`branch`** (text)
  * **`commit`** (text)
  * **`language`** (text)
  * **`package_name`** (text)
  * **`package_version`** (text)
  * **`config_json`** (jsonb)

**API:**

* **`ApiType`**
  
  * **`id`** (PK, UUID)
  * **`source_snapshot_id`** (FK)
  * **`type_uid`** (text, unique per snapshot)
  * **`name`** (text)
  * **`namespace`** (text)
  * **`kind`** (text)
  * **`accessibility`** (text)
  * **`is_static`** (bool)
  * **`is_generic`** (bool)
  * **`generic_parameters`** (jsonb)
  * **`summary`** (text)
  * **`remarks`** (text)
  * **`attributes`** (jsonb)

* **`ApiMember`**
  
  * **`id`** (PK, UUID)
  * **`ApiType_id`** (FK → `ApiType.id`)
  * **`member_uid`** (text, unique per snapshot)
  * **`name`** (text)
  * **`kind`** (text)
  * **`accessibility`** (text)
  * **`is_static`** (bool)
  * **`is_extension_method`** (bool)
  * **`is_async`** (bool)
  * **`return_type`** (text)
  * **`summary`** (text)
  * **`remarks`** (text)
  * **`generic_parameters`** (jsonb)
  * **`attributes`** (jsonb)
  * **`source_file_path`** (text)
  * **`source_start_line`** (int)
  * **`source_end_line`** (int)

* **`ApiParameter`**
  
  * **`id`** (PK, UUID)
  * **`ApiMember_id`** (FK)
  * **`name`** (text)
  * **`type`** (text)
  * **`position`** (int)
  * **`has_default_value`** (bool)
  * **`default_value_literal`** (text)

* **`ApiMember_doc_link`**
  
  * **`id`** (PK, UUID)
  * **`ApiMember_id`** (FK)
  * **`doc_uid`** (text)
  * **`section_uid`** (text)

**Docs & code:**

* **`doc_page`**
  
  * **`id`** (PK, UUID)
  * **`source_snapshot_id`** (FK)
  * **`doc_uid`** (text, unique per snapshot)
  * **`source_path`** (text)
  * **`title`** (text)
  * **`language`** (text)
  * **`url`** (text)
  * **`raw_markdown`** (text)

* **`doc_section`**
  
  * **`id`** (PK, UUID)
  * **`doc_page_id`** (FK)
  * **`section_uid`** (text)
  * **`heading`** (text)
  * **`level`** (int)
  * **`content_markdown`** (text)
  * **`order_index`** (int)

* **`code_block`**
  
  * **`id`** (PK, UUID)
  * **`doc_section_id`** (FK)
  * **`code_uid`** (text)
  * **`language`** (text)
  * **`content`** (text)
  * **`declared_packages`** (jsonb)
  * **`tags`** (text[])
  * **`inline_comments`** (text)

* * *

3. Diff layer (Diff Mapper)

---------------------------

### 3.1 Diff artifact JSON

Input: `oldSnapshotId`, `newSnapshotId`.  
Output: change set keyed by `type_uid` / `member_uid`.
    {
      "diffRun": {
        "diffId": "uuid-or-ulid",
        "timestampUtc": "2025-01-02T10:00:00Z",
        "oldSnapshotId": "old-snapshot-uuid",
        "newSnapshotId": "new-snapshot-uuid",
        "schemaVersion": "1.0.0"
      },
      "typeChanges": [
        {
          "typeUid": "csharp:Microsoft.SemanticKernel.KernelBuilder",
          "changeKind": "modified", // added | removed | modified
          "oldPresence": true,
          "newPresence": true,
          "detail": {
            "nameChanged": false,
            "namespaceChanged": false,
            "attributesChanged": true,
            "summaryChanged": true
          }
        }
      ],
      "memberChanges": [
        {
          "memberUid": "csharp:Microsoft.SemanticKernel.KernelBuilder::Build()",
          "changeKind": "modified", // added | removed | modified
          "oldSignature": "Kernel Build(CancellationToken cancellationToken = default)",
          "newSignature": "Kernel Build(CancellationToken cancellationToken = default, KernelOptions options = null)",
          "breaking": true,
          "detail": {
            "parametersAdded": [ "options" ],
            "parametersRemoved": [],
            "parametersTypeChanged": [],
            "returnTypeChanged": false,
            "attributeChanges": [
              {
                "changeKind": "added",
                "attributeName": "Experimental"
              }
            ]
          }
        }
      ],
      "docChanges": [
        {
          "docUid": "doc:csharp:getting-started-kernel-builder",
          "changeKind": "modified",
          "urlChanged": false,
          "titleChanged": false
        }
      ]
    }

### 3.2 Diff DB schema

* **`snapshot_diff`**
  
  * **`id`** (PK, UUID)
  * **`old_snapshot_id`** (FK → `source_snapshot.id`)
  * **`new_snapshot_id`** (FK → `source_snapshot.id`)
  * **`timestamp_utc`** (timestamptz)
  * **`schema_version`** (text)

* **`ApiType_diff`**
  
  * **`id`** (PK, UUID)
  * **`snapshot_diff_id`** (FK)
  * **`type_uid`** (text)
  * **`change_kind`** (text)
  * **`detail_json`** (jsonb)

* **`ApiMember_diff`**
  
  * **`id`** (PK, UUID)
  * **`snapshot_diff_id`** (FK)
  * **`member_uid`** (text)
  * **`change_kind`** (text)
  * **`old_signature`** (text)
  * **`new_signature`** (text)
  * **`breaking`** (bool)
  * **`detail_json`** (jsonb)

* **`doc_page_diff`**
  
  * **`id`** (PK, UUID)
  * **`snapshot_diff_id`** (FK)
  * **`doc_uid`** (text)
  * **`change_kind`** (text)
  * **`detail_json`** (jsonb)

* * *

4. Truth layer (Truth Synthesizer)

----------------------------------

This agent takes a snapshot + diffs and builds a **canonical SK feature model**, more semantic than raw APIs.

### 4.1 Truth artifact JSON

    {
      "truthRun": {
        "truthId": "uuid-or-ulid",
        "timestampUtc": "2025-01-02T12:00:00Z",
        "snapshotId": "snapshot-uuid",
        "schemaVersion": "1.0.0"
      },
      "features": [
        {
          "featureId": "feature:kernel-building",
          "name": "Kernel building",
          "language": "csharp",
          "primaryTypes": [
            "csharp:Microsoft.SemanticKernel.KernelBuilder"
          ],
          "relatedTypes": [
            "csharp:Microsoft.SemanticKernel.Kernel"
          ],
          "primaryMembers": [
            "csharp:Microsoft.SemanticKernel.KernelBuilder::Build()"
          ],
          "description": "How to construct and configure a Kernel instance.",
          "docRefs": [
            {
              "docUid": "doc:csharp:getting-started-kernel-builder",
              "sectionUid": "sec:kernel-builder:overview"
            }
          ],
          "tags": [ "kernel", "builder", "configuration" ],
          "introducedInVersion": "1.20.0",
          "lastSeenVersion": "1.23.4"
        }
      ]
    }

### 4.2 Truth DB schema

* **`truth_run`**
  
  * **`id`** (PK, UUID)
  * **`snapshot_id`** (FK → `source_snapshot.id`)
  * **`timestamp_utc`** (timestamptz)
  * **`schema_version`** (text)

* **`feature`**
  
  * **`id`** (PK, UUID)
  * **`truth_run_id`** (FK)
  * **`feature_uid`** (text, unique) // e.g., `feature:kernel-building`
  * **`name`** (text)
  * **`language`** (text)
  * **`description`** (text)
  * **`tags`** (text[])
  * **`introduced_in_version`** (text)
  * **`last_seen_version`** (text)

* **`feature_type_link`**
  
  * **`id`** (PK, UUID)
  * **`feature_id`** (FK)
  * **`type_uid`** (text)
  * **`role`** (text) // primary | related

* **`feature_member_link`**
  
  * **`id`** (PK, UUID)
  * **`feature_id`** (FK)
  * **`member_uid`** (text)
  * **`role`** (text) // primary | helper

* **`feature_doc_link`**
  
  * **`id`** (PK, UUID)
  * **`feature_id`** (FK)
  * **`doc_uid`** (text)
  * **`section_uid`** (text)

* * *

5. Samples and execution (Sample Engineer + Executor & Verifier)

----------------------------------------------------------------

### 5.1 Sample artifact JSON (from Sample Engineer)

    {
      "sampleRun": {
        "sampleRunId": "uuid-or-ulid",
        "timestampUtc": "2025-01-02T13:00:00Z",
        "snapshotId": "snapshot-uuid",
        "schemaVersion": "1.0.0"
      },
      "samples": [
        {
          "sampleId": "sample:csharp:kernel-builder:minimal",
          "featureId": "feature:kernel-building",
          "language": "csharp",
          "code": "using Microsoft.SemanticKernel;\n...\nvar builder = Kernel.CreateBuilder();\n...",
          "entryPoint": "Program.cs", // or fully qualified Main if needed
          "targetFramework": "net8.0",
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

### 5.2 Execution artifact JSON (from Executor & Verifier)

    {
      "executionRun": {
        "executionRunId": "uuid-or-ulid",
        "timestampUtc": "2025-01-02T14:00:00Z",
        "snapshotId": "snapshot-uuid",
        "sampleRunId": "sample-run-uuid",
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
          "status": "passed", // passed | failed | build_failed | skipped
          "buildLog": "build stdout/stderr truncated as needed...",
          "runLog": "runtime stdout/stderr...",
          "exception": null,
          "durationMs": 1234
        }
      ]
    }

### 5.3 Sample & execution DB schema

* **`sample_run`**
  
  * **`id`** (PK, UUID)
  * **`snapshot_id`** (FK)
  * **`timestamp_utc`** (timestamptz)
  * **`schema_version`** (text)

* **`sample`**
  
  * **`id`** (PK, UUID)
  * **`sample_run_id`** (FK)
  * **`sample_uid`** (text, unique) // `sampleId`
  * **`feature_uid`** (text) // link to `feature.feature_uid`
  * **`language`** (text)
  * **`code`** (text)
  * **`entry_point`** (text)
  * **`target_framework`** (text)
  * **`package_references`** (jsonb)
  * **`derived_from_code_uid`** (text)
  * **`tags`** (text[])

* **`sample_ApiMember_link`**
  
  * **`id`** (PK, UUID)
  * **`sample_id`** (FK → `sample.id`)
  * **`member_uid`** (text)

* **`execution_run`**
  
  * **`id`** (PK, UUID)
  * **`snapshot_id`** (FK)
  * **`sample_run_id`** (FK)
  * **`timestamp_utc`** (timestamptz)
  * **`environment_json`** (jsonb)
  * **`schema_version`** (text)

* **`execution_result`**
  
  * **`id`** (PK, UUID)
  * **`execution_run_id`** (FK)
  * **`sample_uid`** (text)
  * **`status`** (text)
  * **`build_log`** (text)
  * **`run_log`** (text)
  * **`exception_json`** (jsonb)
  * **`duration_ms`** (int)

* * *

6. Adversarial review layer (Adversarial Reviewer)

--------------------------------------------------

### 6.1 Review artifact JSON

    {
      "reviewRun": {
        "reviewRunId": "uuid-or-ulid",
        "timestampUtc": "2025-01-02T15:00:00Z",
        "snapshotId": "snapshot-uuid",
        "schemaVersion": "1.0.0"
      },
      "reviews": [
        {
          "targetKind": "sample", // sample | feature | doc | ApiMember
          "targetId": "sample:csharp:kernel-builder:minimal",
          "status": "disputed",   // approved | disputed | risky | deprecated
          "summary": "Sample uses a planner API that is marked obsolete.",
          "issues": [
            {
              "code": "USES_DEPRECATED_MEMBER",
              "severity": "warning",
              "relatedMemberUid": "csharp:Microsoft.SemanticKernel.Planners.LegacyPlanner::Plan()",
              "details": "Docs recommend using NewPlanner instead."
            }
          ]
        }
      ]
    }

### 6.2 Review DB schema

* **`review_run`**
  
  * **`id`** (PK, UUID)
  * **`snapshot_id`** (FK)
  * **`timestamp_utc`** (timestamptz)
  * **`schema_version`** (text)

* **`review_item`**
  
  * **`id`** (PK, UUID)
  * **`review_run_id`** (FK)
  * **`target_kind`** (text) // sample | feature | doc | ApiMember
  * **`target_uid`** (text) // `sample_uid`, `feature_uid`, `doc_uid`, `member_uid`
  * **`status`** (text) // approved | disputed | risky | deprecated
  * **`summary`** (text)

* **`review_issue`**
  
  * **`id`** (PK, UUID)
  * **`review_item_id`** (FK)
  * **`code`** (text)
  * **`severity`** (text) // info | warning | error
  * **`related_member_uid`** (text)
  * **`details`** (text)

* * *

7. RAG librarian layer (indexing & retrieval view)

--------------------------------------------------

This is where you shape what your assistant actually sees.

### 7.1 RAG chunk artifact JSON

RAG Librarian reads from all the above and emits **chunk records**:
    {
      "ragRun": {
        "ragRunId": "uuid-or-ulid",
        "timestampUtc": "2025-01-02T16:00:00Z",
        "snapshotId": "snapshot-uuid",
        "schemaVersion": "1.0.0"
      },
      "chunks": [
        {
          "chunkId": "rag:feature:kernel-building:overview",
          "kind": "feature", // feature | sample | ApiMember | doc_section | diff
          "featureId": "feature:kernel-building",
          "text": "Kernel building in Semantic Kernel (C#) allows you to ...",
          "metadata": {
            "language": "csharp",
            "skVersion": "1.23.4",
            "status": "verified", // verified | unverified | disputed
            "tags": [ "kernel", "builder" ],
            "sourceSnapshotId": "snapshot-uuid",
            "sourceDocUid": "doc:csharp:getting-started-kernel-builder",
            "sourceSectionUid": "sec:kernel-builder:overview"
          }
        }
      ]
    }

### 7.2 RAG DB schema

* **`rag_run`**
  
  * **`id`** (PK, UUID)
  * **`snapshot_id`** (FK)
  * **`timestamp_utc`** (timestamptz)
  * **`schema_version`** (text)

* **`rag_chunk`**
  
  * **`id`** (PK, UUID)
  * **`rag_run_id`** (FK)
  * **`chunk_uid`** (text, unique)
  * **`kind`** (text)
  * **`text`** (text)
  * **`metadata_json`** (jsonb)
  * **`embedding_vector`** (vector) // if using pgvector, etc.

* * *

8. How the whole thing flows (no mismatches)

--------------------------------------------

1. **Ingestion agent**
   
   * Writes **Ingestion JSON** → ETL into `ingestion_run`, `source_snapshot`, `api_*`, `doc_*`.

2. **Diff Mapper**
   
   * Chooses two `source_snapshot` rows.
   * Computes diffs → **Diff JSON** → ETL into `snapshot_diff`, `api_*_diff`, `doc_page_diff`.

3. **Truth Synthesizer**
   
   * Reads `api_*`, `doc_*`, optionally diffs.
   * Emits **Truth JSON** → ETL into `truth_run`, `feature_*`.

4. **Sample Engineer**
   
   * Reads `feature_*`, `ApiMember`, `code_block`.
   * Emits **Sample JSON** → ETL into `sample_run`, `sample`, `sample_ApiMember_link`.

5. **Executor & Verifier**
   
   * Reads `sample` records, compiles/runs, emits **Execution JSON** → ETL into `execution_run`, `execution_result`.

6. **Adversarial Reviewer**
   
   * Reads everything above, emits **Review JSON** → ETL into `review_run`, `review_item`, `review_issue`.

7. **RAG Librarian**
   
   * Reads all layers, synthesizes **RAG Chunk JSON** → ETL into `rag_run`, `rag_chunk`, plus embeddings.

Every layer is tied together by **`snapshot_id`**, global UIDs (`type_uid`, `member_uid`, `doc_uid`, `feature_uid`, `sample_uid`), and **never mutates** previous results.

* * *

If you want, next step we can:

* Design one **C# DTO set** for the ingestion JSON and the DB load, or
* Pick a single feature (e.g., C# multi-agent orchestration) and walk through what one full vertical slice looks like in data.
