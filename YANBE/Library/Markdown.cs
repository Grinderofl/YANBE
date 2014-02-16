using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Pygments;

namespace YANBE.Library
{
    public class Markdown
    {
        private readonly Highlighter _highlighter;

        public Markdown(Highlighter highlighter)
        {
            _highlighter = highlighter;
        }

        public string Transform(string source, bool external = true)
        {
            if (external)
                source = HttpUtility.UrlDecode(source);
            var md = new MarkdownSharp.Markdown();
            var transformed = md.Transform(source);

            var start = "<p>```c#";
            var end = "```</p>";

            while (transformed.IndexOf(start, StringComparison.Ordinal) > -1)
            {
                var startIndex = transformed.IndexOf(start, StringComparison.Ordinal);
                var endIndex = transformed.IndexOf(end, startIndex, StringComparison.Ordinal);
                var firstHalf = transformed.Substring(0, startIndex);
                var secondHalf = transformed.Substring(endIndex + end.Length);
                var text = transformed.Substring(startIndex + start.Length, endIndex - startIndex - start.Length).Replace("&gt;", ">").Replace("&lt;", "<");
                transformed = firstHalf + _highlighter.HighlightToHtml(HtmlRemoval.StripTagsRegexCompiled(text), "c#", "vs", lineNumberStyle: LineNumberStyle.table, fragment: false) + secondHalf;
            }
            /*if (external)
                return HttpUtility.UrlEncode(transformed);*/
            return transformed;
        }
    }
}