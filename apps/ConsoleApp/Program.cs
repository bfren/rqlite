// Rqlite: Test Apps
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using Microsoft.Extensions.DependencyInjection;
using RndF;
using Rqlite.Client;

var (app, log) = Jeebs.Apps.Host.Create(args, (ctx, services) => services.AddRqlite());

var factory = app.Services.GetRequiredService<IRqliteClientFactory>();
using var client0 = factory.CreateClientWithDefaults();
using var client1 = factory.CreateClient();
using var client2 = factory.CreateClient("localhost1");

var version = await client1.GetVersionAsync();
log.Inf("Version: {Version}", version);

Console.WriteLine();
log.Inf("0 - using client defaults");
var createTableResult = await client0.ExecuteAsync("CREATE TABLE foo (id INTEGER NOT NULL PRIMARY KEY, name TEXT, age INTEGER)");
log.Dbg("Create table result: {@Result}", createTableResult);

Console.WriteLine();
log.Inf("1 - using client defaults");
var name = Rnd.Str;
var insertRowResult0 = await client0.ExecuteAsync($"INSERT INTO foo(name, age) VALUES('{name}', {Rnd.Int})");
log.Dbg("Insert row result: {@Row}", insertRowResult0);

Console.WriteLine();
log.Inf("2 - without specifiying client name");
var insertRowCommand1 = "INSERT INTO foo(name, age) VALUES(:name, :age)";
var insertRowResult1 = await client1.ExecuteAsync(true,
	(insertRowCommand1, new { name = Rnd.Str, age = Rnd.Int }),
	(insertRowCommand1, new { name = Rnd.Str, age = Rnd.Int })
);
log.Dbg("Insert row result: {@Row}", insertRowResult1);

Console.WriteLine();
log.Inf("3 - without specifiying client name");
var queryResult0 = await client1.QueryAsync($"SELECT * FROM foo WHERE age > {Rnd.Int}");
log.Dbg("Query result: {@Row}", queryResult0);

Console.WriteLine();
log.Inf("4 - without specifiying client name");
var queryResult1 = await client1.QueryAsync<ConsoleApp.Person>($"SELECT * FROM foo WHERE age > {Rnd.Int}");
log.Dbg("Query result: {@Row}", queryResult1);

Console.WriteLine();
log.Inf("5 - with client name");
var queryResult2 = await client2.QueryAsync("SELECT * FROM foo WHERE age > :age", new { age = Rnd.Int });
log.Dbg("Query result: {@Row}", queryResult2);

Console.WriteLine();
log.Inf("6 - with client name");
var query3 = "SELECT * FROM foo WHERE age > :age";
var queryResult3 = await client2.QueryAsync<ConsoleApp.Person>(
	(query3, new { age = Rnd.Int }),
	(query3, new { age = Rnd.Int })
);
log.Dbg("Query 0 result: {@Row}", queryResult3.Results[0].Rows!);
log.Dbg("Query 1 result: {@Row}", queryResult3.Results[1].Rows!);
log.Dbg("Flattened query result: {@Row}", queryResult3.Flatten());

Console.WriteLine();
log.Inf("7 - with client name");
var query4 = "SELECT * FROM foo WHERE name = :name";
var queryResult4 = await client2.QueryAsync(query4, new { name });
log.Dbg($"{name} is {{Age}}", queryResult4.Results[0].Values[0].First().GetInt32());

Console.ReadLine();

namespace ConsoleApp
{
	public readonly record struct Person(int Id, string Name, int Age);
}
