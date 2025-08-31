// Project Name: LightweightAI.Core
// File Name: DefaultNormalizer.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace LightweightAI.Core.Analyzers;


public sealed class DefaultNormalizer : INormalizer
{
    private static readonly HashSet<string> SeverityValues = new(StringComparer.OrdinalIgnoreCase)
        { "Verbose", "Info", "Warn", "Error", "Critical" };

    private static readonly string[] ProcessIdKeys = { "ProcessId", "Pid", "proc_id" };
    private static readonly string[] UserKeys = { "User", "UserName", "Account", "Sid" };
    private static readonly string[] IpKeys = { "Ip", "IpAddress", "SrcIp", "DestIp" };
    private static readonly string[] PortKeys = { "Port", "SrcPort", "DestPort" };





    public RawEvent Normalize(RawEvent input)
    {
        Dictionary<string, object?> fields = new(input.Fields, StringComparer.OrdinalIgnoreCase);

        // Coerce timestamp drift (future skew > 5 min)
        DateTimeOffset ts = input.TimestampUtc > DateTimeOffset.UtcNow.AddMinutes(5)
            ? DateTimeOffset.UtcNow
            : input.TimestampUtc;

        // Normalize severity (map synonyms)
        var sevNorm = input.Severity switch
        {
            "Information" => "Info",
            "Warning" => "Warn",
            _ => input.Severity
        };
        var sev = SeverityValues.Contains(sevNorm) ? sevNorm : "Info";

        // Fill missing host
        var host = string.IsNullOrWhiteSpace(input.Host) ? "unknown" : input.Host;

        // Coerce numeric strings & known field types
        foreach (var k in fields.Keys.ToList())
        {
            var v = fields[k];
            if (v is string s)
            {
                if (int.TryParse(s, out var i) && (ProcessIdKeys.Contains(k, StringComparer.OrdinalIgnoreCase) ||
                                                   PortKeys.Contains(k, StringComparer.OrdinalIgnoreCase)))
                {
                    fields[k] = i;
                    continue;
                }

                if (double.TryParse(s, out var d)) fields[k] = d;
            }
        }

        // Canonical projected fields (so downstream encoding can rely on consistent keys)
        if (!fields.ContainsKey("ProcessId"))
        {
            var pidKey = ProcessIdKeys.FirstOrDefault(pk => fields.ContainsKey(pk));
            if (pidKey is not null && fields[pidKey] is int pidVal) fields["ProcessId"] = pidVal;
        }

        if (!fields.ContainsKey("UserName"))
        {
            var userKey = UserKeys.FirstOrDefault(uk => fields.ContainsKey(uk));
            if (userKey is not null) fields["UserName"] = fields[userKey];
        }

        return input with { TimestampUtc = ts, Severity = sev, Host = host, Fields = fields };
    }
}