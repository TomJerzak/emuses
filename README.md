# emuses

Emuses is simple session manager for .net core.

## Getting Started

See our sample project (Emuses.Example) or use the instructions below.

- Startup.cs
  
```C#
public class Startup
{
  private readonly IStorage _fileStorage = new FileStorage(@"C:\Temp\Emuses\");
  private readonly Session _session;
  ...
  public Startup(IHostingEnvironment env)
  {
    ...
    _session = new Session(30, _fileStorage);
  }
}
```
    
```C#
  public void ConfigureServices(IServiceCollection services)
  {
    ...
    services.AddScoped<ISession>(session => _session);
    services.AddScoped<IStorage>(storage => _fileStorage);
    
    services.AddMvc();
  }
```
  
```C#
  public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
  {
    ...
    app.UseEmuses(_session);

    app.UseMvc(routes => ...
  }
}
```
  
## 