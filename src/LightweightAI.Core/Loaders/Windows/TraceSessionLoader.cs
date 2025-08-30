// Project Name: LightweightAI.Core
// File Name: TraceSessionLoader.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using System.Runtime.CompilerServices;

using LightweightAI.Core.Abstractions;
using LightweightAI.Core.Engine.Models;



namespace LightweightAI.Core.Loaders.Windows;


/// <summary>
///     Placeholder ETW trace session loader. In future will hook to real-time ETW providers (e.g. Kernel,
///     Microsoft-Windows-DotNETRuntime)
///     capturing structured fields. Currently emits synthetic events for pipeline integration tests.
/// </summary>
public sealed class TraceSessionLoader(int count = 1000) : ISourceLoader
{
    public async IAsyncEnumerable<RawEvent> LoadAsync(SourceRequest request,
        [EnumeratorCancellation] CancellationToken ct = default)
    {
        for (var i = 0; i < count; i++)
        {
            ct.ThrowIfCancellationRequested();
            DateTimeOffset ts = DateTimeOffset.UtcNow;
            Dictionary<string, object?> payload = new()
            {
                ["Provider"] = "Synthetic.Provider",
                ["ActivityId"] = Guid.NewGuid(),
                ["Seq"] = i,
                ["Phase"] = i % 2 == 0 ? "Start" : "Stop"
            };
            yield return new RawEvent(request.SourceKey, 0, ts, Environment.MachineName, null, "Info", payload,
                $"etw:{i}");
            if (i % 128 == 0) await Task.Yield();
        }
    }
}