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
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            Console.ReadKey();
            client = new RestClient("http://localhost:63231/api/collections");
            request = new RestRequest(Method.GET);
            request.AddHeader("authorization", "Bearer eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiIsImtpZCI6Ik5ESTVRMEV4TWprNVJUZEJSVGhEUXpVMlJUSXpOa1UyUmpGQk5qTkNORVJHT0VSR016bERRdyJ9.eyJpc3MiOiJodHRwczovL3Nsb2JhY2hldi5ldS5hdXRoMC5jb20vIiwic3ViIjoiQzllZUNSNGZlb0dHRnB5MFZWUmtlZHE4dEhEZm8yZzdAY2xpZW50cyIsImF1ZCI6Imh0dHBzOi8vZmVlZGdyZS9hcGkiLCJpYXQiOjE1MTQ5OTU3NjUsImV4cCI6MTUxNTA4MjE2NSwic2NvcGUiOiJ3cml0ZTpjb2xsZWN0aW9ucyByZWFkOmNvbGxlY3Rpb25zIiwiZ3R5IjoiY2xpZW50LWNyZWRlbnRpYWxzIn0.iXT0S-EZVIs4sqmQrq4PggZWkVsnYEzZ3vZsw_YZhT7KWKOLNEwKJEKMyVAHLjHUt6HuS56qPpy0s61rFnWOG8eBA0BN87Uif9oB7JYedPOqBWLX__SCo_HxEjiElv23ibw2X5s2EelhSOdiXvkC7aQ32IqkDg57e7_9vJm2TI72oCMcVEEdClTSbGNh5jsvjB3JdwQ8ps5zwBeW4GY52jQoMD0gxQ06r7KKonw4AI38UXYXSVfM-Ab3N1PH7MQScykuzWQ9GyGHANGgNbnFyK3fGTKCIjriEyFfezRgciaobsENJU9ox7MUxi4JmsklTl2DmmgbNE88mwg4YkItgA");
            response = client.Execute(request);
            Console.WriteLine(response.Content);
        }
    }
}
