// Project Name: LightweightAI.Core
// File Name: FileSystemLoader.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using System.Security.Cryptography;



namespace LightweightAI.Core.Loaders.FileSystem;


public sealed class FileSystemLoader(FileSystemLoaderConfig config, IFileSystemSink sink, ILoggerSeverity log)
    : IDisposable
{
    private const string SchemaVersion = "1.0";
    private const string CollectionMethod = "Directory Enumeration";
    private const string SourceId = "filesystem";
    private const string Loader = nameof(FileSystemLoader);

    private readonly FileSystemLoaderConfig _config = config ?? throw new ArgumentNullException(nameof(config));

    private readonly Dictionary<string, FileRecord> _lastSnapshot = new(StringComparer.OrdinalIgnoreCase);
    private readonly ILoggerSeverity _log = log ?? throw new ArgumentNullException(nameof(log));
    private readonly IFileSystemSink _sink = sink ?? throw new ArgumentNullException(nameof(sink));
    private bool _disposed;





    public void Dispose()
    {
        if (this._disposed) return;
        this._disposed = true;
    }





    public async Task StartAsync(CancellationToken ct)
    {
        this._log.Info(
            $"{Loader} starting. Paths={this._config.Paths.Count}, Interval={this._config.SampleInterval}, DeltaOnly={this._config.DeltaOnly}");

        while (!ct.IsCancellationRequested)
            try
            {
                Dictionary<string, FileRecord> snapshot = CollectSnapshot();

                IReadOnlyList<FileRecord> toEmit;
                if (this._config.DeltaOnly)
                    toEmit = DiffSnapshots(this._lastSnapshot, snapshot);
                else
                    toEmit = snapshot.Values.Select(r =>
                    {
                        r.ChangeType = "Unchanged";
                        return r;
                    }).ToList();

                if (toEmit.Count > 0)
                    await this._sink.EmitBatchAsync(toEmit, ct).ConfigureAwait(false);

                this._lastSnapshot.Clear();
                foreach (KeyValuePair<string, FileRecord> kvp in snapshot)
                    this._lastSnapshot[kvp.Key] = kvp.Value;

                await Task.Delay(this._config.SampleInterval, ct).ConfigureAwait(false);
            }
            catch (OperationCanceledException)
            {
                break;
            }
            catch (Exception ex)
            {
                this._log.Error($"{Loader} loop error: {ex.Message}");
                if (this._config.FailFast) throw;
                await Task.Delay(TimeSpan.FromSeconds(2), ct).ConfigureAwait(false);
            }

        this._log.Info($"{Loader} stopped.");
    }





    private Dictionary<string, FileRecord> CollectSnapshot()
    {
        Dictionary<string, FileRecord> result = new(StringComparer.OrdinalIgnoreCase);

        foreach (PathSpec pathSpec in this._config.Paths)
            try
            {
                if (!Directory.Exists(pathSpec.Path))
                {
                    this._log.Warn($"{Loader} missing directory '{pathSpec.Path}'");
                    continue;
                }

                IEnumerable<string> files = Directory.EnumerateFiles(pathSpec.Path, "*",
                    pathSpec.Recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);

                foreach (var file in files)
                {
                    if (IsExcluded(file)) continue;

                    try
                    {
                        var info = new FileInfo(file);
                        var rec = new FileRecord
                        {
                            FullPath = file,
                            Name = info.Name,
                            DirectoryPath = info.DirectoryName ?? "",
                            Size = info.Length,
                            CreationTimeUtc = info.CreationTimeUtc,
                            LastWriteTimeUtc = info.LastWriteTimeUtc,
                            Attributes = info.Attributes.ToString(),
                            Hash = this._config.IncludeHash ? ComputeHash(file) : "",
                            Host = Environment.MachineName,
                            SourceId = SourceId,
                            LoaderName = Loader,
                            SchemaVersion = SchemaVersion,
                            CollectionMethod = CollectionMethod,
                            RecordId = $"{file}:{Environment.MachineName}",
                            ChangeType = "Unchanged"
                        };

                        result[file] = rec;

                        if (this._config.AuditLog)
                            this._log.Debug(
                                $"{Loader} audit File='{rec.FullPath}' Size={rec.Size} Schema='{SchemaVersion}'");
                    }
                    catch (Exception ex)
                    {
                        this._log.Warn($"{Loader} failed to stat file '{file}': {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                this._log.Warn($"{Loader} failed to enumerate '{pathSpec.Path}': {ex.Message}");
            }

        return result;
    }





    private bool IsExcluded(string filePath)
    {
        if (this._config.ExcludePatterns != null &&
            this._config.ExcludePatterns.Any(p => filePath.Contains(p, StringComparison.OrdinalIgnoreCase)))
            return true;
        if (this._config.IncludePatterns != null &&
            !this._config.IncludePatterns.Any(p => filePath.Contains(p, StringComparison.OrdinalIgnoreCase)))
            return true;
        return false;
    }





    private static List<FileRecord> DiffSnapshots(Dictionary<string, FileRecord> oldSnap,
        Dictionary<string, FileRecord> newSnap)
    {
        List<FileRecord> changes = new();

        foreach ((var path, FileRecord cur) in newSnap)
            if (!oldSnap.TryGetValue(path, out FileRecord? old))
                changes.Add(CloneWithChange(cur, "Added"));
            else if (HasChanged(old, cur)) changes.Add(CloneWithChange(cur, "Modified"));

        foreach ((var path, FileRecord old) in oldSnap)
            if (!newSnap.ContainsKey(path))
                changes.Add(CloneWithChange(old, "Removed"));

        return changes;
    }





    private static bool HasChanged(FileRecord oldRec, FileRecord newRec)
    {
        return oldRec.Size != newRec.Size ||
               oldRec.LastWriteTimeUtc != newRec.LastWriteTimeUtc ||
               oldRec.Attributes != newRec.Attributes ||
               (!string.IsNullOrEmpty(newRec.Hash) && oldRec.Hash != newRec.Hash);
    }





    private static FileRecord CloneWithChange(FileRecord src, string changeType)
    {
        return new FileRecord
        {
            FullPath = src.FullPath,
            Name = src.Name,
            DirectoryPath = src.DirectoryPath,
            Size = src.Size,
            CreationTimeUtc = src.CreationTimeUtc,
            LastWriteTimeUtc = src.LastWriteTimeUtc,
            Attributes = src.Attributes,
            Hash = src.Hash,
            Host = src.Host,
            SourceId = src.SourceId,
            LoaderName = src.LoaderName,
            SchemaVersion = src.SchemaVersion,
            CollectionMethod = src.CollectionMethod,
            RecordId = src.RecordId,
            ChangeType = changeType
        };
    }





    private static string ComputeHash(string filePath)
    {
        try
        {
            using var sha256 = SHA256.Create();
            using FileStream stream = File.OpenRead(filePath);
            var hash = sha256.ComputeHash(stream);
            return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
        }
        catch
        {
            return "";
        }
    }
}


// -------------------------
// DTOs and config
// -------------------------