// Project Name: LightweightAI.Core
// File Name: IDriverSink.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace LightweightAI.Core.Loaders.Drivers;


public interface IDriverSink
{
    Task EmitBatchAsync(IReadOnlyList<DriverRecord> batch, CancellationToken ct);
}