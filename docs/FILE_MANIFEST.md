# Minimal File Manifest (src/ subtree)

Purpose: Provide a linear, walk-free list of repository files exactly as produced by `git ls-files`, restricted to `src/` and excluding build artifacts (`bin/`, `obj/`) and any `.git*` paths. No additional metadata is included.

Adjusting scope:
- To include the entire repo: change the path filter in `scripts/generate_minimal_manifest.py` (search for `p.startswith("src/")`).
- To add/exclude patterns: modify `EXCLUDE_PATTERNS` in the script.

## 1. Directory Tree (Informational)

```
<!-- AUTO-GENERATED: do not edit manually -->
src/
  AICompanion.Tests/
    Assertions/
      AssertionFluentExtensions.cs
      BufferGuards.cs
      DecisionAssertions.cs
      DiffHelper.cs
      MetricDecision.Assertions.cs
      MetricDecision.cs
      MetricDecisionAssertions.cs
      MetricDecisionFluent.cs
      ProvenanceAssertions.cs
      ProvenancedDecision.cs
      ProvenancedDecisionFluent.cs
    Engine/
      BufferGuards.cs
      FusionBrokerTest.cs
    AICompanion.Tests.csproj
    LightweightAI_Core_Engine_ProvenanceLogger_Tests.cs
  LightAIClient/
    LightAIClient.csproj
    Program.cs
  LightweightAI.Core/
    Engine/
      config/
        taxonomy/
          category_map.json
          eventid_map.json
        fusion.json
        host_registry.json
        meta_flags.json
        rules.json
        severity_scale.json
        source_registry.json
      models/
        calibration/
          snapshot_iso.json
          stream_platt.json
        Aggregation.cs
        ModelFactory.cs
        ReslientModels.cs
        SnapshotTrendModel.cs
        StreamAnomalyModel.cs
      AggregatedEvent.cs
      AggregatedMetric.cs
      Config.cs
      ConfigRule.cs
      Contracts.cs
      DecisionInput.cs
      DecisionOutput.cs
      EventContext.cs
      ExampleRule.cs
      FusionBroker.cs
      FusionConfig.cs
      FusionEngine.cs
      FusionFactory.cs
      FusionPipeline.cs
      HysteresisDecider.cs
      IFaces.cs
      IFusionEngine.cs
      IFusionPipeline.cs
      IModel.cs
      IPipelineRunner.cs
      IProvenanceLogger.cs
      IRule.cs
      IRunner.cs
      ISeverityMapper.cs
      Pipeline.cs
      PipelineRunner.cs
      PipelineScheduler.cs
      ProvenanceLogger.cs
      RuleResult.cs
      RulesEngine.cs
      SeverityMapper.cs
      Snapshot.cs
      SnapshotTrendModel.cs
      StreamAnomalyModel.cs
      UnifiedAggregator.cs
    Loaders/
      Drivers/
        DriverLoader.cs
        DriverRefinery.cs
        drivers.ps1
      Events/
        EventLogLoader.cs
        EventLogRefinery.cs
        event.ps1
      Services/
        ServiceLoader.cs
        ServiceRefinery.cs
        sc.exe.ps1
      qANDa/
        AnswerEnvelope.cs
        Audit.cs
        ContextWindow.cs
        PipelineRunner.cs
        QueryCollector.cs
        RefineryStage.cs
        StopWordFilter.cs
        TestHarness.cs
        Tokenizer.cs
        wholepipe2.cs
        wholepipeline.cs
    Orchestrator/
      IFaces.cs
      Merge.cs
      UnifiedDatasetBuilder.cs
    Refinery/
      Coordinator.cs
      DriverLoader.cs
      IRefineryLoader.cs
      RefineryPipeline.cs
    Training/
      AuditFriendlyTrainer.cs
      ProvenanceLog.cs
      SlidingWindowBuffer.cs
      TraningSample.cs
      usage.cs
    data/
      kb/
        4625.md
        sysmon_1.md
      Conversational.json
      category_train.csv
      risk_train.csv
      rules.json
      techniques_train.csv
    LightweightAI.Core.csproj
    Program.cs
```

## 2. Relative File Paths (Canonical Order)

Source: `git ls-files src` with exclusions.

