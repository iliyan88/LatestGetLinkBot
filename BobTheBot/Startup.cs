using System;
using System.IO;
using Serilog;
using BobTheBot;
using BobTheBot.ApplicationServices;
using BobTheBot.Cache;
using BobTheBot.Entities;
using BobTheBot.Kernel;
using BobTheBot.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace WebApplication
{
    public class Startup
    {

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            if (env.IsDevelopment())
            {
                //builder.AddUserSecrets();

                Log.Logger = new LoggerConfiguration()
                    .MinimumLevel.Debug()
                    .WriteTo.RollingFile(Path.Combine(env.ContentRootPath, "log-{Date}.txt"))
                    .CreateLogger();
            }
            else
            {
                Log.Logger = new LoggerConfiguration()
                    .MinimumLevel.Error()
                    .WriteTo.RollingFile(Path.Combine(env.ContentRootPath, "log-{Date}.txt"))
                    .CreateLogger();
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        private void AddDbContextsServices(IServiceCollection services)
        {
            services.AddDbContext<AppDbContext>((optionsAction) =>
            {
                optionsAction.UseSqlite(
                   "Data Source=BobTheBot.db"
                );
            });
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Adding options so we can inject configurations.
            services.AddOptions();

            // Register the bot credentials section
            services.Configure<BotCredentials>(Configuration.GetSection("BotCredentials"));

            // we'll be catching tokens, so enable MemoryCache.
            services.AddMemoryCache();

            services.AddMvc();

            AddDbContextsServices(services);
            AddCommonServices(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            loggerFactory.AddSerilog();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        private static void Migrate(IServiceProvider services)
        {
            var dbContext = services.GetService<AppDbContext>();
            if (dbContext != null)
            {
                dbContext.Database.Migrate();
            }
        }

        private void DbMigrate(IServiceScopeFactory scopeFactory, IHostingEnvironment env)
        {

            using (var scope = scopeFactory.CreateScope())
            {
                var services = scope.ServiceProvider;
                Migrate(services);
            }
        }

        protected void AddCommonServices(IServiceCollection services)
        {
            services.AddScoped<Microsoft.Extensions.Internal.ISystemClock, SystemClock>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ISearchKeyRepository, SearchKeyRepository>();
            services.AddScoped<IWordCache, WordCache>();
            services.AddScoped<IUserToReplyRepository, UserToReplyRepository>();
            services.AddScoped<SearchKeyService>();
            services.AddScoped<MessageService>();
            services.AddScoped<UserService>();
        }
    }
}