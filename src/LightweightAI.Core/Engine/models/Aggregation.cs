// LightweightAI.Aggregation and Modeling — code only

using System;
using System.Buffers;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Security.Cryptography;

namespace LightweightAI.Core.Models
{
	public readonly record struct EventRecord(
		string SourceId,
		string Provider,
		string EventId,
		string Actor,
		string Subject,
		string Host,
		DateTimeOffset Timestamp,
		long Sequence,
		int Severity,
		string PayloadHash,
		ImmutableDictionary<string, string> Fields
	);



	public readonly record struct TrendPoint(DateTimeOffset At, double Value);

	public readonly record struct AnomalySignal(
		DateTimeOffset At,
		double Value,
		double ZScore,
		bool IsAnomaly
	);

	public sealed record AggregatorConfig(
		TimeSpan Window,
		ImmutableArray<string> GroupBy,
		ImmutableDictionary<string, double> DimensionWeights
	);

	public sealed record TrendConfig(
		int Window,
		double Alpha
	);

	public sealed record AnomalyConfig(
		double Alpha,
		double ZThreshold,
		double MinVariance
	);

	public sealed class RefineryContext
	{
		public string SourceId { get; init; } = "";
		public string Provider { get; init; } = "";
		public DateTimeOffset Now { get; init; } = DateTimeOffset.UtcNow;
		public ImmutableDictionary<string, string> Hints { get; init; } = ImmutableDictionary<string, string>.Empty;
	}
}

namespace LightweightAI.Core.Abstractions
{
	using LightweightAI.Core.Models;

	public interface IDataRefineryStep
	{
		EventRecord Execute(in EventRecord e, in RefineryContext ctx);
	}

	public interface IDataRefineryPipeline
	{
		EventRecord Process(in EventRecord e, in RefineryContext ctx);
	}

	public interface IAggregator
	{
		IEnumerable<AggregatedMetric> Aggregate(IEnumerable<EventRecord> events, AggregatorConfig cfg);
	}

	public interface IUnifiedAggregator
	{
		IEnumerable<AggregatedMetric> AggregateUnified(IEnumerable<EventRecord> events, AggregatorConfig cfg);
	}

	public interface ISnapshotTrendModel
	{
		Snapshot Update(in TrendPoint p);
		Snapshot Current { get; }
	}

	public interface IStreamAnomalyModel
	{
		AnomalySignal Update(in TrendPoint p);
	}
}

namespace LightweightAI.Refinery
{
	using LightweightAI.Core.Abstractions;
	using LightweightAI.Core.Models;

	public sealed class DataRefineryPipeline : IDataRefineryPipeline
	{
		private readonly ImmutableArray<IDataRefineryStep> _steps;

		public DataRefineryPipeline(IEnumerable<IDataRefineryStep> steps)
		{
			_steps = steps.ToImmutableArray();
		}

		public EventRecord Process(in EventRecord e, in RefineryContext ctx)
		{
			var cur = e;
			foreach (var step in _steps)
			{
				cur = step.Execute(cur, ctx);
			}
			return cur;
		}
	}

	public sealed class NormalizeFieldsStep : IDataRefineryStep
	{
		public EventRecord Execute(in EventRecord e, in RefineryContext ctx)
		{
			var map = e.Fields;
			if (!map.ContainsKey("actor") && !string.IsNullOrWhiteSpace(e.Actor))
				map = map.SetItem("actor", e.Actor);
			if (!map.ContainsKey("host") && !string.IsNullOrWhiteSpace(e.Host))
				map = map.SetItem("host", e.Host);
			if (!map.ContainsKey("provider") && !string.IsNullOrWhiteSpace(e.Provider))
				map = map.SetItem("provider", e.Provider);
			return e with { Fields = map };
		}
	}

	public sealed class EnrichProvenanceStep : IDataRefineryStep
	{
		public EventRecord Execute(in EventRecord e, in RefineryContext ctx)
		{
			var map = e.Fields
				.SetItem("source_id", e.SourceId)
				.SetItem("provider", e.Provider)
				.SetItem("event_id", e.EventId)
				.SetItem("seq", e.Sequence.ToString());
			return e with { Fields = map };
		}
	}

