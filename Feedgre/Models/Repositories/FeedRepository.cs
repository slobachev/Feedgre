using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Feedgre.Models.Repositories
{
    public class FeedRepository : IFeedRepository
    {
        private FeedDBContext _dbContext;
        private bool disposed = false;

        public FeedRepository(FeedDBContext dBContext)
        {
            _dbContext = dBContext;
        }

        public void DeleteFeed(int feedID)
        {
            throw new NotImplementedException();
        }

        public Feed GetFeedByID(int feedId)
        {
            return _dbContext.Feeds.FirstOrDefault(f => f.Id == feedId);
        }

        public IEnumerable<Feed> GetFeeds()
        {
            return _dbContext.Feeds.ToList();
        }


        public void InsertFeed(Feed feed)
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            _dbContext.SaveChanges();
        }

        public void UpdateFeed(Feed feed)
        {
            throw new NotImplementedException();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _dbContext.Dispose();
                }
            }
            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
