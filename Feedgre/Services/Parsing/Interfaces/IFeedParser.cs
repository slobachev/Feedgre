using Feedgre.Models;
using System.Collections.Generic;

namespace Feedgre.Services.Parsing
{
    /// <summary>
    /// Parser interface
    /// </summary>
    public interface IFeedParser
    {
        IList<FeedItem> ParseFeed(string url);
    }
}
