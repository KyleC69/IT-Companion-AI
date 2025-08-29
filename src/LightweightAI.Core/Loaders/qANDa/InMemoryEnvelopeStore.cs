// Project Name: LightweightAI.Core
// File Name: InMemoryEnvelopeStore.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using LightweightAI.Core.Engine.Interfaces;
using LightweightAI.Core.Engine.models;

namespace LightweightAI.Core.Loaders.qANDa;

public class InMemoryEnvelopeStore : IEnvelopeStore
{
    private readonly Dictionary<Guid, object> _map = new();

    public void Save<T>(QAEnvelope<T> envelope)
    {
        this._map[envelope.Id] = envelope;
        Console.WriteLine($"[Repo] Saved QAEnvelope {envelope.Id}");
    }

    public QAEnvelope<T>? Get<T>(Guid id)
    {
        return this._map.TryGetValue(id, out var obj) ? obj as QAEnvelope<T> : null;
    }
}