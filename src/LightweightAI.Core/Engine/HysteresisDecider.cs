// Project Name: LightweightAI.Core
// File Name: HysteresisDecider.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace LightweightAI.Core.Engine;


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





    public bool Decide(double value)
    {
        if (value > this._upper)
            this._state = true;
        else if (value < this._lower) this._state = false;
        return this._state;
    }
}