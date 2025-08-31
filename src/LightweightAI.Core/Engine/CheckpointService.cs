// Project Name: LightweightAI.Core
// File Name: CheckpointService.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using LightweightAI.Core.Interfaces;



namespace LightweightAI.Core.Engine;


public class CheckpointService(ICheckpointStore store)
{
    public void FreezeKnownGood(string name, string configHash, TrainingContext training)
    {
        var cp = new OrchestrationCheckpoint(name, configHash, training, DateTime.UtcNow);
        store.Save(cp);
    }





    public TrainingContext ResetToKnownGood(string name)
    {
        OrchestrationCheckpoint cp = store.Get(name)
                                     ?? throw new InvalidOperationException($"Checkpoint '{name}' not found.");
        Console.WriteLine($"[Checkpoint] Reset to '{name}' (hash {cp.ConfigHash})");
        return cp.TrainingContext;
    }
}