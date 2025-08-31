// Project Name: LightweightAI.Core
// File Name: AlertDispatcher.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using System.Threading.Channels;



namespace LightweightAI.Core.Engine;


public sealed class AlertDispatcher : IAlertDispatcher, IAsyncDisposable
{
    private readonly Channel<ProvenancedDecision> _channel;
    private readonly CancellationTokenSource _cts = new();
    private readonly ILoggerSeverity<AlertDispatcher> _log;
    private readonly List<Func<ProvenancedDecision, CancellationToken, Task>> _sinks = new();





    public AlertDispatcher(ILoggerSeverity<AlertDispatcher> log, int capacity = 1024)
    {
        this._log = log;
        this._channel = Channel.CreateBounded<ProvenancedDecision>(new BoundedChannelOptions(capacity)
            { SingleReader = true, SingleWriter = false, FullMode = BoundedChannelFullMode.DropOldest });
        _ = Task.Run(ConsumeAsync);
    }





    public async ValueTask DisposeAsync()
    {
        this._cts.Cancel();
        this._channel.Writer.Complete();
        await Task.CompletedTask;
    }





    public Task DispatchAsync(ProvenancedDecision decision, CancellationToken ct = default)
    {
        return this._channel.Writer.WriteAsync(decision, ct).AsTask();
    }





    public void AddSink(Func<ProvenancedDecision, CancellationToken, Task> sink)
    {
        this._sinks.Add(sink);
    }





    // New helper to register typed sink implementations
    public void AddSink(Alerts.IAlertSink sink)
    {
        AddSink((d, ct) => sink.SendAsync(d, ct));
    }





    private async Task ConsumeAsync()
    {
        try
        {
            await foreach (ProvenancedDecision d in this._channel.Reader.ReadAllAsync(this._cts.Token))
            foreach (Func<ProvenancedDecision, CancellationToken, Task> sink in this._sinks)
                try
                {
                    await sink(d, this._cts.Token);
                }
                catch (Exception ex)
                {
                    this._log?.LogError(ex, "Alert sink failure");
                }
        }
        catch (OperationCanceledException)
        {
            /* shutdown */
        }
    }






}



public class ILoggerSeverity<T>
{
    public void LogError(Exception ex, string message) => Console.WriteLine($"{message}:{ex.Message}");


    public void LogWarning(string message) => Console.WriteLine($"{message}");


    public void LogInformation(string message) => Console.WriteLine($"{message}");



    // Structured overload similar to ILogger pattern (Exception, message, params object[])
    public void LogWarning(Exception ex, string messageTemplate, params object?[] args)
    {
        Console.WriteLine($"WARN: {string.Format(messageTemplate, args)} Exception={ex.GetType().Name}:{ex.Message}");
    }
}