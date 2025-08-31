// Central stubs to satisfy missing platform/API references (minimal compile-only)


using System.Collections;

using Microsoft.Diagnostics.Tracing;
using Microsoft.Diagnostics.Tracing.Parsers;
using Microsoft.Diagnostics.Tracing.Session;



namespace LightweightAI.Core.Engine.Compat;




// Tracing level enum placeholder
public enum TracingLevel { Critical = 1, Error = 2, Warning = 3, Informational = 4, Verbose = 5 }

// Extension placeholders
internal static class TraceEventSessionExtensions
{
    public static void DisableKernelProvider(this TraceEventSession _, KernelTraceEventParser.Keywords __) { }
}

internal static class EtwSourceExtensions
{
    public static void RegisterUnhandledEvent(this ETWTraceEventSource _, Action<TraceEvent> __) { }
}



// Align mismatch between Engine.Models.AnomalySignal and Models.AnomalySignal