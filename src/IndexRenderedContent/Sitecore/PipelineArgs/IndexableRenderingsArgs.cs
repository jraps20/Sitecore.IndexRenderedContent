using System.Collections.Generic;
using Sitecore.Data;
using Sitecore.Data.Items;

namespace IndexRenderedContent.Sitecore.PipelineArgs
{
    public class IndexableRenderingsArgs : global::Sitecore.Pipelines.PipelineArgs
    {
        public Item IndexedItem { get; set; }
        public IEnumerable<ID> ValidRenderingIds { get; set; }

        public IndexableRenderingsArgs(Item indexedItem)
        {
            IndexedItem = indexedItem;
        }
    }
}