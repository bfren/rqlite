// Rqlite: Test Apps
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using Microsoft.Extensions.DependencyInjection;
using RndF;
using Rqlite.Client;

var (app, log) = Jeebs.Apps.Host.Create(args, (ctx, services) => services.AddRqlite());

var factory = app.Services.GetRequiredService<IRqliteClientFactory>();
using var client0 = factory.CreateClient();
using var client1 = factory.CreateClient("localhost1");

var version = await client0.GetVersionAsync();
log.Inf("Version: {Version}", version);

log.Inf("0");
var createTableResult = await client0.ExecuteAsync("CREATE TABLE foo (id INTEGER NOT NULL PRIMARY KEY, name TEXT, age INTEGER)");
log.Dbg("Create table result: {@Result}", createTableResult);

log.Inf("1");
var insertRowResult0 = await client0.ExecuteAsync($"INSERT INTO foo(name, age) VALUES('{Rnd.Str}', {Rnd.Int})");
log.Dbg("Insert row result: {@Row}", insertRowResult0);

log.Inf("2");
var insertRowCommand1 = "INSERT INTO foo(name, age) VALUES(:name, :age)";
var insertRowResult1 = await client0.ExecuteAsync(true,
	(insertRowCommand1, new { name = Rnd.Str, age = Rnd.Int }),
	(insertRowCommand1, new { name = Rnd.Str, age = Rnd.Int })
);
log.Dbg("Insert row result: {@Row}", insertRowResult1);

log.Inf("3");
var queryResult0 = await client0.QueryAsync($"SELECT * FROM foo WHERE age > {Rnd.Int}");
log.Dbg("Query result: {@Row}", queryResult0);

log.Inf("4");
var queryResult1 = await client0.QueryAsync<ConsoleApp.Person>($"SELECT * FROM foo WHERE age > {Rnd.Int}");
log.Dbg("Query result: {@Row}", queryResult1);

log.Inf("5");
var queryResult2 = await client0.QueryAsync("SELECT * FROM foo WHERE age > :age", new { age = Rnd.Int });
log.Dbg("Query result: {@Row}", queryResult2);

log.Inf("6");
var query3 = "SELECT * FROM foo WHERE age > :age";
var queryResult3 = await client1.QueryAsync<ConsoleApp.Person>(
	(query3, new { age = Rnd.Int }),
	(query3, new { age = Rnd.Int })
);
log.Dbg("Query 0 result: {@Row}", queryResult3.Results[0].Rows!);
log.Dbg("Query 1 result: {@Row}", queryResult3.Results[1].Rows!);
log.Dbg("Flattened query result: {@Row}", queryResult3.Flatten());

Console.ReadLine();

namespace ConsoleApp
{
	public readonly record struct Person(int Id, string Name, int Age);
}
