// Project Name: LightweightAI.Core
// File Name: INormalizer.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using LightweightAI.Core.Engine.Models;


namespace LightweightAI.Core.Abstractions;


public interface INormalizer
{
    /// <summary>Coerce types, fill nulls, align timestamps, normalize severity.</summary>
    RawEvent Normalize(RawEvent input);
}