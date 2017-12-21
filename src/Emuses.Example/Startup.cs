using System.Collections.Generic;
using Emuses.Storages;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Emuses.Example
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IStorage>(storage => new FileStorage(@"C:\Temp\Emuses\"));

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

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

            app.UseEmuses(new EmusesConfiguration
            {
                OpenSessionPage = "/Account/Login",
                SessionExpiredPage = "/Account/Expired",
                NoSessionAccessPages = new List<string>() {"/Account/Login", "Account/Logout"},
                Storage = new FileStorage(@"C:\Temp\Emuses\")
            });

//            app.UseEmuses(new EmusesConfiguration()
//            {
//                OpenSessionPage = "/Account/Login",
//                SessionExpiredPage = "/Account/Expired",
//                NoSessionAccessPages = new List<string>() {"/Account/Login", "Account/Logout"},
//                Storage = new PostgresStorage("Host=127.0.0.1;Username=emuses;Password=emuses;Database=emuses")
//            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
