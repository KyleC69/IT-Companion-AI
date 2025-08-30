// Project Name: LightweightAI.Core
// File Name: IAlertDispatcher.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using LightweightAI.Core.Engine.Provenance;


namespace LightweightAI.Core.Abstractions;


public interface IAlertDispatcher
{
    Task DispatchAsync(ProvenancedDecision decision, CancellationToken ct = default);
}