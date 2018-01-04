# Feedgre
Web API to consume aggregated RSS, Atom feeds

This is my realisation of popular service https://feedly.com/ presented yet as a WEB API 
written in ASP.NET Core using thrird-parties like Restsharp, sqlite and Entity Framework Core for persistence,
xUnit for testing, Newtonsoft for serialization and Auth0 for a base authentication and autorisation. 
It is also worth mentioning implementation of in-memory caching and logging.

Below are listed URL's to interact with the API:
### Requires authentication:
- http://localhost:63231/api/collections
- http://localhost:63231/api/collections/id
- http://localhost:63231/api/collections/id/all
- http://localhost:63231/api/collections/id/unsubscribe
### No authentication:
- http://localhost:63231/api/feeds
- http://localhost:63231/api/feeds/id

In conclusion all requirements are fulfilled, however there are some things to be completed 
like implementation of the full authentication and authorization, unit and integration testing of controllers and repositories


