Using the following folder/project skeleton (FLEXIBLE) Generate drop in classes to generate AI models to achieve the following goals:

-- Conversational Model with narrow scope to windows security to answer questions like:
EXAMPLE - How do I prevent outside users from accessing my network.
EXAMPLE - How do I fix the Event ID xxxx error.
EXAMPLE - Generate a group policy to do x, y, or z.
-- Model to make threat assessments by examining the system for unsecure settings, searching for known anti-patterns, exploits etc. by examining event logs, 3rd party logs, registry exploits, WMI ,ETW, leveraging as many built in monitoring tools as possible.
-- Preferred libraries: Microsoft.ML, ONNX,  ONNX.Sharp
-- Restrict code to c# and light scripting
-- Library will be dropped in client application for user-facing UI
-- Lightweight.Cli is minimal console app for model training and training data retrieval and organization
-- Models need ability to train as it goes by discovering patterns in streamed in data
-- Future expansion will be enterprise wide capable. Keep this in mind when planning design
-- Do not include any paid AI models or cloud based services in design decisions.
-- Provide small dataset samples (10 records) as examples of training data for each model created
-- Generate as many models needed to achieve above goals.

 ```
  LightweightAI.sln
  ├─ src/
  │  ├─ LightweightAI.Core/          # ThreatAssessment, runtime loader
  │  ├─ LightweightAI.Models/        # ML.NET pipelines, trainers, evaluators
  │  ├─ LightweightAI.KnowledgeBase/ # JSON/Markdown KB and rule mapping loader
  │  ├─ LightweightAI.Cli/           # train-category, train-risk, train-techniques, eval, pack
  ├─ artifacts/
  │  ├─ models/   # .zip/.onnx files
  │  ├─ kb/       # JSON and markdown files
  │  └─ manifest.json
  ├─ data/
  │  ├─ category_train.csv
  │  ├─ risk_train.csv
  │  ├─ techniques_train.csv
  │  └─ instructions.jsonl
  └─ docs/
  ```