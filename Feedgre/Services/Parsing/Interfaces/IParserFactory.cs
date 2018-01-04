using Feedgre.Models;

namespace Feedgre.Services.Parsing.Interfaces
{
    /// <summary>
    /// Parser factory
    /// </summary>
    public interface IParserFactory
    {
        IFeedParser CreateParser(FeedType type);
    }
}
