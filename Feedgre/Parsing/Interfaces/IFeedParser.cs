using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Feedgre.Models
{
    interface IFeedParser
    {
        IList<FeedItem> ParseFeed(string url);
    }
}
