// Project Name: LightweightAI.Core
// File Name: IAppendOnlyAuditLog.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace LightweightAI.Core.Interfaces;


public interface IAppendOnlyAuditLog
{
    Task AppendAsync(AuditRecord record, CancellationToken ct = default);
    Task<IReadOnlyList<AuditRecord>> ReadRangeAsync(long fromSequence, int maxCount, CancellationToken ct = default);
}