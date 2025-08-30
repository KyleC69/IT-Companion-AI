// Project Name: LightweightAI.Core
// File Name: IEventLoader.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace LightweightAI.Core.Interfaces;


/// <summary>
///     Contract for loading events of a given type.
/// </summary>
/// <typeparam name="T">The event type to load.</typeparam>
public interface IEventLoader<T>
{
    /// <summary>
    ///     Loads events from the underlying source.
    /// </summary>
    /// <returns>Sequence of loaded events.</returns>
    IEnumerable<T> LoadEvents();
}