// Project Name: LightweightAI.Core
// File Name: ICheckpointStore.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace LightweightAI.Core.Engine.Interfaces;


public interface ICheckpointStore
{
    void Save(OrchestrationCheckpoint checkpoint);
    OrchestrationCheckpoint? Get(string name);
}