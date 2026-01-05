using System;
using System.IO;
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

    [TestMethod]
    public async Task HarvestFromDirectoryAsync_WhenSnapshotMissing_BootstrapsSnapshotAndRun()
    {
        await using var db = new AIAgentRagContext(new DbContextOptionsBuilder<AIAgentRagContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString("N"))
            .Options);

        var harvester = new ApiHarvester(db, new NullGitHubClientFactory());

        var requestedSnapshotId = Guid.NewGuid();

        try
        {
            // Directory will not exist in unit test; we only care that bootstrap runs before load.
            await harvester.HarvestFromDirectoryAsync(requestedSnapshotId, "z:\\does-not-exist", CancellationToken.None);
            Assert.Fail("Expected DirectoryNotFoundException was not thrown.");
        }
        catch (DirectoryNotFoundException)
        {
        }

        Assert.IsTrue(await db.ingestion_runs.AnyAsync(), "Expected an ingestion_run row to be created.");
        Assert.IsTrue(await db.source_snapshots.AnyAsync(), "Expected a source_snapshot row to be created.");

        var snapshot = await db.source_snapshots.SingleAsync();
        Assert.AreNotEqual(Guid.Empty, snapshot.id);
        Assert.AreNotEqual(Guid.Empty, snapshot.ingestion_run_id);
        Assert.IsFalse(string.IsNullOrWhiteSpace(snapshot.snapshot_uid));
    }

    [TestMethod]
    public async Task HarvestFromDirectoryAsync_WhenSnapshotExists_DoesNotCreateDuplicateSnapshot()
    {
        await using var db = new AIAgentRagContext(new DbContextOptionsBuilder<AIAgentRagContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString("N"))
            .Options);

        var harvester = new ApiHarvester(db, new NullGitHubClientFactory());

        // Seed an existing run + snapshot with a known id.
        var run = new ITCompanionAI.AIVectorDb.ingestion_run
        {
            id = Guid.NewGuid(),
            timestamp_utc = DateTime.UtcNow,
            schema_version = "1.0.0"
        };

        var snapshotId = Guid.NewGuid();
        var snapshot = new ITCompanionAI.AIVectorDb.source_snapshot
        {
            id = snapshotId,
            ingestion_run_id = run.id,
            snapshot_uid = snapshotId.ToString("D"),
            language = "csharp"
        };

        db.ingestion_runs.Add(run);
        db.source_snapshots.Add(snapshot);
        await db.SaveChangesAsync();

        try
        {
            await harvester.HarvestFromDirectoryAsync(snapshotId, "z:\\does-not-exist", CancellationToken.None);
            Assert.Fail("Expected DirectoryNotFoundException was not thrown.");
        }
        catch (DirectoryNotFoundException)
        {
        }

        Assert.AreEqual(1, await db.ingestion_runs.CountAsync());
        Assert.AreEqual(1, await db.source_snapshots.CountAsync());
        Assert.AreEqual(snapshotId, (await db.source_snapshots.SingleAsync()).id);
    }
}
