// Project Name: LightweightAI.Core
// File Name: ServiceLoader.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using System.Data;

using LightweightAI.Core.Engine.config;


namespace LightweightAI.Core.Loaders.Services;


public class ServiceLoader : IRefineryLoader
{
    public DataTable Load(SourceConfig config)
    {
        if (config == null || config.Path == null) throw new ArgumentNullException(nameof(config), "SourceConfig or its Path cannot be null.");

        return ReadData(config);
    }




    /// <summary>
    /// Reads data from the specified <see cref="SourceConfig"/> and returns it as a <see cref="DataTable"/>.
    /// </summary>
    /// <param name="config">The configuration containing the source path for the data to be read.</param>
    /// <returns>A <see cref="DataTable"/> containing the data read from the source.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="config"/> or its <see cref="SourceConfig.Path"/> is <c>null</c>.</exception>
    internal DataTable ReadData(SourceConfig config)
    {
        using var reader = new StreamReader(config.Path);
        using var csv = new CsvHelper.CsvReader(reader, System.Globalization.CultureInfo.InvariantCulture);
        var dataTable = new DataTable();
        try
        {
            // Read the CSV header and add columns to the DataTable
            csv.Read();
            csv.ReadHeader();
            foreach (var header in csv.HeaderRecord)
            {
                dataTable.Columns.Add(header);
            }
            // Read the CSV records and populate the DataTable
            while (csv.Read())
            {
                var row = dataTable.NewRow();
                foreach (var header in csv.HeaderRecord)
                {
                    row[header] = csv.GetField(header);
                }
                dataTable.Rows.Add(row);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error reading data: {ex.Message}");
            throw;
        }
        return dataTable;
    }
}