using System;
using System.Linq;
using Sitecore;
using Sitecore.Configuration;
using Sitecore.ContentSearch;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Events;

namespace IndexRenderedContent.Sitecore.Events
{
    public class VersionRemoved
    {
        public void OnVersionRemoved(object sender, EventArgs args)
        {
            if (args == null)
                return;
            if (!(Event.ExtractParameter(args, 0) is Item item))
                return;

            bool Func(ItemUri uri) => (uri != null) && uri != item.Uri;

            var linkedItems = (
                    from l
                        in Globals.LinkDatabase.GetReferrers(item, FieldIDs.LayoutField)
                    select l.GetSourceItem().Uri)
                .Where(Func)
                .ToList();

            linkedItems.AddRange(
                (
                    from l
                        in Globals.LinkDatabase.GetReferrers(item, FieldIDs.FinalLayoutField)
                    select l.GetSourceItem().Uri)
                .Where(Func)
                .ToList());

            linkedItems = linkedItems.DistinctBy(i => i.ItemID).ToList();

            var index = ContentSearchManager.GetIndex(Settings.GetSetting("Search.Index"));

            foreach (var linkedItem in linkedItems)
            {
                index.Update(new SitecoreItemUniqueId(linkedItem));
            }
        }
    }
}