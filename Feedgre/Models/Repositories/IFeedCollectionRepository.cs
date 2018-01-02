using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Feedgre.Models.Repositories
{
    public interface IFeedCollectionRepository : IDisposable
    {
        IEnumerable<FeedCollection> GetFeedCollections();
        FeedCollection GetCollectionByID(int collectionId);
        void InsertCollection(FeedCollection student);
        void DeleteCollection(int collectionID);
        void UpdateCollection(FeedCollection collection);
        IEnumerable<Feed> GetFeeds(int id);
        void Subscribe(int collectionId, int feedId);
        int GetIdByTitle(string title);
        void Save();
    }
}
