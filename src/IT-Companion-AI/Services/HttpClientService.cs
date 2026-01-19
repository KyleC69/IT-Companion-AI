using System.Collections.Concurrent;
using System.Diagnostics;
using System.Net;
using System.Net.Http.Headers;



namespace ITCompanionAI.Services;


//Encapsulates an HttpClient with predefined settings for HTTPS requests.
public class HttpClientService : HttpClient
{


    /// <summary>
    ///     Initializes a new instance of the <see cref="HttpClientService" /> class.
    ///     Configures the HttpClient with default settings for HTTPS requests.
    /// </summary>
    public HttpClientService()
        : base(CreateHandlerPipeline())
    {
        Timeout = TimeSpan.FromSeconds(60);

        DefaultRequestHeaders.UserAgent.Clear();
        DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("ITCompanionAI", "1.0"));
        DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("Mozilla", "5.0"));

        DefaultRequestHeaders.Accept.Clear();
        DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/html"));
        DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xhtml+xml"));
        DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml", 0.9));
        DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*", 0.8));

        DefaultRequestHeaders.AcceptLanguage.Clear();
        DefaultRequestHeaders.AcceptLanguage.ParseAdd("en-US,en;q=0.9");

        DefaultRequestHeaders.Referrer = new Uri("https://learn.microsoft.com/");

        DefaultRequestHeaders.TryAddWithoutValidation("Sec-Fetch-Site", "same-origin");
        DefaultRequestHeaders.TryAddWithoutValidation("Sec-Fetch-Mode", "navigate");
        DefaultRequestHeaders.TryAddWithoutValidation("Sec-Fetch-Dest", "document");
        DefaultRequestHeaders.TryAddWithoutValidation("Upgrade-Insecure-Requests", "1");
    }








    public async Task<string> GetWebDocumentAsync(string url, CancellationToken cancellationToken = default)
    {
        using HttpResponseMessage response = await this.GetAsync(url, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadAsStringAsync(cancellationToken);
    }








    private static HttpMessageHandler CreateHandlerPipeline()
    {
        CookieContainer cookies = new();

        HttpClientHandler handler = new()
        {
            CookieContainer = cookies,
            AllowAutoRedirect = true,
            AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
        };

        return new PerHostRateLimitingHandler(handler, TimeSpan.FromSeconds(10));
    }








    public string GetWebDocument(string url)
    {
        HttpResponseMessage response = this.GetAsync(url).GetAwaiter().GetResult();
        response.EnsureSuccessStatusCode();
        return response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
    }
}





/// <summary>
///     Provides per-host rate limiting for outgoing HTTP requests by enforcing a minimum delay between requests to the
///     same
///     host.
/// </summary>
/// <remarks>
///     This handler ensures that requests to each host are spaced at least the specified minimum delay
///     apart, regardless of the number of concurrent requests. Rate limiting is applied independently for each host, and
///     requests to different hosts are not delayed relative to each other. This can help prevent overwhelming remote
///     servers or triggering rate limits imposed by external services.
/// </remarks>
/// <param name="innerHandler">The inner HTTP message handler that processes the requests after rate limiting is applied.</param>
/// <param name="minDelay">The minimum time interval to wait between consecutive requests to the same host.</param>
internal sealed class PerHostRateLimitingHandler(HttpMessageHandler innerHandler, TimeSpan minDelay) : DelegatingHandler(innerHandler)
{
    private readonly ConcurrentDictionary<string, HostGate> _gates = new(StringComparer.OrdinalIgnoreCase);
    private readonly TimeSpan _minDelay = minDelay;








    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var host = request.RequestUri?.Host;
        if (string.IsNullOrWhiteSpace(host))
        {
            return await base.SendAsync(request, cancellationToken);
        }

        HostGate gate = _gates.GetOrAdd(host, static _ => new HostGate());
        TimeSpan? delay;

        await gate.Semaphore.WaitAsync(cancellationToken);
        try
        {
            var now = Stopwatch.GetTimestamp();
            var next = gate.NextAllowedTick;

            delay = now >= next
                ? null
                : TimeSpan.FromSeconds((next - now) / (double)Stopwatch.Frequency);

            gate.NextAllowedTick = Stopwatch.GetTimestamp() + (long)(_minDelay.TotalSeconds * Stopwatch.Frequency);
        }
        finally
        {
            gate.Semaphore.Release();
        }

        if (delay is { } d && d > TimeSpan.Zero)
        {
            await Task.Delay(d, cancellationToken);
        }

        return await base.SendAsync(request, cancellationToken);
    }








    private sealed class HostGate
    {
        public SemaphoreSlim Semaphore { get; } = new(1, 1);
        public long NextAllowedTick { get; set; } = Stopwatch.GetTimestamp();
    }
}