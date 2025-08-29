using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

using LightweightAI.Core.Abstractions;

using Microsoft.Extensions.Logging;


namespace LightweightAI.Core.Engine;


public sealed class AlertDispatcher : IAlertDispatcher, IAsyncDisposable
{
    private readonly Channel<ProvenancedDecision> _channel;
    private readonly ILogger<AlertDispatcher> _log;
    private readonly List<Func<ProvenancedDecision, CancellationToken, Task>> _sinks = new();
    private readonly CancellationTokenSource _cts = new();

    public AlertDispatcher(ILogger<AlertDispatcher> log, int capacity = 1024)
    {
        _log = log;
        _channel = Channel.CreateBounded<ProvenancedDecision>(new BoundedChannelOptions(capacity) { SingleReader = true, SingleWriter = false, FullMode = BoundedChannelFullMode.DropOldest });
        _ = Task.Run(ConsumeAsync);
    }

    public Task DispatchAsync(ProvenancedDecision decision, CancellationToken ct = default)
        => _channel.Writer.WriteAsync(decision, ct).AsTask();

    public void AddSink(Func<ProvenancedDecision, CancellationToken, Task> sink) => _sinks.Add(sink);

    // New helper to register typed sink implementations
    public void AddSink(Alerts.IAlertSink sink) => AddSink((d, ct) => sink.SendAsync(d, ct));

    private async Task ConsumeAsync()
    {
        try
        {
            await foreach (var d in _channel.Reader.ReadAllAsync(_cts.Token))
                foreach (var sink in _sinks)
                {
                    try { await sink(d, _cts.Token); }
                    catch (Exception ex) { _log?.LogError(ex, "Alert sink failure"); }
                }
        }
        catch (OperationCanceledException) { /* shutdown */ }
    }

    public async ValueTask DisposeAsync()
    {
        _cts.Cancel();
        _channel.Writer.Complete();
        await Task.CompletedTask;
    }
}
