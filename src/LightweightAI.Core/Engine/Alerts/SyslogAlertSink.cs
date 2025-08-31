// Project Name: LightweightAI.Core
// File Name: SyslogAlertSink.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using System.Net;
using System.Net.Sockets;
using System.Text;



namespace LightweightAI.Core.Engine.Alerts;


/// <summary>
///     UDP syslog sink (RFC 5424 style minimal message). Production: add structured data / TCP option.
/// </summary>
public sealed class SyslogAlertSink(string host, int port = 514, int facility = 1) : IAlertSink
{
    private readonly IPEndPoint _endpoint = new(IPAddress.Parse(host), port);
    public string Name => "syslog";





    public async Task SendAsync(ProvenancedDecision decision, CancellationToken ct = default)
    {
        using var udp = new UdpClient();
        var pri = facility * 8 + 5; // notice level
        var msg =
            $"<{pri}>1 {DateTimeOffset.UtcNow:O} {Environment.MachineName} miniAI - - - risk={decision.Risk:F2} sev={decision.Severity} corr={decision.CorrelationId} sum=\"{decision.Summary}\"";
        var bytes = Encoding.UTF8.GetBytes(msg);
        await udp.SendAsync(bytes, bytes.Length, this._endpoint);
    }
}