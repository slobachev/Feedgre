using Feedgre.Models;
using System.Collections.Generic;

namespace Feedgre.Services.Parsing
{
    public interface IFeedParser
    {
        IList<FeedItem> ParseFeed(string url);
    }
}
