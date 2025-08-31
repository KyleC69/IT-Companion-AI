// Project Name: LightweightAI.Core
// File Name: ServiceRefinery.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace LightweightAI.Core.Loaders.Services;


public static class ServiceRefinery
{
    // Mapping raw headers to canonical names
    private static readonly Dictionary<string, string> ColumnMap = new()
    {
        { "Name", "ServiceName" },
        { "StartMode", "StartupTypeRaw" },
        { "State", "StatusRaw" },
        { "StartName", "LogOnAs" },
        { "Description", "Description" }
    };





    public static System.Data.DataTable Process(System.Data.DataTable raw)
    {
        System.Data.DataTable table = CanonicalizeColumns(raw);
        table = CoerceTypes(table);
        table = DeriveFeatures(table);
        table = TagContext(table, "Service");
        table = AlignTimestamps(table);
        return table;
    }





    private static System.Data.DataTable CanonicalizeColumns(System.Data.DataTable table)
    {
        foreach (System.Data.DataColumn col in table.Columns)
            if (ColumnMap.TryGetValue(col.ColumnName, out var canonical))
                col.ColumnName = canonical;
        return table;
    }





    private static System.Data.DataTable CoerceTypes(System.Data.DataTable table)
    {
        // Example: force strings, map enums, normalize case
        foreach (System.Data.DataRow row in table.Rows)
        {
            row["ServiceName"] = row["ServiceName"]?.ToString()?.Trim();
            row["StartupTypeRaw"] = row["StartupTypeRaw"]?.ToString()?.ToLower();
            row["StatusRaw"] = row["StatusRaw"]?.ToString()?.ToLower();
        }

        return table;
    }





    private static System.Data.DataTable DeriveFeatures(System.Data.DataTable table)
    {
        table.Columns.Add("StartupType");
        table.Columns.Add("Status");

        foreach (System.Data.DataRow row in table.Rows)
        {
            row["StartupType"] = row["StartupTypeRaw"] switch
            {
                "auto" or "automatic" => "Automatic",
                "manual" => "Manual",
                "disabled" => "Disabled",
                _ => "Unknown"
            };
            row["Status"] = row["StatusRaw"] switch
            {
                "running" => "Running",
                "stopped" => "Stopped",
                _ => "Other"
            };
        }

        return table;
    }





    private static System.Data.DataTable TagContext(System.Data.DataTable table, string type)
    {
        table.Columns.Add("SourceType");
        foreach (System.Data.DataRow row in table.Rows)
            row["SourceType"] = type;
        return table;
    }





    private static System.Data.DataTable AlignTimestamps(System.Data.DataTable table)
    {
        table.Columns.Add("CollectedUtc", typeof(DateTime));
        DateTime now = DateTime.UtcNow;
        foreach (System.Data.DataRow row in table.Rows)
            row["CollectedUtc"] = now;
        return table;
    }
}