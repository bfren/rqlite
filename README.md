# Rqlite .NET Client

![GitHub release (latest SemVer including pre-releases)](https://img.shields.io/github/v/release/bfren/rqlite?include_prereleases&label=Version) ![Nuget](https://img.shields.io/nuget/dt/rqlite?label=Downloads) ![GitHub](https://img.shields.io/github/license/bfren/rqlite?label=Licence)

[![Test](https://github.com/bfren/rqlite/actions/workflows/test.yml/badge.svg)](https://github.com/bfren/rqlite/actions/workflows/test.yml) ![Publish](https://github.com/bfren/rqlite/workflows/Publish/badge.svg)

Unofficial .NET client for [Rqlite](https://rqlite.io), download [NuGet package](https://nuget.org/packages/rqlite).

Documentation (including API explorer) is available [here](https://bfren.github.io/rqlite).

## Features

- Fully asynchronous throughout
- Configure multiple named connections in settings
- Execute (e.g. INSERT), Query and Scalar support
- Return query results as objects
- Use parameters with queries
- Support for transactions and multiple statements

## Getting Started

The simplest way to start testing is to use [Docker](https://docker.com):

```bash
# in a temporary directory of your choice
$ git checkout https://github.com/bfren/rqlite .
$ chmod +x run.sh
$ ./run.sh

# in a new terminal
$ dotnet run --project apps/ReadmeApp
```

This will execute the code below (taken from ReadmeApp's `Project.cs` file).  You can see additional options and code in the ConsoleApp project.

```csharp
// register Rqlite with dependency injection
var (app, log) = Jeebs.Apps.Host.Create(args, (ctx, services) => services.AddRqlite());

// get IRqliteClientFactory instance
var factory = app.Services.GetRequiredService<IRqliteClientFactory>();

// create default IRqliteClient, listening on http://localhost:4001
using var client = factory.CreateClient();

// create a table
await client.ExecuteAsync("DROP TABLE IF EXISTS foo");
await client.ExecuteAsync("CREATE TABLE foo (id INTEGER NOT NULL PRIMARY KEY, name TEXT, age INTEGER)");

// 0: insert a row using parameters
var sql0 = "INSERT INTO foo(name, age) VALUES(:name, :age)";
var param0 = new { name = "Fred", age = 42 };
var query0 = await client.ExecuteAsync(sql0, param0);
query0.Audit(
	fail: e => log.Err(e.Message),
	ok: x => Console.WriteLine("Inserted record {0}.", x.Select(r => r.LastInsertId).First())
);
// Output: 'Inserted record 1.'

// 1: query the database using parameters
var sql1 = "SELECT * FROM foo WHERE name = :name";
var param1 = new { name = "Fred" };
var query1 = await client.QueryAsync<Person>(sql1, param1);
query1.Audit(
	fail: e => log.Err(e.Message),
	ok: x => Console.WriteLine("{0} is {1}.", x.First().Name, x.First().Age)
);
// Output: 'Fred is 42.'

// 2: get value as a simple type
var sql2 = "SELECT age FROM foo WHERE name = :name";
var query2 = await client.GetScalarAsync<int>(sql2, param1);
query2.Audit(
	fail: e => log.Err(e.Message),
	ok: x => Console.WriteLine("Fred is {0}.", x)
);
// Output: 'Fred is 42.'

// 3: map results to a complex type
var query3 = await client.QueryAsync<Person>(sql1, param1);
query3.Audit(
	fail: e => log.Err(e.Message),
	ok: x => Console.WriteLine("Found {0}.", x.First())
);
// Output: 'Found Person { Id = 1, Name = Fred, Age = 42 }.'

// 4: insert multiple rows at once using tuples
var param2 = new { name = "Bella", age = 31 };
var param3 = new { name = "Alex", age = 59 };
var query4 = await client.ExecuteAsync(
	(sql0, param2),
	(sql0, param3)
);
query4.Audit(
	fail: e => log.Err(e.Message),
	ok: x => Console.WriteLine("Inserted records {0}.", string.Join(", ", x.Select(r => r.LastInsertId)))
);
// Output: 'Inserted records 2, 3.'

internal sealed record Person(int Id, string Name, int Age);
```

## Licence

> [MIT](https://mit.bfren.dev/2023)

## Copyright

> Copyright (c) 2023-2026 [bfren](https://bfren.dev) (unless otherwise stated)
