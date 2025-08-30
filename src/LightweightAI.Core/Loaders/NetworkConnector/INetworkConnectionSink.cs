// Project Name: LightweightAI.Core
// File Name: INetworkConnectionSink.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace LightweightAI.Core.Loaders.NetworkConnector;


public interface INetworkConnectionSink
{
    Task EmitBatchAsync(IReadOnlyList<NetworkConnectionRecord> batch, CancellationToken ct);
}