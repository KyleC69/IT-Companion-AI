using Microsoft.Extensions.Configuration;

using Octokit;



namespace ITCompanionAI.Helpers;


public sealed record GitHubClientOptions(
    string ProductName,
    Uri ApiBaseAddress,
    string Token)
{
    public const string TokenConfigKey = "AIAPP:GITHUB_TOKEN";
    public const string SectionName = "AIAPP:GitHub";








    public static GitHubClientOptions FromConfiguration(IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(configuration);

        var productName = configuration[$"{SectionName}:ProductName"];
        if (string.IsNullOrWhiteSpace(productName))
        {
            productName = "ITCompanionAI";
        }

        var apiBase = configuration[$"{SectionName}:ApiBaseAddress"];
        if (string.IsNullOrWhiteSpace(apiBase))
        {
            apiBase = "https://api.github.com/";
        }

        if (!Uri.TryCreate(apiBase, UriKind.Absolute, out Uri apiBaseAddress))
        {
            throw new InvalidOperationException(
                $"Invalid GitHub API base address in configuration key '{SectionName}:ApiBaseAddress'.");
        }

        var token = configuration[TokenConfigKey];
        if (string.IsNullOrWhiteSpace(token))
        {
            throw new InvalidOperationException(
                $"Missing GitHub token in configuration key '{TokenConfigKey}'. Add it via user-secrets (recommended) or environment variables.");
        }

        return new GitHubClientOptions(productName, apiBaseAddress, token);
    }
}





public interface IGitHubClientFactory
{
    GitHubClient CreateClient();
}





public sealed class GitHubClientFactory : IGitHubClientFactory
{
    private readonly IConfiguration _configuration;








    public GitHubClientFactory(IConfiguration configuration)
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    }








    public GitHubClient CreateClient()
    {
        GitHubClientOptions options = GitHubClientOptions.FromConfiguration(_configuration);

        ProductHeaderValue productInfo = new(options.ProductName);
        GitHubClient client = new(productInfo, options.ApiBaseAddress)
        {
            Credentials = new Credentials(options.Token)
        };

        return client;
    }
}