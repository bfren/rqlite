// Rqlite: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using Rqlite.Client.Exceptions;

namespace Rqlite.Client.RqliteClientFactoryTests.CreateClientTests;

/// <see cref="RqliteClientFactory.CreateClient(string)"/>

public class when_Clients_does_not_contain_named_client : RqliteClientFactoryTests
{
	public string ClientName { get; set; }

	public Dictionary<string, RqliteOptions.Client> Clients { get; set; }

	public when_Clients_does_not_contain_named_client()
	{
		ClientName = Rnd.Str;
		Clients = new()
		{
			{ Rnd.Str, new() },
			{ Rnd.Str, new() }
		};
	}

	[Fact]
	public void throws_UnknownClientException()
	{
		// Arrange
		var (factory, _) = Setup(new() { DefaultClientName = ClientName, Clients = Clients });

		// Act
		IRqliteClient act0() => factory.CreateClient();
		IRqliteClient act1() => factory.CreateClient(ClientName);

		// Assert
		Assert.Throws<UnknownClientException>(act0);
		Assert.Throws<UnknownClientException>(act1);
	}

	[Fact]
	public void exception_contains_requested_client_name()
	{
		// Arrange
		var (factory, _) = Setup(new() { Clients = Clients });

		// Act
		IRqliteClient act() => factory.CreateClient(ClientName);

		// Assert
		var ex = Assert.Throws<UnknownClientException>(act);
		Assert.Contains(ClientName, ex.ToString());
	}
}

public class when_Clients_contains_named_client : RqliteClientFactoryTests
{
	public string ClientName { get; set; }

	public Dictionary<string, RqliteOptions.Client> Clients { get; set; }

	public Dictionary<string, RqliteOptions.Client> ClientsWithIncludeTimings { get; set; }

	public when_Clients_contains_named_client()
	{
		ClientName = Rnd.Str;
		Clients = new()
		{
			{ ClientName, new() }
		};
		ClientsWithIncludeTimings = new()
		{
			{ ClientName, new(){ IncludeTimings = false } }
		};
	}

	[Fact]
	public void calls_HttpClientFactory_CreateClient()
	{
		// Arrange
		var (factory, v) = Setup(new() { Clients = Clients });

		// Act
		_ = factory.CreateClient(ClientName);

		// Assert
		v.HttpClientFactory.Received().CreateClient(ClientName);
	}

	[Fact]
	public void uses_global_IncludeTimings()
	{
		// Arrange
		var (factory, _) = Setup(new() { Clients = Clients, IncludeTimings = true });

		// Act
		var result = factory.CreateClient(ClientName);

		// Assert
		var client = Assert.IsType<RqliteClient>(result);
		var single = Assert.Single(client.ExecuteUri().QueryVars.AllKeys);
		Assert.Equal("timings", single);
	}

	[Fact]
	public void client_IncludeTimings_overrides_global_IncludeTimings()
	{
		// Arrange
		var (factory, _) = Setup(new() { Clients = ClientsWithIncludeTimings, IncludeTimings = true });

		// Act
		var result = factory.CreateClient(ClientName);

		// Assert
		var client = Assert.IsType<RqliteClient>(result);
		Assert.Empty(client.ExecuteUri().QueryVars.AllKeys);
	}

	[Fact]
	public void returns_RqliteClient()
	{
		// Arrange
		var (factory, _) = Setup(new() { Clients = Clients });

		// Act
		var result = factory.CreateClient(ClientName);

		// Assert
		Assert.IsType<RqliteClient>(result);
	}
}
