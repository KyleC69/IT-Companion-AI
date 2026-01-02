using Markdig;


using System;
using System.Collections.Generic;
using System.Text;

using Markdig.Syntax;

using Microsoft.CodeAnalysis;

namespace ITCompanionAI.AgentFramework.Ingestion;

internal class Helpers
{
}
public static class RoslynTypeCollector
{
    public static IEnumerable<INamedTypeSymbol> CollectTypes(Compilation compilation)
    {
        var stack = new Stack<INamespaceSymbol>();
        stack.Push(compilation.GlobalNamespace);

        while (stack.Count > 0)
        {
            var ns = stack.Pop();

            foreach (var nestedNs in ns.GetNamespaceMembers())
                stack.Push(nestedNs);

            foreach (var type in ns.GetTypeMembers())
                foreach (var t in CollectNestedTypes(type))
                    yield return t;
        }
    }

    private static IEnumerable<INamedTypeSymbol> CollectNestedTypes(INamedTypeSymbol type)
    {
        yield return type;

        foreach (var nested in type.GetTypeMembers())
            foreach (var t in CollectNestedTypes(nested))
                yield return t;
    }
}
public static  class XmlDocExtractor
{
    
    // Currently, this class only extracts summaries and remarks from XML documentation.
    // Additional methods can be added here to extract other XML documentation tags or process them further.
    public static string? GetSummary(ISymbol symbol)
        => symbol.GetDocumentationCommentXml()
                 ?.ExtractTag("summary")
                 ?.Trim();

    public static string? GetRemarks(ISymbol symbol)
        => symbol.GetDocumentationCommentXml()
                 ?.ExtractTag("remarks")
                 ?.Trim();

    private static string? ExtractTag(this string xml, string tag)
    {
        if (string.IsNullOrWhiteSpace(xml)) return null;
        var start = xml.IndexOf($"<{tag}>", StringComparison.OrdinalIgnoreCase);
        var end = xml.IndexOf($"</{tag}>", StringComparison.OrdinalIgnoreCase);
        if (start < 0 || end < 0) return null;
        return xml.Substring(start + tag.Length + 2, end - start - tag.Length - 2);
    }
}
public static class AttributeExtractor
{
    public static string? From(ISymbol symbol)
    {
        return ""; // symbol.GetAttributes()

    }
}
public static class ParameterExtractor
{
    public static List<ApiParameter> From(IMethodSymbol method)
    {
        return method.Parameters.Select(p => new ApiParameter
        {
            Name = p.Name,
            Type = p.Type.ToDisplayString(),
            Position = p.Ordinal,
            HasDefaultValue = p.HasExplicitDefaultValue,
            DefaultValueLiteral = p.HasExplicitDefaultValue ? p.ExplicitDefaultValue?.ToString() : null
        }).ToList();
    }
}


public static class SourceLocator
{
    public static ApiSourceLocation? From(ISymbol symbol)
    {
        var loc = symbol.Locations.FirstOrDefault(l => l.IsInSource);
        if (loc == null) return null;

        var span = loc.GetLineSpan();
        return new ApiSourceLocation
        {
            FilePath = span.Path,
            StartLine = span.StartLinePosition.Line + 1,
            EndLine = span.EndLinePosition.Line + 1
        };
    }
}



public class ApiHelpers
{
    public DocPage Parse(string filePath, string markdown)
    {
        var doc = Markdown.Parse(markdown);

        return new DocPage
        {
            Id = default,
            SourceSnapshotId = default,
            DocUid = DocUidGenerator.FromPath(filePath),
            SourcePath = filePath,
            Title = ExtractTitle(doc),
            Language = null,
            Url = null,
            RawMarkdown = markdown,
            DocSections = null,
            SourceSnapshot = null,
        };
    }





    private string? ExtractTitle(MarkdownDocument Doc)
    {
        return null;
    }


    public class MarkdownDocParser : ApiDocParser
    {
    }


    public List<CodeBlock> ExtractCodeBlocks(string markdown)
    {
        var doc = Markdown.Parse(markdown);
        var blocks = new List<CodeBlock>();

        foreach (var cb in doc.Descendants<FencedCodeBlock>())
        {
            blocks.Add(new CodeBlock
            {
                CodeUid = CodeUidGenerator.FromBlock(cb),
                Language = cb.Info,
                Content = cb.Lines.ToString(),
                DeclaredPackages = PackageDetector.From(cb),
                Tags = TagExtractor.From(cb)
            });
        }

        return blocks;
    }





    private List<DocSection> ExtractSections(MarkdownDocument doc)
    {
        var sections = new List<DocSection>();
        var blocks = doc.ToList();

        for (int i = 0; i < blocks.Count; i++)
        {
            if (blocks[i] is HeadingBlock heading)
            {
                var content = ExtractSectionContent(blocks, i);

                sections.Add(new DocSection
                {
                    SectionUid = SectionUidGenerator.FromHeading(heading),
                    Heading = heading.Inline?.FirstChild?.ToString() ?? "",
                    Level = heading.Level,
                    ContentMarkdown = content,
                    CodeBlocks = ExtractCodeBlocks(content)
                });
            }
        }

        return sections;
    }





    private string? ExtractSectionContent(List<Block> Blocks, int I)
    {
        return null;
    }




public class TagExtractor
{
    public static string? From(FencedCodeBlock Cb)
    {
        return null;
    }
}


}



public class DocUidGenerator
{
    public static string FromPath(string FilePath)
    {
        return null;
    }
}



public class CodeUidGenerator
{
    public static string? FromBlock(FencedCodeBlock Cb)
    {
        return null;
    }
}



public class PackageDetector
{
    public static string? From(FencedCodeBlock Cb)
    {
        return null;
    }
}



internal class SectionUidGenerator
{
    public static string FromHeading(HeadingBlock Heading)
    {
        return null;
    }
}