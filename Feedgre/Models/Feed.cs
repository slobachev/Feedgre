﻿namespace Feedgre.Models
{
    /// <summary>
    /// Represents a feed source
    /// </summary>
    public class Feed
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
        public int Followers { get; set; }
        public FeedType Type { get; set; }
    }
}
