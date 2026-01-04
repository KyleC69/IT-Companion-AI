<!-- 
Document: AI Layers Architecture.md
Version: 1.0.0
Last Updated: 2026-01-04
Author: Development Team
Status: Active
Description: Detailed design specification for the Agentic AI system architecture

Change History:
- 1.0.0 (2026-01-04): Initial version with version tracking header
-->


---

# **Agentic AI System — Detailed Design Specification (v1.0)**  
### *Architecture, Responsibilities, Contracts, and Implementation Requirements*  
### *Updated for Azure AI / GitHub Models + Local ONNX/WinML Tools + Semantic Kernel*

---

# **0. Purpose of This Document**

This document defines the **authoritative design** for the Agentic AI Windows IT Companion.  
It specifies:

- the architecture  
- the responsibilities of each subsystem  
- the required interfaces  
- the required behaviors  
- the required message formats  
- the required tool schemas  
- the required orchestration loop  
- the required integration with Semantic Kernel  
- the required use of Azure AI / GitHub Models for reasoning  
- the required use of local ONNX/WinML models for specialist tasks  

This document is **binding**.  
Any AgentAI implementing this system MUST follow it exactly.

---

# **1. High‑Level Architecture Overview**

The system consists of **seven cooperating layers**, each with strict responsibilities:

1. **Reasoning Model Layer**  
2. **Retrieval Layer (RAG)**  
3. **Specialist Models & Tools Layer**  
4. **Memory Layer**  
5. **Safety & Validation Layer**  
6. **Orchestration Layer (Agent Loop)**  
7. **Windows Introspection Layer**

These layers MUST remain separate.  
No layer may absorb responsibilities from another.

---

# **2. Reasoning Model Layer (Remote Phi‑4 via Azure AI)**

### **2.1 Purpose**

This layer performs:

- planning  
- reasoning  
- tool selection  
- interpreting observations  
- generating structured tool actions  
- producing final answers  

### **2.2 Requirements**

- MUST use **Azure AI / GitHub Models** for inference  
- MUST be invoked **through Semantic Kernel**  
- MUST support **tool schema injection**  
- MUST output **structured JSON** for tool actions  
- MUST NOT hallucinate tools  
- MUST NOT execute tools directly  
- MUST NOT perform tasks better suited to specialist models  



# **3. Retrieval Layer (RAG)**

### **3.1 Purpose**

Provides contextual knowledge to the reasoning model:

- Windows documentation  
- logs  
- registry snapshots  
- PowerShell output  
- device inventory  
- enterprise policies  
- your own documentation  

### **3.2 Requirements**

- MUST use embeddings for semantic search  
- MUST return top‑k relevant chunks  
- MUST inject retrieved context into the model prompt  
- MUST remain separate from memory  


# **4. Specialist Models & Tools Layer**

### **4.1 Purpose**

Handles tasks the reasoning model should NOT do:

- embeddings  
- classification  
- log categorization  
- lightweight vision  
- structured parsing  
- Windows introspection  
- PowerShell execution  
- registry reading  
- event log parsing  

### **4.2 Requirements**

- MUST use **local ONNX/WinML models** where appropriate  
- MUST expose deterministic tools  
- MUST return structured JSON  
- MUST be registered in a **ToolRegistry**  
- MUST be exposed to Semantic Kernel as plugins  

### **4.4 Required Tools (Initial)**

- `echo.text` (simple echo tool)  
- `system.info` (safe, read‑only system info)  
- Implement common Agent AI tools
---

# **5. Memory Layer**

### **5.1 Purpose**

Stores:

- device history  
- user preferences  
- past actions  
- known issues  
- environment state  
- resolved incidents  

### **5.2 Requirements**

- MUST be persistent  
- MUST be queryable  
- MUST be separate from RAG  
- MUST be updated after each loop  

### **5.4 Initial Implementation**

- Full implementation 

---

# **6. Safety & Validation Layer**

### **6.1 Purpose**

