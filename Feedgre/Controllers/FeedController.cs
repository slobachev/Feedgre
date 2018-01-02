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

namespace Feedgre.Controllers
{
    [Produces("application/json")]
    [Route("api/feeds")]
    public class FeedController : Controller
    {
        private readonly IFeedParser _rssParser;
        private readonly IFeedParser _atomParser;
        private readonly IFeedRepository _repository;
        private IMemoryCache _cache;
        private readonly ILogger _logger;

        public FeedController(IServiceProvider serviceProvider, IFeedRepository repository, IMemoryCache memoryCache, ILogger<FeedController> logger)
        {
            var services = serviceProvider.GetServices<IFeedParser>();
            _rssParser = services.First(o => o.GetType() == typeof(RssParser));
            _atomParser = services.First(o => o.GetType() == typeof(AtomParser));
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
                    case "RSS":
                        feedItems = _rssParser.ParseFeed(feed.Link);
                        break;
                    case "Atom":
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
        public void Post([FromBody]string value)
        {
        }
        
        // PUT: api/feeds/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }
        
        // DELETE: api/feeds/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
