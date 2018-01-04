namespace Feedgre.Models
{
    /// <summary>
    /// Represents a relation between collections and feeds as subscriptions
    /// </summary>
    public class Subscription
    {
        public int Id { get; set; }
        public int FeedId { get; set; }
        public int CollectionId { get; set; }
    }
}
