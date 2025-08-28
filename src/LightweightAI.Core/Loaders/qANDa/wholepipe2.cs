using System;
using System.Collections.Generic;
using System.Linq;

#region Domain Models

public enum ProvImportance { Critical, Important, Verbose }

public record TrainingContext(
    string DatasetRevision,
    string PreprocessingHash,
    string ModelVersion,
    DateTime TrainingDateUtc,
    string[] Hyperparameters,
    string[] Metrics
);

public record ProvenanceEntry(
    string Stage,
    DateTime TimestampUtc,
    string Detail,
    ProvImportance Importance = ProvImportance.Important,
    object? Parameters = null
);

public interface IProvenanceArchive
{
    void Archive(Guid envelopeId, IEnumerable<ProvenanceEntry> entries);
}

public class InMemoryProvenanceArchive : IProvenanceArchive
{
    private readonly Dictionary<Guid, List<ProvenanceEntry>> _archive = new();
    public void Archive(Guid envelopeId, IEnumerable<ProvenanceEntry> entries)
    {
        if (!_archive.TryGetValue(envelopeId, out var list))
        {
            list = new List<ProvenanceEntry>();
            _archive[envelopeId] = list;
        }
        list.AddRange(entries);
        Console.WriteLine($"[Archive] Archived {entries.Count()} entries for {envelopeId}");
    }
    public IReadOnlyList<ProvenanceEntry> Get(Guid id) =>
        _archive.TryGetValue(id, out var list) ? list : Array.Empty<ProvenanceEntry>();
}

public class ProvenanceLog
{
    private readonly List<ProvenanceEntry> _entries = new();

    public void Add(string stage, string detail, ProvImportance importance = ProvImportance.Important, object? parameters = null)
    {
        _entries.Add(new ProvenanceEntry(stage, DateTime.UtcNow, detail, importance, parameters));
    }

    public IReadOnlyList<ProvenanceEntry> Entries => _entries.AsReadOnly();

    // Prune: move verbose, old entries to archive; add one summary breadcrumb.
    public void PruneNonEssential(Guid envelopeId, TimeSpan ageThreshold, IProvenanceArchive archive)
    {
        var cutoff = DateTime.UtcNow - ageThreshold;
        var toArchive = _entries
            .Where(e => e.Importance == ProvImportance.Verbose && e.TimestampUtc <= cutoff)
            .ToList();

        if (toArchive.Count == 0) return;

        archive.Archive(envelopeId, toArchive);
        _entries.RemoveAll(e => toArchive.Contains(e));

        Add(stage: "provenance",
            detail: $"Pruned {toArchive.Count} verbose entries older than {ageThreshold}",
            importance: ProvImportance.Important,
            parameters: new { prunedCount = toArchive.Count, thresholdSeconds = (int)ageThreshold.TotalSeconds });
    }
}

public record QAEnvelope<TAnswer>(
    Guid Id,
    string Question,
    TAnswer Answer,
    TrainingContext TrainingContext,
    ProvenanceLog Provenance
)
{
    public QAEnvelope<TAnswer> WithProvenance(string stage, string detail, object? parameters = null, ProvImportance importance = ProvImportance.Important)
    {
        Provenance.Add(stage, detail, importance, parameters);
        return this;
    }
}

#endregion

#region Checkpointing

public record OrchestrationCheckpoint(
    string Name,
    string ConfigHash,
    TrainingContext TrainingContext,
    DateTime CreatedUtc
);

public interface ICheckpointStore
{
    void Save(OrchestrationCheckpoint checkpoint);
    OrchestrationCheckpoint? Get(string name);
}

public class InMemoryCheckpointStore : ICheckpointStore
{
    private readonly Dictionary<string, OrchestrationCheckpoint> _store = new();
    public void Save(OrchestrationCheckpoint checkpoint)
    {
        _store[checkpoint.Name] = checkpoint;
        Console.WriteLine($"[Checkpoint] Saved '{checkpoint.Name}' @ {checkpoint.CreatedUtc:o} (hash {checkpoint.ConfigHash})");
    }
    public OrchestrationCheckpoint? Get(string name) =>
        _store.TryGetValue(name, out var cp) ? cp : null;
}

