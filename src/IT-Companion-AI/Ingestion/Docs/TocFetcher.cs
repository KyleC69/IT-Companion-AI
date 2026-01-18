using System.Text.Json;
using System.Text.Json.Serialization;

using ITCompanionAI.Services;

using Microsoft.Extensions.Logging;



namespace ITCompanionAI.Ingestion.Docs;


public sealed class TocFetcher
{
    private readonly HttpClient _http;
    private readonly ILogger<TocFetcher> _logger;








    public TocFetcher()
    {
        _http = App.GetService<HttpClientService>();
        _logger = App.GetService<ILogger<TocFetcher>>();
    }








    public async Task<List<TocItem>> FetchAsync(string baseUrl)
    {
        Uri baseUri = new(baseUrl);

        // Try likely TOC locations (adjust to your needs)
        var candidates = new[]
        {
            new Uri(baseUri, "toc.json"),
            new Uri(baseUri, "../toc.json"),
            new Uri(baseUri, "../../toc.json")
        };

        foreach (Uri uri in candidates)
        {
            using HttpResponseMessage resp = await _http.GetAsync(uri);
            if (!resp.IsSuccessStatusCode) continue;

            var mediaType = resp.Content.Headers.ContentType?.MediaType;
            if (mediaType is not null && !mediaType.Contains("json", StringComparison.OrdinalIgnoreCase))
                continue;

            // Microsoft Learn TOC is commonly shaped as { "items": [ ... ] } with fields like "toc_title" and "children".
            // Some doc sets may still return a raw array; support both.
            var json = await resp.Content.ReadAsStringAsync();
            if (string.IsNullOrWhiteSpace(json))
                continue;

            JsonSerializerOptions jsonPropertyName = new()
            {
                PropertyNameCaseInsensitive = true
            };

            // 1) Try Learn's wrapper schema.
            try
            {
                LearnTocResponse2? learn = JsonSerializer.Deserialize<LearnTocResponse2>(json, jsonPropertyName);
                if (learn?.Items is { Count: > 0 })
                    return learn.Items;
            }
            catch (JsonException)
            {
                // fall through
                _logger.LogError("Failed to deserialize LearnTocResponse2 from {Json}", json);
            }

            // 2) Try raw array schema.
            try
            {
                //    var toc = JsonSerializer.Deserialize<List<TocItem>>(json, jsonPropertyName);
                //  if (toc is not null) return toc;
            }
            catch (JsonException)
            {
                // fall through
                _logger.LogError("Failed to deserialize LearnTocResponse2 from {Json}", json);
            }
        }

        throw new InvalidOperationException($"No toc.json found for {baseUrl}");
    }








    private static string? NormalizeHref(string? href)
    {
        if (string.IsNullOrWhiteSpace(href))
            return href;

        // Learn TOC often returns site-relative paths like "overview/xyz".
        if (href.StartsWith("http", StringComparison.OrdinalIgnoreCase))
            return href;

        href = href.TrimStart('/');
        return $"https://learn.microsoft.com/en-us/agent-framework/{href}";
    }








    public static class TocNormalizer
    {
        public static void Normalize(List<TocItem> items, string baseUrl)
        {
            var baseUri = new Uri(baseUrl, UriKind.Absolute);

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
                    item.Href = new Uri(root, item.Href.TrimStart('/')).ToString().TrimEnd('/');

                if (item.Items?.Count > 0)
                    Normalize(item.Items, baseUrl);
            }
        }
    }





    private sealed class LearnTocResponse2
    {
        [JsonPropertyName("items")] public List<TocItem> Items { get; set; } = new();
    }





    public class TocItem
    {
        [JsonPropertyName("toc_title")] public string Name { get; set; }

        [JsonPropertyName("href")] public string Href { get; set; }

        // Learn TOC uses "children" in many cases, but some doc sets/nodes use "items".
        [JsonPropertyName("children")] public List<TocItem> Children { get; set; } = new();

        [JsonPropertyName("items")] public List<TocItem> NestedItems { get; set; } = new();





        [JsonIgnore]
        public List<TocItem> Items
        {
            get
            {
                if (Children.Count > 0) return Children;
                if (NestedItems.Count > 0) return NestedItems;
                return new List<TocItem>();
            }
        }
    }
}