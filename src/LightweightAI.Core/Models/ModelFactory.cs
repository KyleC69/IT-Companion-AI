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
    public static IModel CreateStreamModel()
    {
        return new StreamAnomalyModel();
    }





    //TODO: Implement SnapshotTrendModel
    public static IModel CreateSnapshotModel()
    {
        return new SnapshotTrendModel();
    }
}


// Placeholder models