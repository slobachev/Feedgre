using Feedgre.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace Feedgre.Services.Parsing
{
    public class AtomParser : IFeedParser
    {
        /// <summary>
        /// Parses an Atom feed and returns a <see cref="IList&amp;lt;Item&amp;gt;"/>.
        /// </summary>
        public virtual IList<FeedItem> ParseFeed(string url)
        {
            try
            {
                XDocument doc = XDocument.Load(url);
                // Feed/Entry
                var entries = from item in doc.Root.Elements().Where(i => i.Name.LocalName == "entry")
                              select new FeedItem
                              {
                                  Body = item.Elements().FirstOrDefault(i => i.Name.LocalName == "content").Value,
                                  Link = item.Elements().FirstOrDefault(i => i.Name.LocalName == "link").Attribute("href").Value,
                                  PublishDate = Utilities.ParseDate(item.Elements().FirstOrDefault(i => i.Name.LocalName == "published").Value),
                                  Title = item.Elements().FirstOrDefault(i => i.Name.LocalName == "title").Value,
                                  Author = item.Elements().FirstOrDefault(i => i.Name.LocalName == "author").Value,
                                  Image = Regex.Match(item.Elements().FirstOrDefault(i => i.Name.LocalName == "content").Value, "<img.+?src=[\"'](.+?)[\"'].+?>", RegexOptions.IgnoreCase).Groups[1].Value
                              };
                return entries.ToList();
            }
            catch
            {
                return new List<FeedItem>();
            }
        }
    }
}
