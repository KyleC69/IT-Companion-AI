using ITCompanionAI.Ingestion;





internal class Verify
{
    public static void NotNull(DocsPage page, string pageName)
    {
        ArgumentNullException.ThrowIfNull(page, pageName);

        if (string.IsNullOrWhiteSpace(page.Url))
            throw new ArgumentException("Page URL cannot be null or empty.", $"{pageName}.{nameof(page.Url)}");


        // Populated by database
        /*  if (string.IsNullOrWhiteSpace(page.Uid))
              throw new ArgumentException("Page UID cannot be null or empty.", $"{pageName}.{nameof(page.Uid)}");*/

        if (string.IsNullOrWhiteSpace(page.Title))
            throw new ArgumentException("Page Title cannot be null or empty.", $"{pageName}.{nameof(page.Title)}");

        if (page.Breadcrumb is null || page.Breadcrumb.Length == 0 || page.Breadcrumb.Any(string.IsNullOrWhiteSpace))
            throw new ArgumentException("Page Breadcrumb cannot be null, empty, or contain blank items.", $"{pageName}.{nameof(page.Breadcrumb)}");

        if (string.IsNullOrWhiteSpace(page.Html))
            throw new ArgumentException("Page HTML cannot be null or empty.", $"{pageName}.{nameof(page.Html)}");

        if (string.IsNullOrWhiteSpace(page.Markdown))
            throw new ArgumentException("Page Markdown cannot be null or empty.", $"{pageName}.{nameof(page.Markdown)}");

        if (string.IsNullOrWhiteSpace(page.NormalizedMarkdown))
            throw new ArgumentException("Page NormalizedMarkdown cannot be null or empty.", $"{pageName}.{nameof(page.NormalizedMarkdown)}");

        if (string.IsNullOrWhiteSpace(page.Hash))
            throw new ArgumentException("Page Hash cannot be null or empty.", $"{pageName}.{nameof(page.Hash)}");

        if (page.LastFetched == default)
            throw new ArgumentException("Page LastFetched must be set.", $"{pageName}.{nameof(page.LastFetched)}");
    }
}