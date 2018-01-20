using Sitecore.Data.Items;

namespace IndexRenderedContent.Sitecore.PipelineArgs
{
    public class RenderedContentStringArgs : global::Sitecore.Pipelines.PipelineArgs
    {
        public Item IndexedItem { get; set; }
        public string RenderedContent{ get; set; }

        public RenderedContentStringArgs(Item indexedItem, string renderedContent)
        {
            IndexedItem = indexedItem;
            RenderedContent = renderedContent;
        }
    }
}