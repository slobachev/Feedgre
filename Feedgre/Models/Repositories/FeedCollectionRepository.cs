using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        public void DeleteCollection(int collectionID)
        {
            throw new NotImplementedException();
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

        public void InsertCollection(FeedCollection item)
        {
            _dbContext.FeedCollection.Add(item);
        }

        public void Subscribe(int collectionId, int feedId)
        {
            var sub = new Subscription
            {
                FeedId = feedId,
                CollectionId = collectionId
            };
            _dbContext.Subscriptions.Add(sub);
        }

        public void Save()
        {
            _dbContext.SaveChanges();
        }

        public void UpdateCollection(FeedCollection collection)
        {
            throw new NotImplementedException();
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
