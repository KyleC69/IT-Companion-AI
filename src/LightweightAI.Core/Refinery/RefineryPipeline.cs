// Project Name: LightweightAI.Core
// File Name: RefineryPipeline.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using System.Text.RegularExpressions;


namespace LightweightAI.Core.DataRefineries
{
    // Shared EventId enforcement
    public static class EventIdValidator
    {
        // <machine>.<log/event source>.<rawID> — enforce this format
        private static readonly Regex Pattern =
            new(@"^[^.]+\.[^.]+\.[^.\s]+$", RegexOptions.Compiled);





        public static void Enforce(string eventId)
        {
            if (string.IsNullOrWhiteSpace(eventId))
                throw new ArgumentException("EventId is required");

            if (!Pattern.IsMatch(eventId))
                throw new ArgumentException(
                    $"EventId '{eventId}' does not match <machine>.<source>.<rawId> format");
        }





        public static string Compose(string machine, string source, string rawId)
        {
            return $"{machine}.{source}.{rawId}";
        }
    }



    // ===== METRIC REFINERY =====
    public sealed class MetricRefinery
    {
        public IReadOnlyList<MetricSample> Process(IReadOnlyList<MetricSample> raw)
        {
            if (raw == null || raw.Count == 0)
                throw new ArgumentException("Metric window is empty");

            // Validate EventId for *each* sample
            foreach (MetricSample sample in raw)
                EventIdValidator.Enforce(sample.EventId);

            // Optional: normalize timestamps, scrub NaNs, clamp values, etc.
            return raw;
        }
    }



    // ===== Q&A REFINERY =====
    public sealed class QaRefinery
    {
        public QaQuestion Process(QaQuestion raw)
        {
            if (raw is null)
                throw new ArgumentNullException(nameof(raw));

            EventIdValidator.Enforce(raw.EventId);

            // Optional: sanitize text, trim whitespace, apply casing rules, etc.
            return raw;
        }
    }
}



// ====== SAMPLE DOMAIN TYPES (simplified) ======
public sealed record MetricSample(
    string EventId,
    string MetricKey,
    double Value,
    DateTimeOffset Timestamp,
    string SourceId
);



public sealed record QaQuestion(
    string EventId,
    DateTimeOffset Timestamp,
    string SourceId,
    string Text
);