// Project Name: LightweightAI.Core
// File Name: EventLogIngest.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace LightweightAI.Core.Loaders.Events;


/// <summary>
///     Ingests Windows Event Logs (Security, System, Application) in real-time.
///     Supports batching and multi-channel subscription.
/// </summary>
public class EventLogIngest(ITelemetryEmitter emitter, IAuditLogger audit) : IIntakeDriver
{
    private readonly IAuditLogger _audit = audit ?? throw new ArgumentNullException(nameof(audit));
    private readonly string[] _channels = { "Security", "System", "Application" };
    private readonly ITelemetryEmitter _emitter = emitter ?? throw new ArgumentNullException(nameof(emitter));





    public Task ExecuteAsync()
    {
        foreach (var channel in this._channels)
            try
            {
                var log = new System.Diagnostics.EventLog(channel)
                {
                    EnableRaisingEvents = true
                };

                log.EntryWritten += (s, e) =>
                {
                    try
                    {
                        var evt = new TelemetryEvent
                        {
                            Timestamp = e.Entry.TimeGenerated,
                            Source = e.Entry.Source,
                            Severity = (int)e.Entry.EntryType,
                            Message = e.Entry.Message,
                            Channel = channel
                        };

                        this._emitter.Emit(evt);
                    }
                    catch (Exception ex)
                    {
                        this._audit.Error($"[EventLogIngest] Failed to emit event from {channel}", ex);
                    }
                };

                this._audit.Info($"[EventLogIngest] Subscribed to {channel} log.");
            }
            catch (Exception ex)
            {
                this._audit.Error($"[EventLogIngest] Failed to subscribe to {channel}", ex);
            }

        return Task.CompletedTask;
    }
}


//TODO: Define TelemetryEvent class as per your application's requirements.
public record TelemetryEvent
{
    public DateTime Timestamp { get; init; }
    public string Source { get; init; }
    public int Severity { get; init; }
    public string Message { get; init; }
    public string Channel { get; init; }
}



//TODO: Define ITelemetryEmitter interface and TelemetryEvent class as per your application's requirements.
public interface ITelemetryEmitter
{
    void Emit(TelemetryEvent Evt);
}