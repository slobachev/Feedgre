using Microsoft.EntityFrameworkCore;

namespace Feedgre.Models
{
    public class FeedDBContext : DbContext
    {
        public DbSet<FeedCollection> FeedCollection { get; set; }
        public DbSet<Feed> Feeds { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=App_Data/Feed.db");
        }
    }
}
