// Project Name: LightweightAI.Core
// File Name: EnvelopeRehydrator.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using LightweightAI.Core.Engine.Interfaces;
using LightweightAI.Core.Engine.models;


namespace LightweightAI.Core.Engine;
public class EnvelopeRehydrator
{
    private readonly IEnvelopeStore _store;





    public EnvelopeRehydrator(IEnvelopeStore store)
    {
        this._store = store;
    }





    public QAEnvelope<T> Rehydrate<T>(Guid id)
    {
        return this._store.Get<T>(id) ?? throw new InvalidOperationException($"Envelope {id} not found.");
    }





    // Example: resume post-normalization if model was never invoked.
    public QAEnvelope<string> ResumeModelIfPending(QAEnvelope<string> qa,
        Func<string, (string Answer, double Confidence)> infer)
    {
        var alreadyModeled =
            qa.Provenance.Entries.Any(e => e.Stage == "model" && e.Detail.StartsWith("Generated answer"));
        if (alreadyModeled) return qa;

        //qa = qa.WithProvenance("model", "Invoking model (rehydrated path)", new { qa.TrainingContext.ModelVersion });
        //var (ans, conf) = infer(qa.Question);
        //qa = qa with { Answer = ans }
        //    .WithProvenance("model", "Generated answer", new { confidence = conf }, ProvImportance.Critical);

        return qa;
    }
}