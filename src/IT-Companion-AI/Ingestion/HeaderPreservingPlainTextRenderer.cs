using Markdig;


using System;
using System.Collections.Generic;
using System.Text;

using Markdig.Helpers;
using Markdig.Renderers;
using Markdig.Renderers.Normalize.Inlines;
using Markdig.Renderers.Normalize;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;




namespace ITCompanionAI.Ingestion;

internal class HeaderPreservingPlainTextRenderer: TextRendererBase
{
    public HeaderPreservingPlainTextRenderer(TextWriter writer)
            : base(writer)
    {
        // Replace the default heading renderer
        ObjectRenderers.Replace<HeadingRenderer>(new MyHeadingRenderer());
    }

    private class MyHeadingRenderer : MarkdownObjectRenderer<TextRendererBase, HeadingBlock>
    {
        protected override void Write(TextRendererBase renderer, HeadingBlock obj)
        {
            // Write the heading markers (#, ##, ###, etc.)
            renderer.Write(new LiteralInline { Content = new StringSlice(new string('#', obj.Level)) });
            renderer.Write(new LiteralInline { Content = new StringSlice(" ") });

            if (obj.Inline != null)
            {
                renderer.Write(obj.Inline);
            }

            renderer.Write(new LineBreakInline());
        }
    }

}
