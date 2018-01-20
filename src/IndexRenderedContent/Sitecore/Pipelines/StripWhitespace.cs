using System.Text.RegularExpressions;
using IndexRenderedContent.Sitecore.PipelineArgs;

namespace IndexRenderedContent.Sitecore.Pipelines
{
    public class StripWhitespace
    {
        public void Process(RenderedContentStringArgs args)
        {
            if (string.IsNullOrWhiteSpace(args.RenderedContent))
                return;

            args.RenderedContent = args.RenderedContent.Replace(System.Environment.NewLine, "");

            args.RenderedContent = Regex.Replace(args.RenderedContent, @"\s+", " ");
        }
    }
}