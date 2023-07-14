# Rqlite .NET Client

![GitHub release (latest SemVer including pre-releases)](https://img.shields.io/github/v/release/bfren/rqlite?include_prereleases&label=Version) ![Nuget](https://img.shields.io/nuget/dt/rqlite?label=Downloads) ![GitHub](https://img.shields.io/github/license/bfren/rqlite?label=Licence)

[![Test](https://github.com/bfren/rqlite/actions/workflows/test.yml/badge.svg)](https://github.com/bfren/rqlite/actions/workflows/test.yml) ![Publish](https://github.com/bfren/rqlite/workflows/Publish/badge.svg)

Simple .NET client for [Rqlite](https://rqlite.io), with deserialisation support.

## Features

- Configure multiple connections in settings
- Execute (e.g. INSERT) and Query support
- Return query results as rows or map to objects
- Use parameters for Execute and Query
- Support for transactions and multiple statements

## Getting Started

The simplest way to start testing is to use [Docker](https://docker.com):

```bash
docker run -p4001:4001 rqlite/rqlite
```

Install the [NuGet package](https://nuget.org/packages/rqlite).  You can see a functioning example of the code below in the AppConsole project of this repository - look in Program.cs.

```csharp
# register Rqlite with dependency injection in your host startup
# (currently supports Microsoft.Extensions.DependencyInjection)
services.AddRqlite();

# add IRqliteClientFactory to a class constructor, or use IServiceProvider
var factory = provider.GetRequiredService<IRqliteClientFactory>();

# creates default IRqliteClient, listening on http://localhost:4001
using var client = factory.CreateClient();

# create a table
await client.ExecuteAsync("CREATE TABLE foo (id INTEGER NOT NULL PRIMARY KEY, name TEXT, age INTEGER");

# insert a row using parameters
var param = new { name = "Fred", age = 42 };
await client.ExecuteAsync("INSERT INTO foo(name, age) VALUES(:name, :age)", param);

# query the database using parameters
var param = new { name = "Fred" };
var query = await client.QueryAsync("SELECT * FROM foo WHERE name = :name", param);
Console.WriteLine("Fred is {0}.", query.Results[0].Values[0].First().GetInt32());
# Output: 'Fred is 42.'
```
