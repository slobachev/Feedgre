using System;
using System.Collections.Generic;

namespace Feedgre.Models.Repositories
{
    /// <summary>
    /// Interface for a feed source
    /// </summary>
    public interface IFeedRepository : IDisposable
    {
        /// <summary>
        /// Returns all feeds from database
        /// </summary>
        IEnumerable<Feed> GetFeeds();

        /// <summary>
        /// Returns a feed by specific id
        /// </summary>
        Feed GetFeedByID(int feedId);

        /// <summary>
        /// Creates a new feed
        /// </summary>
        int CreateFeed(Feed feed);

        /// <summary>
        /// Deletes a specific feed
        /// </summary>
        int DeleteFeed(int feedID);

        /// <summary>
        /// Updates a specific feed
        /// </summary>
        int UpdateFeed(Feed feed);

        /// <summary>
        /// Gets feed id by title
        /// </summary>
        int GetIdByTitle(string title);

        /// <summary>
        /// Saves changes in a database
        /// </summary>
        int Save();
    }
}
