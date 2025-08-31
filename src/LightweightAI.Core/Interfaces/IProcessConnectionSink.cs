// Project Name: LightweightAI.Core
// File Name: IProcessConnectionSink.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using LightweightAI.Core.Refinery;



namespace LightweightAI.Core.Interfaces;


public interface IProcessConnectionSink
{
    Task EmitBatchAsync(IReadOnlyList<ProcessConnectionRecord> batch, CancellationToken ct);
}