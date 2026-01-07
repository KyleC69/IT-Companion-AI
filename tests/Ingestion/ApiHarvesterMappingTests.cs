// Project Name: CompanionTests
// File Name: ApiHarvesterMappingTests.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz KyleC69
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using System;

using ITCompanionAI.Helpers;

using Microsoft.VisualStudio.TestTools.UnitTesting;




[TestClass]
public sealed class ApiHarvesterMappingTests
{
    private sealed class NullGitHubClientFactory : IGitHubClientFactory
    {
        public Octokit.GitHubClient CreateClient()
        {
            throw new NotSupportedException("Network calls are not allowed in unit tests.");
        }
    }
}