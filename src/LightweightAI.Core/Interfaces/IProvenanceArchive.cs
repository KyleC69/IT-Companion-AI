// Project Name: LightweightAI.Core
// File Name: IProvenanceArchive.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using LightweightAI.Core.Engine.Provenance;



namespace LightweightAI.Core.Interfaces;


public interface IProvenanceArchive
{
    void Archive(Guid envelopeId, IEnumerable<ProvenanceEntry> entries);
    void Log(ProvenancedDecision decision);
}