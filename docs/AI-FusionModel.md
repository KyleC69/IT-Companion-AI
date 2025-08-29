# =====================================================================
# Document Title   : Fusion Model Specification
# Version          : v0.1.0
# Date Created     : 2025-08-28
# Last Modified    : 2025-08-28
# Author(s)        : Unknown
# Reviewed By      : 
# Status           : Draft
# Purpose          : Specification of fast/slow models, rulepack, and fusion scoring with hysteresis.
# Related Docs     : AI-Overview.md; AI-TechnicalSpecs.md; AI-TrainingDataPresets.md
# Change Log       :
#   - 2025-08-28 | System | Initial header applied
# =====================================================================
**Fusion of fast-twitch and slow-burn risk models with rule-based scoring**


**Creating risk score model**

I need to craft a formula for fused risk, combining two risk scores \(r_f\) and \(r_s\). The fused risk score \(R\) uses a logit transformation and adds rule-based scoring. There should be hysteresis thresholds \(T_{ok}<T_warn<T_alert\) and dwell time factors. I'll also mention example features briefly from previous discussions. 

## Architecture overview

- **Fast‑twitch stream model**
  - **Purpose:** Catch bursts, spikes, and sudden config flips from raw events.
  - **Input:** Event stream features (counts, severities, deltas) over short windows.
  - **Output:** Risk score \(r_f \in [0,1]\) with top contributing signals.

- **Slow‑burn snapshot model**
  - **Purpose:** Detect drift and chronic risk from your aggregated snapshots.
  - **Input:** Daily/hourly “health snapshots” (rolled metrics across sources).
  - **Output:** Risk score \(r_s \in [0,1]\) with feature attributions.

- **Rulepack layer**
  - **Purpose:** Enforce non‑negotiables and encode domain knowledge.
  - **Input:** Deterministic checks (e.g., “Unsigned kernel driver”).
  - **Output:** Rule score \(r_{rules} \in [0,1]\) plus triggered rules.

- **Fusion and decisioning**
  - **Purpose:** Combine \(r_f, r_s, r_{rules}\) into one verdict and rationale.
  - **Output:** Final risk \(R \in [0,1]\), state (OK/Warn/Alert), and explanation.

---

## Models and features

- **Fast‑twitch stream model**
  - **Signals:**  
    - **Rate shifts:** EWMA residuals, robust z‑scores on error counts.  
    - **Bursts:** Max within rolling window, interarrival‑time anomalies.  
    - **Config flips:** Firewall rule changes, service state toggles, driver installs.
  - **Windows:**  
    - **Short:** \(5\text{–}15\) minutes for spikes.  
    - **Medium:** \(1\text{–}6\) hours for sustained deviations.

- **Slow‑burn snapshot model**
  - **Signals:**  
    - **Exposure:** Open ports vs. policy, external surface changes.  
    - **Hygiene:** Patch age, `OldestDriverAgeDays`, `UnsignedDriverCount`.  
    - **Health:** `CriticalErrorCount_24h`, `ServiceRestartBurst`.  
    - **Behavior:** Outbound volume to rare ASNs, auth failure ratio.
  - **Detection:**  
    - **Robust baselines:** Median/MAD trend z‑scores.  
    - **Isolation‑style scoring:** Isolation Forest or one‑class SVM (resource‑bound).

- **Rulepack**
  - **Hard fails:** Unsigned kernel driver, AV disabled, firewall off on public profile.
  - **Soft fails:** New admin user outside maintenance window, RDP exposed externally.
  - **Policy deltas:** High‑risk rule changes without change ticket.
## Fusion and decisioning

- **Calibrate individual scores**
  
  - **Goal:** Make \(r_f\) and \(r_s\) comparable. Use a small calibration set and fit Platt scaling or isotonic regression to map raw model scores to \([0,1]\).

- **Fuse with logit stacking**

  - Let \(\mathrm{logit}(p)=\ln\!\left(\frac{p}{1-p}\right)\) and \(\sigma(x)=\frac{1}{1+e^{-x}}\).

  - \[
    R \;=\; \sigma\!\Big(\alpha \cdot \mathrm{logit}(r_f) \;+\; \beta \cdot \mathrm{logit}(r_s) \;+\; \gamma \cdot r_{rules} \;+\; b\Big)
    \]

  - **Weights:**  
    - **\(\alpha\):** Sensitivity to spikes (e.g., \(0.8\)).  
    - **\(\beta\):** Sensitivity to drift (e.g., \(1.2\)).  
    - **\(\gamma\):** Rule leverage (e.g., \(2.5\)).  
    - **\(b\):** Bias term to align with acceptable baseline alert rate.

