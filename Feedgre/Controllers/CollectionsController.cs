using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Feedgre.Models;
using Feedgre.Models.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Feedgre.Services.Parsing;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace Feedgre.Controllers
{
    [Produces("application/json")]
    [Route("api/collections")]
    public class CollectionsController : Controller
    {
        private readonly IFeedParser _rssParser;
        private readonly IFeedParser _atomParser;
        private readonly IFeedCollectionRepository _repository;
        private readonly ILogger<CollectionsController> _logger;
        private IMemoryCache _cache;

        public CollectionsController(IServiceProvider serviceProvider, IFeedCollectionRepository repository, IMemoryCache memoryCache, ILogger<CollectionsController> logger)
        {
            var services = serviceProvider.GetServices<IFeedParser>();
            _rssParser = services.First(o => o.GetType() == typeof(RssParser));
            _atomParser = services.First(o => o.GetType() == typeof(AtomParser));
            _repository = repository;
            _logger = logger;
            _cache = memoryCache;
        }
        // GET: api/collections
        [HttpGet]
        public IActionResult Get()
        {
            _logger.LogInformation("Retrieved all collections");
            return Ok(_repository.GetFeedCollections());
        }

        // GET: api/collections/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var feeds = _repository.GetFeeds(id);
            if (feeds == null)
            {
                return NotFound();
            }
            _logger.LogInformation("Retrieved feeds for the collection {id}", id);
            return Ok(feeds);
        }

        // GET: api/collections/5/all
        [HttpGet("{id}/all")]
        public IActionResult GetAllFeed(int id)
        {
            var feeds = _repository.GetFeeds(id);
            List<FeedItem> feedItems;

            if (feeds == null)
            {
                return NotFound();
            }

            var nameForCache = "collection " + id;

            if (!_cache.TryGetValue(nameForCache, out feedItems))
            {
                feedItems = new List<FeedItem>();
                // Key not in cache, so get data.
                foreach (var feed in feeds)
                {
                    switch (feed.Type)
                    {
                        case "RSS":
                            feedItems.AddRange(_rssParser.ParseFeed(feed.Link));
                            break;
                        case "Atom":
                            feedItems.AddRange(_atomParser.ParseFeed(feed.Link));
                            break;
                        default:
                            return BadRequest("No parser for the feed");
                    }
                }

                // Set cache options.
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    // Keep in cache for this time, reset time if accessed.
                    .SetSlidingExpiration(TimeSpan.FromMinutes(20));

                // Save data in cache.
                _cache.Set(nameForCache, feedItems, cacheEntryOptions);

                _logger.LogInformation("Cached all feed from the {nameForCache}", nameForCache);
            }
            else
            {
                _logger.LogInformation("Retrieved {nameForCache} content from cache", nameForCache);
            }

            return Ok(feedItems);
        }

        // POST: api/collections/1
        [HttpPost("{id}")]
        public IActionResult AddFeed([FromRoute]int id, int feedId)
        {
            var feeds = _repository.GetFeeds(id);
            var feed = feeds.FirstOrDefault(i => i.Id == feedId);
            if (feed != null)
            {
                _logger.LogError("The collection {collectionId} already subscribed to {feedTitle} feed", feedId, feed.Title);
                return RedirectToRoute("");
            }

            _repository.Subscribe(id, feedId);
            _repository.Save();
            _logger.LogInformation("Subscribed the {collectionId} collection to {feedTitle} feed", feedId, feed.Title);
            return Ok(feedId);
        }

        // POST: api/collections
        [HttpPost]
        public IActionResult Post(FeedCollection item)
        {
            if (item == null)
            {
                _logger.LogError("Failed to create collection");
                return BadRequest();
            }

            _repository.InsertCollection(item);
            _repository.Save();
            var itemId = _repository.GetIdByTitle(item.Title);
            _logger.LogInformation("Collection {collectionTitle} is successfully created with id {id}", item.Title, itemId);
            return Ok(itemId);
        }

        // PUT: api/collections/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]FeedCollection item)
        {
        }

        // DELETE: api/collections/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {

        }
    }
}
