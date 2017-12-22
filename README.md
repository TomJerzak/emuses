# emuses

[![Latest version](https://img.shields.io/nuget/v/emuses.svg)](https://www.nuget.org/packages?q=emuses)
  
[![Build Status](https://api.travis-ci.org/teja-1010100/emuses.svg?branch=develop)](https://travis-ci.org/teja-1010100/emuses/branches)

Emuses is simple session manager for .net core.

## Getting Started

See our sample project (Emuses.Example) or use the instructions below.

- Save sessions to PostgreSQL.
  
Database configuration:
```SQL
CREATE USER emuses WITH LOGIN;
ALTER USER emuses WITH PASSWORD 'emuses';
  
CREATE DATABASE emuses WITH OWNER = emuses ENCODING = 'UTF8' TABLESPACE = pg_default CONNECTION LIMIT = -1;
```
  
```SQL  
CREATE TABLE public.sessions
(
  session_id character varying(50) NOT NULL,
  expiration_date timestamp without time zone NOT NULL,
  version character varying(50) NOT NULL,
  session_timeout integer NOT NULL
) WITH (OIDS = FALSE) TABLESPACE pg_default;
  
ALTER TABLE public.sessions OWNER to emuses;
```
  
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
