// Project Name: LightweightAI.Core
// File Name: IInventorySink.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using LightweightAI.Core.Loaders.Windows;



public interface IInventorySink
{
    Task EmitBatchAsync(IReadOnlyList<WmiRecord> batch, CancellationToken ct);
}