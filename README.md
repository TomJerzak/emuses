# emuses

[![Latest version](https://img.shields.io/nuget/v/emuses.svg)](https://www.nuget.org/packages?q=emuses)

Emuses is simple session manager for .net core.

## Getting Started

See our sample project (Emuses.Example) or use the instructions below.

- Save to PostgreSQL.
  
Startup.cs  
```C#
  public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
  {
    ...
    app.UseEmuses(60, new PostgresStorage("Host=127.0.0.1;Username=emuses;Password=emuses;Database=emuses")); 

    app.UseMvc(routes => ...
    ...
  }
}
```
    
- Save to files.
  
Startup.cs  
```C#
  public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
  {
    ...
    app.UseEmuses(60, new FileStorage(@"C:\Temp\Emuses\"));

    app.UseMvc(routes => ...
    ...
  }
}
```
