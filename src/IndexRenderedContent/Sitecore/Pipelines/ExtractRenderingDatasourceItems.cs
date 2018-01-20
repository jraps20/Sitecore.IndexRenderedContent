using System.Linq;
using IndexRenderedContent.Sitecore.PipelineArgs;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Globalization;

namespace IndexRenderedContent.Sitecore.Pipelines
{
    public class ExtractRenderingDatasourceItems
    {
        public string FieldId { get; set; }

        public void Process(ExtractRenderingsDatasourceArgs args)
        {
            if (string.IsNullOrWhiteSpace(FieldId) || !ID.IsID(FieldId))
                return;
            
            var fieldId = new ID(FieldId);

            var layoutField = args.IndexedItem.Fields[fieldId];
            
            var layoutXml = LayoutField.GetFieldValue(layoutField);

            if (string.IsNullOrEmpty(layoutXml))
                return;

            var renderings = args.IndexedItem.Visualization.GetRenderings(args.DeviceItem, false);

            if (renderings == null || renderings.Length == 0)
                return;
            
            for (var renderingIndex = renderings.Length - 1; renderingIndex >= 0; renderingIndex--)
            {
                var rendering = renderings[renderingIndex];

                if (rendering == null || rendering.Database != args.IndexedItem.Database)
                    continue;

                if (!args.IndexableRenderingIds.Contains(rendering.RenderingID))
                    continue;

                var datasourceId = rendering.Settings.DataSource;

                if (!ID.IsID(datasourceId))
                    continue;

                var datasourceItem = GetDatasourceWithFallback(args.IndexedItem.Database, new ID(datasourceId), args.IndexedItem.Language);

                if (datasourceItem == null || datasourceItem.Versions.Count == 0)
                    continue;

                args.DatasourceItems.Add(datasourceItem);
            }
        }

        private static Item GetDatasourceWithFallback(Database database, ID datasourceId, Language language)
        {
            while (true)
            {
                var datasourceItem = database.GetItem(datasourceId, language);

                if (datasourceItem != null && datasourceItem.Versions.Count != 0)
                    return datasourceItem;

                var fallbackLang = global::Sitecore.Data.Managers.LanguageFallbackManager.GetFallbackLanguage(language, database);

                if (fallbackLang == null)
                    return null;

                language = fallbackLang;
            }
        }
    }
}