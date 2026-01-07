using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using ITCompanionAI.AgentFramework.Ingestion;
using ITCompanionAI.Helpers;

using Microsoft.VisualStudio.TestTools.UnitTesting;


using Microsoft.Extensions.Logging;
using ITCompanionAI.KBCurator;

using Microsoft.Extensions.Logging.Abstractions;




[TestClass]
public sealed class ApiHarvesterMappingTests
{
    private sealed class NullGitHubClientFactory : IGitHubClientFactory
    {
        public Octokit.GitHubClient CreateClient() => throw new NotSupportedException("Network calls are not allowed in unit tests.");
    }

    
    
    
    
    
    
    
}
