
using Feedgre.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RestSharp;
using System;

namespace FeedgreConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new RestClient("https://slobachev.eu.auth0.com/oauth/token");
            var request = new RestRequest(Method.POST);
            request.AddHeader("content-type", "application/json");
            request.AddParameter("application/json", "{\"client_id\":\"C9eeCR4feoGGFpy0VVRkedq8tHDfo2g7\",\"client_secret\":\"vL7P4L4njfkLmJGkUo7SqZpZzOyd-2BrwLYq3MnIKKBNiD5pMeGZ58yCP2-lDLT-\",\"audience\":\"https://feedgre/api\",\"grant_type\":\"client_credentials\"}", ParameterType.RequestBody);

            IRestResponse response = client.ExecuteAsync(request).Result;
            var jwt = JsonConvert.DeserializeObject<JWT>(response.Content);
            string bearerToken = jwt.token_type + " " + jwt.access_token;

            ShowCollections(bearerToken);

            var newCollection = new { Title = "Yolo" };
            var jsonSerializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            AddCollection(bearerToken, newCollection, jsonSerializerSettings);
            ShowCollections(bearerToken);

        }

        private static void AddCollection(string bearerToken, object newCollection, JsonSerializerSettings serializerSettings)
        {


            var client = new RestClient("http://localhost:63231/api/collections");
            var request = new RestRequest(Method.POST);
            request.AddHeader("authorization", bearerToken);

            string jsonObject = JsonConvert.SerializeObject(newCollection, Formatting.Indented, serializerSettings);
            Console.WriteLine(jsonObject);
            request.AddParameter("application/json", jsonObject, ParameterType.RequestBody);
            var response = client.ExecuteAsync(request).Result;
        }

        private static void ShowCollections(string bearerToken)
        {
            var client = new RestClient("http://localhost:63231/api/collections");
            var request = new RestRequest(Method.GET);
            request.AddHeader("authorization", bearerToken);
            var response = client.ExecuteAsync(request).Result;
            var pageContent = JsonConvert.DeserializeObject(response.Content);
            Console.WriteLine(pageContent);
        }
    }
}
