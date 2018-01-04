using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Feedgre.Models;
using Feedgre.Models.Repositories;
using Feedgre.Services.Parsing;
using Feedgre.Services.Parsing.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using FeedgreAPI.Services.Authorization;

namespace Feedgre
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            using (var client = new FeedDBContext())
            {
                client.Database.EnsureCreated();
            }
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            string domain = $"https://{Configuration["Auth0:Domain"]}/";
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(options =>
            {
                options.Authority = domain;
                options.Audience = Configuration["Auth0:ApiIdentifier"];
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("read:collections", policy => policy.Requirements.Add(new HasScopeRequirement("read:collections", domain)));
                options.AddPolicy("write:collections", policy => policy.Requirements.Add(new HasScopeRequirement("write:collections", domain)));
            });

            // register the scope authorization handler
            services.AddSingleton<IAuthorizationHandler, HasScopeHandler>();

            services.AddMemoryCache();

            services.AddSingleton<IFeedParser, RssParser>();
            services.AddSingleton<IFeedParser, AtomParser>();
            services.AddSingleton<IParserFactory, ParserFactory>();

            services.AddEntityFrameworkSqlite().AddDbContext<FeedDBContext>();
            services.AddTransient<IFeedCollectionRepository, FeedCollectionRepository>();
            services.AddTransient<IFeedRepository, FeedRepository>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

            }

            loggerFactory.AddConsole(Configuration.GetSection("Logging.Console"));

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute("default", "{controller=collections}/{action=Get}/{id?}");
            });
        }
    }
}
