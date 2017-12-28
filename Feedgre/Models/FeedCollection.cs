using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Feedgre.Models
{
    public class FeedCollection
    {
        public string Title { get; set; }
        public Uri Icon { get; set; }
        public IEnumerable<Feed> Feeds { get; set; }
    }
}
