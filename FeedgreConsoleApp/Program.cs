using Newtonsoft.Json;
using RestSharp;
using System;

namespace FeedgreConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            string bearerToken = GetBearerToken();
            Console.WriteLine("Retrieved the bearer access token from Auth0 domain as a test client");

            //No authentication
            var feeds = GetFeeds();
            Console.WriteLine("\nAll feeds");
            Console.WriteLine(feeds);

            var feedId = 3;
            var feedItems = GetFeedItems(feedId);
            Console.WriteLine($"\nFeed items of the feed {feedId}");
            Console.WriteLine(feedItems);

            //Requires authentication
            var collections = GetCollections(bearerToken);
            Console.WriteLine("\nAll collections:");
            Console.WriteLine(collections);

            var targetCollectionId = 1;
            var collectionFeeds = GetCollectionFeeds(bearerToken, targetCollectionId);
            Console.WriteLine($"\nCollection {targetCollectionId} feeds:");
            Console.WriteLine(collectionFeeds);

            var newCollection = new { Title = "Suprema" };

            var newCollectionId = AddCollection(bearerToken, newCollection);
            Console.WriteLine($"\nCreated collection id: {newCollectionId}");

            collections = GetCollections(bearerToken);
            Console.WriteLine("All collections");
            Console.WriteLine(collections);


            SubscribeToFeed(bearerToken, targetCollectionId, feedId);
            Console.WriteLine($"\nSubscribed collection {targetCollectionId} to feed {feedId}");

            //UnsubscribeFromFeed(bearerToken, targetCollectionId, feedId);
            //Console.WriteLine($"\nUnsubscribed collection {targetCollectionId} from feed {feedId}");

            collectionFeeds = GetCollectionFeeds(bearerToken, targetCollectionId);
            Console.WriteLine($"\nCollection {targetCollectionId} feeds:");
            Console.WriteLine(collectionFeeds);

            var collectionFeedItems = GetCollectionFeedItems(bearerToken, targetCollectionId);
            Console.WriteLine($"\nAll feed items of the collection {targetCollectionId}:");
            Console.WriteLine(collectionFeedItems);
            Console.WriteLine("\nTa-da, that's it");
        }

        private static string GetBearerToken()
        {
            var client = new RestClient("https://slobachev.eu.auth0.com/oauth/token");
            var request = new RestRequest(Method.POST);
            request.AddHeader("content-type", "application/json");
            request.AddParameter("application/json", "{\"client_id\":\"C9eeCR4feoGGFpy0VVRkedq8tHDfo2g7\",\"client_secret\":\"vL7P4L4njfkLmJGkUo7SqZpZzOyd-2BrwLYq3MnIKKBNiD5pMeGZ58yCP2-lDLT-\",\"audience\":\"https://feedgre/api\",\"grant_type\":\"client_credentials\"}", ParameterType.RequestBody);

            IRestResponse response = client.ExecuteAsync(request).Result;
            var jwt = JsonConvert.DeserializeObject<JWT>(response.Content);
            var bearerToken = jwt.token_type + " " + jwt.access_token;
            return bearerToken;
        }

        private static object AddCollection(string bearerToken, object newCollection)
        {
            var client = new RestClient("http://localhost:63231/api/collections");
            var request = new RestRequest(Method.POST);
            request.AddHeader("authorization", bearerToken);

            string jsonObject = JsonConvert.SerializeObject(newCollection, Formatting.Indented);
            request.AddParameter("application/json", jsonObject, ParameterType.RequestBody);
            var result = client.ExecuteAsync(request).Result;
            return JsonConvert.DeserializeObject(result.Content);
        }

        private static object GetCollections(string bearerToken)
        {
            var client = new RestClient("http://localhost:63231/api/collections");
            var request = new RestRequest(Method.GET);
            request.AddHeader("authorization", bearerToken);
            var response = client.ExecuteAsync(request).Result;
            var pageContent = JsonConvert.DeserializeObject(response.Content);
            return pageContent;
        }

        private static object GetCollectionFeeds(string bearerToken, int collId)
        {
            var client = new RestClient($"http://localhost:63231/api/collections/{collId}");
            var request = new RestRequest(Method.GET);
            request.AddHeader("authorization", bearerToken);
            var response = client.ExecuteAsync(request).Result;
            var pageContent = JsonConvert.DeserializeObject(response.Content);
            return pageContent;
        }

        private static object GetCollectionFeedItems(string bearerToken, int collId)
        {
            var client = new RestClient($"http://localhost:63231/api/collections/{collId}/all");
            var request = new RestRequest(Method.GET);
            request.AddHeader("authorization", bearerToken);
            var response = client.ExecuteAsync(request).Result;
            var pageContent = JsonConvert.DeserializeObject(response.Content);
            return pageContent;
        }

        private static void SubscribeToFeed(string bearerToken, int collId, int feedId)
        {
            var client = new RestClient($"http://localhost:63231/api/collections/{collId}");
            var request = new RestRequest(Method.POST);
            request.AddHeader("authorization", bearerToken);
            string jsonObject = JsonConvert.SerializeObject(feedId, Formatting.Indented);
            request.AddParameter("application/json", jsonObject, ParameterType.RequestBody);
            var result = client.ExecuteAsync(request).Result;
        }

        private static void UnsubscribeFromFeed(string bearerToken, int collId, int feedId)
        {
            var client = new RestClient($"http://localhost:63231/api/collections/{collId}/unsubscribe");
            var request = new RestRequest(Method.POST);
            request.AddHeader("authorization", bearerToken);
            string jsonObject = JsonConvert.SerializeObject(feedId, Formatting.Indented);
            request.AddParameter("application/json", feedId, ParameterType.RequestBody);
            var result = client.ExecuteAsync(request).Result;
        }

        private static object GetFeeds()
        {
            var client = new RestClient("http://localhost:63231/api/feeds");
            var request = new RestRequest(Method.GET);
            var response = client.ExecuteAsync(request).Result;
            var pageContent = JsonConvert.DeserializeObject(response.Content);
            return pageContent;

        }

        private static object GetFeedItems(int id)
        {
            var client = new RestClient($"http://localhost:63231/api/feeds/{id}");
            var request = new RestRequest(Method.GET);
            var response = client.ExecuteAsync(request).Result;
            var pageContent = JsonConvert.DeserializeObject(response.Content);
            return pageContent;
        }
    }
}
