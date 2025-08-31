// Project Name: LightweightAI.Core
// File Name: RetrainingScheduler.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace LightweightAI.Core.Training;


/// <summary>
///     Placeholder rolling retraining scheduler tracking elapsed wall time and signaling when
///     a configured retraining interval (hours -> weeks) has elapsed.
/// </summary>
public sealed class RetrainingScheduler
{
    private readonly TimeSpan _interval;
    private DateTime _next;





    public RetrainingScheduler(TimeSpan interval)
    {
        this._interval = interval;
        this._next = DateTime.UtcNow + this._interval;
    }





    public bool ShouldRetrain(DateTime utcNow)
    {
        if (utcNow >= this._next)
        {
            this._next = utcNow + this._interval;
            return true;
        }

        return false;
    }
}