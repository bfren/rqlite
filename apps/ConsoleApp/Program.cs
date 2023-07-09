// Rqlite: Test Apps
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using ConsoleApp.Types;
using Microsoft.Extensions.DependencyInjection;
using RndF;
using Rqlite.Client;

var (app, log) = Jeebs.Apps.Host.Create(args, (ctx, services) => services.AddRqlite());

var factory = app.Services.GetRequiredService<IRqliteClientFactory>();
using var client = factory.CreateClient();

var version = await client.GetVersionAsync();
log.Inf("Version: {Version}", version);

var createTableResult = await client.ExecuteAsync("CREATE TABLE foo (id INTEGER NOT NULL PRIMARY KEY, name TEXT, age INTEGER)");
log.Inf("Create table result: {@Result}", createTableResult);

var insertRowResult0 = await client.ExecuteAsync($"INSERT INTO foo(name, age) VALUES('{Rnd.Str}', {Rnd.Int})");
log.Inf("Insert row result: {@Row}", insertRowResult0);

var insertRowCommand1 = "INSERT INTO foo(name, age) VALUES(:name, :age)";
var insertRowResult1 = await client.ExecuteAsync(
	(insertRowCommand1, new { name = Rnd.Str, age = Rnd.Int }),
	(insertRowCommand1, new { name = Rnd.Str, age = Rnd.Int })
);
log.Inf("Insert row result: {@Row}", insertRowResult1);

var queryResult0 = await client.QueryAsync($"SELECT * FROM foo WHERE age > {Rnd.Int}");
log.Inf("Query result: {@Row}", queryResult0);

var queryResult1 = await client.QueryAsync<Person>($"SELECT * FROM foo WHERE age > {Rnd.Int}");
log.Inf("Query result: {@Row}", queryResult1);

var queryResult2 = await client.QueryAsync($"SELECT * FROM foo WHERE age > :age", new { age = Rnd.Int });
log.Inf("Query result: {@Row}", queryResult2);

var queryResult3 = await client.QueryAsync<Person>($"SELECT * FROM foo WHERE age > :age", new { age = Rnd.Int });
log.Inf("Query result: {@Row}", queryResult3);

Console.ReadLine();

namespace ConsoleApp.Types
{
	public readonly record struct Person(int Id, string Name, int Age);
}
