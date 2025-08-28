// Project Name: LightweightAI.Core
// File Name: wholepipeline.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace LightweightAI.Core.Loaders.qANDa;


#region Domain Models

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
    object? Parameters = null
);



public class ProvenanceLog
{
    private readonly List<ProvenanceEntry> _entries = new();

    public IReadOnlyList<ProvenanceEntry> Entries => this._entries.AsReadOnly();





    public void Add(string stage, string detail, object? parameters = null)
    {
        this._entries.Add(new ProvenanceEntry(stage, DateTime.UtcNow, detail, parameters));
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
    public QAEnvelope<TAnswer> WithProvenance(string stage, string detail, object? parameters = null)
    {
        this.Provenance.Add(stage, detail, parameters);
        return this;
    }
}

#endregion

#region Pipeline Services

public class TrainingContextProvider
{
    public TrainingContext GetActiveContext()
    {
        return new TrainingContext(
            "2025-07-15",
            "sha256:abcd1234...",
            "qa-model-v2.1",
            new DateTime(2025, 7, 15),
            new[] { "context=5", "threshold=0.85" },
            new[] { "f1=0.92", "precision=0.94" }
        );
    }
}



public class Normalizer
{
    public string Normalize(string input, out int removedCount)
    {
        // Dummy stopword removal
        HashSet<string> stopwords = new() { "is", "the", "a" };
        var tokens = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        List<string> filtered = new();

        removedCount = 0;
        foreach (var t in tokens)
            if (stopwords.Contains(t.ToLower()))
                removedCount++;
            else
                filtered.Add(t);
        return string.Join(' ', filtered);
    }
}



public class Model
{
    public (string Answer, double Confidence) Infer(string question)
    {
        // Dummy model output
        return ("Provenance logging is the recorded history of data as it moves through the system.", 0.91);
    }
}



public class QARepository
{
    private readonly List<object> _store = new();





    public void Save<T>(QAEnvelope<T> envelope)
    {
        this._store.Add(envelope);
        Console.WriteLine($"[Repo] Saved QAEnvelope {envelope.Id}");
    }
}

#endregion

#region Orchestrator

public class QAPipeline
{
    private readonly TrainingContextProvider _contextProvider = new();
    private readonly Model _model = new();
    private readonly Normalizer _normalizer = new();
    private readonly QARepository _repo = new();





    public string HandleQuestion(string question, string clientId)
    {
        // Collector Layer
        TrainingContext trainingCtx = this._contextProvider.GetActiveContext();
        QAEnvelope<string> qa = new QAEnvelope<string>(
            Guid.NewGuid(),
            question,
            string.Empty,
            trainingCtx,
            new ProvenanceLog()
        ).WithProvenance("collector", "Received from client-ui", new { srcId = clientId });

        //// Refinery Layer
        //qa.WithProvenance("normalizer", "Starting normalization");
        //var normalized = _normalizer.Normalize(qa.Question, out var removedCount);
        //qa = qa with { Question = normalized }.WithProvenance("normalizer", "Stopwords filtered", new { removedCount });

        //// Model Layer
        //qa.WithProvenance("model", "Invoking model", new { model = trainingCtx.ModelVersion });
        //var (answer, confidence) = _model.Infer(qa.Question);
        //qa = qa with { Answer = answer }.WithProvenance("model", "Generated answer", new { confidence });


        qa = qa with { Question = "normalized" };
        //    qa = qa.WithProvenance("normalizer", "Stopwords filtered", new { removedCount });

        qa = qa with { Answer = "answer" };
//        qa = qa.WithProvenance("model", "Generated answer", new { confidence });

        // Persistence Layer
        this._repo.Save(qa);

        // Return to caller
        return qa.Answer;
    }
}

#endregion

#region Program

public static class Program
{
    public static void Main()
    {
        var pipeline = new QAPipeline();

        var userQuestion = "What is the purpose of provenance logging in AI pipelines?";
        var answer = pipeline.HandleQuestion(userQuestion, "client-xyz");

        Console.WriteLine();
        Console.WriteLine($"Q: {userQuestion}");
        Console.WriteLine($"A: {answer}");
    }
}

#endregion