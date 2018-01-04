using System;
using System.Collections.Generic;

namespace Feedgre.Models.Repositories
{
    public interface IFeedCollectionRepository : IDisposable
    {
        IEnumerable<FeedCollection> GetFeedCollections();
        FeedCollection GetCollectionByID(int collectionId);
        int CreateCollection(FeedCollection student);
        int DeleteCollection(int collectionID);
        int UpdateCollection(FeedCollection collection);
        IEnumerable<Feed> GetFeeds(int id);
        int Subscribe(int collectionId, int feedId);
        int Unsubscribe(int collectionId, int feedId);
        int GetIdByTitle(string title);
        int Save();
    }
}
