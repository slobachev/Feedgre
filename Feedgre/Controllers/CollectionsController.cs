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
using Feedgre.Services.Parsing.Interfaces;

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

        public CollectionsController(IParserFactory parserFactory, IFeedCollectionRepository repository, IMemoryCache memoryCache, ILogger<CollectionsController> logger)
        {
            _rssParser = parserFactory.CreateParser(FeedType.RSS);
            _atomParser = parserFactory.CreateParser(FeedType.Atom);
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
                        case FeedType.RSS:
                            feedItems.AddRange(_rssParser.ParseFeed(feed.Link));
                            break;
                        case FeedType.Atom:
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
        public IActionResult AddFeed(int id, int feedId)
        {
            var feeds = _repository.GetFeeds(id);
            var feed = feeds.FirstOrDefault(i => i.Id == feedId);
            if (feed != null)
            {
                _logger.LogError("The collection {collectionId} already subscribed to {feedTitle} feed", feedId, feed.Title);
                return RedirectToRoute("");
            }

            try
            {
                _repository.Subscribe(id, feedId);
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to subscribe due to the database problems {ex}", ex);
                return BadRequest(ex);
            }

            _logger.LogInformation("Subscribed the collection {collectionId} to {feedTitle} feed", feedId, feed.Title);
            return Ok(feedId);
        }

        // POST: api/collections
        [HttpPost]
        public IActionResult CreateCollection(FeedCollection item)
        {
            if (item == null)
            {
                _logger.LogError("Failed to create collection");
                return BadRequest();
            }

            try
            {
                _repository.CreateCollection(item);
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to create collection due to the database problems {ex}", ex);
                return BadRequest(ex);
            }

            var itemId = _repository.GetIdByTitle(item.Title);
            _logger.LogInformation("Collection {collectionTitle} is successfully created with id {id}", item.Title, itemId);
            return Ok(itemId);
        }

        // PUT: api/collections/5
        [HttpPut("{id}")]
        public IActionResult UpdateCollection(int id, [FromBody]FeedCollection item)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError("Failed to update collection due to the model problems");
                return BadRequest(ModelState);
            }

            if (id != item.Id)
            {
                _logger.LogError("Failed to update collection due to the id mismatch");
                return BadRequest();
            }
            try
            {
                _repository.UpdateCollection(item);
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to update collection due to the database problems {ex}", ex);
                return BadRequest(ex);
            }

            _logger.LogInformation("Collection {id} is successfully updated", id);
            return Ok();
        }

        // DELETE: api/collections/5
        [HttpDelete("{id}")]
        public IActionResult DeleteCollection(int id)
        {
            try
            {
                _repository.DeleteCollection(id);
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to delete collection due to the database problems {ex}", ex);
                return BadRequest(ex);
            }

            _logger.LogInformation("Collection {id} is successfully deleted", id);
            return Ok();
        }
    }
}
