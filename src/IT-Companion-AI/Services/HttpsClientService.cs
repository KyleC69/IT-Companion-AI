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
    {
        HttpFetcher = CreateHttpClient();
    }








    /// <summary>
    ///     Gets or sets the HttpClient instance used for making requests.
    ///     This property is initialized with predefined settings in the constructor.
    /// </summary>
    public HttpClient HttpFetcher { get; }








    public static HttpClient CreateHttpClient()
    {
        CookieContainer cookies = new();

        HttpClientHandler handler = new()
        {
            CookieContainer = cookies,
            AllowAutoRedirect = true,
            AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
        };

        HttpClient http = new(handler)
        {
            Timeout = TimeSpan.FromSeconds(60)
        };

        http.DefaultRequestHeaders.UserAgent.Clear();
        http.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("ITCompanionAI", "1.0"));
        http.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("Mozilla", "5.0"));

        http.DefaultRequestHeaders.Accept.Clear();
        http.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/html"));
        http.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xhtml+xml"));
        http.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml", 0.9));
        http.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*", 0.8));

        http.DefaultRequestHeaders.AcceptLanguage.Clear();
        http.DefaultRequestHeaders.AcceptLanguage.ParseAdd("en-US,en;q=0.9");

        http.DefaultRequestHeaders.Referrer = new Uri("https://learn.microsoft.com/");

        // If the site is picky about fetch metadata headers:
        http.DefaultRequestHeaders.TryAddWithoutValidation("Sec-Fetch-Site", "same-origin");
        http.DefaultRequestHeaders.TryAddWithoutValidation("Sec-Fetch-Mode", "navigate");
        http.DefaultRequestHeaders.TryAddWithoutValidation("Sec-Fetch-Dest", "document");
        http.DefaultRequestHeaders.TryAddWithoutValidation("Upgrade-Insecure-Requests", "1");

        return http;
    }
}