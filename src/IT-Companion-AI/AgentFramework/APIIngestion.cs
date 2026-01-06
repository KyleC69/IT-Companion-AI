// Project Name: SKAgent
// File Name: APIIngestion.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz KyleC69
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using System.Data;

using ITCompanionAI.AgentFramework.Ingestion;
using ITCompanionAI.Helpers;
using ITCompanionAI.KnowledgeBase;

using Microsoft.CodeAnalysis;
using Microsoft.Data.SqlClient;


namespace ITCompanionAI.AgentFramework;


/// <summary>
/// </summary>
public class APIIngestion : RoslynHarvesterBase
{
    private readonly KnowledgeBaseContext _db;







    public APIIngestion(KnowledgeBaseContext db, IGitHubClientFactory gitHubClientFactory) : base(gitHubClientFactory)
    {
        _db = db;
    }







    public APIIngestion(KnowledgeBaseContext db)
    {
        _db = db;
    }













    public async Task StartIngestionAsync(Guid runid,CancellationToken token)
    {
        
            // Load the solution from the specified directory
            Solution solution = await LoadSolutionFromDirectoryAsync("D:\\SKAPIRepo\\semantic-kernel\\dotnet\\src", token).ConfigureAwait(false);
            
           
            var api= await WalkSolutionAsync(solution, token).ConfigureAwait(false);
            List<SyntaxTypeTree> list=api.Types.ToList();

        foreach (SyntaxTypeTree type in list)
        {

       //    foreach (var t in type.)
            {
                
                
            }
           

            // You would then iterate through members, parameters, etc., and add them similarly
            // For now, this just adds the type.
        }













    }







    private async Task MapTypeObjAsync(ApiType Type)
    {
        //method should map the properties of the type to the db type
        // ApiType and be added to the db context
        //unknown values should be an empty string or a null collection initializer []
        //some values are calculate by helpers and need to be called here
        //obj will be added to db context but not saved !! Important !!


        _db.ApiTypes.Add(Type);
    }







    public async Task<Guid> BeginIngestionRunAsync(KnowledgeBaseContext db,
        string schemaVersion,
        string? notes)
    {
        var ingestionRunIdParam = new SqlParameter
        {
            ParameterName = "@IngestionRunId",
            SqlDbType = SqlDbType.UniqueIdentifier,
            Direction = ParameterDirection.Output
        };


        return Guid.Empty;
    }







    public async Task<Guid> CreateSnapshotAsync(
        KnowledgeBaseContext db,
        Guid ingestionRunId,
        string snapshotUid,
        string? repoUrl,
        string? branch,
        string? commit,
        string? language,
        string? packageName,
        string? packageVersion,
        string? configJson)
    {
        if (db == null)
        {
            throw new ArgumentNullException(nameof(db), "Database context cannot be null.");
        }

        if (string.IsNullOrWhiteSpace(snapshotUid))
        {
            throw new ArgumentException("Snapshot UID cannot be null or empty.", nameof(snapshotUid));
        }

        try
        {


            return Guid.Empty;
        }
        catch (SqlException sqlEx)
        {
            // Log the exception (logging mechanism assumed to be in place)
            throw new InvalidOperationException("An error occurred while executing the SQL command.", sqlEx);
        }
        catch (Exception ex)
        {
            // Log the exception (logging mechanism assumed to be in place)
            throw new InvalidOperationException("An unexpected error occurred during snapshot creation.", ex);
        }
    }
}