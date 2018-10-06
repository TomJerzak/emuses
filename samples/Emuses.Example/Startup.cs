using System.Collections.Generic;
using System.Reflection;
using Emuses.Dashboard.Controllers;
using Emuses.Storages;
using Emuses.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace Emuses.Example
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IHostedService, SessionClearTask>();
            services.AddScoped<ISessionStorage>(storage => new FileStorage(@"C:\Temp\Emuses\"));
            // services.AddScoped<ISessionStorage>(storage => new PostgresStorage("Host=127.0.0.1;Username=emuses;Password=emuses;Database=emuses"));

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)            
                .AddApplicationPart(typeof(SessionController).GetTypeInfo().Assembly);

            services.Configure<RazorViewEngineOptions>(options =>
            {
                options.FileProviders.Add(new EmbeddedFileProvider(typeof(SessionController).GetTypeInfo().Assembly));
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            // app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();            

            app.UseEmuses(options =>
            {
                // options.LoginPage = "/Account/Login";
                // options.Logger = true;
                options.OpenSessionPage = "/Account/Login";
                options.SessionExpiredPage = "/Account/Expired";
                options.NoSessionAccessPages = new List<string> { "/Account/Login", "Account/Logout" };
                options.Storage = new FileStorage(@"C:\Temp\Emuses\");
            });

            /*app.UseEmuses(options =>
            {
                options.OpenSessionPage = "/Account/Login";
                options.SessionExpiredPage = "/Account/Expired";
                options.NoSessionAccessPages = new List<string> {"/Account/Login", "Account/Logout"};
                options.Storage = new PostgresStorage("Host=127.0.0.1;Username=emuses;Password=emuses;Database=emuses");
            });*/

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
