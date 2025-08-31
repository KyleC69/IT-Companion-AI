// Project Name: LightweightAI.Core
// File Name: ModelFactory.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace LightweightAI.Core.Models;


public static class ModelFactory
{
    //TODO: Implement StreamAnomalyModel
    public static IStreamAnomalyModel CreateStreamModel()
    {
        var cfg = new AnomalyConfig(Alpha: 0.3, ZThreshold: 2.7, 3.0);
        var model = new StreamAnomalyModel(cfg);
        return model;
    }
}



// Placeholder models