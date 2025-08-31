// Project Name: LightweightAI.Core
// File Name: IFeatureEncoder.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace LightweightAI.Core.Interfaces;


public interface IFeatureEncoder
{
    /// <summary>Transform normalized event into feature vector.</summary>
    EncodedEvent Encode(RawEvent normalized);
}