public class CheckpointService
{
    private readonly ICheckpointStore _store;
    public CheckpointService(ICheckpointStore store) => _store = store;

    public void FreezeKnownGood(string name, string configHash, TrainingContext training)
    {
        var cp = new OrchestrationCheckpoint(name, configHash, training, DateTime.UtcNow);
        _store.Save(cp);
    }

    public TrainingContext ResetToKnownGood(string name)
    {
        var cp = _store.Get(name)
            ?? throw new InvalidOperationException($"Checkpoint '{name}' not found.");
        Console.WriteLine($"[Checkpoint] Reset to '{name}' (hash {cp.ConfigHash})");
        return cp.TrainingContext;
    }
}

#endregion

#region Storage and Rehydration

public interface IEnvelopeStore
{
    void Save<T>(QAEnvelope<T> envelope);
    QAEnvelope<T>? Get<T>(Guid id);
}

public class InMemoryEnvelopeStore : IEnvelopeStore
{
    private readonly Dictionary<Guid, object> _map = new();
    public void Save<T>(QAEnvelope<T> envelope)
    {
        _map[envelope.Id] = envelope;
        Console.WriteLine($"[Repo] Saved QAEnvelope {envelope.Id}");
    }
    public QAEnvelope<T>? Get<T>(Guid id) =>
        _map.TryGetValue(id, out var obj) ? obj as QAEnvelope<T> : null;
}

public class EnvelopeRehydrator
{
    private readonly IEnvelopeStore _store;
    public EnvelopeRehydrator(IEnvelopeStore store) => _store = store;

    public QAEnvelope<T> Rehydrate<T>(Guid id)
        => _store.Get<T>(id) ?? throw new InvalidOperationException($"Envelope {id} not found.");

    // Example: resume post-normalization if model was never invoked.
    public QAEnvelope<string> ResumeModelIfPending(QAEnvelope<string> qa, Func<string, (string Answer, double Confidence)> infer)
    {
        var alreadyModeled = qa.Provenance.Entries.Any(e => e.Stage == "model" && e.Detail.StartsWith("Generated answer"));
        if (alreadyModeled) return qa;

        qa = qa.WithProvenance("model", "Invoking model (rehydrated path)", new { qa.TrainingContext.ModelVersion }, ProvImportance.Important);
        var (ans, conf) = infer(qa.Question);
        qa = qa with { Answer = ans }
            .WithProvenance("model", "Generated answer", new { confidence = conf }, ProvImportance.Critical);

        return qa;
    }
}

#endregion

#region Pipeline Services

public class TrainingContextProvider
{
    public TrainingContext GetActiveContext() => new TrainingContext(
        DatasetRevision: "2025-07-15",
        PreprocessingHash: "sha256:abcd1234...",
        ModelVersion: "qa-model-v2.1",
        TrainingDateUtc: new DateTime(2025, 7, 15),
        Hyperparameters: new[] { "context=5", "threshold=0.85" },
        Metrics: new[] { "f1=0.92", "precision=0.94" }
    );
}

public class Normalizer
{
    public string Normalize(string input, out int removedCount)
    {
        var stopwords = new HashSet<string> { "is", "the", "a" };
        var tokens = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        var filtered = new List<string>();
        removedCount = 0;
        foreach (var t in tokens)
        {
            if (stopwords.Contains(t.ToLower())) removedCount++;
            else filtered.Add(t);
        }
        return string.Join(' ', filtered);
    }
}

public class Model
{
    public (string Answer, double Confidence) Infer(string question)
        => ("Provenance logging is the recorded history of data as it moves through the system.", 0.91);
}

#endregion

#region Orchestrator with checkpoint, prune, rehydrate