Prevents:

- destructive commands  
- hallucinated actions  
- unsafe registry edits  
- accidental file deletions  
- unauthorized operations  

### **6.2 Requirements**

- MUST validate user input  
- MUST validate tool actions  
- MUST block unsafe operations  
- MUST annotate blocked actions with metadata  

---

# **7. Orchestration Layer (Agent Loop)**

### **7.1 Purpose**

This is the engine that ties everything together.

### **7.2 Required Behavior**

The loop MUST follow this exact sequence:

1. Load memory  
2. Retrieve RAG context  
3. Build prompt  
4. Call reasoning model  
5. Parse tool actions  
6. Validate tool actions  
7. Execute tools  
8. Return observations  
9. Call model again if needed  
10. Store memory  

### **7.3 Required Interface**

```csharp
public interface IAgentLoop
{
    Task<AgentMessage> RunOnceAsync(IChatHistory history);
    Task<AgentMessage> RunUntilCompletionAsync(IChatHistory history);
}
```

---

# **8. Windows Introspection Layer**

### **8.1 Purpose**

Provides environmental awareness:

- PowerShell  
- registry  
- event logs  
- installed software  
- network configuration  
- Windows Update state  
- performance counters  
- WMI/WinRM  

### **8.2 Requirements**

- MUST be exposed as tools  
- MUST be safe  
- MUST be structured  
- MUST be auditable  

### **8.3 Initial Tools**

- `system.info`  
- `echo.text`  

---

# **9. Message Model**

### **9.1 Required Roles**

- `system`  
- `user`  
- `assistant`  
- `tool`  

### **9.2 Required Message Type**

```csharp
public sealed class AgentMessage
{
    public Guid Id { get; init; }
    public MessageRole Role { get; init; }
    public DateTimeOffset TimestampUtc { get; init; }
    public string Content { get; init; }
    public string? PayloadJson { get; init; }
    public IReadOnlyDictionary<string, string> Metadata { get; init; }
}
```

---

# **10. Tool Action Schema**

```csharp
public sealed class ToolAction
{
    public string ToolName { get; init; }
    public string ActionName { get; init; }
    public string CorrelationId { get; init; }
    public JsonElement Parameters { get; init; }
    public string? Reason { get; init; }
}
```

---

# **11. Tool Observation Schema**

```csharp
public sealed class ToolObservation
{
    public string ToolName { get; init; }
    public string ActionName { get; init; }
    public string CorrelationId { get; init; }
    public int ExitCode { get; init; }
    public string Status { get; init; }
    public string? ErrorMessage { get; init; }
    public JsonElement Data { get; init; }
}
```

---

# **12. Semantic Kernel Integration**

### **12.1 Requirements**

- MUST be used for:
  - model invocation  
  - plugin registration  
  - tool exposure  
  - message formatting  

- MUST NOT be removed  
- MUST NOT be replaced  

### **12.2 Required Factory**

```csharp
public interface ISemanticKernelFactory
{
    Task<Kernel> CreateKernelAsync(CancellationToken cancellationToken = default);
}
```

---

# **13. System Prompt Requirements**

The system prompt MUST:

- define the agent identity  
- define allowed actions  
- define tool usage rules  
- define JSON schema for tool actions  
- forbid hallucinated tools  
- instruct the model to use tools  
- instruct the model to mark final answers  

A non‑empty prompt MUST be included.

---

# **14. Non‑Functional Requirements**

The system MUST be:

- modular  
- auditable  
- deterministic  
- explicit  
- schema‑driven  
- safe  
- extensible  
- framework‑agnostic (except SK)  

---

# **15. What This Document Enables**

With this design:

- The AgentAI can implement the entire starting block without ambiguity  
- Semantic Kernel is guaranteed to remain in the system  
- Azure AI remote inference is fully supported  
- Local ONNX/WinML tools are fully supported  
- The agent loop is deterministic and auditable  
- All future layers can be added cleanly  

---
