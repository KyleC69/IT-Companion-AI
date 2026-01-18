

**Designing Multi-Agent System for Semantic Kernel**



* * *

### Agent overview

| Agent                    | Core role                                        | Main artifact                                        |
| ------------------------ | ------------------------------------------------ | ---------------------------------------------------- |
| **Spec Harvester**       | Pulls raw SK API & docs from source              | API snapshots, doc blobs                             |
| **Diff Mapper**          | Compares versions, flags API surface changes     | Change sets, migration notes                         |
| **Truth Synthesizer**    | Builds canonical SK API knowledge graph          | Structured SK schema (types, methods, relationships) |
| **Sample Engineer**      | Generates code samples & usage patterns          | Sample code artifacts                                |
| **Executor & Verifier**  | Compiles/runs samples, validates behavior        | Execution logs, pass/fail marks                      |
| **Adversarial Reviewer** | Attacks claims & samples, hunts inconsistencies  | Review reports, “disputed” tags                      |
| **RAG Librarian**        | Structures & indexes all artifacts for retrieval | Chunked, versioned RAG corpus                        |

This is not a “chat with docs” setup; it’s closer to a small, specialized research lab.

* * *

1. Overall architecture

-----------------------

#### Pipeline shape

1. **Ingestion layer:** Spec Harvester + Diff Mapper
   
   * Watches Semantic Kernel repos, docs, NuGet packages, releases.
   * Produces versioned snapshots and diffs.

2. **Understanding layer:** Truth Synthesizer
   
   * Converts raw snapshots into a canonical SK schema: APIs, options, extension points, lifecycle, examples.

3. **Execution layer:** Sample Engineer + Executor & Verifier
   
   * Synthesizes minimal, focused samples for each important API and pattern.
   * Compiles/runs them in a harness to verify they match the current SK behavior.

4. **Adversarial layer:** Adversarial Reviewer
   
   * Cross-examines docs, code, and observed behavior.
   * Marks anything that is inferred, ambiguous, or contradicted by runtime evidence.

5. **Serving layer:** RAG Librarian
   
   * Chunks and indexes the _artifacts_ (schema, samples, diffs, reviews, execution logs), not raw text only.
   * Exposes a retrieval view tailored for: “Give me SK answers with evidence and version tags.”

Control-wise, you can do this with a simple orchestrator rather than a free-form planner, but each agent still uses natural-language prompts plus structured inputs/outputs.

* * *

2. Spec Harvester and Diff Mapper

---------------------------------

#### Spec Harvester (Source-of-truth agent)

**Mandate:** “Only trust what you can fetch from SK source-of-truth.”

