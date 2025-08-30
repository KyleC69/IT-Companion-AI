// Project Name: LightweightAI.Core
// File Name: ServiceRecord.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace LightweightAI.Core.Loaders.Services;


public sealed class ServiceRecord
{
    // Identity
    public string ServiceName { get; set; } = "";
    public string DisplayName { get; set; } = "";

    // Configuration and state
    public string? StartMode { get; set; } // Boot|System|Automatic|Manual|Disabled
    public string ServiceType { get; set; } = ""; // OwnProcess|ShareProcess|KernelDriver|FileSystemDriver|...
    public string Status { get; set; } = ""; // Running|Stopped|Paused|...

    // Binary details
    public string? BinaryPath { get; set; }
    public string? BinaryPathHash { get; set; } // SHA-256 hex
    public string? DigitalSignature { get; set; } // Signer CN/Subject
    public bool? SignatureValid { get; set; } // CA chain validation result
    public DateTime? LastChangeUtc { get; set; } // Executable last write time

    // Anomalies
    public bool IsZombie { get; set; } // SCM entry without registry key

    // Provenance
    public string Host { get; set; } = "";
    public string SourceId { get; set; } = "";
    public string LoaderName { get; set; } = "";
    public string SchemaVersion { get; set; } = "";
    public string CollectionMethod { get; set; } = "";
    public string RecordId { get; set; } = "";

    // Change tracking
    public string ChangeType { get; set; } = ""; // Added|Removed|Modified|Unchanged|Zombie
    public string? PreviousStartMode { get; set; }
    public string? PreviousStatus { get; set; }
    public string? PreviousBinaryPathHash { get; set; }
    public bool? PreviousSignatureValid { get; set; }
}