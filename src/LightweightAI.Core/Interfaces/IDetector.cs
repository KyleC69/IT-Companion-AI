// Project Name: LightweightAI.Core
// File Name: IDetector.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using LightweightAI.Core.Engine.Models;


namespace LightweightAI.Core.Abstractions;


public interface IDetector
{
    string Name { get; }





    /// <summary>Update internal state and return a score in [0,1]. Higher = more anomalous.</summary>
    float UpdateAndScore(in EncodedEvent example, DateTimeOffset nowUtc);
}