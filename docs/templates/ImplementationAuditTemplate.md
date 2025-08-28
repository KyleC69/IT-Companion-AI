Alright — let’s get our scaffolding in place so we can drop the `docs` content straight in when you paste it.  

Here’s the **blank audit table** we’ll use as we parse each document:  

| Feature / Capability | Doc Reference | Intended Module(s) | Implementation Location | Status | Notes / Gaps |
|----------------------|---------------|--------------------|-------------------------|--------|--------------|
| *(example)* Fusion hysteresis state machine | `fusion.md` §3.2 | Orchestrator | `Orchestrator/FusionEngine.cs` | ✅ Implemented | Matches doc spec, dwell time param present |
| … | … | … | … | … | … |

**How we’ll fill it in:**  
1. **Feature / Capability** – the specific function or behavior described in the docs.  
2. **Doc Reference** – filename and section so we can trace it back.  
3. **Intended Module(s)** – where the docs say it should live (Engine, Refinery, etc.).  
4. **Implementation Location** – actual file/class in `LightweightAI.Core`.  
5. **Status** – ✅ implemented, ⚠ partial, ❌ missing.  
6. **Notes / Gaps** – drift from spec, missing params, or integration issues.  

Once you paste the first doc (or even just a section), I’ll:  
- Extract every feature it describes.  
- Drop them into this table.  
- Cross‑check against the repo’s code layout.  

When we’ve walked through the whole `docs` folder, we’ll have a complete “green light” map for training readiness.  

Go ahead and paste the first doc or section you want to start with, and I’ll begin populating the table.
