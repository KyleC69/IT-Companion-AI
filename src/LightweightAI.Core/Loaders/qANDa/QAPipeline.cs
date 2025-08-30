// Project Name: LightweightAI.Core
// File Name: QAPipeline.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using LightweightAI.Core.Engine;
using LightweightAI.Core.Engine.Models;
using LightweightAI.Core.Engine.Provenance;

using ProvenanceLog = LightweightAI.Core.Training.ProvenanceLog;


namespace LightweightAI.Core.Loaders.qANDa;


public class QAPipeline
{
    private const string KnownGoodName = "qa-orchestrator-dual-lineage";
    private const string ConfigHash = "cfg:qa@1.0.0-lineage";
    private readonly InMemoryProvenanceArchive _archive = new();

    private readonly CheckpointService _checkpointSvc = new(new InMemoryCheckpointStore());
    private readonly TrainingContextProvider _contextProvider = new();
    private readonly Model _model = new();
    private readonly Normalizer _normalizer = new();

    private readonly InMemoryEnvelopeStore _store = new();





    public QAPipeline()
    {
        TrainingContext training = this._contextProvider.GetActiveContext();
        this._checkpointSvc.FreezeKnownGood(KnownGoodName, ConfigHash, training);
    }





    public QAEnvelope<string> HandleQuestion(string question, string clientId)
    {
        TrainingContext trainingCtx = this._checkpointSvc.ResetToKnownGood(KnownGoodName);

        QAEnvelope<string> qa = new QAEnvelope<string>(
            Guid.NewGuid(),
            question,
            string.Empty,
            trainingCtx,
            new ProvenanceLog()
        ).WithProvenance("collector", "Received from client-ui", new { srcId = clientId });

        qa = qa.WithProvenance("normalizer", "Starting normalization");
        var normalized = this._normalizer.Normalize(qa.Question, out var removedCount);
        qa = (qa with { Question = normalized })
            .WithProvenance("normalizer", "Stopwords filtered", new { removedCount });

        // Prune verbose normalization chatter older than 1 second (demo threshold)
        qa.Provenance.PruneNonEssential(qa.Id, TimeSpan.FromSeconds(1), this._archive);

        qa = qa.WithProvenance("model", "Invoking model", new { model = trainingCtx.ModelVersion });
        var (answer, confidence) = this._model.Infer(qa.Question);
        qa = (qa with { Answer = answer })
            .WithProvenance("model", "Generated answer", new { confidence });

        this._store.Save(qa);

        return qa;
    }





    public QAEnvelope<string> RehydrateAndResume(Guid id)
    {
        var rehydrator = new EnvelopeRehydrator(this._store);
        QAEnvelope<string> qa = rehydrator.Rehydrate<string>(id);
        qa = rehydrator.ResumeModelIfPending(qa, this._model.Infer);


        qa.Provenance.PruneNonEssential(qa.Id, TimeSpan.FromSeconds(1), this._archive);

        this._store.Save(qa);
        return qa;
    }
}