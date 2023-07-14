// Rqlite: Test Apps
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using Microsoft.Extensions.DependencyInjection;
using Rqlite.Client;

// register Rqlite with dependency injection
var (app, log) = Jeebs.Apps.Host.Create(args, (ctx, services) => services.AddRqlite());

// get IRqliteClientFactory instance
var factory = app.Services.GetRequiredService<IRqliteClientFactory>();

// create default IRqliteClient, listening on http://localhost:4001
using var client = factory.CreateClient();

// create a table
await client.ExecuteAsync("DROP TABLE foo");
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
