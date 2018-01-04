using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Feedgre.Models.Repositories
{
    public class FeedCollectionRepository : IFeedCollectionRepository
    {

        private FeedDBContext _dbContext;
        private bool disposed = false;

        public FeedCollectionRepository(FeedDBContext dBContext)
        {
            _dbContext = dBContext;
        }

        public int DeleteCollection(int collectionID)
        {
            _dbContext.Entry(new FeedCollection()
            {
                Id = collectionID
            }).State = EntityState.Deleted;
            return _dbContext.SaveChanges();
        }

        public IEnumerable<Feed> GetFeeds(int id)
        {
            var feeds = (from F in _dbContext.Feeds
                         join S in _dbContext.Subscriptions on F.Id equals S.FeedId
                         join FC in _dbContext.FeedCollection on S.CollectionId equals FC.Id
                         where FC.Id == id
                         select new Feed
                         {
                             Id = F.Id,
                             Title = F.Title,
                             Description = F.Description,
                             Followers = F.Followers,
                             Link = F.Link,
                             Type = F.Type
                         });
            return feeds;
        }

        public FeedCollection GetCollectionByID(int collectionId)
        {
            return _dbContext.FeedCollection.Find(collectionId);
        }

        public IEnumerable<FeedCollection> GetFeedCollections()
        {
            return _dbContext.FeedCollection.ToList();
        }

        public int CreateCollection(FeedCollection item)
        {
            _dbContext.FeedCollection.Add(item);
            return _dbContext.SaveChanges();
        }

        public int Subscribe(int collectionId, int feedId)
        {
            var sub = new Subscription
            {
                FeedId = feedId,
                CollectionId = collectionId
            };
            _dbContext.Subscriptions.Add(sub);
            return _dbContext.SaveChanges();
        }

        public int Unsubscribe(int collectionId, int feedId)
        {
            var subscription = _dbContext.Subscriptions
                .AsNoTracking()
                .FirstOrDefault(s => s.CollectionId == collectionId && s.FeedId == feedId);

            _dbContext.Entry(new Subscription()
            {
                Id = subscription.Id
            }).State = EntityState.Deleted;
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

        public int UpdateCollection(FeedCollection collection)
        {
            _dbContext.Entry(collection).State = EntityState.Modified;
            return _dbContext.SaveChanges();
        }

        public int GetIdByTitle(string title)
        {
            var target = _dbContext.FeedCollection.FirstOrDefault(i => i.Title == title);
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
