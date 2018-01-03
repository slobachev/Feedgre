using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Feedgre.Models;
using Microsoft.EntityFrameworkCore;
using Feedgre.Models.Repositories;
using Feedgre.Services.Parsing;
using Feedgre.Services.Parsing.Interfaces;

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
            services.AddMemoryCache();
            services.AddSingleton<IFeedParser, RssParser>();
            services.AddSingleton<IFeedParser, AtomParser>();
            services.AddEntityFrameworkSqlite().AddDbContext<FeedDBContext>();
            services.AddTransient<IFeedCollectionRepository, FeedCollectionRepository>();
            services.AddTransient<IFeedRepository, FeedRepository>();
            services.AddTransient<IServiceProvider, ServiceProvider>();
            services.AddSingleton<IParserFactory, ParserFactory>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            loggerFactory.AddConsole(Configuration.GetSection("Logging.Console"));


            app.UseMvc(routes =>
            {
                routes.MapRoute("default", "{controller=collections}/{action=Get}/{id?}");
            });
        }
    }
}
