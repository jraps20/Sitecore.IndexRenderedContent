using System.Collections.Generic;
using Sitecore.Data.Items;

namespace IndexRenderedContent.Sitecore.PipelineArgs
{
    public class GetDatasourceContentArgs : global::Sitecore.Pipelines.PipelineArgs
    {
        public List<string> Content { get; set; }
        public Item DatasourceItem { get; set; }
        public Item IndexedItem { get; set; }

        public GetDatasourceContentArgs(Item datasourceItem, List<string> content, Item indexedItem)
        {
            DatasourceItem = datasourceItem;
            Content = content;
            IndexedItem = indexedItem;
        }
    }
}