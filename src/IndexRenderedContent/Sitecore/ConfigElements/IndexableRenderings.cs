using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Sitecore.Configuration;
using Sitecore.Data;
using Sitecore.Xml;

namespace IndexRenderedContent.Sitecore.ConfigElements
{
    public class IndexableRenderings
    {
        public string Name { get; set; }
        public ID Id { get; set; }

        public static List<IndexableRenderings> Renderings()
        {
            return (from XmlNode node in Factory.GetConfigNodes("indexableRenderings/rendering")
                select new IndexableRenderings
                {
                    Name = XmlUtil.GetAttribute("name", node),
                    Id = new ID(XmlUtil.GetAttribute("id", node))
                }).ToList();
        }
    }
}