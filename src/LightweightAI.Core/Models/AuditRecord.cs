// Project Name: LightweightAI.Core
// File Name: AuditRecord.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace LightweightAI.Core.Models;


public sealed record AuditRecord(
    long Sequence,
    DateTimeOffset TimestampUtc,
    string Actor,
    string Action,
    string PayloadJson,
    string HashHex,
    string? PreviousHashHex);