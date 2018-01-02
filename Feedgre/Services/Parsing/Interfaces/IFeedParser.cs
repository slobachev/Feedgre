using Feedgre.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Feedgre.Services.Parsing
{
    public interface IFeedParser
    {
        IList<FeedItem> ParseFeed(string url);
    }
}
