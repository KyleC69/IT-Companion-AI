// Project Name: LightweightAI.Core
// File Name: Model.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace LightweightAI.Core.Models;


public class Model
{
    public (string Answer, double Confidence) Infer(string question)
    {
        return ("Provenance logging is the recorded history of data as it moves through the system.", 0.91);
    }
}