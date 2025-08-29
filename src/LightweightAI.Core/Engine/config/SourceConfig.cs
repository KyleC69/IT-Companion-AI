// Project Name: LightweightAI.Core
// File Name: SourceConfig.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace LightweightAI.Core.Engine.config;

/// <summary>
/// Configuration describing a single raw data source (file/stream). Currently minimal – will
/// expand to include source identity, format metadata, schema version, and optional filtering.
/// </summary>
public class SourceConfig
{
    /// <summary>
    /// Raw stream containing source data (CSV, JSON lines, etc.). Caller owns lifecycle.
    /// </summary>
    public Stream Path { get; set; }
}