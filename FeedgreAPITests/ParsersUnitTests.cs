using Feedgre.Models;
using Feedgre.Services.Parsing;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace FeedgreAPITests
{
    public class ParsersUnitTests
    {
        [Fact]
        public void CreateRss_FactoryShouldCreateRssParser()
        {
            var parserFactory = new ParserFactory();

            IFeedParser rssParser = parserFactory.CreateParser(FeedType.RSS);

            Assert.IsType<RssParser>(rssParser);
        }

        [Fact]
        public void CreateRss_FactoryShouldCreateAtomParser()
        {
            var parserFactory = new ParserFactory();

            IFeedParser rssParser = parserFactory.CreateParser(FeedType.Atom);

            Assert.IsType<AtomParser>(rssParser);
        }

        [Fact]
        public void GetEmpty_ParserShouldReturnEmptyListOnBadUrl()
        {
            var parserFactory = new ParserFactory();
            IFeedParser rssParser = parserFactory.CreateParser(FeedType.Atom);
            var url = "";

            var items = rssParser.ParseFeed(url);

            Assert.Empty(items);
        }

        [Fact]
        public void NotEmpty_RssParserShouldReturnValues()
        {
            var parserFactory = new ParserFactory();
            IFeedParser rssParser = parserFactory.CreateParser(FeedType.RSS);
            var url = "http://rss.nytimes.com/services/xml/rss/nyt/World.xml";

            var items = rssParser.ParseFeed(url);

            Assert.NotEmpty(items);
        }

        [Fact]
        public void NotEmpty_AtomParserShouldReturnValues()
        {
            var parserFactory = new ParserFactory();
            IFeedParser rssParser = parserFactory.CreateParser(FeedType.Atom);
            var url = "https://www.theverge.com/rss/index.xml";

            var items = rssParser.ParseFeed(url);

            Assert.NotEmpty(items);
        }

        [Fact]
        public void GetItems_ParserShouldReturnCollectionOfItems()
        {
            var parserFactory = new ParserFactory();
            IFeedParser rssParser = parserFactory.CreateParser(FeedType.Atom);
            var url = "https://www.theverge.com/rss/index.xml";

            var items = rssParser.ParseFeed(url);

            Assert.IsType<List<FeedItem>>(items);
        }

    }
}
