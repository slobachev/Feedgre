using Feedgre.Models;
using Feedgre.Services.Parsing.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Feedgre.Services.Parsing
{
    public class ParserFactory : IParserFactory
    {
        /// <summary>
        /// Decides which parser to instantiate according to the provided type.
        /// </summary>
        public IFeedParser CreateParser(FeedType type)
        {
            switch (type)
            {
                case FeedType.RSS:
                    return new RssParser();
                case FeedType.Atom:
                    return new AtomParser();
                default:
                    return new RssParser();
            }
        }
    }
}