* **Inputs:** Repo URLs, doc URLs, NuGet metadata, release notes.
* **Tasks:**
  * Enumerate repos (C#, Python, JS SK, etc.).
  * Pull API signatures via Roslyn-like analysis or simple AST reflection (even via pre-generated docs or `dotnet` tools).
  * Pull markdown/website docs, samples, configuration guides.
* **Output:**
  * Versioned JSON snapshot per language: types, methods, overloads, parameters, attributes, comments, doc links.

This agent does not “interpret.” It extracts, normalizes, and timestamps.

#### Diff Mapper (Change-focused agent)

**Mandate:** “Focus on what changed and how it affects users.”

* **Inputs:** Old snapshot, new snapshot.
* **Tasks:**
  * Detect adds/removes/renames, signature changes, breaking vs non-breaking changes.
  * Identify doc sections not updated despite API change.
  * Produce migration hints (“`KernelConfig` is now `KernelBuilder`,” “planner X deprecated in favor of Y”).
* **Output:**
  * Machine-readable change set + human-readable migration notes.

This is your version-awareness backbone: the RAG system can always answer “this is correct for SK vX.Y; in vX.Z it changed like this.”

* * *

3. Truth Synthesizer (canonical model of SK)

--------------------------------------------

**Mandate:** “Build a coherent, minimal representation of SK, grounded in the snapshots and diffs.”

* **Inputs:** Latest snapshot, diff change sets, selected doc text.
* **Tasks:**
  * Normalize disparate data (code signatures + docs) into one schema:
    * Kernels, plugins, planners, memories, connectors, agents, pipelines, config objects.
  * Capture relationships:
    * “`KernelBuilder` composes plugins and config; planners consume skills; handlers are bound to tools.”
  * Annotate each entity with:
    * Version-introduced, last-seen, sample references, known pitfalls.
* **Output:**
  * A structured SK knowledge graph or JSON schema that feeds both:
    * Sample Engineer, and
    * RAG Librarian (as higher-level chunks).

You already think this way (handlers, planners, metadata); the Truth Synthesizer is basically your SK meta-model builder agent.

* * *

4. Sample Engineer and Executor & Verifier

------------------------------------------

#### Sample Engineer (contract-focused generator)

**Mandate:** “For every important API and pattern, produce minimal, compilable, idiomatic samples that demonstrate the intended contract.”

* **Inputs:**
  * SK schema node (e.g., `KernelBuilder`, `ChatCompletionAgent`, `ToolCallHandler`).
  * Known behavior constraints (“must compile on .NET X”, “target C# SK vX.Y”).
* **Tasks:**
  * Generate small, focused samples: one responsibility per sample.
  * Include assertions or observable outputs where possible (logging, simple console checks).
  * Reference the exact package versions and using statements.
* **Output:**
  * Sample artifacts (file content + metadata):
    * Target version, expected behavior, dependencies, tags (“multi-agent”, “RAG”, “planners”, etc.).

#### Executor & Verifier (reality enforcer)

**Mandate:** “Believe the runtime over the model.”

* **Inputs:** Sample artifacts.
* **Tasks:**
  * Compile samples in a controlled environment (e.g., `dotnet build` in Docker or temp directory).
  * Run them if safe (non-destructive) with controlled inputs.
  * Capture: build output, runtime logs, exceptions, behavior.
* **Output:**
  * Execution report: success/failure, stack traces, captured output, runtime configuration.
  * Binary flag and structured error classification.

Your RAG layer should _never_ treat a sample as “trusted” unless this agent has assigned a “verified” status.

* * *

5. Adversarial Reviewer

-----------------------

**Mandate:** “Assume the other agents are wrong until proven otherwise.”

* **Inputs:**
  * SK schema, docs, spec snapshots.
  * Samples and their execution reports.
* **Tasks:**
  * Look for inconsistencies:
    * Doc says parameter optional; code requires it.
    * Sample uses deprecated patterns.
    * Behavior differs from description (e.g., planner using unexpected tools).
  * Perform “what could be misleading?” analysis:
    * Incorrect default assumptions.
    * Non-obvious coupling (e.g., config objects that silently change behavior).
  * Flag content with risk tags: “deprecated,” “ambiguous,” “unverified inference,” “contradicted-by-runtime.”
* **Output:**
  * Review annotations attached to both docs and samples.
  * Lists of “needs re-synthesis” items for Truth Synthesizer and Sample Engineer.

This embodies your adversarial, contract-driven philosophy inside the system. You essentially have an agent that encodes “Kyle’s skepticism.”

* * *

6. RAG Librarian (how answers get assembled)

--------------------------------------------

**Mandate:** “Serve only grounded, version-aware, evidence-backed SK knowledge.”

* **Inputs:**
  * SK schema, samples, diffs, docs, review tags, execution reports.
* **Tasks:**
  * Chunk artifacts by semantics, not raw size:
    * API-level chunks (one type or feature per unit).
    * Sample-plus-execution chunks (code + outcome).
    * Diff chunks (what changed for a given feature).
  * Index with metadata:
    * SK version, language (C#, Python), status (verified, unverified, disputed), last-checked timestamp.
  * At query time:
    * Retrieve artifacts; filter by version; prefer verified, non-disputed chunks.
    * Provide both the explanation and the supporting artifacts (links to sample, logs, diffs).
* **Output:**
  * A retrieval-first view that any answering agent (or you) can use to build SK explanations with citations, version notes, and known caveats.

* * *

7. Coordination and failure modes

---------------------------------

Rather than a “smart planner that calls agents,” use a **deterministic pipeline** plus a small coordinator:

* **Coordinator responsibilities:**
  
  * Trigger full or partial runs (e.g., when SK publishes a new version).
  * Track which schema nodes have verified samples and which are pending.
  * Route “disputed” items back through the cycle (re-synthesize sample → re-run → re-review).

* **Failure handling:**
  
  * If Executor & Verifier fails, that sample is marked “invalid” and downgraded in RAG.
  * If Adversarial Reviewer flags a doc as ambiguous, RAG includes a warning whenever that chunk is retrieved.
  * If Diff Mapper flags a breaking change, older samples get tagged as historical, not recommended.

* * *

8. How to start concretely (minimal vertical slice)

---------------------------------------------------

To avoid boiling the ocean, start with one SK feature you care about deeply (e.g., “multi-agent orchestration in SK C#”):

1. **Limit scope:** Agents, planners, handlers, and RAG connectors for C# SK only.
2. **Implement Spec Harvester + Diff Mapper** for those namespaces.
3. **Build a minimal SK schema** for just those types.
4. **Have Sample Engineer generate 5–10 targeted samples** around your real use cases.
5. **Wire Executor & Verifier** to compile and run them locally.
6. **Use Adversarial Reviewer** to annotate what’s misleading or fragile.
7. **Index everything with RAG Librarian** and use that as your own SK assistant.

Once that’s solid, extend horizontally to other SK areas (memories, connectors) and vertically to other languages.

If you want, next step we can design the exact JSON contracts between two of these agents (say Sample Engineer ↔ Executor & Verifier) so your pipeline is as artifact-grade as your ONNX work.
