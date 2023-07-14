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

Install the [NuGet package](https://nuget.org/packages/rqlite).  You can see a functioning example of the code below in the ReadmeApp project of this repository.

```csharp
// register Rqlite with dependency injection in your host startup
// (currently supports Microsoft.Extensions.DependencyInjection)
services.AddRqlite();

// add IRqliteClientFactory to a class constructor, or use IServiceProvider
var factory = provider.GetRequiredService<IRqliteClientFactory>();

// creates default IRqliteClient, listening on http://localhost:4001
using var client = factory.CreateClient();

// create a table
await client.ExecuteAsync("DROP TABLE IF EXISTS foo");
await client.ExecuteAsync("CREATE TABLE foo (id INTEGER NOT NULL PRIMARY KEY, name TEXT, age INTEGER)");

// 0: insert a row using parameters
var sql0 = "INSERT INTO foo(name, age) VALUES(:name, :age)";
var param0 = new { name = "Fred", age = 42 };
var query0 = await client.ExecuteAsync(sql0, param0);
Console.WriteLine("Inserted record {0}.", query0.Results.Select(r => r.LastInsertId).First());
// Output: 'Inserted record 1.'

// 1: query the database using parameters
var sql1 = "SELECT * FROM foo WHERE name = :name";
var param1 = new { name = "Fred" };
var query1 = await client.QueryAsync(sql1, param1);
Console.WriteLine("Fred is {0}.", query1.Results[0].Values![0][2].GetInt32());
// Output: 'Fred is 42.'

// 2: get value as a simple type
// For scalar queries, Flatten() removes additional info from the result (including errors!)
// and returns the first value as the specified type
var sql2 = "SELECT age FROM foo WHERE name = :name";
var query2 = await client.ScalarAsync<int>(sql2, param1).Flatten();
Console.WriteLine("Fred is {0}.", query2);
// Output: 'Fred is 42.'

// 3: map results to a complex type
// For mapped queries, Flatten() removes additional info from the result (including errors!)
// and returns all the matching rows as objects of the specified type
var query3 = await client.QueryAsync<Person>(sql1, param1).Flatten();
Console.WriteLine("Found {0}.", query3.First());
// Output: 'Found Person { Id = 1, Name = Fred, Age = 42 }.'

// 4: insert multiple rows at once using tuples
var param2 = new { name = "Bella", age = 31 };
var param3 = new { name = "Alex", age = 59 };
var query4 = await client.ExecuteAsync(
    (sql0, param2),
    (sql0, param3)
);
Console.WriteLine("Inserted records {0}.", string.Join(", ", query4.Results.Select(r => r.LastInsertId)));
// Output: 'Inserted records 2, 3.'

internal sealed record Person(int Id, string Name, int Age);
```
