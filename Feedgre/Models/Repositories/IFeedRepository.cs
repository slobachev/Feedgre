using System;
using System.Collections.Generic;

namespace Feedgre.Models.Repositories
{
    public interface IFeedRepository : IDisposable
    {
        IEnumerable<Feed> GetFeeds();
        Feed GetFeedByID(int feedId);
        int CreateFeed(Feed feed);
        int DeleteFeed(int feedID);
        int UpdateFeed(Feed feed);
        int GetIdByTitle(string title);
        int Save();
    }
}