	public sealed class DeduplicateStep : IDataRefineryStep
	{
		private static string StableHash(string sourceId, string provider, string eventId, long seq, string payloadHash)
		{
			var input = $"{sourceId}|{provider}|{eventId}|{seq}|{payloadHash}";
			var bytes = System.Text.Encoding.UTF8.GetBytes(input);
			Span<byte> hash = stackalloc byte[32];
			SHA256.HashData(bytes, hash);
			return Convert.ToHexString(hash);
		}

		public EventRecord Execute(in EventRecord e, in RefineryContext ctx)
		{
			var collisionKey = StableHash(e.SourceId, e.Provider, e.EventId, e.Sequence, e.PayloadHash);
			return e with { Fields = e.Fields.SetItem("collision_key", collisionKey) };
		}
	}
}

namespace LightweightAI.Aggregation
{
	using LightweightAI.Core.Abstractions;
	using LightweightAI.Core.Models;

	public sealed class DeterministicAggregator : IAggregator
	{
		public IEnumerable<AggregatedMetric> Aggregate(IEnumerable<EventRecord> events, AggregatorConfig cfg)
		{
			var ordered = events
				.OrderBy(e => e.Timestamp)
				.ThenBy(e => e.Sequence)
				.ThenBy(e => e.PayloadHash, StringComparer.Ordinal);

			var byWindow = ordered.GroupBy(e =>
			{
				var startTicks = e.Timestamp.UtcDateTime.Ticks - (e.Timestamp.UtcDateTime.Ticks % cfg.Window.Ticks);
				var start = new DateTimeOffset(new DateTime(startTicks, DateTimeKind.Utc));
				var end = start.Add(cfg.Window);
				return (start, end);
			});

			foreach (var win in byWindow)
			{
				var grouped = win.GroupBy(e => BuildKey(e, cfg.GroupBy));
				foreach (var g in grouped)
				{
					long count = 0;
					double weighted = 0d;
					var dims = ImmutableDictionary<string, double>.Empty.ToBuilder();

					foreach (var e in g)
					{
						count++;
						foreach (var kv in cfg.DimensionWeights)
						{
							if (e.Fields.TryGetValue(kv.Key, out var s) && double.TryParse(s, out var v))
							{
								if (dims.ContainsKey(kv.Key)) dims[kv.Key] += v * kv.Value;
								else dims[kv.Key] = v * kv.Value;
							}
						}
						weighted += e.Severity;
					}

					yield return new AggregatedMetric(
						Key: g.Key,
						WindowStart: win.Key.start,
						WindowEnd: win.Key.end,
						Count: count,
						WeightedScore: weighted,
						Dimensions: dims.ToImmutable()
					);
				}
			}
		}

		private static string BuildKey(in EventRecord e, ImmutableArray<string> fields)
		{
			if (fields.Length == 0) return "all";
			var parts = ArrayPool<string>.Shared.Rent(fields.Length);
			try
			{
				for (int i = 0; i < fields.Length; i++)
				{
					var f = fields[i];
					e.Fields.TryGetValue(f, out var v);
					parts[i] = $"{f}={v ?? ""}";
				}
				return string.Join("|", parts.AsSpan(0, fields.Length).ToArray());
			}
			finally
			{
				ArrayPool<string>.Shared.Return(parts, clearArray: true);
			}
		}
	}

	public sealed class UnifiedAggregator : IUnifiedAggregator
	{
		private readonly ImmutableArray<IAggregator> _aggregators;

		public UnifiedAggregator(IEnumerable<IAggregator> aggregators)
		{
			_aggregators = aggregators.ToImmutableArray();
		}

		public IEnumerable<AggregatedMetric> AggregateUnified(IEnumerable<EventRecord> events, AggregatorConfig cfg)
		{
			var results = new List<AggregatedMetric>(_aggregators.Length * 8);
			foreach (var agg in _aggregators)
			{
				results.AddRange(agg.Aggregate(events, cfg));
			}

			foreach (var m in results
				.OrderBy(m => m.WindowStart)
				.ThenBy(m => m.Key, StringComparer.Ordinal))
			{
				yield return m;
			}
		}
	}
}

namespace LightweightAI.Modeling
{
	using LightweightAI.Core.Abstractions;
	using LightweightAI.Core.Models;

	using System.Runtime.CompilerServices;

