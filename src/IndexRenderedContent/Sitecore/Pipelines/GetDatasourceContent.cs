using System.Linq;
using HtmlAgilityPack;
using IndexRenderedContent.Sitecore.PipelineArgs;
using Sitecore.ContentSearch;
using Sitecore.Data.Items;
using Sitecore.Pipelines;

namespace IndexRenderedContent.Sitecore.Pipelines
{
    public class GetDatasourceContent
    {
        public void Process(GetDatasourceContentArgs args)
        {
            args.DatasourceItem.Fields.ReadAll();

            foreach (var field in args.DatasourceItem.Fields.Where(field => !field.Name.StartsWith("__")))
            {
                if (!IndexOperationsHelper.IsTextField(new SitecoreItemDataField(field)))
                    continue;

                var fieldValue = StripHtmlTags(field.Value ?? string.Empty);

                if (!string.IsNullOrWhiteSpace(fieldValue))
                    args.Content.Add(fieldValue);
            }

            foreach (Item child in args.DatasourceItem.Children)
            {
                var getDatasourceContentArgs = new GetDatasourceContentArgs(child, args.Content, args.IndexedItem);

                CorePipeline.Run("indexing.getDatasourceContent", getDatasourceContentArgs);
            }
        }

        private static string StripHtmlTags(string source)
        {
            if (source == null)
                return null;

            var doc = new HtmlDocument();
            doc.LoadHtml(source);
            return doc.DocumentNode.InnerText;
        }
    }
}