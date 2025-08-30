// Project Name: LightweightAI.Core
// File Name: IProcessConnectionSink.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace LightweightAI.Core.Refinery;


public interface IProcessConnectionSink
{
    Task EmitBatchAsync(IReadOnlyList<ProcessConnectionRecord> batch, CancellationToken ct);
}