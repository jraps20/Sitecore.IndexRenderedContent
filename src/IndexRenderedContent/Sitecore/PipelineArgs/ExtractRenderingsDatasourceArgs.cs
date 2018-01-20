using System.Collections.Generic;
using Sitecore.Data;
using Sitecore.Data.Items;

namespace IndexRenderedContent.Sitecore.PipelineArgs
{
    public class ExtractRenderingsDatasourceArgs : global::Sitecore.Pipelines.PipelineArgs
    {
        public IEnumerable<ID> IndexableRenderingIds { get; set; }
        public List<Item> DatasourceItems { get; set; }

        public DeviceItem DeviceItem { get; set; }
        public Item IndexedItem { get; set; }

        public ExtractRenderingsDatasourceArgs(Item indexedItem, IEnumerable<ID> indexableRenderingIds, DeviceItem deviceItem)
        {
            IndexedItem = indexedItem;
            IndexableRenderingIds = indexableRenderingIds;
            DeviceItem = deviceItem;
            DatasourceItems = new List<Item>();
        }
    }
}