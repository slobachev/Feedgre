using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Feedgre.Models;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;
using Feedgre.Models.Repositories;
using Feedgre.Services.Parsing;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Feedgre.Services.Parsing.Interfaces;

namespace Feedgre.Controllers
{
    [Produces("application/json")]
    [Route("api/feeds")]
    public class FeedsController : Controller
    {
        private readonly IFeedParser _rssParser;
        private readonly IFeedParser _atomParser;
        private readonly IFeedRepository _repository;
        private IMemoryCache _cache;
        private readonly ILogger<FeedsController> _logger;

        public FeedsController(IParserFactory parserFactory, IFeedRepository repository, IMemoryCache memoryCache, ILogger<FeedsController> logger)
        {
            _rssParser = parserFactory.CreateParser(FeedType.RSS);
            _atomParser = parserFactory.CreateParser(FeedType.Atom);
            _repository = repository;
            _cache = memoryCache;
            _logger = logger;
        }

        // GET: api/feeds
        [HttpGet]
        public IActionResult Get()
        {
            _logger.LogInformation("Retrieved all feeds");
            return Ok(_repository.GetFeeds());
        }

        // GET: api/feeds/id
        [HttpGet("{id}")]
        public IActionResult GetFeed(int id)
        {
            var feed = _repository.GetFeedByID(id);
            IList<FeedItem> feedItems;

            if (feed == null)
            {
                return NotFound();
            }

            var nameForCache = feed.Title;

            if (!_cache.TryGetValue(nameForCache, out feedItems))
            {
                // Key not in cache, so get data.
                switch (feed.Type)
                {
                    case FeedType.RSS:
                        feedItems = _rssParser.ParseFeed(feed.Link);
                        break;
                    case FeedType.Atom:
                        feedItems = _atomParser.ParseFeed(feed.Link);
                        break;
                    default:
                        return BadRequest("No parser for the feed");
                }

                // Set cache options.
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    // Keep in cache for this time, reset time if accessed.
                    .SetSlidingExpiration(TimeSpan.FromMinutes(20));

                // Save data in cache.
                _cache.Set(nameForCache, feedItems, cacheEntryOptions);

                _logger.LogInformation("Cached content of {nameForCache} feed", nameForCache);
            }
            else
            {
                _logger.LogInformation("Retrieved {nameForCache} content from cache", nameForCache);
            }
            return Ok(feedItems);
        }
        
        // POST: api/feeds
        [HttpPost]
        public IActionResult CreateFeed(Feed item)
        {
            if (item == null)
            {
                _logger.LogError("Failed to create feed");
                return BadRequest();
            }

            try
            {
                _repository.CreateFeed(item);
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to create feed due to the database problems {ex}", ex);
                return BadRequest(ex);
            }

            var itemId = _repository.GetIdByTitle(item.Title);
            _logger.LogInformation("Feed {feedTitle} is successfully created with id {id}", item.Title, itemId);
            return Ok(itemId);
        }
        
        // PUT: api/feeds/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]Feed item)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError("Failed to update feed due to the model problems");
                return BadRequest(ModelState);
            }

            if (id != item.Id)
            {
                _logger.LogError("Failed to update feed due to the id mismatch");
                return BadRequest();
            }
            try
            {
                _repository.UpdateFeed(item);
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to update feed due to the database problems {ex}", ex);
                return BadRequest(ex);
            }

            _logger.LogInformation("Feed {id} is successfully updated", id);
            return Ok();
        }
        
        // DELETE: api/feeds/5
        [HttpDelete("{id}")]
        public IActionResult DeleteFeed(int id)
        {
            try
            {
                _repository.DeleteFeed(id);
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to delete feed due to the database problems {ex}", ex);
                return BadRequest(ex);
            }

            _logger.LogInformation("Feed {id} is successfully deleted", id);
            return Ok();
        }
    }
}
