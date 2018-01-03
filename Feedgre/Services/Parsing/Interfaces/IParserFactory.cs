using Feedgre.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Feedgre.Services.Parsing.Interfaces
{
    public interface IParserFactory
    {
        IFeedParser CreateParser(FeedType type);
    }
}
