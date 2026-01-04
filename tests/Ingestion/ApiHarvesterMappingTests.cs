using System;
using System.Threading;
using System.Threading.Tasks;

using ITCompanionAI.AgentFramework.Ingestion;
using ITCompanionAI.DatabaseContext;
using ITCompanionAI.Helpers;

using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CompanionTests.Ingestion;

[TestClass]
public sealed class ApiHarvesterMappingTests
{
    private sealed class NullGitHubClientFactory : IGitHubClientFactory
    {
        public Octokit.GitHubClient CreateClient() => throw new NotSupportedException("Network calls are not allowed in unit tests.");
    }

    [TestMethod]
    public async Task HarvestAsync_WhenSnapshotIdEmpty_Throws()
    {
        await using var db = new AIAgentRagContext(new DbContextOptionsBuilder<AIAgentRagContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString("N"))
            .Options);

        var harvester = new ApiHarvester(db, new NullGitHubClientFactory());

        try
        {
            await harvester.HarvestAsync(Guid.Empty, "o", "r", "b", "d", CancellationToken.None);
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.Fail("Expected ArgumentException was not thrown.");
        }
        catch (ArgumentException)
        {
        }
    }

    [TestMethod]
    public async Task HarvestAsync_WhenOwnerMissing_Throws()
    {
        await using var db = new AIAgentRagContext(new DbContextOptionsBuilder<AIAgentRagContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString("N"))
            .Options);

        var harvester = new ApiHarvester(db, new NullGitHubClientFactory());

        try
        {
            await harvester.HarvestAsync(Guid.NewGuid(), " ", "r", "b", "d", CancellationToken.None);
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.Fail("Expected ArgumentException was not thrown.");
        }
        catch (ArgumentException)
        {
        }
    }

    [TestMethod]
    public async Task HarvestAsync_WhenNetworkAttempted_ThrowsNotSupported()
    {
        await using var db = new AIAgentRagContext(new DbContextOptionsBuilder<AIAgentRagContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString("N"))
            .Options);

        var harvester = new ApiHarvester(db, new NullGitHubClientFactory());

        try
        {
            await harvester.HarvestAsync(Guid.NewGuid(), "o", "r", "b", "d", CancellationToken.None);
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.Fail("Expected NotSupportedException was not thrown.");
        }
        catch (NotSupportedException)
        {
        }
    }
}