```
<!-- AUTO-GENERATED: do not edit manually -->
src/AICompanion.Tests/AICompanion.Tests.csproj
src/AICompanion.Tests/Assertions/AssertionFluentExtensions.cs
src/AICompanion.Tests/Assertions/BufferGuards.cs
src/AICompanion.Tests/Assertions/DecisionAssertions.cs
src/AICompanion.Tests/Assertions/DiffHelper.cs
src/AICompanion.Tests/Assertions/MetricDecision.Assertions.cs
src/AICompanion.Tests/Assertions/MetricDecision.cs
src/AICompanion.Tests/Assertions/MetricDecisionAssertions.cs
src/AICompanion.Tests/Assertions/MetricDecisionFluent.cs
src/AICompanion.Tests/Assertions/ProvenanceAssertions.cs
src/AICompanion.Tests/Assertions/ProvenancedDecision.cs
src/AICompanion.Tests/Assertions/ProvenancedDecisionFluent.cs
src/AICompanion.Tests/Engine/BufferGuards.cs
src/AICompanion.Tests/Engine/FusionBrokerTest.cs
src/AICompanion.Tests/LightweightAI_Core_Engine_ProvenanceLogger_Tests.cs
src/LightAIClient/LightAIClient.csproj
src/LightAIClient/Program.cs
src/LightweightAI.Core/Engine/AggregatedEvent.cs
src/LightweightAI.Core/Engine/AggregatedMetric.cs
src/LightweightAI.Core/Engine/Config.cs
src/LightweightAI.Core/Engine/ConfigRule.cs
src/LightweightAI.Core/Engine/Contracts.cs
src/LightweightAI.Core/Engine/DecisionInput.cs
src/LightweightAI.Core/Engine/DecisionOutput.cs
src/LightweightAI.Core/Engine/EventContext.cs
src/LightweightAI.Core/Engine/ExampleRule.cs
src/LightweightAI.Core/Engine/FusionBroker.cs
src/LightweightAI.Core/Engine/FusionConfig.cs
src/LightweightAI.Core/Engine/FusionEngine.cs
src/LightweightAI.Core/Engine/FusionFactory.cs
src/LightweightAI.Core/Engine/FusionPipeline.cs
src/LightweightAI.Core/Engine/HysteresisDecider.cs
src/LightweightAI.Core/Engine/IFaces.cs
src/LightweightAI.Core/Engine/IFusionEngine.cs
src/LightweightAI.Core/Engine/IFusionPipeline.cs
src/LightweightAI.Core/Engine/IModel.cs
src/LightweightAI.Core/Engine/IPipelineRunner.cs
src/LightweightAI.Core/Engine/IProvenanceLogger.cs
src/LightweightAI.Core/Engine/IRule.cs
src/LightweightAI.Core/Engine/IRunner.cs
src/LightweightAI.Core/Engine/ISeverityMapper.cs
src/LightweightAI.Core/Engine/Pipeline.cs
src/LightweightAI.Core/Engine/PipelineRunner.cs
src/LightweightAI.Core/Engine/PipelineScheduler.cs
src/LightweightAI.Core/Engine/ProvenanceLogger.cs
src/LightweightAI.Core/Engine/RuleResult.cs
src/LightweightAI.Core/Engine/RulesEngine.cs
src/LightweightAI.Core/Engine/SeverityMapper.cs
src/LightweightAI.Core/Engine/Snapshot.cs
src/LightweightAI.Core/Engine/SnapshotTrendModel.cs
src/LightweightAI.Core/Engine/StreamAnomalyModel.cs
src/LightweightAI.Core/Engine/UnifiedAggregator.cs
src/LightweightAI.Core/Engine/config/fusion.json
src/LightweightAI.Core/Engine/config/host_registry.json
src/LightweightAI.Core/Engine/config/meta_flags.json
src/LightweightAI.Core/Engine/config/rules.json
src/LightweightAI.Core/Engine/config/severity_scale.json
src/LightweightAI.Core/Engine/config/source_registry.json
src/LightweightAI.Core/Engine/config/taxonomy/category_map.json
src/LightweightAI.Core/Engine/config/taxonomy/eventid_map.json
src/LightweightAI.Core/Engine/models/Aggregation.cs
src/LightweightAI.Core/Engine/models/ModelFactory.cs
src/LightweightAI.Core/Engine/models/ReslientModels.cs
src/LightweightAI.Core/Engine/models/SnapshotTrendModel.cs
src/LightweightAI.Core/Engine/models/StreamAnomalyModel.cs
src/LightweightAI.Core/Engine/models/calibration/snapshot_iso.json
src/LightweightAI.Core/Engine/models/calibration/stream_platt.json
src/LightweightAI.Core/LightweightAI.Core.csproj
src/LightweightAI.Core/Loaders/Drivers/DriverLoader.cs
src/LightweightAI.Core/Loaders/Drivers/DriverRefinery.cs
src/LightweightAI.Core/Loaders/Drivers/drivers.ps1
src/LightweightAI.Core/Loaders/Events/EventLogLoader.cs
src/LightweightAI.Core/Loaders/Events/EventLogRefinery.cs
src/LightweightAI.Core/Loaders/Events/event.ps1
src/LightweightAI.Core/Loaders/Services/ServiceLoader.cs
src/LightweightAI.Core/Loaders/Services/ServiceRefinery.cs
src/LightweightAI.Core/Loaders/Services/sc.exe.ps1
src/LightweightAI.Core/Loaders/qANDa/AnswerEnvelope.cs
src/LightweightAI.Core/Loaders/qANDa/Audit.cs
src/LightweightAI.Core/Loaders/qANDa/ContextWindow.cs
src/LightweightAI.Core/Loaders/qANDa/PipelineRunner.cs
src/LightweightAI.Core/Loaders/qANDa/QueryCollector.cs
src/LightweightAI.Core/Loaders/qANDa/RefineryStage.cs
src/LightweightAI.Core/Loaders/qANDa/StopWordFilter.cs
src/LightweightAI.Core/Loaders/qANDa/TestHarness.cs
src/LightweightAI.Core/Loaders/qANDa/Tokenizer.cs
src/LightweightAI.Core/Loaders/qANDa/wholepipe2.cs
src/LightweightAI.Core/Loaders/qANDa/wholepipeline.cs
src/LightweightAI.Core/Orchestrator/IFaces.cs
src/LightweightAI.Core/Orchestrator/Merge.cs
src/LightweightAI.Core/Orchestrator/UnifiedDatasetBuilder.cs
src/LightweightAI.Core/Program.cs
src/LightweightAI.Core/Refinery/Coordinator.cs
src/LightweightAI.Core/Refinery/DriverLoader.cs
src/LightweightAI.Core/Refinery/IRefineryLoader.cs
src/LightweightAI.Core/Refinery/RefineryPipeline.cs
src/LightweightAI.Core/Training/AuditFriendlyTrainer.cs
src/LightweightAI.Core/Training/ProvenanceLog.cs
src/LightweightAI.Core/Training/SlidingWindowBuffer.cs
src/LightweightAI.Core/Training/TraningSample.cs
src/LightweightAI.Core/Training/usage.cs
src/LightweightAI.Core/data/Conversational.json
src/LightweightAI.Core/data/category_train.csv
src/LightweightAI.Core/data/kb/4625.md
src/LightweightAI.Core/data/kb/sysmon_1.md
src/LightweightAI.Core/data/risk_train.csv
src/LightweightAI.Core/data/rules.json
src/LightweightAI.Core/data/techniques_train.csv
```

