// Project Name: LightweightAI.Core
// File Name: NetworkConnectionLoader.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using System.Net;
using System.Net.NetworkInformation;

using LightweightAI.Core.Interfaces;


namespace LightweightAI.Core.Loaders.NetworkConnector;


public sealed class NetworkConnectionLoader(
    NetworkConnectionLoaderConfig config,
    INetworkConnectionSink sink,
    ILoggerSeverity log)
    : IDisposable
{
    private const string SchemaVersion = "1.0";
    private const string CollectionMethod = "IPGlobalProperties API";
    private const string SourceId = "netconns";
    private const string Loader = nameof(NetworkConnectionLoader);

    private readonly NetworkConnectionLoaderConfig _config = config ?? throw new ArgumentNullException(nameof(config));

    private readonly Dictionary<string, NetworkConnectionRecord> _lastSnapshot = new(StringComparer.OrdinalIgnoreCase);
    private readonly ILoggerSeverity _log = log ?? throw new ArgumentNullException(nameof(log));
    private readonly INetworkConnectionSink _sink = sink ?? throw new ArgumentNullException(nameof(sink));
    private bool _disposed;





    public void Dispose()
    {
        if (this._disposed) return;
        this._disposed = true;
    }





    public async Task StartAsync(CancellationToken ct)
    {
        this._log.Info(
            $"{Loader} starting. Interval={this._config.SampleInterval}, DeltaOnly={this._config.DeltaOnly}");

        while (!ct.IsCancellationRequested)
            try
            {
                Dictionary<string, NetworkConnectionRecord> snapshot = CollectSnapshot();

                IReadOnlyList<NetworkConnectionRecord> toEmit;
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
                foreach (KeyValuePair<string, NetworkConnectionRecord> kvp in snapshot)
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





    private Dictionary<string, NetworkConnectionRecord> CollectSnapshot()
    {
        Dictionary<string, NetworkConnectionRecord> result = new(StringComparer.OrdinalIgnoreCase);
        var ipProps = IPGlobalProperties.GetIPGlobalProperties();

        try
        {
            foreach (TcpConnectionInformation conn in ipProps.GetActiveTcpConnections())
            {
                var rec = new NetworkConnectionRecord
                {
                    Protocol = "TCP",
                    LocalAddress = conn.LocalEndPoint.Address.ToString(),
                    LocalPort = conn.LocalEndPoint.Port,
                    RemoteAddress = conn.RemoteEndPoint.Address.ToString(),
                    RemotePort = conn.RemoteEndPoint.Port,
                    State = conn.State.ToString(),
                    OwningPid = conn.ProcessId,
                    Host = Environment.MachineName,
                    SourceId = SourceId,
                    LoaderName = Loader,
                    SchemaVersion = SchemaVersion,
                    CollectionMethod = CollectionMethod,
                    RecordId = $"TCP:{conn.LocalEndPoint}-{conn.RemoteEndPoint}:{conn.ProcessId}",
                    ChangeType = "Unchanged"
                };

                result[rec.RecordId] = rec;

                if (this._config.AuditLog)
                    this._log.Debug(
                        $"{Loader} audit TCP {rec.LocalAddress}:{rec.LocalPort} -> {rec.RemoteAddress}:{rec.RemotePort} State={rec.State} PID={rec.OwningPid} Schema={SchemaVersion}");
            }
        }
        catch (Exception ex)
        {
            this._log.Warn($"{Loader} failed to enumerate TCP connections: {ex.Message}");
        }

        try
        {
            foreach (IPEndPoint listener in ipProps.GetActiveTcpListeners())
            {
                var rec = new NetworkConnectionRecord
                {
                    Protocol = "TCP-LISTEN",
                    LocalAddress = listener.Address.ToString(),
                    LocalPort = listener.Port,
                    RemoteAddress = "",
                    RemotePort = 0,
                    State = "LISTENING",
                    OwningPid = 0,
                    Host = Environment.MachineName,
                    SourceId = SourceId,
                    LoaderName = Loader,
                    SchemaVersion = SchemaVersion,
                    CollectionMethod = CollectionMethod,
                    RecordId = $"TCP-LISTEN:{listener.Address}:{listener.Port}",
                    ChangeType = "Unchanged"
                };

                result[rec.RecordId] = rec;

                if (this._config.AuditLog)
                    this._log.Debug(
                        $"{Loader} audit TCP-LISTEN {rec.LocalAddress}:{rec.LocalPort} Schema={SchemaVersion}");
            }
        }
        catch (Exception ex)
        {
            this._log.Warn($"{Loader} failed to enumerate TCP listeners: {ex.Message}");
        }

        try
        {
            foreach (IPEndPoint listener in ipProps.GetActiveUdpListeners())
            {
                var rec = new NetworkConnectionRecord
                {
                    Protocol = "UDP-LISTEN",
                    LocalAddress = listener.Address.ToString(),
                    LocalPort = listener.Port,
                    RemoteAddress = "",
                    RemotePort = 0,
                    State = "LISTENING",
                    OwningPid = 0,
                    Host = Environment.MachineName,
                    SourceId = SourceId,
                    LoaderName = Loader,
                    SchemaVersion = SchemaVersion,
                    CollectionMethod = CollectionMethod,
                    RecordId = $"UDP-LISTEN:{listener.Address}:{listener.Port}",
                    ChangeType = "Unchanged"
                };

                result[rec.RecordId] = rec;

                if (this._config.AuditLog)
                    this._log.Debug(
                        $"{Loader} audit UDP-LISTEN {rec.LocalAddress}:{rec.LocalPort} Schema={SchemaVersion}");
            }
        }
        catch (Exception ex)
        {
            this._log.Warn($"{Loader} failed to enumerate UDP listeners: {ex.Message}");
        }

        return result;
    }





    private static List<NetworkConnectionRecord> DiffSnapshots(Dictionary<string, NetworkConnectionRecord> oldSnap,
        Dictionary<string, NetworkConnectionRecord> newSnap)
    {
        List<NetworkConnectionRecord> changes = new();

        foreach ((var id, NetworkConnectionRecord cur) in newSnap)
            if (!oldSnap.TryGetValue(id, out NetworkConnectionRecord? old))
                changes.Add(CloneWithChange(cur, "Added"));
            else if (HasChanged(old, cur)) changes.Add(CloneWithChange(cur, "Modified"));

        foreach ((var id, NetworkConnectionRecord old) in oldSnap)
            if (!newSnap.ContainsKey(id))
                changes.Add(CloneWithChange(old, "Removed"));

        return changes;
    }





    private static bool HasChanged(NetworkConnectionRecord oldRec, NetworkConnectionRecord newRec)
    {
        return oldRec.State != newRec.State || oldRec.OwningPid != newRec.OwningPid;
    }





    private static NetworkConnectionRecord CloneWithChange(NetworkConnectionRecord src, string changeType)
    {
        return new NetworkConnectionRecord
        {
            Protocol = src.Protocol,
            LocalAddress = src.LocalAddress,
            LocalPort = src.LocalPort,
            RemoteAddress = src.RemoteAddress,
            RemotePort = src.RemotePort,
            State = src.State,
            OwningPid = src.OwningPid,
            Host = src.Host,
            SourceId = src.SourceId,
            LoaderName = src.LoaderName,
            SchemaVersion = src.SchemaVersion,
            CollectionMethod = src.CollectionMethod,
            RecordId = src.RecordId,
            ChangeType = changeType
        };
    }
}


// -------------------------
// DTOs and config
// -------------------------