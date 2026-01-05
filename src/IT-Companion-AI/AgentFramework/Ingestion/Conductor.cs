using System;
using System.Collections.Generic;
using System.Text;
using ITCompanionAI.AgentFramework;
using ITCompanionAI.DatabaseContext;


namespace ITCompanionAI.AgentFramework.Ingestion;


public class IngestionOrchestrator 
{
    private readonly AIAgentRagContext _db;
    private readonly ApiHarvester _apiHarvester;
    private readonly ApiDocHarvester _docHarvester;
    private readonly ApiDocParser _docParser;

    public IngestionOrchestrator(
        AIAgentRagContext db,
        ApiHarvester apiHarvester,
        ApiDocHarvester docHarvester,
        ApiDocParser docParser)
    {
        _db = db;
        _apiHarvester = apiHarvester;
        _docHarvester = docHarvester;
        _docParser = docParser;
    }

    public async Task<IngestionArtifact> RunAsync(IngestionRequest request, CancellationToken ct = default)
    {
        // 1. Harvest API  Returns ApiSurfaceDto
    

        // 2. Harvest docs
        IEnumerable<object> rawDocs = await _docHarvester.HarvestDocsAsync(request, ct);

        // 3. Parse docs
      // Extracts rawdocs

        // 4. Map DTO → EF entities
   //     var apiEntities = ApiEfMapper.ToEntities(rawApi);
   //     var docEntities = DocEfMapper.ToEntities(parsedDocs);

        // 5. Create ingestion run record
      
        var infor = new IngestionRunInfo
            {
                RunId = null,
                TimestampUtc = default,
                SchemaVersion = null,
                SourceSnapshot = null,
              
            }
            ;

        // 6. Save to DB
        /*    _db.IngestionRuns.Add(run);
            _db.ApiTypes.AddRange(apiEntities.Types);
            _db.ApiMembers.AddRange(apiEntities.Members);
            _db.DocPages.AddRange(docEntities.Pages);
            _db.DocSections.AddRange(docEntities.Sections);
            _db.DocCodeBlocks.AddRange(docEntities.CodeBlocks);

            await _db.SaveChangesAsync(ct);  */

        // 7. Return the assembled artifact
        return new IngestionArtifact
        {
          //  IngestionRun = run,
     //       ApiSurface = apiEntities.ToDto(),
    //        Docs = docEntities.ToDto()
        };
    }
}


