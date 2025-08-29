// Project Name: LightweightAI.Core
// File Name: EventLogLoader.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using System.Data;


namespace LightweightAI.Core.Loaders.Events;


public class EventLogLoader : IRefineryLoader
{
    /// <summary>
    ///     Loads event log data based on the provided source configuration.
    /// </summary>
    /// <param name="config">The configuration specifying the source of the event log data.</param>
    /// <returns>A <see cref="DataTable" /> containing the loaded event log data.</returns>
    public DataTable Load(Engine.config.SourceConfig config)
    {
        //  return CsvHelper.Load(config.Path);
        return new DataTable();
    }
}