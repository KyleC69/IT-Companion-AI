using System.Text.Json;
using System.Text.Json.Serialization;

using ITCompanionAI.Services;

using Microsoft.Extensions.Logging;

using YamlDotNet.Serialization;




namespace ITCompanionAI.Ingestion.Docs;





public sealed class TocFetcher
{
    private readonly HttpClientService _httpClient;
    private readonly ILogger<TocFetcher> _logger;








    public TocFetcher()
    {
        _httpClient = App.GetService<HttpClientService>();
        _logger = App.GetService<ILogger<TocFetcher>>();
    }








    /// <summary>
    ///     Asynchronously fetches a table of contents (TOC) from the specified base URL.
    ///     Grabs the toc.yml file from github directly using their API octocat. then we will grab all of the relative links
    ///     from the TOC.
    /// </summary>
    /// <param name="baseUrl">
    ///     The base URL from which the TOC will be fetched.
    /// </param>
    /// <returns>
    ///     A task representing the asynchronous operation. The task result contains YML file of <see cref="TocItem" /> objects
    ///     representing the fetched TOC, or an empty list if no TOC could be retrieved.
    /// </returns>
    /// <exception cref="JsonException">
    ///     Thrown when the TOC JSON cannot be deserialized.
    /// </exception>
    /// <exception cref="ArgumentNullException">
    ///     Thrown when the <paramref name="baseUrl" /> is null or empty.
    /// </exception>
    public async Task<List<string>> FetchAsync(string baseUrl)
    {

        //TODO: and a gate to ensure we only fetch TOC from Doc Repo on github, and not from any arbitrary URL, to avoid SSRF risks.
        HttpResponseMessage ymlTOC = await _httpClient.GetAsync(baseUrl);

        ymlTOC.EnsureSuccessStatusCode();
        var tocString = await ymlTOC.Content.ReadAsStringAsync();


        IDeserializer deserializer = new DeserializerBuilder().IgnoreUnmatchedProperties().Build();
        TocRoot anodes = deserializer.Deserialize<TocRoot>(tocString);

        var mdHrefs = Flatten(anodes?.items ?? [])
                .Select(n => n.href)
                .Where(h => !string.IsNullOrWhiteSpace(h) && h.EndsWith(".md", StringComparison.OrdinalIgnoreCase))
                .ToList();


        return mdHrefs;
    }








    private static IEnumerable<TocNode> Flatten(IEnumerable<TocNode> nodes)
    {
        return nodes.SelectMany(n => new[]
        {
                n
        }.Concat(Flatten(n.items ?? [])));
    }








    private static string NormalizeHref(string href)
    {
        if (string.IsNullOrWhiteSpace(href))
        {
            return href;
        }

        // Learn TOC often returns site-relative paths like "overview/xyz".
        if (href.StartsWith("http", StringComparison.OrdinalIgnoreCase))
        {
            return href;
        }

        href = href.TrimStart('/');
        return $"https://learn.microsoft.com/en-us/agent-framework/{href}";
    }








    public sealed class TocRoot
    {
        public List<TocNode>? items { get; set; }
    }





    public sealed class TocNode
    {
        public string? name { get; set; }
        public string? href { get; set; }
        public bool? expanded { get; set; }
        public List<TocNode>? items { get; set; }
    }





    public static class TocNormalizer
    {
        public static void Normalize(List<TocItem> items, string baseUrl)
        {
            Uri baseUri = new(baseUrl, UriKind.Absolute);

            // Resolve relative hrefs against the directory that contains the docset's toc.json.
            // For Learn, toc.json lives at the docset root, so base for hrefs should be:
            //   https://learn.microsoft.com/en-us/agent-framework/
            // even if baseUrl points to a deep page.
            var segments = baseUri.AbsolutePath.Split('/', StringSplitOptions.RemoveEmptyEntries);
            Uri root;
            if (segments.Length >= 2)
            {
                // keep /{locale}/{docset}/
                root = new Uri(baseUri, $"/{segments[0]}/{segments[1]}/");
            }
            else
            {
                root = new Uri(baseUri, "/");
            }

            foreach (TocItem item in items)
            {
                if (!string.IsNullOrWhiteSpace(item.Href) &&
                    !item.Href.StartsWith("http", StringComparison.OrdinalIgnoreCase))
                {
                    item.Href = new Uri(root, item.Href.TrimStart('/')).ToString().TrimEnd('/');
                }

                if (item.Items?.Count > 0)
                {
                    Normalize(item.Items, baseUrl);
                }
            }
        }
    }





    private sealed class LearnTocResponse2
    {
        [JsonPropertyName("items")] public List<TocItem> Items { get; set; } = [];
    }





    public class TocItem
    {
        [JsonPropertyName("toc_title")] public string Name { get; set; }

        [JsonPropertyName("href")] public string Href { get; set; }

        // Learn TOC uses "children" in many cases, but some doc sets/nodes use "items".
        [JsonPropertyName("children")] public List<TocItem> Children { get; set; } = [];

        [JsonPropertyName("items")] public List<TocItem> NestedItems { get; set; } = [];


        [JsonIgnore] public List<TocItem> Items => Children.Count > 0 ? Children : NestedItems.Count > 0 ? NestedItems : [];
    }
}