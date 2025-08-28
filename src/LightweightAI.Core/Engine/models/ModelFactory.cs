// Project Name: LightweightAI.Core
// File Name: ModelFactory.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace LightweightAI.Core.Engine.models;


public static class ModelFactory
{
    public static IModel CreateStreamModel()
    {
        return new StreamAnomalyModel();
    }





    public static IModel CreateSnapshotModel()
    {
        return new SnapshotTrendModel();
    }
}


// Placeholder models