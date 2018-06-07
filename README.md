# emuses

[![Latest version](https://img.shields.io/nuget/v/emuses.svg)](https://www.nuget.org/packages?q=emuses) [![SonarCloud Scan Build Status](https://sonarcloud.io/api/badges/gate?key=Emuses)](https://sonarcloud.io/api/badges/gate?key=Emuses)
  
## Build status

&nbsp; | `master` | `develop`
--- | --- | --- 
**Windows** | [![Build status](https://ci.appveyor.com/api/projects/status/w5f9n0klhma23htn/branch/master?svg=true)](https://ci.appveyor.com/project/teja-1010100/emuses/branch/master) | [![Build status](https://ci.appveyor.com/api/projects/status/w5f9n0klhma23htn/branch/develop?svg=true)](https://ci.appveyor.com/project/teja-1010100/emuses/branch/develop) 
**Linux / OS X** | [![Travis CI Build Status](https://api.travis-ci.org/teja-1010100/emuses.svg?branch=master)](https://travis-ci.org/teja-1010100/emuses/branches) | [![Travis CI Build Status](https://api.travis-ci.org/teja-1010100/emuses.svg?branch=develop)](https://travis-ci.org/teja-1010100/emuses/branches)
  
## Overview

Emuses is simple session manager for .net core.

## Installation

Emuses is available as a NuGet package. You can install it using the NuGet Package Console window:
  
```
PM> Install-Package Emuses
```
  
## Getting Started

See our sample project (Emuses.Example) or use the instructions below.

#### 1. Storage

##### 1.1 Save sessions to PostgreSQL.
  
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
```csharp
  public void ConfigureServices(IServiceCollection services)
  {
    ...
    services.AddSingleton<IHostedService, SessionClearTask>();
    services.AddScoped<ISessionStorage>(storage => new PostgresStorage("Host=127.0.0.1;Username=emuses;Password=emuses;Database=emuses"));

    services.AddMvc();
    ...
  }
```
  
```csharp
  public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
  {
    ...
    app.UseEmuses(options =>
    {
      options.OpenSessionPage = "/Account/Login";
      options.SessionExpiredPage = "/Account/Expired";
      options.NoSessionAccessPages = new List<string> {"/Account/Login", "Account/Logout"};
      options.Storage = new PostgresStorage("Host=127.0.0.1;Username=emuses;Password=emuses;Database=emuses");
    });

    app.UseMvc(routes => ...
    ...
  }
```
    
##### 1.2 Save sessions to files.
  
Startup.cs  
```csharp
  public void ConfigureServices(IServiceCollection services)
  {
    ...
    services.AddSingleton<IHostedService, SessionClearTask>();
    services.AddScoped<ISessionStorage>(storage => new FileStorage(@"C:\Temp\Emuses\"));

    services.AddMvc();
    ...
  }
```
    
```csharp
  public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
  {
    ...
    app.UseEmuses(options =>
    {
      options.OpenSessionPage = "/Account/Login";
      options.SessionExpiredPage = "/Account/Expired";
      options.NoSessionAccessPages = new List<string> {"/Account/Login", "Account/Logout"};
      options.Storage = new FileStorage(@"C:\Temp\Emuses\");
    });

    app.UseMvc(routes => ...
    ...
  }
```
  
#### 2. Dashboard
  
Startup.cs
```csharp
  public void ConfigureServices(IServiceCollection services)
  {
    ...
    services
      .AddMvc()
      .AddApplicationPart(typeof(SessionController).GetTypeInfo().Assembly);

    services.Configure<RazorViewEngineOptions>(options =>
    {
      options.FileProviders.Add(new EmbeddedFileProvider(typeof(SessionController).GetTypeInfo().Assembly));
    });
  }
```
  