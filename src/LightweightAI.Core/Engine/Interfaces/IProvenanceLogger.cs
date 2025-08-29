// Project Name: LightweightAI.Core
// File Name: IProvenanceLogger.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using LightweightAI.Core.Engine.Provenence;


namespace LightweightAI.Core.Engine.Interfaces;


public interface IProvenanceLogger
{
    void Log(ProvenancedDecision decision);
}