public class QAPipeline
{
    private readonly TrainingContextProvider _contextProvider = new();
    private readonly Normalizer _normalizer = new();
    private readonly Model _model = new();

    private readonly InMemoryEnvelopeStore _store = new();
    private readonly InMemoryProvenanceArchive _archive = new();

    private readonly CheckpointService _checkpointSvc = new(new InMemoryCheckpointStore());

    private const string KnownGoodName = "qa-orchestrator-dual-lineage";
    private const string ConfigHash = "cfg:qa@1.0.0-lineage";

    public QAPipeline()
    {
        // Freeze a known-good orchestration on startup
        var training = _contextProvider.GetActiveContext();
        _checkpointSvc.FreezeKnownGood(KnownGoodName, ConfigHash, training);
    }

    public QAEnvelope<string> HandleQuestion(string question, string clientId)
    {
        // Reset to known-good training context before each session (optional but safe)
        var trainingCtx = _checkpointSvc.ResetToKnownGood(KnownGoodName);

        // Collector
        var qa = new QAEnvelope<string>(
            Id: Guid.NewGuid(),
            Question: question,
            Answer: string.Empty,
            TrainingContext: trainingCtx,
            Provenance: new ProvenanceLog()
        ).WithProvenance("collector", "Received from client-ui", new { srcId = clientId }, ProvImportance.Critical);

        // Refinery
        qa = qa.WithProvenance("normalizer", "Starting normalization", importance: ProvImportance.Verbose);
        var normalized = _normalizer.Normalize(qa.Question, out var removedCount);
        qa = (qa with { Question = normalized })
             .WithProvenance("normalizer", "Stopwords filtered", new { removedCount }, ProvImportance.Important);

        // Prune verbose normalization chatter older than 1 second (demo threshold)
        qa.Provenance.PruneNonEssential(qa.Id, TimeSpan.FromSeconds(1), _archive);

        // Model
        qa = qa.WithProvenance("model", "Invoking model", new { model = trainingCtx.ModelVersion }, ProvImportance.Important);
        var (answer, confidence) = _model.Infer(qa.Question);
        qa = (qa with { Answer = answer })
             .WithProvenance("model", "Generated answer", new { confidence }, ProvImportance.Critical);

        // Persist
        _store.Save(qa);

        return qa;
    }

    public QAEnvelope<string> RehydrateAndResume(Guid id)
    {
        var rehydrator = new EnvelopeRehydrator(_store);
        var qa = rehydrator.Rehydrate<string>(id);
        qa = rehydrator.ResumeModelIfPending(qa, _model.Infer);

        // Optional: prune again after resume to keep hot memory lean
        qa.Provenance.PruneNonEssential(qa.Id, TimeSpan.FromSeconds(1), _archive);

        // Save updated
        _store.Save(qa);
        return qa;
    }
}

#endregion

#region Program

public static class Program
{
    public static void Main()
    {
        var pipe = new QAPipeline();

        // Initial run
        var qa = pipe.HandleQuestion("What is the purpose of provenance logging in AI pipelines?", "client-xyz");

        Console.WriteLine();
        Console.WriteLine($"[Result] Q: {qa.Question}");
        Console.WriteLine($"[Result] A: {qa.Answer}");
        Console.WriteLine("[Result] Provenance (hot):");
        foreach (var e in qa.Provenance.Entries)
            Console.WriteLine($"  - {e.TimestampUtc:o} [{e.Importance}] {e.Stage}: {e.Detail}");

        // Rehydrate and (if needed) resume later
        var resumed = pipe.RehydrateAndResume(qa.Id);

        Console.WriteLine();
        Console.WriteLine("[Rehydrated] Provenance (hot):");
        foreach (var e in resumed.Provenance.Entries)
            Console.WriteLine($"  - {e.TimestampUtc:o} [{e.Importance}] {e.Stage}: {e.Detail}");
    }
}

#endregion