using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Feedgre.Models;

namespace Feedgre.Controllers
{
    [Produces("application/json")]
    [Route("api/collections")]
    public class CollectionsController : Controller
    {
        // GET: api/collections
        [HttpGet]
        public IActionResult Get()
        {
            var parser = new RssParser();
            return Ok(parser.ParseFeed("http://feeds.feedburner.com/SmartCitiesReadwrite"));
        }

        // GET: api/collections/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/collections
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/collections/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/collections/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
