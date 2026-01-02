using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using System.Text;
using ITCompanionAI;

using ITCompanionAI.AgentFramework.Ingestion;

namespace ITCompanionAI;

public class DependencyInjection
{


    public void RegisterServices() 
    {
        var path=Path.Combine(@"d:\skApiRepo\semantic-kernel\dotnet","src");

    //    var orchestrator = new IngestionOrchestrator(new AiagentRagContext(), new ApiHarvester(path), new XmlDocExtractor(), new MarkdownDocParser());



    }

}
