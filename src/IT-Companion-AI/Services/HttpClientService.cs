using System.Collections.Concurrent;
using System.Diagnostics;
using System.Net;
using System.Net.Http.Headers;

using Polly;
using Polly.Retry;




namespace ITCompanionAI.Services;





//Encapsulates an HttpClient with predefined settings for HTTPS requests.
public class HttpClientService : HttpClient
{
    private const int MaxRetries = 3;
    private static readonly TimeSpan DefaultBackoff = TimeSpan.FromSeconds(2);

    private static readonly ResiliencePipeline<HttpResponseMessage> RetryPipeline = CreateRetryPipeline();








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
    }








    public async Task<string> GetWebDocumentAsync(string url, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(url))
        {
            throw new ArgumentException("URL cannot be null or empty.", nameof(url));
        }

        Uri uri = ValidateHttpUrl(url);
        try
        {
            HttpResponseMessage response = await this.GetAsync(uri, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                return string.Empty;
            }



            return await response.Content.ReadAsStringAsync(cancellationToken);
        }
        catch (HttpRequestException ex)
        {
            throw new InvalidOperationException($"Failed to fetch the document from {url}.", ex);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("An unexpected error occurred while fetching the web document.", ex);
        }
    }








    public async Task<string> GetJsonAsync(string url, CancellationToken cancellationToken = default)
    {
        Uri uri = ValidateHttpUrl(url);

        using HttpRequestMessage request = new(HttpMethod.Get, uri);
        request.Headers.Accept.Clear();
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        HttpResponseMessage response = await this.SendAsync(request, cancellationToken).ConfigureAwait(false);

        if (!response.IsSuccessStatusCode)
        {
            var preview = await SafeReadBodyPreviewAsync(response, cancellationToken).ConfigureAwait(false);
            throw new HttpRequestException($"Request to {url} failed with status code {(int)response.StatusCode} ({response.ReasonPhrase}). {preview}");
        }

        return await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
    }








    private static Uri ValidateHttpUrl(string url)
    {
        if (string.IsNullOrWhiteSpace(url))
        {
            throw new ArgumentException("URL cannot be null or whitespace.", nameof(url));
        }

        if (!Uri.TryCreate(url, UriKind.Absolute, out Uri uri) || uri.Scheme is not ("http" or "https"))
        {
            throw new ArgumentException("URL must be an absolute http/https URL.", nameof(url));
        }

        return uri;
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








    private static bool IsTransientStatus(HttpStatusCode statusCode)
    {
        return statusCode is HttpStatusCode.RequestTimeout
                or HttpStatusCode.TooManyRequests
                or HttpStatusCode.InternalServerError
                or HttpStatusCode.BadGateway
                or HttpStatusCode.ServiceUnavailable
                or HttpStatusCode.GatewayTimeout;
    }








    private static TimeSpan GetRetryDelay(HttpResponseMessage response, int attempt)
    {
        if (response.Headers.RetryAfter?.Delta is { } delta)
        {
            return delta;
        }

        // Basic exponential backoff with jitter.
        TimeSpan baseDelay = TimeSpan.FromMilliseconds(DefaultBackoff.TotalMilliseconds * Math.Pow(2, attempt));
        var jitterMs = Random.Shared.Next(0, 250);
        return baseDelay + TimeSpan.FromMilliseconds(jitterMs);
    }








    private static ResiliencePipeline<HttpResponseMessage> CreateRetryPipeline()
    {
        var builder = new ResiliencePipelineBuilder<HttpResponseMessage>();

        var retry = new RetryStrategyOptions<HttpResponseMessage>
        {
                MaxRetryAttempts = MaxRetries,
                BackoffType = DelayBackoffType.Exponential,
                UseJitter = true,
                ShouldHandle = static args =>
                {
                    if (args.Outcome.Exception is HttpRequestException)
                    {
                        return ValueTask.FromResult(true);
                    }

                    if (args.Outcome.Result is { } response && IsTransientStatus(response.StatusCode))
                    {
                        return ValueTask.FromResult(true);
                    }

                    return ValueTask.FromResult(false);
                },
                DelayGenerator = static args =>
                {
                    if (args.Outcome.Result is { } response && response.Headers.RetryAfter?.Delta is { } delta)
                    {
                        return ValueTask.FromResult<TimeSpan?>(delta);
                    }

                    var baseDelayMs = DefaultBackoff.TotalMilliseconds * Math.Pow(2, args.AttemptNumber);
                    var jitterMs = Random.Shared.Next(0, 250);
                    return ValueTask.FromResult<TimeSpan?>(TimeSpan.FromMilliseconds(baseDelayMs + jitterMs));
                }
        };

        builder.AddRetry(retry);
        return builder.Build();
    }








    private static async Task<string> SafeReadBodyPreviewAsync(HttpResponseMessage response, CancellationToken cancellationToken)
    {
        if (response?.Content == null)
        {
            return string.Empty;
        }

        try
        {
            var content = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
            if (string.IsNullOrWhiteSpace(content))
            {
                return string.Empty;
            }

            const int MaxPreviewLength = 512;
            return content.Length <= MaxPreviewLength
                    ? $"Body: '{content}'"
                    : $"Body: '{content[..MaxPreviewLength]}'";
        }
        catch
        {
            return string.Empty;
        }
    }
}





internal static class HttpRequestMessageExtensions
{
    public static HttpRequestMessage Clone(this HttpRequestMessage request)
    {
        HttpRequestMessage clone = new(request.Method, request.RequestUri)
        {
                Version = request.Version,
                VersionPolicy = request.VersionPolicy
        };

        foreach (var header in request.Headers) _ = clone.Headers.TryAddWithoutValidation(header.Key, header.Value);

        return clone;
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
    private static readonly TimeSpan GateTtl = TimeSpan.FromMinutes(30);
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
            gate.LastUsedTick = Stopwatch.GetTimestamp();
        }
        finally
        {
            _ = gate.Semaphore.Release();
        }

        PruneStaleGates();

        if (delay is { } d && d > TimeSpan.Zero)
        {
            await Task.Delay(d, cancellationToken);
        }

        return await base.SendAsync(request, cancellationToken);
    }








    private void PruneStaleGates()
    {
        if (_gates.Count < 64)
        {
            return;
        }

        var now = Stopwatch.GetTimestamp();
        var ttlTicks = (long)(GateTtl.TotalSeconds * Stopwatch.Frequency);

        foreach (var kvp in _gates)
        {
            HostGate gate = kvp.Value;
            if (now - gate.LastUsedTick > ttlTicks)
            {
                _ = _gates.TryRemove(kvp.Key, out _);
            }
        }
    }








    private sealed class HostGate
    {
        public SemaphoreSlim Semaphore { get; } = new(1, 1);
        public long NextAllowedTick { get; set; } = Stopwatch.GetTimestamp();
        public long LastUsedTick { get; set; } = Stopwatch.GetTimestamp();
    }
}