	public sealed class SnapshotTrendModel : ISnapshotTrendModel
	{
		private readonly TrendConfig _cfg;
		private readonly Queue<TrendPoint> _window;
		private double _sum;
		private double _sumSquares;
		private double _ema;
		private TrendPoint _last;
		private bool _hasLast;

		public SnapshotTrendModel(TrendConfig cfg)
		{
			_cfg = cfg;
			_window = new Queue<TrendPoint>(Math.Max(4, cfg.Window));
			_sum = 0d;
			_sumSquares = 0d;
			_ema = 0d;
			_hasLast = false;
			_last = new TrendPoint(DateTimeOffset.MinValue, 0);
			Current = new Snapshot(DateTimeOffset.MinValue, 0, 0, 0, 0, 0, 0);
		}

		public Snapshot Current { get; private set; }

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Snapshot Update(in TrendPoint p)
		{
			if (_window.Count == 0)
			{
				_ema = p.Value;
			}
			else
			{
				_ema = _cfg.Alpha * p.Value + (1 - _cfg.Alpha) * _ema;
			}

			_window.Enqueue(p);
			_sum += p.Value;
			_sumSquares += p.Value * p.Value;

			if (_window.Count > _cfg.Window)
			{
				var old = _window.Dequeue();
				_sum -= old.Value;
				_sumSquares -= old.Value * old.Value;
			}

			var n = _window.Count;
			var mean = _sum / Math.Max(1, n);
			var variance = Math.Max(0, (_sumSquares / Math.Max(1, n)) - (mean * mean));
			var std = Math.Sqrt(variance);

			var trendDelta = 0d;
			if (_hasLast) trendDelta = p.Value - _last.Value;
			_last = p;
			_hasLast = true;

			Current = new Snapshot(
				At: p.At,
				Count: n,
				Sum: _sum,
				Mean: mean,
				StdDev: std,
				Ema: _ema,
				TrendDelta: trendDelta
			);

			return Current;
		}
	}

	public sealed class StreamAnomalyModel : IStreamAnomalyModel
	{
		private readonly AnomalyConfig _cfg;
		private double _ema;
		private double _emaSq;
		private bool _init;

		public StreamAnomalyModel(AnomalyConfig cfg)
		{
			_cfg = cfg;
			_ema = 0d;
			_emaSq = 0d;
			_init = false;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public AnomalySignal Update(in TrendPoint p)
		{
			if (!_init)
			{
				_ema = p.Value;
				_emaSq = p.Value * p.Value;
				_init = true;
				return new AnomalySignal(p.At, p.Value, 0, false);
			}

			_ema = _cfg.Alpha * p.Value + (1 - _cfg.Alpha) * _ema;
			_emaSq = _cfg.Alpha * (p.Value * p.Value) + (1 - _cfg.Alpha) * _emaSq;

			var variance = Math.Max(_cfg.MinVariance, _emaSq - _ema * _ema);
			var std = Math.Sqrt(variance);
			var z = std > 0 ? (p.Value - _ema) / std : 0d;
			var isAnom = Math.Abs(z) >= _cfg.ZThreshold;

			return new AnomalySignal(p.At, p.Value, z, isAnom);
		}
	}
}

namespace LightweightAI.Unified
{
	using LightweightAI.Aggregation;
	using LightweightAI.Core.Abstractions;
	using LightweightAI.Core.Models;
	using LightweightAI.Modeling;

	public static class UnifiedAggregatorFactory
	{
		public static IUnifiedAggregator CreateDefault()
		{
			return new UnifiedAggregator(new IAggregator[]
			{
				new DeterministicAggregator()
			});
		}
	}

	public sealed class DataRefineries
	{
		public static IDataRefineryPipeline CreateDefaultPipeline()
		{
			return new Refinery.DataRefineryPipeline(new IDataRefineryStep[]
			{
				new Refinery.NormalizeFieldsStep(),
				new Refinery.EnrichProvenanceStep(),
				new Refinery.DeduplicateStep()
			});
		}
	}

	public sealed class Models
	{
		public static ISnapshotTrendModel CreateSnapshotTrend(TrendConfig cfg) => new SnapshotTrendModel(cfg);
		public static IStreamAnomalyModel CreateStreamAnomaly(AnomalyConfig cfg) => new StreamAnomalyModel(cfg);
	}
}