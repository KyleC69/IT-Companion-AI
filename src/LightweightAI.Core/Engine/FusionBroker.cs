// Project Name: LightweightAI.Core
// File Name: FusionBroker.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using LightweightAI.Core.Engine;


namespace AICompanion.Tests;


// === FusionBroker.cs ===
public class FusionBroker
{
    private readonly IProvenanceLogger _provenanceLogger;
    private readonly IRunner _runner;
    private readonly ISeverityMapper _severityMapper;





    /// <summary>
    ///     Initializes a new instance of the <see cref="FusionBroker" /> class.
    /// </summary>
    /// <param name="severityMapper">
    ///     An implementation of <see cref="ISeverityMapper" /> used to map model outputs to severity levels.
    /// </param>
    /// <param name="provenanceLogger">
    ///     An implementation of <see cref="IProvenanceLogger" /> used to log decisions with provenance metadata.
    /// </param>
    /// <param name="runner">
    ///     An implementation of <see cref="IRunner" /> used to enqueue decision outputs for further processing.
    /// </param>
    public FusionBroker(ISeverityMapper severityMapper,
        IProvenanceLogger provenanceLogger,
        IRunner runner)
    {
        this._severityMapper = severityMapper;
        this._provenanceLogger = provenanceLogger;
        this._runner = runner;
    }





    /// <summary>
    ///     Processes the provided metrics and logs a decision with provenance metadata.
    /// </summary>
    /// <param name="metrics">
    ///     The metrics associated with the decision, including score and additional data.
    /// </param>
    /// <param name="fusionSignature">
    ///     A unique identifier representing the fusion process that generated the metrics.
    /// </param>
    /// <param name="modelId">
    ///     The identifier of the model used to generate the metrics.
    /// </param>
    /// <param name="modelVersion">
    ///     The version of the model used to generate the metrics.
    /// </param>
    public void Process(
        MetricDecision metrics,
        string fusionSignature,
        string modelId,
        string modelVersion)
    {
        var decision = new ProvenancedDecision
        {
            Metrics = metrics,
            EventId = 0,
            FusionSignature = fusionSignature,
            ModelId = modelId,
            ModelVersion = modelVersion,
            Severity = metrics.Score, //  HACK: Missing scale mapping
            Timestamp = DateTime.UtcNow
        };

        this._provenanceLogger.Log(decision);
    }





    // SeverityMapper.cs
    public class SeverityMapper : ISeverityMapper
    {
        public int MapSeverity(object modelOutput, ConfigSnapshot config)
        {
            var key = modelOutput?.ToString() ?? "UNKNOWN";
            return config.SeverityMap.TryGetValue(key, out var score)
                ? score
                : 0;
        }
    }



    // === FusionResult.cs ===
    public class FusionResult
    {
        public required string EventId { get; set; }

        // Hash of the exact input set (for provenance)
        public required string InputHash { get; set; }

        public required string ModelId { get; set; }
        public required string ModelVersion { get; set; }

        // The raw output from the model stage (type kept generic here)
        public required object ModelOutput { get; set; }
    }



    // === ConfigSnapshot.cs ===
    public class ConfigSnapshot
    {
        // Incrementing revision number or hash so we can tie decisions to a config state
        public int Revision { get; set; }

        // Cache of severity mapping or any other lookup tables
        public Dictionary<string, int> SeverityMap { get; set; } = new();

        // Timestamp of when this snapshot was loaded
        public DateTime LoadedAtUtc { get; set; }

        // Optional: anything else the broker stage might need without re‑querying
        public Dictionary<string, object> Settings { get; set; } = new();
    }



    // ISeverityMapper.cs
}