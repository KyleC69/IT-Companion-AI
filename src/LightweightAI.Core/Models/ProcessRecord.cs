// Project Name: LightweightAI.Core
// File Name: ProcessRecord.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace LightweightAI.Core.Models;


public partial class ProcessRecord
{
    // Identity and relationships
    public int ProcessId { get; set; }
    public int? ParentProcessId { get; set; }
    public string ProcessName { get; set; } = "";
    public string? MainWindowTitle { get; set; }

    // Binary and execution
    public string? ExecutablePath { get; set; }
    public string? BinaryHash { get; set; } // SHA-256 hex
    public string? DigitalSignature { get; set; } // Signer CN/Subject
    public bool? SignatureValid { get; set; } // CA chain validation result
    public DateTime? StartTimeUtc { get; set; }
    public string? CommandLine { get; set; }
    public int? SessionId { get; set; }

    // User
    public string? UserSid { get; set; }
    public string? UserName { get; set; }

    // Provenance
    public string Host { get; set; } = "";
    public string SourceId { get; set; } = "";
    public string LoaderName { get; set; } = "";
    public string SchemaVersion { get; set; } = "";
    public string CollectionMethod { get; set; } = "";
    public string RecordId { get; set; } = "";

    // Change tracking
    public string ChangeType { get; set; } = ""; // Added|Removed|Modified|Unchanged
    public string? PreviousBinaryHash { get; set; }
    public bool? PreviousSignatureValid { get; set; }
}

// Add missing members used by analyzers / correlators

public partial class ProcessRecord
{
    public int Pid { get; set; }
    public DateTime? StartTime { get; set; }
}