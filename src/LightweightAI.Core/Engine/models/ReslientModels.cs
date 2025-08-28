using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace ResilientModels
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading;

	namespace ResilientModels
	{
		// ======================= CLOCK =======================
		public interface IClock
		{
			DateTimeOffset UtcNow { get; }
		}

		public sealed class SystemClock : IClock
		{
			public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
		}

		// ======================= EVENT ID FACTORY =======================
		public static class EventIdFactory
		{
			public static Guid FromThreePartKey(string machine, string source, string rawId)
			{
				if (string.IsNullOrWhiteSpace(machine))
					throw new ArgumentException("Machine segment cannot be null/empty", nameof(machine));
				if (string.IsNullOrWhiteSpace(source))
					throw new ArgumentException("Source segment cannot be null/empty", nameof(source));
				if (string.IsNullOrWhiteSpace(rawId))
					throw new ArgumentException("RawId segment cannot be null/empty", nameof(rawId));

				var compositeKey = $"{machine}.{source}.{rawId}";
				return GuidUtility.Create(GuidUtility.UrlNamespace, compositeKey);
			}
		}

		// ======================= ENVELOPE + METADATA =======================
		public sealed record ModelMetadata(
			string ModelId,
			string ModelVersion,
			string FusionSignature
		);

		public sealed record WindowInfo(DateTimeOffset Start, DateTimeOffset End);

		public sealed record DecisionEnvelope<TPayload>(
			Guid EventId,
			DateTimeOffset Timestamp,
			string SourceId,
			ModelMetadata Model,
			TPayload Payload,
			WindowInfo? Window,
			string? CauseCode,
			IReadOnlyList<string> Warnings,
			TimeSpan ProcessingDuration,
			IReadOnlyDictionary<string, string>? Tags
		);

		// ======================= MODEL CONTRACT =======================
		public interface IModel<in TRequest, TPayload>
		{
			DecisionEnvelope<TPayload> Execute(TRequest request, CancellationToken ct = default);
		}

		public sealed record StrategyResult<TPayload>(
			TPayload Payload,
			WindowInfo? Window = null,
			string? CauseCode = null,
			IReadOnlyList<string>? Warnings = null,
			IReadOnlyDictionary<string, string>? Tags = null
		);

		// ======================= Q&A DTOs =======================
		public sealed record QaQuestion(
			Guid EventId,
			DateTimeOffset Timestamp,
			string SourceId,
			string Text,
			IReadOnlyList<string>? Context = null,
			IReadOnlyDictionary<string, string>? Hints = null
		);

		public sealed record QaAnswerPayload(string Answer, double Confidence);

		public interface IQaStrategy
		{
			StrategyResult<QaAnswerPayload> Answer(QaQuestion question, CancellationToken ct = default);
		}

		// ======================= METRIC DTOs =======================
		public sealed record MetricSample(
			string MetricKey,
			double Value,
			DateTimeOffset Timestamp,
			string SourceId
		);

		public sealed record MetricDecisionPayload(
			string MetricKey,
			double Score,
			bool IsAlert
		);

		public interface IMetricStrategy
		{
			StrategyResult<MetricDecisionPayload> Decide(IReadOnlyList<MetricSample> window, CancellationToken ct = default);
		}

		// ======================= PIPELINE BASE =======================
		public abstract class PipelineBase
		{
			protected readonly IClock Clock;
			protected readonly ModelMetadata Metadata;

			protected PipelineBase(IClock clock, ModelMetadata metadata)
			{
				Clock = clock;
				Metadata = metadata;
			}

			protected DecisionEnvelope<TPayload> BuildEnvelope<TPayload>(
				Guid eventId,
				string sourceId,
				StrategyResult<TPayload> result,
				DateTimeOffset start)
			{
				var now = Clock.UtcNow;
				return new DecisionEnvelope<TPayload>(
					EventId: eventId,
					Timestamp: now,
					SourceId: sourceId,
					Model: Metadata,
					Payload: result.Payload,
					Window: result.Window,
					CauseCode: result.CauseCode,
					Warnings: result.Warnings ?? Array.Empty<string>(),
					ProcessingDuration: now - start,
					Tags: result.Tags
				);
			}
		}

		// ======================= Q&A PIPELINE =======================
		public sealed class QaPipeline : PipelineBase, IModel<QaQuestion, QaAnswerPayload>
		{
			private readonly IQaStrategy _strategy;
			private readonly double _lowConfidenceThreshold;

			public QaPipeline(IQaStrategy strategy, IClock clock, ModelMetadata metadata, double lowConfidenceThreshold = 0.35)
				: base(clock, metadata)
			{
				_strategy = strategy;
				_lowConfidenceThreshold = lowConfidenceThreshold;
			}

			public DecisionEnvelope<QaAnswerPayload> Execute(QaQuestion request, CancellationToken ct = default)
			{
				var t0 = Clock.UtcNow;
				var eventId = request.EventId != Guid.Empty
					? request.EventId
					: EventIdFactory.FromThreePartKey(Environment.MachineName, request.SourceId, request.Timestamp.ToUnixTimeMilliseconds().ToString());

				var result = _strategy.Answer(request, ct);

				if (result.Payload.Confidence < _lowConfidenceThreshold && string.IsNullOrWhiteSpace(result.CauseCode))
				{
					var warnings = result.Warnings?.ToList() ?? new List<string>();
					warnings.Add($"Confidence {result.Payload.Confidence:0.000} below threshold {_lowConfidenceThreshold:0.000}");
					result = result with { CauseCode = "LowConfidence", Warnings = warnings };
				}

				return BuildEnvelope(eventId, request.SourceId, result, t0);
			}
		}

		// ======================= METRIC PIPELINE =======================
		public sealed class MetricPipeline : PipelineBase, IModel<IReadOnlyList<MetricSample>, MetricDecisionPayload>
		{
			private readonly IMetricStrategy _strategy;

			public MetricPipeline(IMetricStrategy strategy, IClock clock, ModelMetadata metadata)
				: base(clock, metadata)
			{
				_strategy = strategy;
			}

			public DecisionEnvelope<MetricDecisionPayload> Execute(IReadOnlyList<MetricSample> window, CancellationToken ct = default)
			{
				if (window == null || window.Count == 0)
					throw new ArgumentException("Metric window is empty.", nameof(window));

				var t0 = Clock.UtcNow;
				var head = window[0];

				var eventId = EventIdFactory.FromThreePartKey(
					Environment.MachineName,
					head.MetricKey,         // log/event source
					head.SourceId           // raw ID
				);

				var result = _strategy.Decide(window, ct);

				if (result.Window is null)
				{
					result = result with { Window = new WindowInfo(window.Min(s => s.Timestamp), window.Max(s => s.Timestamp)) };
				}

				return BuildEnvelope(eventId, head.SourceId, result, t0);
			}
		}

		// ======================= SAMPLE STRATEGIES =======================
		public sealed class KeywordQaStrategy : IQaStrategy
		{
			private readonly Dictionary<string, string> _kb;
			private readonly double _matchConfidence;
			private readonly double _fallbackConfidence;

			public KeywordQaStrategy(IReadOnlyDictionary<string, string> knowledgeBase, double matchConfidence = 0.9, double fallbackConfidence = 0.2)
			{
				_kb = new Dictionary<string, string>(knowledgeBase, StringComparer.OrdinalIgnoreCase);
				_matchConfidence = matchConfidence;
				_fallbackConfidence = fallbackConfidence;
			}

			public StrategyResult<QaAnswerPayload> Answer(QaQuestion question, CancellationToken ct = default)
			{
				foreach (var kvp in _kb)
				{
					if (question.Text.IndexOf(kvp.Key, StringComparison.OrdinalIgnoreCase) >= 0)
					{
						return new StrategyResult<QaAnswerPayload>(
							new QaAnswerPayload(kvp.Value, _matchConfidence),
							CauseCode: "DirectKeywordMatch",
							Tags: new Dictionary<string, string> { ["matchedKeyword"] = kvp.Key }
						);
					}
				}

				return new StrategyResult<QaAnswerPayload>(
					new QaAnswerPayload("I don't have that answer yet.", _fallbackConfidence),
					CauseCode: "FallbackNoMatch",
					Warnings: new[] { "No keyword hit in KB" }
				);
			}
		}

		public sealed class ThresholdMetricStrategy : IMetricStrategy
		{
			private readonly double _threshold;
			private readonly string _modelKey;

			public ThresholdMetricStrategy(double threshold, string modelKey = "metric_threshold_v1")
			{
				_threshold = threshold;
				_modelKey = modelKey;
			}

			public StrategyResult<MetricDecisionPayload> Decide(IReadOnlyList<MetricSample> window, CancellationToken ct = default)
			{
				var metricKey = window[0].MetricKey;
				var avg = window.Average(s => s.Value);
				var max = window.Max(s => s.Value);
				var score = avg;
				var isAlert = max >= _threshold;

				if (Math.Abs(score - _threshold) < 1e-9)
				{
					isAlert = (window.Max(s => s.Timestamp.UtcTicks) % 2 == 0);
				}

				var warnings = new List<string>();
				if (window.Count < 5) warnings.Add("ShortWindow");

				return new StrategyResult<MetricDecisionPayload>(
					new MetricDecisionPayload(metricKey, score, isAlert),
					CauseCode: isAlert ? "ThresholdTrip" : "WithinRange",
					Warnings: warnings,
					Tags: new Dictionary<string, string> {