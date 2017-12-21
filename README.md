# emuses

[![Latest version](https://img.shields.io/nuget/v/emuses.svg)](https://www.nuget.org/packages?q=emuses)

Emuses is simple session manager for .net core.

## Getting Started

See our sample project (Emuses.Example) or use the instructions below.

- Save sessions to PostgreSQL.
  
Startup.cs  
```C#
  public void ConfigureServices(IServiceCollection services)
  {
    ...
    services.AddScoped<IStorage>(storage => new PostgresStorage("Host=127.0.0.1;Username=emuses;Password=emuses;Database=emuses"));

    services.AddMvc();
    ...
  }
```
  
```C#
  public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
  {
    ...
    app.UseEmuses(new EmusesConfiguration()
    {
      OpenSessionPage = "/Account/Login",
      SessionExpiredPage = "/Account/Expired",
      NoSessionAccessPages = new List<string>() {"/Account/Login", "Account/Logout"},
      Storage = new PostgresStorage("Host=127.0.0.1;Username=emuses;Password=emuses;Database=emuses")
    });

    app.UseMvc(routes => ...
    ...
  }
```
    
- Save sessions to files.
  
Startup.cs  
```C#
  public void ConfigureServices(IServiceCollection services)
  {
    ...
    services.AddScoped<IStorage>(storage => new FileStorage(@"C:\Temp\Emuses\"));

    services.AddMvc();
    ...
  }
```
    
```C#
  public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
  {
    ...
    app.UseEmuses(new EmusesConfiguration()
    {
      OpenSessionPage = "/Account/Login",
      SessionExpiredPage = "/Account/Expired",
      NoSessionAccessPages = new List<string>() {"/Account/Login", "Account/Logout"},
      Storage = new FileStorage(@"C:\Temp\Emuses\")
    });

    app.UseMvc(routes => ...
    ...
  }
```
