// Project Name: LightweightAI.Core
// File Name: HysteresisDecider.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace LightweightAI.Core.Engine;

/// <summary>
/// Implements a simple hysteresis state machine used to suppress alert oscillation
/// around a threshold boundary. The decider flips to <c>true</c> only after the
/// observed value exceeds an upper threshold and returns to <c>false</c> only after
/// it drops below a lower threshold.
/// </summary>
public class HysteresisDecider
{
    private readonly double _lower;
    private readonly double _upper;
    private bool _state;





    public HysteresisDecider(double lower, double upper)
    {
        this._lower = lower;
        this._upper = upper;
    }





    /// <summary>
    /// Applies hysteresis logic to the provided value and returns the current latched state.
    /// </summary>
    public bool Decide(double value)
    {
        if (value > this._upper)
            this._state = true;
        else if (value < this._lower) this._state = false;
        return this._state;
    }
}