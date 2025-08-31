// Project Name: LightweightAI.Core
// File Name: ISysmonBookmarkStore.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using System.Diagnostics.Eventing.Reader;


namespace LightweightAI.Core.Loaders.Sysmon;


/// <summary>
///     Bookmark storage interface for Sysmon event log reading.
///     Implementations can persist to memory, file, or external store.
/// </summary>
public interface ISysmonBookmarkStore
{
    EventBookmark? Load(string sourceKey, string channel);
    void Save(string sourceKey, string channel, EventBookmark bookmark);
}