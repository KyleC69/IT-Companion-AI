// Project Name: LightweightAI.Core
// File Name: PipelineRunner.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace LightweightAI.Core.Engine;


public class PipelineRunner : IPipelineRunner
{
    private readonly UnifiedAggregator _aggregator;
    private readonly HysteresisDecider _decider;
    private readonly RulesEngine _rulesEngine;





    public PipelineRunner()
    {
        this._rulesEngine = new RulesEngine();
        this._aggregator = new UnifiedAggregator();

        // Load hysteresis thresholds from config if present
        var fusionConfigPath = Path.Combine("Engine", "config", "fusion.json");
        if (File.Exists(fusionConfigPath))
        {
            Dictionary<string, double>? cfg =
                System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, double>>(
                    File.ReadAllText(fusionConfigPath));
            this._decider = new HysteresisDecider(cfg["lower"], cfg["upper"]);
        }
        else
        {
            this._decider = new HysteresisDecider(0.4, 0.7);
        }

        // Load rules from config
        var rulesPath = Path.Combine("Engine", "config", "rules.json");
        if (File.Exists(rulesPath))
        {
            var json = File.ReadAllText(rulesPath);
            foreach (IRule rule in ConfigRule.LoadFromJson(json)) this._rulesEngine.AddRule(rule);
        }
        else
        {
            this._rulesEngine.AddRule(new ExampleRule());
        }
    }





    public void Initialize()
    {
        Console.WriteLine("[INIT] Pipeline Runner initialized.");
    }





    public void Run()
    {
        Console.WriteLine("[RUN] Starting pipeline with live loader...");
        IEnumerable<EventContext> events = LoadEventsFromPs();

        foreach (EventContext ctx in events)
        {
            IEnumerable<RuleResult> results = this._rulesEngine.Evaluate(ctx);
            AggregatedEvent agg = this._aggregator.Aggregate(results);
            var alert = this._decider.Decide(agg.Score);

            Console.WriteLine($"[EVENT] {ctx.EventId} => Score: {agg.Score:F2} | Triggered: {alert}");
        }
    }





    public void Teardown()
    {
        Console.WriteLine("[DONE] Pipeline Runner teardown complete.");
    }





    private IEnumerable<EventContext> LoadEventsFromPs()
    {
        var psi = new System.Diagnostics.ProcessStartInfo
        {
            FileName = "powershell.exe",
            Arguments = "-ExecutionPolicy Bypass -File \"Loaders\\Events\\event.ps1\"",
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using var proc = System.Diagnostics.Process.Start(psi);
        var json = proc.StandardOutput.ReadToEnd();
        proc.WaitForExit();

        return System.Text.Json.JsonSerializer.Deserialize<List<EventContext>>(json,
            new System.Text.Json.JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? new List<EventContext>();
    }
}