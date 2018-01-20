using System.Collections.Generic;
using System.Linq;
using IndexRenderedContent.Sitecore.PipelineArgs;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.ComputedFields;
using Sitecore.Data.Items;
using Sitecore.Pipelines;

namespace IndexRenderedContent.Sitecore.ComputedFields
{
    public class RenderedContent : AbstractComputedIndexField
    {
        public override object ComputeFieldValue(IIndexable indexable)
        {
            var item = (Item)(indexable as SitecoreIndexableItem);

            if (item == null)
                return null;
            
            var indexableRenderingArgs = new IndexableRenderingsArgs(item);
            
            CorePipeline.Run("indexing.getIndexableRenderings", indexableRenderingArgs);
            
            if (!indexableRenderingArgs.ValidRenderingIds.Any())
                return null;

            var deviceItem = item.Database.Resources.Devices.GetAll().FirstOrDefault(d => d.IsDefault);

            if (deviceItem == null)
                return null;

            var extractRenderingsDatasourceArgs = new ExtractRenderingsDatasourceArgs(item, indexableRenderingArgs.ValidRenderingIds, deviceItem);

            CorePipeline.Run("indexing.extractRenderingsDatasources", extractRenderingsDatasourceArgs);
            
            if (!extractRenderingsDatasourceArgs.DatasourceItems.Any())
                return null;
            
            extractRenderingsDatasourceArgs.DatasourceItems = extractRenderingsDatasourceArgs.DatasourceItems.DistinctBy(i => i.ID).ToList();

            var content = new List<string>();

            foreach (var itemDatasource in extractRenderingsDatasourceArgs.DatasourceItems)
            {
                var getDatasourceContentArgs = new GetDatasourceContentArgs(itemDatasource, content, item);

                CorePipeline.Run("indexing.getDatasourceContent", getDatasourceContentArgs);
            }

            if (!content.Any())
                return null;

            var aggregateContent = content.Aggregate((a, b) => $"{a} {b}");

            var renderedContentStringArgs = new RenderedContentStringArgs(item, aggregateContent);

            CorePipeline.Run("indexing.renderedContent.Saving", renderedContentStringArgs);
            
            return renderedContentStringArgs.RenderedContent;
        }
    }
}