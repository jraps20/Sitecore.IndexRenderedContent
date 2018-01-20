using System.Linq;
using IndexRenderedContent.Sitecore.ConfigElements;
using IndexRenderedContent.Sitecore.PipelineArgs;

namespace IndexRenderedContent.Sitecore.Pipelines
{
    public class GetIndexableRenderings
    {
        public void Process(IndexableRenderingsArgs args)
        {
            args.ValidRenderingIds = IndexableRenderings.Renderings().Select(i => i.Id);
        }
    }
}