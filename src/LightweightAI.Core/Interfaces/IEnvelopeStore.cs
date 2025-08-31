// Project Name: LightweightAI.Core
// File Name: IEnvelopeStore.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace LightweightAI.Core.Interfaces;


public interface IEnvelopeStore
{
    void Save<T>(QAEnvelope<T> envelope);
    QAEnvelope<T>? Get<T>(Guid id);
}