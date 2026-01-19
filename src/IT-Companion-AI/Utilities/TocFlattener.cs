using ITCompanionAI.Ingestion;
using ITCompanionAI.Ingestion.Docs;





public static class TocFlattener
{
    public static IEnumerable<FlatTocEntry> Flatten(
        List<TocFetcher.TocItem> items,
        List<string> parents = null,
        int depth = 0)
    {
        parents ??= new List<string>();

        foreach (TocFetcher.TocItem item in items)
        {
            var title = item.Name ?? string.Empty;
            var breadcrumb = parents.Append(title).ToArray();

            yield return new FlatTocEntry
            {
                Title = title,
                Url = item.Href,
                Uid = Guid.NewGuid().ToString(),
                Depth = depth,
                Breadcrumb = breadcrumb
            };

            if (item.Children.Count > 0)
            {
                foreach (FlatTocEntry child in Flatten(item.Children, breadcrumb.ToList(), depth + 1))
                    yield return child;
            }

            if (item.NestedItems.Count > 0)
            {
                foreach (FlatTocEntry child in Flatten(item.NestedItems, breadcrumb.ToList(), depth + 1))
                    yield return child;
            }
        }
    }
}