- **Thresholds and hysteresis**
  - **Bands:**  
    - **OK:** \(R < 0.30\)  
    - **Warn:** \(0.30 \le R < 0.70\)  
    - **Alert:** \(R \ge 0.70\)
  - **Dwell time:** Require \(N\) consecutive windows to escalate; require \(M > N\) healthy windows to de‑escalate (prevents flapping).
  - **Stateful context:** “Alert sustained 42m; drivers: unsigned=2; firewall=on; burst=z=3.1.”

- **Explanation synthesis**
  - **Top causes:** Merge SHAP‑like attributions from both models + fired rules.
  - **Plain English:** “Spike in 4625 failures (z=3.4) + new inbound 3389 rule + AV service stopped 2m → R=0.83 (Alert).”

---

## Implementation sketch in C#

#### Core contracts
```csharp
public record RiskScore(double Value, IReadOnlyList<string> Drivers);

public interface IStreamModel
{
    RiskScore Score(EventBatch batch, DateTime utcNow);
}

public interface ISnapshotModel
{
    RiskScore Score(AggregatedSnapshot snapshot);
}

public interface IRulesEngine
{
    RiskScore Evaluate(UnifiedContext context);
}
```

#### Fast‑twitch stream model (EWMA + robust z)
```csharp
public sealed class StreamAnomalyModel : IStreamModel
{
    private readonly TimeSpan shortWin = TimeSpan.FromMinutes(10);
    private readonly Queue<(DateTime ts, int count)> window = new();

    public RiskScore Score(EventBatch batch, DateTime now)
    {
        int errCount = batch.Count(e => e.IsSecurityError);
        window.Enqueue((now, errCount));
        while (window.Peek().ts < now - shortWin) window.Dequeue();

        var counts = window.Select(x => (double)x.count).ToArray();
        double median = counts.OrderBy(x => x).ElementAt(counts.Length/2);
        double mad = counts.Select(x => Math.Abs(x - median)).OrderBy(x => x)
                           .ElementAt(counts.Length/2);
        double robustZ = mad == 0 ? 0 : 0.6745 * (errCount - median) / mad;

        double burst = batch.Count(e => e.IsServiceRestart);
        double zBurst = burst > 0 ? Math.Min(4.0, burst / 3.0) : 0;

        double raw = Sigmoid(0.9 * robustZ + 0.6 * zBurst);
        var reasons = new List<string>();
        if (robustZ >= 2) reasons.Add($"Error spike z={robustZ:F1}");
        if (burst > 0) reasons.Add($"Restart burst={burst}");

        return new RiskScore(raw, reasons);
    }

    private static double Sigmoid(double x) => 1.0 / (1.0 + Math.Exp(-x));
}
```

#### Slow‑burn snapshot model (median/MAD drift)
```csharp
public sealed class SnapshotTrendModel : ISnapshotModel
{
    private readonly int history = 60; // e.g., 60 snapshots
    private readonly Dictionary<string, Queue<double>> series = new();

    public RiskScore Score(AggregatedSnapshot snap)
    {
        var drivers = new List<string>();
        double risk = 0;

        risk += Drift("CriticalErrorCount_24h", snap.CriticalErrorCount_24h, 1.0, drivers);
        risk += Drift("UnsignedDriverCount",   snap.UnsignedDriverCount,   1.4, drivers);
        risk += Drift("ServiceRestartBurst",   snap.ServiceRestartBurst,   0.8, drivers);
        risk = Sigmoid(risk);

        return new RiskScore(risk, drivers);
    }

    private double Drift(string key, double val, double w, List<string> drivers)
    {
        if (!series.TryGetValue(key, out var q)) series[key] = q = new Queue<double>();
        q.Enqueue(val); while (q.Count > history) q.Dequeue();

        var arr = q.ToArray();
        if (arr.Length < 8) return 0;

        double med = Median(arr);
        double mad = Median(arr.Select(x => Math.Abs(x - med)).ToArray()) + 1e-6;
        double z = 0.6745 * (val - med) / mad;
        if (z >= 2) drivers.Add($"{key} drift z={z:F1}");
        return w * Math.Max(0, z - 1.5); // ramp after mild deviation
    }

    private static double Median(double[] a)
        => a.OrderBy(x => x).ElementAt(a.Length / 2);

    private static double Sigmoid(double x) => 1.0 / (1.0 + Math.Exp(-x));
}
```

