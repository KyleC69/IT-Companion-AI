// Project Name: LightweightAI.Core
// File Name: FileAuditLog.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using System.Security.Cryptography;
using System.Text;



namespace LightweightAI.Core.Engine;


public sealed class FileAuditLog : IAppendOnlyAuditLog
{
    private readonly object _gate = new();
    private readonly ILoggerSeverity<FileAuditLog> _log;
    private readonly string _path;





    public FileAuditLog(ILoggerSeverity<FileAuditLog> log, string path)
    {
        this._log = log;
        this._path = path;
        Directory.CreateDirectory(Path.GetDirectoryName(this._path)!);
        if (!File.Exists(this._path)) File.WriteAllText(this._path, "");
    }





    public Task AppendAsync(AuditRecord record, CancellationToken ct = default)
    {
        lock (this._gate)
        {
            var line =
                $"{record.Sequence}|{record.TimestampUtc:O}|{record.Actor}|{record.Action}|{record.PayloadJson}|{record.PreviousHashHex}|{record.HashHex}";
            File.AppendAllLines(this._path, new[] { line });
        }

        return Task.CompletedTask;
    }





    public Task<IReadOnlyList<AuditRecord>> ReadRangeAsync(long fromSequence, int maxCount,
        CancellationToken ct = default)
    {
        List<AuditRecord> list = new(maxCount);
        foreach (var line in File.ReadLines(this._path))
        {
            var parts = line.Split('|');
            if (parts.Length < 7) continue;
            var seq = long.Parse(parts[0]);
            if (seq < fromSequence) continue;
            var record = new AuditRecord(
                seq, DateTimeOffset.Parse(parts[1]), parts[2], parts[3], parts[4], parts[6], parts[5]);
            list.Add(record);
            if (list.Count >= maxCount) break;
        }

        return Task.FromResult<IReadOnlyList<AuditRecord>>(list);
    }





    public static string ComputeHash(string? previousHashHex, AuditRecord record)
    {
        using var sha = SHA256.Create();
        var payload =
            $"{previousHashHex}|{record.Sequence}|{record.TimestampUtc:O}|{record.Actor}|{record.Action}|{record.PayloadJson}";
        var bytes = Encoding.UTF8.GetBytes(payload);
        return Convert.ToHexString(sha.ComputeHash(bytes));
    }
}