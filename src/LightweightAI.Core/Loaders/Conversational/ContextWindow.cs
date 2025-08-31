// Project Name: LightweightAI.Core
// File Name: ContextWindow.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace LightweightAI.Core.Loaders.Conversational;


public class ContextWindow
{
    private readonly List<string> _window = new();
    public int MaxSize { get; set; } = 10;





    public void AddToWindow(string query)
    {
        if (this._window.Count >= this.MaxSize) this._window.RemoveAt(0);
        this._window.Add(query);
    }





    public List<string> GetRecent(int count)
    {
        if (count <= 0) return new List<string>();
        return this._window.Skip(Math.Max(0, this._window.Count - count)).ToList();
    }





    public void Clear()
    {
        this._window.Clear();
    }
}