using System;
using System.Collections.Generic;

using ITCompanionAI.Helpers;

using Microsoft.Extensions.Configuration;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CompanionTests.Helpers;

[TestClass]
public sealed class GitHubClientFactoryTests
{
    [TestMethod]
    public void FromConfiguration_WhenTokenMissing_Throws()
    {
        var config = new ConfigurationBuilder().AddInMemoryCollection().Build();

        try
        {
            _ = GitHubClientOptions.FromConfiguration(config);
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.Fail("Expected InvalidOperationException was not thrown.");
        }
        catch (InvalidOperationException)
        {
        }
    }

    [TestMethod]
    public void FromConfiguration_WhenApiBaseInvalid_Throws()
    {
        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(new[]
            {
                new KeyValuePair<string, string?>(GitHubClientOptions.TokenConfigKey, "ghp_testtoken"),
                new KeyValuePair<string, string?>("ITAI:GitHub:ApiBaseAddress", "not a url")
            })
            .Build();

        try
        {
            _ = GitHubClientOptions.FromConfiguration(config);
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.Fail("Expected InvalidOperationException was not thrown.");
        }
        catch (InvalidOperationException)
        {
        }
    }

    [TestMethod]
    public void CreateClient_WhenTokenPresent_SetsCredentials()
    {
        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(new[]
            {
                new KeyValuePair<string, string?>(GitHubClientOptions.TokenConfigKey, "ghp_testtoken"),
                new KeyValuePair<string, string?>("ITAI:GitHub:ProductName", "UnitTests")
            })
            .Build();

        IGitHubClientFactory factory = new GitHubClientFactory(config);
        var client = factory.CreateClient();

        Assert.IsNotNull(client);
        Assert.IsNotNull(client.Credentials);
        Assert.AreEqual("ghp_testtoken", client.Credentials.Password);
    }
}