## 3. HTML File URLs

```
<!-- AUTO-GENERATED: do not edit manually -->
https://github.com/KyleC69/IT-Companion-AI/blob/master/src/AICompanion.Tests/AICompanion.Tests.csproj
https://github.com/KyleC69/IT-Companion-AI/blob/master/src/AICompanion.Tests/Assertions/AssertionFluentExtensions.cs
https://github.com/KyleC69/IT-Companion-AI/blob/master/src/AICompanion.Tests/Assertions/BufferGuards.cs
https://github.com/KyleC69/IT-Companion-AI/blob/master/src/AICompanion.Tests/Assertions/DecisionAssertions.cs
https://github.com/KyleC69/IT-Companion-AI/blob/master/src/AICompanion.Tests/Assertions/DiffHelper.cs
https://github.com/KyleC69/IT-Companion-AI/blob/master/src/AICompanion.Tests/Assertions/MetricDecision.Assertions.cs
https://github.com/KyleC69/IT-Companion-AI/blob/master/src/AICompanion.Tests/Assertions/MetricDecision.cs
https://github.com/KyleC69/IT-Companion-AI/blob/master/src/AICompanion.Tests/Assertions/MetricDecisionAssertions.cs
https://github.com/KyleC69/IT-Companion-AI/blob/master/src/AICompanion.Tests/Assertions/MetricDecisionFluent.cs
https://github.com/KyleC69/IT-Companion-AI/blob/master/src/AICompanion.Tests/Assertions/ProvenanceAssertions.cs
https://github.com/KyleC69/IT-Companion-AI/blob/master/src/AICompanion.Tests/Assertions/ProvenancedDecision.cs
https://github.com/KyleC69/IT-Companion-AI/blob/master/src/AICompanion.Tests/Assertions/ProvenancedDecisionFluent.cs
https://github.com/KyleC69/IT-Companion-AI/blob/master/src/AICompanion.Tests/Engine/BufferGuards.cs
https://github.com/KyleC69/IT-Companion-AI/blob/master/src/AICompanion.Tests/Engine/FusionBrokerTest.cs
https://github.com/KyleC69/IT-Companion-AI/blob/master/src/AICompanion.Tests/LightweightAI_Core_Engine_ProvenanceLogger_Tests.cs
https://github.com/KyleC69/IT-Companion-AI/blob/master/src/LightAIClient/LightAIClient.csproj
https://github.com/KyleC69/IT-Companion-AI/blob/master/src/LightAIClient/Program.cs
https://github.com/KyleC69/IT-Companion-AI/blob/master/src/LightweightAI.Core/Engine/AggregatedEvent.cs
https://github.com/KyleC69/IT-Companion-AI/blob/master/src/LightweightAI.Core/Engine/AggregatedMetric.cs
https://github.com/KyleC69/IT-Companion-AI/blob/master/src/LightweightAI.Core/Engine/Config.cs
https://github.com/KyleC69/IT-Companion-AI/blob/master/src/LightweightAI.Core/Engine/ConfigRule.cs
https://github.com/KyleC69/IT-Companion-AI/blob/master/src/LightweightAI.Core/Engine/Contracts.cs
https://github.com/KyleC69/IT-Companion-AI/blob/master/src/LightweightAI.Core/Engine/DecisionInput.cs
https://github.com/KyleC69/IT-Companion-AI/blob/master/src/LightweightAI.Core/Engine/DecisionOutput.cs
https://github.com/KyleC69/IT-Companion-AI/blob/master/src/LightweightAI.Core/Engine/EventContext.cs
https://github.com/KyleC69/IT-Companion-AI/blob/master/src/LightweightAI.Core/Engine/ExampleRule.cs
https://github.com/KyleC69/IT-Companion-AI/blob/master/src/LightweightAI.Core/Engine/FusionBroker.cs
https://github.com/KyleC69/IT-Companion-AI/blob/master/src/LightweightAI.Core/Engine/FusionConfig.cs
https://github.com/KyleC69/IT-Companion-AI/blob/master/src/LightweightAI.Core/Engine/FusionEngine.cs
https://github.com/KyleC69/IT-Companion-AI/blob/master/src/LightweightAI.Core/Engine/FusionFactory.cs
https://github.com/KyleC69/IT-Companion-AI/blob/master/src/LightweightAI.Core/Engine/FusionPipeline.cs
https://github.com/KyleC69/IT-Companion-AI/blob/master/src/LightweightAI.Core/Engine/HysteresisDecider.cs
https://github.com/KyleC69/IT-Companion-AI/blob/master/src/LightweightAI.Core/Engine/IFaces.cs
https://github.com/KyleC69/IT-Companion-AI/blob/master/src/LightweightAI.Core/Engine/IFusionEngine.cs
https://github.com/KyleC69/IT-Companion-AI/blob/master/src/LightweightAI.Core/Engine/IFusionPipeline.cs
https://github.com/KyleC69/IT-Companion-AI/blob/master/src/LightweightAI.Core/Engine/IModel.cs
https://github.com/KyleC69/IT-Companion-AI/blob/master/src/LightweightAI.Core/Engine/IPipelineRunner.cs
https://github.com/KyleC69/IT-Companion-AI/blob/master/src/LightweightAI.Core/Engine/IProvenanceLogger.cs
https://github.com/KyleC69/IT-Companion-AI/blob/master/src/LightweightAI.Core/Engine/IRule.cs
https://github.com/KyleC69/IT-Companion-AI/blob/master/src/LightweightAI.Core/Engine/IRunner.cs
https://github.com/KyleC69/IT-Companion-AI/blob/master/src/LightweightAI.Core/Engine/ISeverityMapper.cs
https://github.com/KyleC69/IT-Companion-AI/blob/master/src/LightweightAI.Core/Engine/Pipeline.cs
https://github.com/KyleC69/IT-Companion-AI/blob/master/src/LightweightAI.Core/Engine/PipelineRunner.cs
https://github.com/KyleC69/IT-Companion-AI/blob/master/src/LightweightAI.Core/Engine/PipelineScheduler.cs
https://github.com/KyleC69/IT-Companion-AI/blob/master/src/LightweightAI.Core/Engine/ProvenanceLogger.cs
https://github.com/KyleC69/IT-Companion-AI/blob/master/src/LightweightAI.Core/Engine/RuleResult.cs
https://github.com/KyleC69/IT-Companion-AI/blob/master/src/LightweightAI.Core/Engine/RulesEngine.cs
https://github.com/KyleC69/IT-Companion-AI/blob/master/src/LightweightAI.Core/Engine/SeverityMapper.cs
https://github.com/KyleC69/IT-Companion-AI/blob/master/src/LightweightAI.Core/Engine/Snapshot.cs
https://github.com/KyleC69/IT-Companion-AI/blob/master/src/LightweightAI.Core/Engine/SnapshotTrendModel.cs
https://github.com/KyleC69/IT-Companion-AI/blob/master/src/LightweightAI.Core/Engine/StreamAnomalyModel.cs
https://github.com/KyleC69/IT-Companion-AI/blob/master/src/LightweightAI.Core/Engine/UnifiedAggregator.cs
https://github.com/KyleC69/IT-Companion-AI/blob/master/src/LightweightAI.Core/Engine/config/fusion.json
https://github.com/KyleC69/IT-Companion-AI/blob/master/src/LightweightAI.Core/Engine/config/host_registry.json
https://github.com/KyleC69/IT-Companion-AI/blob/master/src/LightweightAI.Core/Engine/config/meta_flags.json
https://github.com/KyleC69/IT-Companion-AI/blob/master/src/LightweightAI.Core/Engine/config/rules.json
https://github.com/KyleC69/IT-Companion-AI/blob/master/src/LightweightAI.Core/Engine/config/severity_scale.json
https://github.com/KyleC69/IT-Companion-AI/blob/master/src/LightweightAI.Core/Engine/config/source_registry.json
https://github.com/KyleC69/IT-Companion-AI/blob/master/src/LightweightAI.Core/Engine/config/taxonomy/category_map.json
https://github.com/KyleC69/IT-Companion-AI/blob/master/src/LightweightAI.Core/Engine/config/taxonomy/eventid_map.json
https://github.com/KyleC69/IT-Companion-AI/blob/master/src/LightweightAI.Core/Engine/models/Aggregation.cs
https://github.com/KyleC69/IT-Companion-AI/blob/master/src/LightweightAI.Core/Engine/models/ModelFactory.cs
https://github.com/KyleC69/IT-Companion-AI/blob/master/src/LightweightAI.Core/Engine/models/ReslientModels.cs
https://github.com/KyleC69/IT-Companion-AI/blob/master/src/LightweightAI.Core/Engine/models/SnapshotTrendModel.cs
https://github.com/KyleC69/IT-Companion-AI/blob/master/src/LightweightAI.Core/Engine/models/StreamAnomalyModel.cs
https://github.com/KyleC69/IT-Companion-AI/blob/master/src/LightweightAI.Core/Engine/models/calibration/snapshot_iso.json
https://github.com/KyleC69/IT-Companion-AI/blob/master/src/LightweightAI.Core/Engine/models/calibration/stream_platt.json
https://github.com/KyleC69/IT-Companion-AI/blob/master/src/LightweightAI.Core/LightweightAI.Core.csproj
https://github.com/KyleC69/IT-Companion-AI/blob/master/src/LightweightAI.Core/Loaders/Drivers/DriverLoader.cs
https://github.com/KyleC69/IT-Companion-AI/blob/master/src/LightweightAI.Core/Loaders/Drivers/DriverRefinery.cs
https://github.com/KyleC69/IT-Companion-AI/blob/master/src/LightweightAI.Core/Loaders/Drivers/drivers.ps1
https://github.com/KyleC69/IT-Companion-AI/blob/master/src/LightweightAI.Core/Loaders/Events/EventLogLoader.cs
https://github.com/KyleC69/IT-Companion-AI/blob/master/src/LightweightAI.Core/Loaders/Events/EventLogRefinery.cs
https://github.com/KyleC69/IT-Companion-AI/blob/master/src/LightweightAI.Core/Loaders/Events/event.ps1
https://github.com/KyleC69/IT-Companion-AI/blob/master/src/LightweightAI.Core/Loaders/Services/ServiceLoader.cs
https://github.com/KyleC69/IT-Companion-AI/blob/master/src/LightweightAI.Core/Loaders/Services/ServiceRefinery.cs
https://github.com/KyleC69/IT-Companion-AI/blob/master/src/LightweightAI.Core/Loaders/Services/sc.exe.ps1
https://github.com/KyleC69/IT-Companion-AI/blob/master/src/LightweightAI.Core/Loaders/qANDa/AnswerEnvelope.cs
https://github.com/KyleC69/IT-Companion-AI/blob/master/src/LightweightAI.Core/Loaders/qANDa/Audit.cs
https://github.com/KyleC69/IT-Companion-AI/blob/master/src/LightweightAI.Core/Loaders/qANDa/ContextWindow.cs
https://github.com/KyleC69/IT-Companion-AI/blob/master/src/LightweightAI.Core/Loaders/qANDa/PipelineRunner.cs
https://github.com/KyleC69/IT-Companion-AI/blob/master/src/LightweightAI.Core/Loaders/qANDa/QueryCollector.cs
https://github.com/KyleC69/IT-Companion-AI/blob/master/src/LightweightAI.Core/Loaders/qANDa/RefineryStage.cs
https://github.com/KyleC69/IT-Companion-AI/blob/master/src/LightweightAI.Core/Loaders/qANDa/StopWordFilter.cs
https://github.com/KyleC69/IT-Companion-AI/blob/master/src/LightweightAI.Core/Loaders/qANDa/TestHarness.cs
https://github.com/KyleC69/IT-Companion-AI/blob/master/src/LightweightAI.Core/Loaders/qANDa/Tokenizer.cs
https://github.com/KyleC69/IT-Companion-AI/blob/master/src/LightweightAI.Core/Loaders/qANDa/wholepipe2.cs
https://github.com/KyleC69/IT-Companion-AI/blob/master/src/LightweightAI.Core/Loaders/qANDa/wholepipeline.cs
https://github.com/KyleC69/IT-Companion-AI/blob/master/src/LightweightAI.Core/Orchestrator/IFaces.cs
https://github.com/KyleC69/IT-Companion-AI/blob/master/src/LightweightAI.Core/Orchestrator/Merge.cs
https://github.com/KyleC69/IT-Companion-AI/blob/master/src/LightweightAI.Core/Orchestrator/UnifiedDatasetBuilder.cs
https://github.com/KyleC69/IT-Companion-AI/blob/master/src/LightweightAI.Core/Program.cs
https://github.com/KyleC69/IT-Companion-AI/blob/master/src/LightweightAI.Core/Refinery/Coordinator.cs
https://github.com/KyleC69/IT-Companion-AI/blob/master/src/LightweightAI.Core/Refinery/DriverLoader.cs
https://github.com/KyleC69/IT-Companion-AI/blob/master/src/LightweightAI.Core/Refinery/IRefineryLoader.cs
https://github.com/KyleC69/IT-Companion-AI/blob/master/src/LightweightAI.Core/Refinery/RefineryPipeline.cs
https://github.com/KyleC69/IT-Companion-AI/blob/master/src/LightweightAI.Core/Training/AuditFriendlyTrainer.cs
https://github.com/KyleC69/IT-Companion-AI/blob/master/src/LightweightAI.Core/Training/ProvenanceLog.cs
https://github.com/KyleC69/IT-Companion-AI/blob/master/src/LightweightAI.Core/Training/SlidingWindowBuffer.cs
https://github.com/KyleC69/IT-Companion-AI/blob/master/src/LightweightAI.Core/Training/TraningSample.cs
https://github.com/KyleC69/IT-Companion-AI/blob/master/src/LightweightAI.Core/Training/usage.cs
https://github.com/KyleC69/IT-Companion-AI/blob/master/src/LightweightAI.Core/data/Conversational.json
https://github.com/KyleC69/IT-Companion-AI/blob/master/src/LightweightAI.Core/data/category_train.csv
https://github.com/KyleC69/IT-Companion-AI/blob/master/src/LightweightAI.Core/data/kb/4625.md
https://github.com/KyleC69/IT-Companion-AI/blob/master/src/LightweightAI.Core/data/kb/sysmon_1.md
https://github.com/KyleC69/IT-Companion-AI/blob/master/src/LightweightAI.Core/data/risk_train.csv
https://github.com/KyleC69/IT-Companion-AI/blob/master/src/LightweightAI.Core/data/rules.json
https://github.com/KyleC69/IT-Companion-AI/blob/master/src/LightweightAI.Core/data/techniques_train.csv
```

