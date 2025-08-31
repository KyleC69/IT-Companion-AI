// Project Name: LightweightAI.Core
// File Name: InMemoryCheckpointStore.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace LightweightAI.Core.Models;


public class InMemoryCheckpointStore : ICheckpointStore
{
    private readonly Dictionary<string, OrchestrationCheckpoint> _store = new();





    public void Save(OrchestrationCheckpoint checkpoint)
    {
        this._store[checkpoint.Name] = checkpoint;
        Console.WriteLine(
            $"[Checkpoint] Saved '{checkpoint.Name}' @ {checkpoint.CreatedUtc:o} (hash {checkpoint.ConfigHash})");
    }





    public OrchestrationCheckpoint? Get(string name)
    {
        return this._store.TryGetValue(name, out OrchestrationCheckpoint? cp) ? cp : null;
    }
}