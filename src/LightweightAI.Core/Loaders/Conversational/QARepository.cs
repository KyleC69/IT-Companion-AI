// Project Name: LightweightAI.Core
// File Name: QARepository.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace LightweightAI.Core.Loaders.Conversational;


public class QARepository
{
    private readonly List<object> _store = new();





    public void Save<T>(QAEnvelope<T> envelope)
    {
        this._store.Add(envelope);
        Console.WriteLine($"[Repo] Saved QAEnvelope {envelope.Id}");
    }
}