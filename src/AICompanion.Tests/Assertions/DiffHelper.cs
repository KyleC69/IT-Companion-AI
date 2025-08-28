// Project Name: AICompanion.Tests
// File Name: DiffHelper.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers



// Project Name: AICompanion.Tests
// File Name: DiffHelper.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using System.Text.Json;


namespace AICompanion.Tests.Assertions;


public static class DiffHelper
{
    public static string Diff<T>(T expected, T actual)
    {
        var expectedJson = JsonSerializer.Serialize(expected, new JsonSerializerOptions { WriteIndented = true });
        var actualJson = JsonSerializer.Serialize(actual, new JsonSerializerOptions { WriteIndented = true });

        if (expectedJson == actualJson)
            return "No differences found.";

        return $"Expected:\n{expectedJson}\n\nActual:\n{actualJson}";
    }
}