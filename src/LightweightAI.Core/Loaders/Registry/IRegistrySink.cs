// Project Name: LightweightAI.Core
// File Name: IRegistrySink.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace LightweightAI.Core.Loaders.Registry;


public interface IRegistrySink
{
    Task EmitBatchAsync(IReadOnlyList<RegistryRecord> batch, CancellationToken ct);
}