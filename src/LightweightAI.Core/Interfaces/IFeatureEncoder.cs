// Project Name: LightweightAI.Core
// File Name: IFeatureEncoder.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using LightweightAI.Core.Engine.Models;


namespace LightweightAI.Core.Abstractions;


public interface IFeatureEncoder
{
    /// <summary>Transform normalized event into feature vector.</summary>
    EncodedEvent Encode(RawEvent normalized);
}