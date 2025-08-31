// Project Name: LightweightAI.Core
// File Name: DriverRefinery.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace LightweightAI.Core.Loaders.Drivers;


public static class DriverRefinery
{
    private static readonly Dictionary<string, string> ColumnMap = new()
    {
        { "DeviceName", "DeviceName" },
        { "DriverVersion", "Version" },
        { "DriverProviderName", "Provider" },
        { "DriverDate", "ReleaseDateRaw" },
        { "IsSigned", "IsSignedRaw" },
        { "InfName", "InfFile" }
    };





    public static System.Data.DataTable Process(System.Data.DataTable raw)
    {
        System.Data.DataTable table = CanonicalizeColumns(raw);
        table = CoerceTypes(table);
        table = DeriveFeatures(table);
        table = TagContext(table, "Driver");
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
            if (DateTime.TryParse(row["ReleaseDateRaw"]?.ToString(), out DateTime dt))
                row["ReleaseDateRaw"] = dt;
            else
                row["ReleaseDateRaw"] = DBNull.Value;

            row["IsSignedRaw"] = row["IsSignedRaw"]?.ToString()?.ToLower() switch
            {
                "true" or "yes" or "1" => true,
                _ => false
            };
        }

        return table;
    }





    private static System.Data.DataTable DeriveFeatures(System.Data.DataTable table)
    {
        table.Columns.Add("DriverAgeDays", typeof(int));
        table.Columns.Add("SignatureStatus");

        DateTime now = DateTime.UtcNow;

        foreach (System.Data.DataRow row in table.Rows)
        {
            DateTime date = row["ReleaseDateRaw"] is DateTime d ? d : now;
            row["DriverAgeDays"] = (now - date).Days;
            row["SignatureStatus"] = (bool)row["IsSignedRaw"] ? "Signed" : "Unsigned";
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