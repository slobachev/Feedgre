using System;
using System.Collections.Generic;

namespace Feedgre.Models.Repositories
{
    /// <summary>
    /// Interface for a user feed collection
    /// </summary>
    public interface IFeedCollectionRepository : IDisposable
    {
        /// <summary>
        /// Returns all feed collections from database
        /// </summary>
        IEnumerable<FeedCollection> GetFeedCollections();

        /// <summary>
        /// Returns a feed collection by specific id
        /// </summary>
        FeedCollection GetCollectionByID(int collectionId);

        /// <summary>
        /// Creates a new feed collection
        /// </summary>
        int CreateCollection(FeedCollection student);

        /// <summary>
        /// Deletes a specific feed collection
        /// </summary>
        int DeleteCollection(int collectionID);

        /// <summary>
        /// Updates a specific feed collection
        /// </summary>
        int UpdateCollection(FeedCollection collection);

        /// <summary>
        /// Returns all feeds for a specific feed collection from database
        /// </summary>
        IEnumerable<Feed> GetFeeds(int id);

        /// <summary>
        /// Subscribes a specified collection to a specified feed
        /// </summary>
        int Subscribe(int collectionId, int feedId);

        /// <summary>
        /// Unsubscribes a specified collection from a specified feed
        /// </summary>
        int Unsubscribe(int collectionId, int feedId);

        /// <summary>
        /// Gets feed collection id by title
        /// </summary>
        int GetIdByTitle(string title);

        /// <summary>
        /// Saves changes in a database
        /// </summary>
        int Save();
    }
}
