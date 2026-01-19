using System.Collections.Concurrent;
using System.Diagnostics;
using System.Net;
using System.Net.Http.Headers;



namespace ITCompanionAI.Services;


//Encapsulates an HttpClient with predefined settings for HTTPS requests.
public class HttpClientService : HttpClient
{
    private const int MaxRetries = 3;
    private static readonly TimeSpan DefaultBackoff = TimeSpan.FromSeconds(2);








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

        _ = DefaultRequestHeaders.TryAddWithoutValidation("Sec-Fetch-Site", "same-origin");
        _ = DefaultRequestHeaders.TryAddWithoutValidation("Sec-Fetch-Mode", "navigate");
        _ = DefaultRequestHeaders.TryAddWithoutValidation("Sec-Fetch-Dest", "document");
        _ = DefaultRequestHeaders.TryAddWithoutValidation("Upgrade-Insecure-Requests", "1");
    }








    public async Task<string> GetWebDocumentAsync(string url, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(url))
        {
            throw new ArgumentException("URL cannot be null or whitespace.", nameof(url));
        }

        if (!Uri.TryCreate(url, UriKind.Absolute, out Uri uri) || uri.Scheme is not ("http" or "https"))
        {
            throw new ArgumentException("URL must be an absolute http/https URL.", nameof(url));
        }

        using HttpRequestMessage request = new(HttpMethod.Get, uri);
        request.Headers.Accept.Clear();
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("text/html"));

        return await SendWithRetryAsync(request, cancellationToken);
    }








    public async Task<string> GetJsonAsync(string url, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(url))
        {
            throw new ArgumentException("URL cannot be null or whitespace.", nameof(url));
        }

        if (!Uri.TryCreate(url, UriKind.Absolute, out Uri uri) || uri.Scheme is not ("http" or "https"))
        {
            throw new ArgumentException("URL must be an absolute http/https URL.", nameof(url));
        }

        using HttpRequestMessage request = new(HttpMethod.Get, uri);
        request.Headers.Accept.Clear();
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        return await SendWithRetryAsync(request, cancellationToken);
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








    private async Task<string> SendWithRetryAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        // HttpRequestMessage is single-use; we recreate it each attempt.
        for (var attempt = 0; attempt <= MaxRetries; attempt++)
        {
            cancellationToken.ThrowIfCancellationRequested();

            using HttpRequestMessage attemptRequest = request.Clone();
            HttpResponseMessage? response = null;
            try
            {
                response = await this.SendAsync(attemptRequest, HttpCompletionOption.ResponseHeadersRead, cancellationToken);

                if (!response.IsSuccessStatusCode)
                {
                    if (attempt < MaxRetries && IsTransientStatus(response.StatusCode))
                    {
                        TimeSpan delay = GetRetryDelay(response, attempt);
                        response.Dispose();
                        await Task.Delay(delay, cancellationToken);
                        continue;
                    }

                    var reason = response.ReasonPhrase ?? string.Empty;
                    var bodyPreview = await SafeReadBodyPreviewAsync(response, cancellationToken);
                    throw new HttpRequestException(
                        $"Request to '{attemptRequest.RequestUri}' failed with {(int)response.StatusCode} {reason}. {bodyPreview}",
                        null,
                        response.StatusCode);
                }

                var content = await response.Content.ReadAsStringAsync(cancellationToken);
                response.Dispose();
                return content;
            }
            catch (OperationCanceledException)
            {
                response?.Dispose();
                throw;
            }
            catch (HttpRequestException) when (attempt < MaxRetries)
            {
                response?.Dispose();
                TimeSpan delay = TimeSpan.FromMilliseconds(DefaultBackoff.TotalMilliseconds * Math.Pow(2, attempt));
                await Task.Delay(delay, cancellationToken);
            }
            finally
            {
                response?.Dispose();
            }
        }

        throw new InvalidOperationException("Retry loop exhausted unexpectedly.");
    }








    private static async Task<string> SafeReadBodyPreviewAsync(HttpResponseMessage response, CancellationToken cancellationToken)
    {
        try
        {
            if (response.Content is null)
            {
                return string.Empty;
            }

            var text = await response.Content.ReadAsStringAsync(cancellationToken);
            if (string.IsNullOrWhiteSpace(text))
            {
                return string.Empty;
            }

            const int max = 512;
            text = text.Length <= max ? text : text[..max];
            return $"Body: '{text}'";
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
        HttpRequestMessage clone = new(request.Method, request.RequestUri);

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
            _ = gate.Semaphore.Release();
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