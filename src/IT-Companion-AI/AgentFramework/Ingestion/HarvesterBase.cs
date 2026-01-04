// Project Name: SKAgent
// File Name: HarvesterBase.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using ITCompanionAI.AgentFramework.Models;

using Microsoft.CodeAnalysis;


namespace ITCompanionAI.AgentFramework.Ingestion;


public class HarvesterBase
{
    public static class SourceLocator
    {
        public static ApiSourceLocation? From(ISymbol symbol)
        {
            Location? loc = symbol.Locations.FirstOrDefault(l => l.IsInSource);
            if (loc == null)
            {
                return null;
            }

            FileLinePositionSpan span = loc.GetLineSpan();
            return new ApiSourceLocation
            {
                FilePath = span.Path,
                StartLine = span.StartLinePosition.Line + 1,
                EndLine = span.EndLinePosition.Line + 1
            };
        }
    }
}