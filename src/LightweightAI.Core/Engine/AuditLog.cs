using System.Text;
using System.Security.Cryptography;

using LightweightAI.Core.Abstractions;

using Microsoft.Extensions.Logging;
namespace LightweightAI.Core.Engine;

public sealed class FileAuditLog : IAppendOnlyAuditLog
{
    private readonly string _path;
    private readonly ILogger<FileAuditLog> _log;
    private readonly object _gate = new();

    public FileAuditLog(ILogger<FileAuditLog> log, string path)
    {
        _log = log;
        _path = path;
        Directory.CreateDirectory(Path.GetDirectoryName(_path)!);
        if (!File.Exists(_path)) File.WriteAllText(_path, "");
    }

    public Task AppendAsync(AuditRecord record, CancellationToken ct = default)
    {
        lock (_gate)
        {
            var line = $"{record.Sequence}|{record.TimestampUtc:O}|{record.Actor}|{record.Action}|{record.PayloadJson}|{record.PreviousHashHex}|{record.HashHex}";
            File.AppendAllLines(_path, new[] { line });
        }
        return Task.CompletedTask;
    }

    public Task<IReadOnlyList<AuditRecord>> ReadRangeAsync(long fromSequence, int maxCount, CancellationToken ct = default)
    {
        var list = new List<AuditRecord>(maxCount);
        foreach (var line in File.ReadLines(_path))
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
        var payload = $"{previousHashHex}|{record.Sequence}|{record.TimestampUtc:O}|{record.Actor}|{record.Action}|{record.PayloadJson}";
        var bytes = Encoding.UTF8.GetBytes(payload);
        return Convert.ToHexString(sha.ComputeHash(bytes));
    }
}