#### Rulepack and fusion
```csharp
public sealed class RulesEngine : IRulesEngine
{
    public RiskScore Evaluate(UnifiedContext ctx)
    {
        var hits = new List<string>();
        double score = 0;

        if (ctx.UnsignedKernelDrivers > 0) { score += 1.0; hits.Add("Unsigned kernel driver"); }
        if (ctx.FirewallPublicProfileOff) { score += 0.8; hits.Add("Firewall off (public)"); }
        if (ctx.NewAdminCreated && !ctx.InMaintenanceWindow) { score += 0.6; hits.Add("New admin outside window"); }

        return new RiskScore(Math.Min(1.0, score), hits);
    }
}

public sealed class FusionEngine
{
    public double Alpha { get; init; } = 0.8;
    public double Beta  { get; init; } = 1.2;
    public double Gamma { get; init; } = 2.5;
    public double Bias  { get; init; } = -1.0;

    public (double R, string State, string Explanation) Fuse(
        RiskScore fast, RiskScore slow, RiskScore rules)
    {
        double L(double p) => Math.Log(Math.Clamp(p, 1e-6, 1 - 1e-6) / (1 - Math.Clamp(p, 1e-6, 1 - 1e-6)));
        double S(double x) => 1.0 / (1.0 + Math.Exp(-x));

        double x = Alpha * L(fast.Value) + Beta * L(slow.Value) + Gamma * rules.Value + Bias;
        double R = S(x);

        string state = R < 0.30 ? "OK" : R < 0.70 ? "Warn" : "Alert";
        string why = string.Join("; ",
            new[] { "Fast:" }.Concat(fast.Drivers))
            + " | "
            + string.Join("; ", new[] { "Slow:" }.Concat(slow.Drivers))
            + " | "
            + string.Join("; ", new[] { "Rules:" }.Concat(rules.Drivers));

        return (R, state, why);
    }
}
```

---

## Training, evaluation, and drift control

- **Calibration set**
  - **Collect:** 2–4 weeks of snapshots + event windows; label notable incidents.  
  - **Calibrate:** Fit Platt or isotonic maps so \(r_f, r_s\) reflect empirical probabilities.

- **Backtesting**
  - **Inject:** Synthetic anomalies (e.g., scripted service flaps, firewall rule flips).  
  - **Score:** Precision/recall, alert density per day, time‑to‑detect, MTTR proxy.

- **Rolling retraining**
  - **Windows:**  
    - **Fast model:** retrain or re‑fit baselines daily, window length \(= 7\text{–}14\) days.  
    - **Slow model:** update weekly with \(30\text{–}90\) days of snapshots.
  - **Guardrails:** Keep a frozen “golden baseline” to compare performance before promoting.

- **Drift detection**
  - **Data drift:** Population Stability Index or KL divergence on key features.  
  - **Performance drift:** Alert rate change > X% week‑over‑week triggers review.

---

- **Alerting**
  - **Routing:** Event Log (custom channel), email, webhook (Teams/Slack), Windows toast.  
  - **Payload:** \(R\), state, top drivers, recent changes, correlation IDs, snapshot IDs.

- **Explainability and audit**
  - **Store:** Inputs, intermediate scores (\(r_f, r_s, r_{rules}\)), fusion weights, final \(R\).  
  - **Replay:** Deterministic re‑scoring for any timestamp given identical inputs + config.  
  - **Change logs:** Version every model, rulepack, and config with semantic versions.

- **Resource posture**
  - **CPU:** Keep stream model O(\(n\)) per window; use queues and batching.  
  - **Memory:** Fixed‑size ring buffers for windows; spill older snapshots to disk.  
  - **Isolation:** Run as a Windows Service with constrained privileges and signed binaries.
