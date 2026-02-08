using PuppeteerSharp;




namespace ITCompanionAI.Helpers;





internal class SourceExtractor
{
    private static readonly SemaphoreSlim BrowserInitLock = new(1, 1);
    private static bool _browserReady;








    public static async Task<string> GetRenderedHtmlAsync(
            string url,
            CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(url))
        {
            throw new ArgumentException("URL cannot be null or whitespace.", nameof(url));
        }

        await EnsureBrowserReadyAsync(cancellationToken).ConfigureAwait(false);

        LaunchOptions options = new()
        {
                Headless = false,
                Args =
                [
                        "--no-sandbox",
                        "--disable-dev-shm-usage",
                        "--disable-gpu"
                ],
                ExecutablePath = "C:\\Users\\TommyCat\\AppData\\Local\\Google\\Chrome\\Application\\chrome.exe"
        };

        cancellationToken.ThrowIfCancellationRequested();

        await using IBrowser browser = await Puppeteer.LaunchAsync(options).ConfigureAwait(false);

        await using IPage page = await browser.NewPageAsync().ConfigureAwait(false);

        await page.SetUserAgentAsync("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36")
                .ConfigureAwait(false);

        await page.SetViewportAsync(new ViewPortOptions { Width = 1365, Height = 900 }).ConfigureAwait(false);

        IResponse response = await page.GoToAsync(
                        url,
                        new NavigationOptions
                        {
                                WaitUntil = [WaitUntilNavigation.Networkidle2],
                                Timeout = (int)TimeSpan.FromSeconds(60).TotalMilliseconds
                        })
                .ConfigureAwait(false);

        if (response is null)
        {
            return string.Empty;
        }

        var statusCode = (int)response.Status;
        if (statusCode < 200 || statusCode >= 300)
        {
            return string.Empty;
        }

        return await page.GetContentAsync().ConfigureAwait(false);
    }








    private static async Task EnsureBrowserReadyAsync(CancellationToken cancellationToken)
    {
        if (_browserReady)
        {
            return;
        }

        await BrowserInitLock.WaitAsync(cancellationToken).ConfigureAwait(false);
        try
        {
            if (_browserReady)
            {
                return;
            }

            _browserReady = true;
        }
        finally
        {
            _ = BrowserInitLock.Release();
        }
    }
}