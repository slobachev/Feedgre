using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Feedgre.Models
{
    public class Feed
    {
        public string Title { get; set; }
        public Uri Link { get; set; }
        public DateTime Updated { get; set; }
        public IEnumerable<FeedItem> FeedItems { get; set; }
    }
}
