// Project Name: LightweightAI.Core
// File Name: WebhookAlertSink.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using System.Net.Http.Json;

using LightweightAI.Core.Engine.Provenance;



namespace LightweightAI.Core.Engine.Alerts;


/// <summary>
///     Simple JSON POST webhook sink for alert forwarding.
/// </summary>
public sealed class WebhookAlertSink(string url) : IAlertSink, IDisposable
{
    private readonly HttpClient _http = new();
    public string Name => "webhook";





    public void Dispose()
    {
        this._http.Dispose();
    }





    public async Task SendAsync(ProvenancedDecision decision, CancellationToken ct = default)
    {
        await this._http.PostAsJsonAsync(url, decision, ct);
    }
}