using ITCompanionAI.Ingestion.Docs;





public static class TocFlattener
{
    public static IEnumerable<FlatTocEntry> Flatten(
            List<TocFetcher.TocItem> items,
            List<string> parents = null,
            int depth = 0)
    {
        parents ??= [];

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
                foreach (FlatTocEntry child in Flatten(item.Children, breadcrumb.ToList(), depth + 1)) yield return child;
            }

            if (item.NestedItems.Count > 0)
            {
                foreach (FlatTocEntry child in Flatten(item.NestedItems, breadcrumb.ToList(), depth + 1)) yield return child;
            }
        }
    }








    public static IEnumerable<(TocFetcher.TocNode Node, string? ParentHref, int Depth, int Order)> FlattenToc(List<TocFetcher.TocNode> nodes, string? parentHref = null, int depth = 0)
    {
        var order = 0;

        foreach (TocFetcher.TocNode node in nodes)
        {
            yield return (node, parentHref, depth, order++);

            if (node.items != null)
            {
                foreach ((TocFetcher.TocNode Node, string ParentHref, int Depth, int Order) child in FlattenToc(node.items, node.href, depth + 1))
                    yield return child;
            }
        }
    }
}