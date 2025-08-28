// Project Name: LightweightAI.Core
// File Name: CheckpointService.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using LightweightAI.Core.Engine.models;


namespace LightweightAI.Core.Engine;



public class CheckpointService
{
    private readonly LightweightAI.Core.Engine.Interfaces.ICheckpointStore _store;





    public CheckpointService(LightweightAI.Core.Engine.Interfaces.ICheckpointStore store)
    {
        this._store = store;
    }





    public void FreezeKnownGood(string name, string configHash, TrainingContext training)
    {
        var cp = new OrchestrationCheckpoint(name, configHash, training, DateTime.UtcNow);
        this._store.Save(cp);
    }





    public TrainingContext ResetToKnownGood(string name)
    {
        OrchestrationCheckpoint cp = this._store.Get(name)
                                     ?? throw new InvalidOperationException($"Checkpoint '{name}' not found.");
        Console.WriteLine($"[Checkpoint] Reset to '{name}' (hash {cp.ConfigHash})");
        return cp.TrainingContext;
    }
}