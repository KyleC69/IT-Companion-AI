// Project Name: LightweightAI.Core
// File Name: PerfCounterLoader.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using System.Diagnostics;
using System.Text.RegularExpressions;

using LightweightAI.Core.Interfaces;


namespace LightweightAI.Core.Loaders.Windows;


public sealed class PerfCounterLoader(PerfCounterLoaderConfig config, IMetricSink sink, ILoggerSeverity log)
    : IDisposable
{
    private const string SchemaVersion = "1.1";
    private const string CollectionMethod = "Perfmon API";
    private const string SourceId = "perfmon";
    private const string Loader = nameof(PerfCounterLoader);

    private static readonly TimeSpan DefaultInstanceRefresh = TimeSpan.FromMinutes(5);

    // Supports:
    //  \Category\Counter
    //  \Category(Instance)\Counter
    //  \Category(*)\Counter
    private static readonly Regex PathRx = new(
        @"^\\?(?<cat>[^\\(]+?)(\((?<inst>.*)\))?\\(?<ctr>.+)$",
        RegexOptions.Compiled | RegexOptions.CultureInvariant);

    private readonly PerfCounterLoaderConfig _config = config ?? throw new ArgumentNullException(nameof(config));

    // Live counter handles, keyed by exact triplet
    private readonly Dictionary<PerfCounterKey, CounterHandle> _handles = new();

    // For delta-only emission
    private readonly Dictionary<PerfCounterKey, double> _lastValues = new();
    private readonly ILoggerSeverity _log = log ?? throw new ArgumentNullException(nameof(log));
    private readonly IMetricSink _sink = sink ?? throw new ArgumentNullException(nameof(sink));

    // Parsed specs from user-provided counter paths (supports wildcards)
    private readonly List<CounterSpec> _specs = ParseSpecs(config.Paths ?? new List<string>());

    private bool _disposed;

    // For wildcard instance refresh throttling
    private DateTime _nextInstanceRefreshUtc = DateTime.UtcNow;





    public void Dispose()
    {
        if (this._disposed) return;
        foreach (CounterHandle h in this._handles.Values)
            h.Dispose();
        this._handles.Clear();
        this._disposed = true;
    }





    public async Task StartAsync(CancellationToken ct)
    {
        this._log.Info(
            $"{Loader} starting. Counters={this._specs.Count}, Interval={this._config.SampleInterval}, BatchSize={this._config.BatchSize}, DeltaOnly={this._config.DeltaOnly}");

        // Initial realization of counters
        RefreshInstancesAndBuildHandles(true);

        // Warm-up read for counters that require two samples
        WarmupCounters();

        while (!ct.IsCancellationRequested)
            try
            {
                // Refresh wildcard instances on cadence
                if (DateTime.UtcNow >= this._nextInstanceRefreshUtc)
                {
                    RefreshInstancesAndBuildHandles(false);
                    this._nextInstanceRefreshUtc =
                        DateTime.UtcNow + (this._config.InstanceRefreshInterval ?? DefaultInstanceRefresh);
                }

                List<PerfCounterSample> samples = CollectOneCycle();

                // Optional delta filter to reduce noise
                if (this._config.DeltaOnly)
                    samples = ApplyDeltaFilter(samples, this._config.DeltaEpsilon);

                // Batch emit
                if (samples.Count > 0)
                    await this._sink.EmitBatchAsync(samples, ct).ConfigureAwait(false);

                await Task.Delay(this._config.SampleInterval, ct).ConfigureAwait(false);
            }
            catch (OperationCanceledException)
            {
                // graceful stop
                break;
            }
            catch (Exception ex)
            {
                this._log.Error($"{Loader} loop error: {ex.Message}");
                if (this._config.FailFast)
                    throw;
                await Task.Delay(TimeSpan.FromSeconds(2), ct).ConfigureAwait(false);
            }

        this._log.Info($"{Loader} stopped.");
    }





    // -------------------------
    // Collection
    // -------------------------





    private List<PerfCounterSample> CollectOneCycle()
    {
        List<PerfCounterSample> batch = new(Math.Min(this._handles.Count, this._config.BatchSize));
        var errors = 0;

        foreach ((PerfCounterKey key, CounterHandle handle) in this._handles)
        {
            if (this._config.MaxErrorsPerCycle > 0 && errors >= this._config.MaxErrorsPerCycle)
            {
                this._log.Warn(
                    $"{Loader} reached MaxErrorsPerCycle={this._config.MaxErrorsPerCycle}. Skipping remaining counters this cycle.");
                break;
            }

            double? value = null;
            var valueStatus = "Good";
            string? error = null;

            try
            {
                var v = handle.Counter.NextValue();
                if (double.IsNaN(v))
                    valueStatus = "NaN";
                else if (double.IsInfinity(v))
                    valueStatus = "Infinity";
                else
                    value = v;
            }
            catch (InvalidOperationException ex)
            {
                valueStatus = "Error";
                error = "InvalidOperation";
                errors++;
                this._log.Warn($"{Loader} counter unavailable: {key} - {ex.Message}");
            }
            catch (UnauthorizedAccessException ex)
            {
                valueStatus = "Error";
                error = "AccessDenied";
                errors++;
                this._log.Error($"{Loader} access denied: {key} - {ex.Message}");
            }
            catch (Exception ex)
            {
                valueStatus = "Error";
                error = ex.GetType().Name;
                errors++;
                this._log.Error($"{Loader} unexpected read error: {key} - {ex.Message}");
            }

            var unit = UnitRegistry.ResolveUnit(key.Category, key.CounterName);
            DateTime now = DateTime.UtcNow;

            var sample = new PerfCounterSample
            {
                Category = key.Category,
                Instance = key.Instance,
                CounterName = key.CounterName,
                Value = value,
                Unit = unit,
                UtcTimestamp = now,
                Host = Environment.MachineName,
                SourceId = SourceId,
                LoaderName = Loader,
                SchemaVersion = SchemaVersion,
                CollectionMethod = CollectionMethod,
                RecordId = Guid.NewGuid().ToString(),
                ValueStatus = valueStatus,
                Error = error
            };

            // Audit log (self-contained per record)
            if (this._config.AuditLog)
                this._log.Debug(
                    $"{Loader} audit Category='{sample.Category}' Instance='{sample.Instance}' Counter='{sample.CounterName}' Unit='{sample.Unit}' Schema='{SchemaVersion}' Method='{CollectionMethod}' Status='{sample.ValueStatus}'");

            batch.Add(sample);

            if (batch.Count >= this._config.BatchSize)
            {
                // Emit intermediate batch synchronously via sink buffer
                this._sink.EmitBatchAsync(batch, CancellationToken.None).GetAwaiter().GetResult();
                batch.Clear();
            }
        }

        return batch;
    }





    private List<PerfCounterSample> ApplyDeltaFilter(List<PerfCounterSample> samples, double epsilon)
    {
        List<PerfCounterSample> filtered = new(samples.Count);
        foreach (PerfCounterSample s in samples)
        {
            var key = new PerfCounterKey(s.Category, s.Instance, s.CounterName);

            if (s.ValueStatus != "Good" || s.Value is null)
            {
                // Always pass through bad/diagnostic states for observability
                filtered.Add(s);
                continue;
            }

            if (!this._lastValues.TryGetValue(key, out var last))
            {
                this._lastValues[key] = s.Value.Value;
                filtered.Add(s); // first observation per key
                continue;
            }

            if (Math.Abs(s.Value.Value - last) >= epsilon)
            {
                this._lastValues[key] = s.Value.Value;
                filtered.Add(s);
            }
            // else drop as noise
        }

        return filtered;
    }





    // -------------------------
    // Handle management
    // -------------------------





    private void RefreshInstancesAndBuildHandles(bool force)
    {
        Dictionary<PerfCounterKey, CounterHandle> realized = new();

        foreach (CounterSpec spec in this._specs)
        {
            IEnumerable<string> instances = ExpandInstances(spec);

            foreach (var instance in instances)
            {
                var key = new PerfCounterKey(spec.Category, instance, spec.Counter);
                if (realized.ContainsKey(key)) continue;

                if (this._handles.TryGetValue(key, out CounterHandle? existing) && !force)
                {
                    realized[key] = existing;
                    continue;
                }

                try
                {
                    var pc = new PerformanceCounter(spec.Category, spec.Counter, instance, readOnly: true);
                    var handle = new CounterHandle(key, pc);
                    realized[key] = handle;
                }
                catch (Exception ex)
                {
                    this._log.Warn($"{Loader} failed to create counter: {key} - {ex.Message}");
                }
            }
        }

        // Dispose handles that are no longer realized (e.g., wildcard instance disappeared)
        foreach (PerfCounterKey obsolete in this._handles.Keys.Except(realized.Keys).ToList())
        {
            this._handles[obsolete].Dispose();
            this._handles.Remove(obsolete);
        }

        // Add or update realized handles
        foreach (KeyValuePair<PerfCounterKey, CounterHandle> kvp in realized) this._handles[kvp.Key] = kvp.Value;

        if (this._config.MaxCounters > 0 && this._handles.Count > this._config.MaxCounters)
        {
            this._log.Warn(
                $"{Loader} realized {this._handles.Count} counters exceeds MaxCounters={this._config.MaxCounters}. Truncating.");
            // Keep deterministic subset by ordering
            HashSet<PerfCounterKey> keep = this._handles.Keys.OrderBy(k => k.Category).ThenBy(k => k.CounterName)
                .ThenBy(k => k.Instance).Take(this._config.MaxCounters).ToHashSet();
            foreach (PerfCounterKey k in this._handles.Keys.ToList())
                if (!keep.Contains(k))
                {
                    this._handles[k].Dispose();
                    this._handles.Remove(k);
                }
        }

        this._log.Info($"{Loader} realized counters: {this._handles.Count}");
    }





    private void WarmupCounters()
    {
        if (this._config.WarmupReads <= 0) return;

        this._log.Debug($"{Loader} warm-up: {this._config.WarmupReads} reads");
        for (var i = 0; i < this._config.WarmupReads; i++)
        {
            foreach (CounterHandle h in this._handles.Values)
                try
                {
                    _ = h.Counter.NextValue();
                }
                catch
                {
                    /* swallow warm-up errors */
                }

            if (i < this._config.WarmupReads - 1)
                Thread.Sleep(TimeSpan.FromMilliseconds(200));
        }
    }





    // -------------------------
    // Spec parsing and expansion
    // -------------------------





    private static List<CounterSpec> ParseSpecs(IEnumerable<string> paths)
    {
        List<CounterSpec> specs = new();
        foreach (var p in paths)
        {
            if (string.IsNullOrWhiteSpace(p)) continue;
            if (!TryParsePath(p.Trim(), out CounterSpec spec, out var error))
                throw new ArgumentException($"Invalid counter path '{p}': {error}");
            specs.Add(spec);
        }

        return specs;
    }





    private static bool TryParsePath(string path, out CounterSpec spec, out string? error)
    {
        Match m = PathRx.Match(path);
        if (!m.Success)
        {
            spec = default;
            error = "Path must be \\Category\\Counter or \\Category(Instance)\\Counter";
            return false;
        }

        var cat = m.Groups["cat"].Value.Trim();
        var inst = m.Groups["inst"].Success ? m.Groups["inst"].Value.Trim() : string.Empty;
        var ctr = m.Groups["ctr"].Value.Trim();

        if (string.IsNullOrWhiteSpace(cat) || string.IsNullOrWhiteSpace(ctr))
        {
            spec = default;
            error = "Category and Counter must be non-empty";
            return false;
        }

        spec = new CounterSpec(cat, ctr, inst);
        error = null;
        return true;
    }





    private static IEnumerable<string> ExpandInstances(CounterSpec spec)
    {
        // No instance specified: single-instance category
        if (string.IsNullOrEmpty(spec.Instance) || spec.Instance == " ")
            return new[] { string.Empty };

        // Explicit instance (no wildcard)
        if (spec.Instance != "*")
            return new[] { spec.Instance };

        // Wildcard: enumerate instances
        try
        {
            var cat = new PerformanceCounterCategory(spec.Category);
            if (!cat.CategoryType.HasFlag(PerformanceCounterCategoryType.MultiInstance))
                return new[] { string.Empty };

            var names = cat.GetInstanceNames();
            return names.Length > 0 ? names : new[] { string.Empty };
        }
        catch
        {
            // Fallback if enumeration fails
            return new[] { string.Empty };
        }
    }





    // -------------------------
    // Data types
    // -------------------------



    private readonly struct CounterSpec(string category, string counter, string instance)
    {
        public string Category { get; } = category;
        public string Counter { get; } = counter;
        public string Instance { get; } = instance; // may be "", or "*"
    }



    private sealed class CounterHandle(PerfCounterKey key, PerformanceCounter counter) : IDisposable
    {
        public PerfCounterKey Key { get; } = key;
        public PerformanceCounter Counter { get; } = counter;





        public void Dispose()
        {
            try
            {
                this.Counter?.Dispose();
            }
            catch
            {
                /* ignore */
            }
        }
    }
}