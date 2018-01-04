using Feedgre.Models;

namespace Feedgre.Services.Parsing.Interfaces
{
    public interface IParserFactory
    {
        IFeedParser CreateParser(FeedType type);
    }
}
