// Rqlite: Test Apps
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using Microsoft.Extensions.DependencyInjection;
using Rqlite.Client;
using Wrap.Extensions;

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
	fFail: e => log.Err(e.Message),
	fOk: x => Console.WriteLine("Inserted record {0}.", x.Select(r => r.LastInsertId).First())
);
// Output: 'Inserted record 1.'

// 1: query the database using parameters
var sql1 = "SELECT * FROM foo WHERE name = :name";
var param1 = new { name = "Fred" };
var query1 = await client.QueryAsync<Person>(sql1, param1);
query1.Audit(
	fFail: e => log.Err(e.Message),
	fOk: x => Console.WriteLine("{0} is {1}.", x.First().Name, x.First().Age)
);
// Output: 'Fred is 42.'

// 2: get value as a simple type
var sql2 = "SELECT age FROM foo WHERE name = :name";
var query2 = await client.GetScalarAsync<int>(sql2, param1);
query2.Audit(
	fFail: e => log.Err(e.Message),
	fOk: x => Console.WriteLine("Fred is {0}.", x)
);
// Output: 'Fred is 42.'

// 3: map results to a complex type
var query3 = await client.QueryAsync<Person>(sql1, param1);
query3.Audit(
	fFail: e => log.Err(e.Message),
	fOk: x => Console.WriteLine("Found {0}.", x.First())
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
	fFail: e => log.Err(e.Message),
	fOk: x => Console.WriteLine("Inserted records {0}.", string.Join(", ", x.Select(r => r.LastInsertId)))
);
// Output: 'Inserted records 2, 3.'

internal sealed record Person(int Id, string Name, int Age);
