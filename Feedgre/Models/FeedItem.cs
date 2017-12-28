using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Feedgre.Models
{
    /// <summary>
    /// Represents a feed item.
    /// </summary>
    public class FeedItem
    {
        public string Link { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public string Description { get; set; }
        public DateTime PublishDate { get; set; }
        public string Author { get; set; }

        /// <summary>
        /// The image of the <see cref="FeedItem"/>.
        /// </summary>
        public string Image { get; set; }
    }
}
