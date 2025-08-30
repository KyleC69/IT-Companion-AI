// Project Name: LightweightAI.Core
// File Name: SmtpAlertSink.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using System.Net;
using System.Net.Mail;

using LightweightAI.Core.Engine.Provenance;



namespace LightweightAI.Core.Engine.Alerts;


/// <summary>
///     Minimal SMTP alert sink (plaintext). For production usage add batching, TLS enforcement and templates.
/// </summary>
public sealed class SmtpAlertSink(
    string host,
    int port,
    string from,
    string to,
    string? user = null,
    string? pass = null)
    : IAlertSink
{
    public string Name => "smtp";





    public async Task SendAsync(ProvenancedDecision decision, CancellationToken ct = default)
    {
        using var client = new SmtpClient(host, port);
        if (!string.IsNullOrEmpty(user)) client.Credentials = new NetworkCredential(user, pass);
        var body =
            $"Time: {decision.TimestampUtc:O}\nCorrelation: {decision.CorrelationId}\nRisk: {decision.Risk:F2}\nSeverity: {decision.Severity}\nSummary: {decision.Summary}";
        using var msg = new MailMessage(from, to, $"[AI Alert] {decision.Summary}", body);
        await client.SendMailAsync(msg, ct);
    }
}