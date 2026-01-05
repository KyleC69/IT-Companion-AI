// Project Name: SKAgent
// File Name: KnowledgeAPI.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz KyleC69
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using System.Data;

using ITCompanionAI.Context;

using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;


namespace ITCompanionAI.AgentFramework;


/// <summary>
/// </summary>
public class KnowledgeAPI(KnowledgeCuratorContext db)
{






    /// <summary>
    /// 
    /// </summary>
    /// <param name="schemaVersion"></param>
    /// <param name="notes"></param>
    /// <returns></returns>
    public async Task<Guid> BeginIngestionRunAsync(string schemaVersion, string? notes)
    {
        var ingestionRunIdParam = new SqlParameter { ParameterName = "@IngestionRunId", SqlDbType = SqlDbType.UniqueIdentifier, Direction = ParameterDirection.Output };
        await db.Database.ExecuteSqlRawAsync("EXEC dbo.BeginIngestionRun @SchemaVersion = {0}, @Notes = {1}, @IngestionRunId = @IngestionRunId OUTPUT", schemaVersion, notes ?? (object)DBNull.Value, ingestionRunIdParam).ConfigureAwait(false);
        return (Guid)ingestionRunIdParam.Value;
    }







    public async Task<Guid> CreateSnapshotAsync(Guid ingestionRunId, string snapshotUid, string? repoUrl, string? branch, string? commit, string? language, string? packageName, string? packageVersion, string? configJson)
    {
        var snapshotIdParam = new SqlParameter { ParameterName = "@SnapshotId", SqlDbType = SqlDbType.UniqueIdentifier, Direction = ParameterDirection.Output };
        await db.Database.ExecuteSqlRawAsync(@"EXEC dbo.CreateSourceSnapshot @IngestionRunId = {0}, @SnapshotUid = {1}, @RepoUrl = {2}, @Branch = {3}, @RepoCommit = {4}, @Language = {5}, @PackageName = {6}, @PackageVersion = {7}, @ConfigJson = {8}, @SnapshotId = @SnapshotId OUTPUT", ingestionRunId, snapshotUid, (object?)repoUrl ?? DBNull.Value, (object?)branch ?? DBNull.Value, (object?)commit ?? DBNull.Value, (object?)language ?? DBNull.Value, (object?)packageName ?? DBNull.Value, (object?)packageVersion ?? DBNull.Value, (object?)configJson ?? DBNull.Value, snapshotIdParam).ConfigureAwait(false);
        return (Guid)snapshotIdParam.Value;
    }
}