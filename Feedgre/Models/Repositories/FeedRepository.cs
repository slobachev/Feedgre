using Microsoft.EntityFrameworkCore;
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

        public int DeleteFeed(int feedID)
        {
            _dbContext.Entry(new Feed()
            {
                Id = feedID
            }).State = EntityState.Deleted;
            return _dbContext.SaveChanges();
        }

        public Feed GetFeedByID(int feedId)
        {
            return _dbContext.Feeds.FirstOrDefault(f => f.Id == feedId);
        }

        public IEnumerable<Feed> GetFeeds()
        {
            return _dbContext.Feeds.ToList();
        }


        public int CreateFeed(Feed feed)
        {
            _dbContext.Feeds.Add(feed);
            return _dbContext.SaveChanges();
        }

        public int Save()
        {
            try
            {
                return _dbContext.SaveChanges();
            }
            catch (DbUpdateException)
            {
                //Thrown when database update fails
                throw;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int UpdateFeed(Feed feed)
        {
            _dbContext.Entry(feed).State = EntityState.Modified;
            return _dbContext.SaveChanges();
        }

        public int GetIdByTitle(string title)
        {
            var target = _dbContext.Feeds.FirstOrDefault(i => i.Title == title);
            if (target == null)
            {
                return -1;
            }

            return target.Id;
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
