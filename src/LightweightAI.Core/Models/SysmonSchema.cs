// Project Name: LightweightAI.Core
// File Name: SysmonSchema.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace LightweightAI.Core.Models;


/// <summary>
///     Static map of known Sysmon EventId -> property names.
///     This enables schema-aware naming instead of positional Prop0/Prop1...
///     Names are based on common Sysmon schemas; unknown or extended fields fall back to positional keys.
/// </summary>
internal static class SysmonSchema
{
    // Minimal, high-signal property names for common EventIds.
    // Extend as needed without breaking existing consumers.
    private static readonly Dictionary<int, string[]> _map = new()
    {
        // 1: Process Create
        [1] = new[]
        {
            "RuleName", "UtcTime", "ProcessGuid", "ProcessId", "Image", "FileVersion", "Description", "Product",
            "Company",
            "OriginalFileName", "CommandLine", "CurrentDirectory", "User", "LogonGuid", "LogonId", "TerminalSessionId",
            "IntegrityLevel", "Hashes", "ParentProcessGuid", "ParentProcessId", "ParentImage", "ParentCommandLine"
        },
        // 3: Network connection
        [3] = new[]
        {
            "RuleName", "UtcTime", "ProcessGuid", "ProcessId", "Image", "User", "Protocol", "Initiated", "SourceIsIpv6",
            "SourceIp", "SourceHostname", "SourcePort", "SourcePortName", "DestinationIsIpv6", "DestinationIp",
            "DestinationHostname", "DestinationPort", "DestinationPortName"
        },
        // 6: Driver loaded
        [6] = new[]
        {
            "RuleName", "UtcTime", "ImageLoaded", "Hashes", "Signed", "Signature", "SignatureStatus"
        },
        // 7: Image loaded
        [7] = new[]
        {
            "RuleName", "UtcTime", "ImageLoaded", "Hashes", "Signed", "Signature", "SignatureStatus"
        },
        // 11: File create
        [11] = new[]
        {
            "RuleName", "UtcTime", "ProcessGuid", "ProcessId", "Image", "TargetFilename", "CreationUtcTime", "User"
        },
        // 13: Registry value set
        [13] = new[]
        {
            "RuleName", "UtcTime", "EventType", "UtcTime2", "ProcessGuid", "ProcessId", "Image", "TargetObject",
            "Details", "User"
        },
        // 22: DNS query
        [22] = new[]
        {
            "RuleName", "UtcTime", "ProcessGuid", "ProcessId", "Image", "User", "QueryName", "QueryStatus",
            "QueryResults"
        }
    };





    public static bool TryGetPropertyNames(int eventId, out string[] names)
    {
        return _map.TryGetValue(eventId, out names!);
    }
}