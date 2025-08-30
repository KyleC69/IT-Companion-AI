// Project Name: LightweightAI.Core
// File Name: InMemorySysmonBookmarkStore.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using System.Diagnostics.Eventing.Reader;


namespace LightweightAI.Core.Loaders.Sysmon;


/// <summary>
///     In-memory bookmark store for Sysmon loader (process lifetime scope).
///     Safe default; replace with a persistent implementation if desired.
/// </summary>
public sealed class InMemorySysmonBookmarkStore : ISysmonBookmarkStore
{
    private readonly Dictionary<(string sourceKey, string channel), EventBookmark> _bookmarks = new();
    private readonly object _gate = new();





    public EventBookmark? Load(string sourceKey, string channel)
    {
        lock (this._gate)
        {
            return this._bookmarks.TryGetValue((sourceKey, channel), out EventBookmark? b) ? b : null;
        }
    }





    public void Save(string sourceKey, string channel, EventBookmark bookmark)
    {
        lock (this._gate)
        {
            this._bookmarks[(sourceKey, channel)] = bookmark;
        }
    }
}