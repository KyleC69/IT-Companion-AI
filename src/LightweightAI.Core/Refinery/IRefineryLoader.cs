// Project Name: LightweightAI.Core
// File Name: IRefineryLoader.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using LightweightAI.Core.Engine.config;


namespace LightweightAI.Core.Refinery;


public interface IRefineryLoader
{
    System.Data.DataTable Load(SourceConfig config);
}