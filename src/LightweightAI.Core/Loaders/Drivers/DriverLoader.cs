// Project Name: LightweightAI.Core
// File Name: DriverLoader.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using LightweightAI.Core.Engine.config;


namespace LightweightAI.Core.Loaders.Drivers;

// TODO: Implement DriverLoader to read from different data sources (e.g., databases, APIs)
public class DriverLoader : IRefineryLoader
{
    public System.Data.DataTable Load(SourceConfig config)
    {
        if (config == null || config.Path == null)
            throw new ArgumentNullException(nameof(config), "SourceConfig or its Path cannot be null.");

        // Assuming a similar approach to ServiceLoader's ReadData method
        using var reader = new StreamReader(config.Path);
        var dataTable = new System.Data.DataTable();

        // Example CSV parsing logic (can be replaced with CsvHelper or similar library)
        var headers = reader.ReadLine()?.Split(',');
        if (headers == null) throw new InvalidOperationException("The source data is empty or improperly formatted.");

        foreach (var header in headers) dataTable.Columns.Add(header);

        while (!reader.EndOfStream)
        {
            var row = reader.ReadLine()?.Split(',');
            if (row != null) dataTable.Rows.Add(row);
        }

        return dataTable;
    }
}