// Project Name: LightweightAI.Core
// File Name: IIngestionConfigurator.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace LightweightAI.Core.Interfaces;


/// <summary>
///     Parses a JSON ingestion configuration document and registers the corresponding source loaders
///     into the <see cref="ISourceRegistry" />.
/// </summary>
public interface IIngestionConfigurator
{
    void Configure(ISourceRegistry registry, IServiceProvider services, string jsonConfig);
}