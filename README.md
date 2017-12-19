# emuses

Emuses is simple session manager for .net core.

## Getting Started

See our sample project (Emuses.Example) or use the instructions below.

- Startup.cs
    
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
   