using Feedgre.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Feedgre.Services.Parsing
{
    public class RssParser : IFeedParser
    {
        /// <summary>
        /// Parses an RSS feed and returns a <see cref="IList&amp;lt;Item&amp;gt;"/>.
        /// </summary>
        public virtual IList<FeedItem> ParseFeed(string url)
        {
            try
            {
                XDocument doc = XDocument.Load(url);
                // RSS/Channel/item
                var entries = from item in doc.Root.Descendants().First(i => i.Name.LocalName == "channel").Elements().Where(i => i.Name.LocalName == "item")
                              select new FeedItem
                              {
                                  Body = item.Elements().FirstOrDefault(i => i.Name.LocalName == "description").Value,
                                  Link = item.Elements().FirstOrDefault(i => i.Name.LocalName == "link").Value,
                                  PublishDate = Utilities.ParseDate(item.Elements().FirstOrDefault(i => i.Name.LocalName == "pubDate").Value),
                                  Title = item.Elements().FirstOrDefault(i => i.Name.LocalName == "title").Value,
                                  Author = item.Elements().FirstOrDefault(i => i.Name.LocalName == "creator").Value,
                                  Image = Regex.Match(item.Elements().FirstOrDefault(i => i.Name.LocalName == "description").Value, "<img.+?src=[\"'](.+?)[\"'].+?>", RegexOptions.IgnoreCase).Groups[1].Value
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
