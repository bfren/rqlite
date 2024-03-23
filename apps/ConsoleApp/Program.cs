// Rqlite: Test Apps
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using Microsoft.Extensions.DependencyInjection;
using RndF;
using Rqlite.Client;
using Wrap;

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
createTableResult.Audit(
	fail: e => log.Err(e.Message),
	ok: x => log.Dbg("Create table result: {@Result}", x)
);

Console.WriteLine();
log.Inf("1 - using client defaults");
var name = Rnd.Str;
var insertRowResult0 = await client0.ExecuteAsync($"INSERT INTO foo(name, age) VALUES('{name}', {Rnd.Int})");
insertRowResult0.Audit(
	fail: e => log.Err(e.Message),
	ok: x => log.Dbg("Insert row result: {@Row}", x)
);

Console.WriteLine();
log.Inf("2 - without specifiying client name");
var insertRowCommand1 = "INSERT INTO foo(name, age) VALUES(:name, :age)";
var insertRowResult1 = await client1.ExecuteAsync(true,
	(insertRowCommand1, new { name = Rnd.Str, age = Rnd.Int }),
	(insertRowCommand1, new { name = Rnd.Str, age = Rnd.Int })
);
insertRowResult1.Audit(
	fail: e => log.Err(e.Message),
	ok: x => log.Dbg("Insert row result: {@Row}", x)
);

Console.WriteLine();
log.Inf("3 - without specifiying client name");
var queryResult0 = await client1.QueryAsync<Person>($"SELECT * FROM foo WHERE age > {Rnd.Int}");
queryResult0.Audit(
	fail: e => log.Err(e.Message),
	ok: x => log.Dbg("Query result: {@List}", x)
);

Console.WriteLine();
log.Inf("4 - without specifiying client name");
var queryResult1 = await client1.QueryAsync<Person>($"SELECT * FROM foo WHERE age > {Rnd.Int}");
queryResult1.Audit(
	fail: e => log.Err(e.Message),
	ok: x => log.Dbg("Query result: {@List}", x)
);

Console.WriteLine();
log.Inf("5 - with client name");
var queryResult2 = await client2.QueryAsync<Person>("SELECT * FROM foo WHERE age > :age", new { age = Rnd.Int });
queryResult2.Audit(
	fail: e => log.Err(e.Message),
	ok: x => log.Dbg("Query result: {@List}", x)
);

Console.WriteLine();
log.Inf("6 - with client name");
var query3 = "SELECT * FROM foo WHERE age > :age";
var queryResult3 = await client2.QueryAsync<Person>(
	(query3, new { age = Rnd.Int }),
	(query3, new { age = Rnd.Int })
);
queryResult3.Audit(
	fail: e => log.Err(e.Message),
	ok: x =>
	{
		log.Dbg("Query 0 result: {@Row}", x[0]);
		log.Dbg("Query 1 result: {@Row}", x[1]);
	}
);

Console.WriteLine();
log.Inf("7 - with client name");
var query4 = "SELECT * FROM foo WHERE name = :name";
var queryResult4 = await client2.QueryAsync<Person>(query4, new { name });
queryResult4.Audit(
	fail: e => log.Err(e.Message),
	ok: x => log.Dbg($"{name} is {{Age}}", x[0].Age)
);

Console.WriteLine();
log.Inf("8 - with client name");
var queryResult5 = await client2.QueryAsync<Person>(query4, new { name });
queryResult5.Audit(
	fail: e => log.Err(e.Message),
	ok: x => log.Dbg($"{name} is {{Age}}", x[0].Age)
);

Console.WriteLine();
log.Inf("9 - as scalar");
var scalarResult0 = await client2.GetScalarAsync<int>("SELECT age FROM foo WHERE name = :name", new { name });
scalarResult0.Audit(
	fail: e => log.Err(e.Message),
	ok: x => log.Dbg($"{name} is {{Age}}", x)
);

Console.ReadLine();

internal sealed record class Person(int Id, string Name, int Age);
