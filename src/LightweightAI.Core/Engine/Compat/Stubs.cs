// Central stubs to satisfy missing platform/API references (minimal compile-only)
using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Diagnostics.Tracing;
using Microsoft.Diagnostics.Tracing.Parsers;
using Microsoft.Diagnostics.Tracing.Session;

namespace LightweightAI.Core.Compat
{
    // ServiceController stub
    public class ServiceController
    {
        public string ServiceName { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string Status { get; set; } = "Unknown";
        public string ServiceType { get; set; } = "Unknown";
        public static ServiceController[] GetServices() => Array.Empty<ServiceController>();
        public static IEnumerable GetDevices() => Array.Empty<ServiceController>();
    }

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
}

namespace LightweightAI.Core.Models
{
    // Align mismatch between Engine.Models.AnomalySignal and Models.AnomalySignal
   
}