using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Feedgre.Models.Repositories
{
    public interface IFeedRepository : IDisposable
    {
        IEnumerable<Feed> GetFeeds();
        Feed GetFeedByID(int feedId);
        void InsertFeed(Feed feed);
        void DeleteFeed(int feedID);
        void UpdateFeed(Feed feed);
        void Save();
    }
}