## 4. Raw File URLs

```
<!-- AUTO-GENERATED: do not edit manually -->
https://raw.githubusercontent.com/KyleC69/IT-Companion-AI/master/src/AICompanion.Tests/AICompanion.Tests.csproj
https://raw.githubusercontent.com/KyleC69/IT-Companion-AI/master/src/AICompanion.Tests/Assertions/AssertionFluentExtensions.cs
https://raw.githubusercontent.com/KyleC69/IT-Companion-AI/master/src/AICompanion.Tests/Assertions/BufferGuards.cs
https://raw.githubusercontent.com/KyleC69/IT-Companion-AI/master/src/AICompanion.Tests/Assertions/DecisionAssertions.cs
https://raw.githubusercontent.com/KyleC69/IT-Companion-AI/master/src/AICompanion.Tests/Assertions/DiffHelper.cs
https://raw.githubusercontent.com/KyleC69/IT-Companion-AI/master/src/AICompanion.Tests/Assertions/MetricDecision.Assertions.cs
https://raw.githubusercontent.com/KyleC69/IT-Companion-AI/master/src/AICompanion.Tests/Assertions/MetricDecision.cs
https://raw.githubusercontent.com/KyleC69/IT-Companion-AI/master/src/AICompanion.Tests/Assertions/MetricDecisionAssertions.cs
https://raw.githubusercontent.com/KyleC69/IT-Companion-AI/master/src/AICompanion.Tests/Assertions/MetricDecisionFluent.cs
https://raw.githubusercontent.com/KyleC69/IT-Companion-AI/master/src/AICompanion.Tests/Assertions/ProvenanceAssertions.cs
https://raw.githubusercontent.com/KyleC69/IT-Companion-AI/master/src/AICompanion.Tests/Assertions/ProvenancedDecision.cs
https://raw.githubusercontent.com/KyleC69/IT-Companion-AI/master/src/AICompanion.Tests/Assertions/ProvenancedDecisionFluent.cs
https://raw.githubusercontent.com/KyleC69/IT-Companion-AI/master/src/AICompanion.Tests/Engine/BufferGuards.cs
https://raw.githubusercontent.com/KyleC69/IT-Companion-AI/master/src/AICompanion.Tests/Engine/FusionBrokerTest.cs
https://raw.githubusercontent.com/KyleC69/IT-Companion-AI/master/src/AICompanion.Tests/LightweightAI_Core_Engine_ProvenanceLogger_Tests.cs
https://raw.githubusercontent.com/KyleC69/IT-Companion-AI/master/src/LightAIClient/LightAIClient.csproj
https://raw.githubusercontent.com/KyleC69/IT-Companion-AI/master/src/LightAIClient/Program.cs
https://raw.githubusercontent.com/KyleC69/IT-Companion-AI/master/src/LightweightAI.Core/Engine/AggregatedEvent.cs
https://raw.githubusercontent.com/KyleC69/IT-Companion-AI/master/src/LightweightAI.Core/Engine/AggregatedMetric.cs
https://raw.githubusercontent.com/KyleC69/IT-Companion-AI/master/src/LightweightAI.Core/Engine/Config.cs
https://raw.githubusercontent.com/KyleC69/IT-Companion-AI/master/src/LightweightAI.Core/Engine/ConfigRule.cs
https://raw.githubusercontent.com/KyleC69/IT-Companion-AI/master/src/LightweightAI.Core/Engine/Contracts.cs
https://raw.githubusercontent.com/KyleC69/IT-Companion-AI/master/src/LightweightAI.Core/Engine/DecisionInput.cs
https://raw.githubusercontent.com/KyleC69/IT-Companion-AI/master/src/LightweightAI.Core/Engine/DecisionOutput.cs
https://raw.githubusercontent.com/KyleC69/IT-Companion-AI/master/src/LightweightAI.Core/Engine/EventContext.cs
https://raw.githubusercontent.com/KyleC69/IT-Companion-AI/master/src/LightweightAI.Core/Engine/ExampleRule.cs
https://raw.githubusercontent.com/KyleC69/IT-Companion-AI/master/src/LightweightAI.Core/Engine/FusionBroker.cs
https://raw.githubusercontent.com/KyleC69/IT-Companion-AI/master/src/LightweightAI.Core/Engine/FusionConfig.cs
https://raw.githubusercontent.com/KyleC69/IT-Companion-AI/master/src/LightweightAI.Core/Engine/FusionEngine.cs
https://raw.githubusercontent.com/KyleC69/IT-Companion-AI/master/src/LightweightAI.Core/Engine/FusionFactory.cs
https://raw.githubusercontent.com/KyleC69/IT-Companion-AI/master/src/LightweightAI.Core/Engine/FusionPipeline.cs
https://raw.githubusercontent.com/KyleC69/IT-Companion-AI/master/src/LightweightAI.Core/Engine/HysteresisDecider.cs
https://raw.githubusercontent.com/KyleC69/IT-Companion-AI/master/src/LightweightAI.Core/Engine/IFaces.cs
https://raw.githubusercontent.com/KyleC69/IT-Companion-AI/master/src/LightweightAI.Core/Engine/IFusionEngine.cs
https://raw.githubusercontent.com/KyleC69/IT-Companion-AI/master/src/LightweightAI.Core/Engine/IFusionPipeline.cs
https://raw.githubusercontent.com/KyleC69/IT-Companion-AI/master/src/LightweightAI.Core/Engine/IModel.cs
https://raw.githubusercontent.com/KyleC69/IT-Companion-AI/master/src/LightweightAI.Core/Engine/IPipelineRunner.cs
https://raw.githubusercontent.com/KyleC69/IT-Companion-AI/master/src/LightweightAI.Core/Engine/IProvenanceLogger.cs
https://raw.githubusercontent.com/KyleC69/IT-Companion-AI/master/src/LightweightAI.Core/Engine/IRule.cs
https://raw.githubusercontent.com/KyleC69/IT-Companion-AI/master/src/LightweightAI.Core/Engine/IRunner.cs
https://raw.githubusercontent.com/KyleC69/IT-Companion-AI/master/src/LightweightAI.Core/Engine/ISeverityMapper.cs
https://raw.githubusercontent.com/KyleC69/IT-Companion-AI/master/src/LightweightAI.Core/Engine/Pipeline.cs
https://raw.githubusercontent.com/KyleC69/IT-Companion-AI/master/src/LightweightAI.Core/Engine/PipelineRunner.cs
https://raw.githubusercontent.com/KyleC69/IT-Companion-AI/master/src/LightweightAI.Core/Engine/PipelineScheduler.cs
https://raw.githubusercontent.com/KyleC69/IT-Companion-AI/master/src/LightweightAI.Core/Engine/ProvenanceLogger.cs
https://raw.githubusercontent.com/KyleC69/IT-Companion-AI/master/src/LightweightAI.Core/Engine/RuleResult.cs
https://raw.githubusercontent.com/KyleC69/IT-Companion-AI/master/src/LightweightAI.Core/Engine/RulesEngine.cs
https://raw.githubusercontent.com/KyleC69/IT-Companion-AI/master/src/LightweightAI.Core/Engine/SeverityMapper.cs
https://raw.githubusercontent.com/KyleC69/IT-Companion-AI/master/src/LightweightAI.Core/Engine/Snapshot.cs
https://raw.githubusercontent.com/KyleC69/IT-Companion-AI/master/src/LightweightAI.Core/Engine/SnapshotTrendModel.cs
https://raw.githubusercontent.com/KyleC69/IT-Companion-AI/master/src/LightweightAI.Core/Engine/StreamAnomalyModel.cs
https://raw.githubusercontent.com/KyleC69/IT-Companion-AI/master/src/LightweightAI.Core/Engine/UnifiedAggregator.cs
https://raw.githubusercontent.com/KyleC69/IT-Companion-AI/master/src/LightweightAI.Core/Engine/config/fusion.json
https://raw.githubusercontent.com/KyleC69/IT-Companion-AI/master/src/LightweightAI.Core/Engine/config/host_registry.json
https://raw.githubusercontent.com/KyleC69/IT-Companion-AI/master/src/LightweightAI.Core/Engine/config/meta_flags.json
https://raw.githubusercontent.com/KyleC69/IT-Companion-AI/master/src/LightweightAI.Core/Engine/config/rules.json
https://raw.githubusercontent.com/KyleC69/IT-Companion-AI/master/src/LightweightAI.Core/Engine/config/severity_scale.json
https://raw.githubusercontent.com/KyleC69/IT-Companion-AI/master/src/LightweightAI.Core/Engine/config/source_registry.json
https://raw.githubusercontent.com/KyleC69/IT-Companion-AI/master/src/LightweightAI.Core/Engine/config/taxonomy/category_map.json
https://raw.githubusercontent.com/KyleC69/IT-Companion-AI/master/src/LightweightAI.Core/Engine/config/taxonomy/eventid_map.json
https://raw.githubusercontent.com/KyleC69/IT-Companion-AI/master/src/LightweightAI.Core/Engine/models/Aggregation.cs
https://raw.githubusercontent.com/KyleC69/IT-Companion-AI/master/src/LightweightAI.Core/Engine/models/ModelFactory.cs
https://raw.githubusercontent.com/KyleC69/IT-Companion-AI/master/src/LightweightAI.Core/Engine/models/ReslientModels.cs
https://raw.githubusercontent.com/KyleC69/IT-Companion-AI/master/src/LightweightAI.Core/Engine/models/SnapshotTrendModel.cs
https://raw.githubusercontent.com/KyleC69/IT-Companion-AI/master/src/LightweightAI.Core/Engine/models/StreamAnomalyModel.cs
https://raw.githubusercontent.com/KyleC69/IT-Companion-AI/master/src/LightweightAI.Core/Engine/models/calibration/snapshot_iso.json
https://raw.githubusercontent.com/KyleC69/IT-Companion-AI/master/src/LightweightAI.Core/Engine/models/calibration/stream_platt.json
https://raw.githubusercontent.com/KyleC69/IT-Companion-AI/master/src/LightweightAI.Core/LightweightAI.Core.csproj
https://raw.githubusercontent.com/KyleC69/IT-Companion-AI/master/src/LightweightAI.Core/Loaders/Drivers/DriverLoader.cs
https://raw.githubusercontent.com/KyleC69/IT-Companion-AI/master/src/LightweightAI.Core/Loaders/Drivers/DriverRefinery.cs
https://raw.githubusercontent.com/KyleC69/IT-Companion-AI/master/src/LightweightAI.Core/Loaders/Drivers/drivers.ps1
https://raw.githubusercontent.com/KyleC69/IT-Companion-AI/master/src/LightweightAI.Core/Loaders/Events/EventLogLoader.cs
https://raw.githubusercontent.com/KyleC69/IT-Companion-AI/master/src/LightweightAI.Core/Loaders/Events/EventLogRefinery.cs
https://raw.githubusercontent.com/KyleC69/IT-Companion-AI/master/src/LightweightAI.Core/Loaders/Events/event.ps1
https://raw.githubusercontent.com/KyleC69/IT-Companion-AI/master/src/LightweightAI.Core/Loaders/Services/ServiceLoader.cs
https://raw.githubusercontent.com/KyleC69/IT-Companion-AI/master/src/LightweightAI.Core/Loaders/Services/ServiceRefinery.cs
https://raw.githubusercontent.com/KyleC69/IT-Companion-AI/master/src/LightweightAI.Core/Loaders/Services/sc.exe.ps1
https://raw.githubusercontent.com/KyleC69/IT-Companion-AI/master/src/LightweightAI.Core/Loaders/qANDa/AnswerEnvelope.cs
https://raw.githubusercontent.com/KyleC69/IT-Companion-AI/master/src/LightweightAI.Core/Loaders/qANDa/Audit.cs
https://raw.githubusercontent.com/KyleC69/IT-Companion-AI/master/src/LightweightAI.Core/Loaders/qANDa/ContextWindow.cs
https://raw.githubusercontent.com/KyleC69/IT-Companion-AI/master/src/LightweightAI.Core/Loaders/qANDa/PipelineRunner.cs
https://raw.githubusercontent.com/KyleC69/IT-Companion-AI/master/src/LightweightAI.Core/Loaders/qANDa/QueryCollector.cs
https://raw.githubusercontent.com/KyleC69/IT-Companion-AI/master/src/LightweightAI.Core/Loaders/qANDa/RefineryStage.cs
https://raw.githubusercontent.com/KyleC69/IT-Companion-AI/master/src/LightweightAI.Core/Loaders/qANDa/StopWordFilter.cs
https://raw.githubusercontent.com/KyleC69/IT-Companion-AI/master/src/LightweightAI.Core/Loaders/qANDa/TestHarness.cs
https://raw.githubusercontent.com/KyleC69/IT-Companion-AI/master/src/LightweightAI.Core/Loaders/qANDa/Tokenizer.cs
https://raw.githubusercontent.com/KyleC69/IT-Companion-AI/master/src/LightweightAI.Core/Loaders/qANDa/wholepipe2.cs
https://raw.githubusercontent.com/KyleC69/IT-Companion-AI/master/src/LightweightAI.Core/Loaders/qANDa/wholepipeline.cs
https://raw.githubusercontent.com/KyleC69/IT-Companion-AI/master/src/LightweightAI.Core/Orchestrator/IFaces.cs
https://raw.githubusercontent.com/KyleC69/IT-Companion-AI/master/src/LightweightAI.Core/Orchestrator/Merge.cs
https://raw.githubusercontent.com/KyleC69/IT-Companion-AI/master/src/LightweightAI.Core/Orchestrator/UnifiedDatasetBuilder.cs
https://raw.githubusercontent.com/KyleC69/IT-Companion-AI/master/src/LightweightAI.Core/Program.cs
https://raw.githubusercontent.com/KyleC69/IT-Companion-AI/master/src/LightweightAI.Core/Refinery/Coordinator.cs
https://raw.githubusercontent.com/KyleC69/IT-Companion-AI/master/src/LightweightAI.Core/Refinery/DriverLoader.cs
https://raw.githubusercontent.com/KyleC69/IT-Companion-AI/master/src/LightweightAI.Core/Refinery/IRefineryLoader.cs
https://raw.githubusercontent.com/KyleC69/IT-Companion-AI/master/src/LightweightAI.Core/Refinery/RefineryPipeline.cs
https://raw.githubusercontent.com/KyleC69/IT-Companion-AI/master/src/LightweightAI.Core/Training/AuditFriendlyTrainer.cs
https://raw.githubusercontent.com/KyleC69/IT-Companion-AI/master/src/LightweightAI.Core/Training/ProvenanceLog.cs
https://raw.githubusercontent.com/KyleC69/IT-Companion-AI/master/src/LightweightAI.Core/Training/SlidingWindowBuffer.cs
https://raw.githubusercontent.com/KyleC69/IT-Companion-AI/master/src/LightweightAI.Core/Training/TraningSample.cs
https://raw.githubusercontent.com/KyleC69/IT-Companion-AI/master/src/LightweightAI.Core/Training/usage.cs
https://raw.githubusercontent.com/KyleC69/IT-Companion-AI/master/src/LightweightAI.Core/data/Conversational.json
https://raw.githubusercontent.com/KyleC69/IT-Companion-AI/master/src/LightweightAI.Core/data/category_train.csv
https://raw.githubusercontent.com/KyleC69/IT-Companion-AI/master/src/LightweightAI.Core/data/kb/4625.md
https://raw.githubusercontent.com/KyleC69/IT-Companion-AI/master/src/LightweightAI.Core/data/kb/sysmon_1.md
https://raw.githubusercontent.com/KyleC69/IT-Companion-AI/master/src/LightweightAI.Core/data/risk_train.csv
https://raw.githubusercontent.com/KyleC69/IT-Companion-AI/master/src/LightweightAI.Core/data/rules.json
https://raw.githubusercontent.com/KyleC69/IT-Companion-AI/master/src/LightweightAI.Core/data/techniques_train.csv
```

## 5. Standalone Plaintext Artifacts

For ingestion agents that only want raw plaintext without parsing this Markdown:

- `manifest/file-paths.txt`        (relative paths)
- `manifest/html-urls.txt`         (HTML blob URLs)
- `manifest/raw-urls.txt`          (raw content URLs)

## Regeneration & Automation
- Generated by `scripts/generate_minimal_manifest.py`.
- Auto-updated on pushes to `master` via workflow: `.github/workflows/minimal-file-manifest.yml`.
- Manual regeneration: `python scripts/generate_minimal_manifest.py` (requires Python 3.9+ and git available).

## Customization Guide
| Goal | Edit | Notes |
|------|------|-------|
| Include whole repo | Change `if not p.startswith("src/")` logic | May enlarge manifest significantly |
| Add new exclusion | Append regex to `EXCLUDE_PATTERNS` | Use raw string patterns |
| Switch branch | Change `BRANCH` constant | Keep workflow branch filter consistent |
| Ignore artifacts in git | Uncomment lines in `.gitignore` | Artifacts are versioned by default |

## Script Exit Codes
- 0 = Success
- 1 (SystemExit) = Missing template doc
- Non-zero from git subprocess = underlying git error

Generated: 2025-08-31T01:12:38Z