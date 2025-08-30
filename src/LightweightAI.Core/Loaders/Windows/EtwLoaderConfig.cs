// Project Name: LightweightAI.Core
// File Name: EtwLoaderConfig.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using Microsoft.Diagnostics.Tracing.Parsers;



namespace LightweightAI.Core.Loaders.Windows;


public sealed class EtwLoaderConfig
{
    // Session
    public string SessionName { get; init; } = "Telemetry-ETW";
    public bool RealTimeSession { get; init; } = true;
    public bool Circular { get; init; } = true;
    public int BufferSizeMB { get; init; } = 64;
    public bool TakeoverExistingSession { get; init; } = true;

    // Providers
    public List<EtwProviderSpec> Providers { get; init; } = new()
    {
        new EtwProviderSpec { ProviderName = "Microsoft-Windows-Kernel-Process", Keywords = -1, Level = 4 }
    };

    // Kernel
    public bool EnableKernel { get; init; } = false;

    // Combine KernelTraceEventParser.Keywords flags as long (e.g., Process | Thread | ImageLoad | FileIOInit | DiskIO | NetworkTCPIP)
    public long KernelKeywords { get; init; } = (long)(
        KernelTraceEventParser.Keywords.Process |
        KernelTraceEventParser.Keywords.Thread |
        KernelTraceEventParser.Keywords.ImageLoad |
        KernelTraceEventParser.Keywords.DiskIO |
        KernelTraceEventParser.Keywords.NetworkTCPIP);

    // Emission
    public int BatchSize { get; init; } = 1024;
    public TimeSpan FlushInterval { get; init; } = TimeSpan.FromSeconds(1);

    // Payload and rendering
    public bool IncludePayload { get; init; } = true;
    public bool IncludeRenderedMessage { get; init; } = true;
    public bool IncludeProcessName { get; init; } = true;

    // Dedup
    public TimeSpan DedupWindow { get; init; } = TimeSpan.FromSeconds(5);
    public int DedupMaxPerWindow { get; init; } = 20;
    public int DedupCapacity { get; init; } = 200_000;

    // Behavior
    public bool FailFast { get; init; } = false;
    public bool AuditLog { get; init; } = true;
}