// Project Name: LightweightAI.Core
// File Name: EventLogRefinery.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace LightweightAI.Core.Loaders.Events;


public static class EventLogRefinery
{
    private static readonly Dictionary<string, string> ColumnMap = new()
    {
        { "TimeCreated", "EventTimeUtc" },
        { "Id", "EventId" },
        { "LevelDisplayName", "SeverityRaw" },
        { "ProviderName", "Source" },
        { "Message", "MessageRaw" }
    };





    public static System.Data.DataTable Process(System.Data.DataTable raw)
    {
        System.Data.DataTable table = CanonicalizeColumns(raw);
        table = CoerceTypes(table);
        table = DeriveFeatures(table);
        table = TagContext(table, "EventLog");
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
        foreach (System.Data.DataRow row in table.Rows)
        {
            if (DateTime.TryParse(row["EventTimeUtc"]?.ToString(), out DateTime dt))
                row["EventTimeUtc"] = dt.ToUniversalTime();
            else
                row["EventTimeUtc"] = DBNull.Value;

            row["EventId"] = int.TryParse(row["EventId"]?.ToString(), out var id) ? id : -1;
            row["SeverityRaw"] = row["SeverityRaw"]?.ToString()?.ToLower();
        }

        return table;
    }





    private static System.Data.DataTable DeriveFeatures(System.Data.DataTable table)
    {
        table.Columns.Add("SeverityScore", typeof(int));
        table.Columns.Add("MessageLength", typeof(int));

        foreach (System.Data.DataRow row in table.Rows)
        {
            // Map human-friendly severity
            row["SeverityScore"] = row["SeverityRaw"] switch
            {
                "critical" => 5,
                "error" => 4,
                "warning" => 3,
                "info" => 2,
                "verbose" => 1,
                _ => 0
            };

            row["MessageLength"] = row["MessageRaw"]?.ToString()?.Length ?? 0;
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