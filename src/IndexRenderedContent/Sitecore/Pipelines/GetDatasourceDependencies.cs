using System.Linq;
using Sitecore;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.Pipelines.GetDependencies;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;

namespace IndexRenderedContent.Sitecore.Pipelines
{
    public class GetDatasourceDependencies : BaseProcessor
    {
        public override void Process(GetDependenciesArgs context)
        {
            Assert.IsNotNull(context.IndexedItem, "indexed item");
            Assert.IsNotNull(context.Dependencies, "dependencies");

            var item = (Item)(context.IndexedItem as SitecoreIndexableItem);

            if (item == null)
                return;

            bool Func(ItemUri uri) => (uri != null) && uri != item.Uri;

            var source = (
                    from l
                        in Globals.LinkDatabase.GetReferrers(item, FieldIDs.LayoutField)
                    select l.GetSourceItem().Uri)
                .Where(Func)
                .ToList();

            source.AddRange(
                (
                    from l
                        in Globals.LinkDatabase.GetReferrers(item, FieldIDs.FinalLayoutField)
                    select l.GetSourceItem().Uri)
                .Where(Func)
            );

            source = source.DistinctBy(i => i.ItemID).ToList();

            context.Dependencies.AddRange(source.Select(x => (SitecoreItemUniqueId)x));
        }
    }
}