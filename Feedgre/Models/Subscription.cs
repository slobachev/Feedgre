using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Feedgre.Models
{
    public class Subscription
    {
        public int Id { get; set; }
        public int FeedId { get; set; }
        public int CollectionId { get; set; }
    }
}
