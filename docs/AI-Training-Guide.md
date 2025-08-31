**KEPT FOR REFERENCE - NOT PART OF DOC MANIFEST** DOCUMENT WILL BE REGENERATED WHEN DESIGN IS STABLE
**AI DO NOT TOUCH**



# Usage Guide

Below is a step-by-step walkthrough for training, integrating, and running your LightweightAI models and knowledge base.

---

## 1. Training Models via CLI

Use the `LightweightAI.Cli` project to train each model. Be sure your current directory is the solution root.

1. Train the conversational intent classifier  
   
   ```bash
   dotnet run --project src/LightweightAI.Cli \
     -- --train-category \
     --data data/category_train.csv \
     --model-out artifacts/models/conversation.zip
   ```

2. Train the unsupervised threat assessment model  

   ```bash
   dotnet run --project src/LightweightAI.Cli \
     -- --train-risk \
     --data data/risk_train.csv \
     --model-out artifacts/models/threatAssessment.zip
   ```

3. Train the technique‐indicator classifier  

   ```bash
   dotnet run --project src/LightweightAI.Cli \
     -- --train-techniques \
     --data data/techniques_train.csv \
     --model-out artifacts/models/techniques.zip
   ```

Each command outputs both a `.zip` (ML.NET) and an `.onnx` file in `artifacts/models/`.

---

## 2. Loading Models in Your Client App

Add references to these NuGet packages in your UI or service project:

- Microsoft.ML  
- Microsoft.ML.OnnxTransformer  
- ONNX.Sharp  
- Newtonsoft.Json  

Then use the helper classes from **LightweightAI.Core**, **.Models**, and **.KnowledgeBase**.

```csharp
using LightweightAI.Core;
using LightweightAI.Models;
using LightweightAI.KnowledgeBase;

// 1. Load the conversation intent model
var convEngine = ModelFactory
  .CreateEngine<ConversationData, ConversationPrediction>("artifacts/models/conversation.zip");

// 2. Load the threat assessment model
var threatModel = ThreatAssessmentModel.Load("artifacts/models/threatAssessment.zip");
var threatEngine = new PredictionEngine<ThreatRecord, ThreatPrediction>(
// supply mlContext and threatModel to PredictionEngine ctor
);

// 3. Load the knowledge base
var kb = KnowledgeBaseLoader
  .Load("artifacts/kb/knowledge.json");
```

---

## 3. Running Inference

### 3.1 Conversational Q&A

```csharp
// userInput is the question string
var input = new ConversationData { Text = userInput };
var result = convEngine.Predict(input);

// Look up the template and render
if (kb.TryGetValue(result.Category, out var template))
{
    // If template has placeholders, use string.Format(template, args)
    Console.WriteLine(template);
}
else
{
    Console.WriteLine("Sorry, I don't have an answer for that yet.");
}
```

### 3.2 Threat Assessment

```csharp
// Gather live telemetry
var record = new ThreatRecord
{
    OpenPortsCount = Telemetry.GetOpenPorts(),
    SuspiciousProcessCount = Telemetry.CountSuspiciousProcesses(),
    RegistryAnomalyCount = Telemetry.CountRegistryAnomalies(),
    EtwEventRate = Telemetry.CalculateEtwRate()
};

// Predict spikes or anomalies
var pred = threatEngine.Predict(record);
if (pred.IsAnomaly)
    Console.WriteLine("Warning: potential threat detected.");
```

---

## 4. Incremental On-the-Fly Training

To adapt your models as new data streams in, call `mlContext.Model.Update`:

```csharp
var ctx = new MLContext();
var newData = ctx.Data.LoadFromEnumerable(new[] { recordOrConversation });
var updatedModel = ctx.Model.Update(convModel, newData);
ctx.Model.Save(updatedModel, schema, "artifacts/models/conversation.zip");
```

Automate this in a background task to fine-tune your pipeline over time.

---

## 5. Knowledge Base JSON Structure

Place `knowledge.json` under `artifacts/kb/`:

```json
[
  {
    "Category": "NetworkAccess",
    "AnswerTemplate": "To block external users, configure your firewall with these rules: {0}..."
  },
  {
    "Category": "EventIDFix",
    "AnswerTemplate": "Event ID {0} often means {1}. Try checking {2}..."
  }
]
```

The loader builds a `Dictionary<string, string>` for fast template lookups.

---

## 6. Future Expansion & Best Practices

- Expose an HTTP or WebSocket API in your UI host for real-time Q&A and threat alerts.  
- Serialize ONNX models into a versioned manifest (`artifacts/manifest.json`) to support auto-updates.  
- Stream ETW/WMI events in batches to retrain both the threat and technique models on real incidents.  
- Add error-handling, logging, and telemetry around all ML inference calls for observability.  

This end-to-end flow gives you fully offline, extensible AI capabilities in a lightweight .NET library—no cloud dependencies required.
