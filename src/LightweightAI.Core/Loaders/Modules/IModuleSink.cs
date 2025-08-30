// Project Name: LightweightAI.Core
// File Name: IModuleSink.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace LightweightAI.Core.Loaders.Modules;


public interface IModuleSink
{
    Task EmitBatchAsync(IReadOnlyList<ModuleRecord> batch, CancellationToken ct);
}