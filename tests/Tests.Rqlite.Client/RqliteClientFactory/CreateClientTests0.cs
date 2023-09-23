// Rqlite: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using Rqlite.Client.Exceptions;

namespace Rqlite.Client.RqliteClientFactoryTests.CreateClientTests;

/// <see cref="RqliteClientFactory.CreateClient()"/>

public class without_DefaultClientName
{
	public class without_Clients : RqliteClientFactoryTests
	{
		[Fact]
		public void returns_client_with_defaults()
		{
			// Arrange
			var (factory, v) = Setup();

			// Act
			var result = factory.CreateClient();

			// Assert
			var client = Assert.IsType<RqliteClient>(result);
			Assert.Equal($"{v.ClientOptions.BaseAddress}/", client.HttpClient.BaseAddress!.AbsoluteUri);
			Assert.Equal(TimeSpan.FromSeconds(v.Options.TimeoutInSeconds), client.HttpClient.Timeout);
		}
	}

	public class with_Clients : RqliteClientFactoryTests
	{
		public Dictionary<string, RqliteOptions.Client> Clients { get; set; }

		public with_Clients()
		{
			Clients = new()
			{
				{ Rnd.Str, new() },
				{ Rnd.Str, new() }
			};
		}

		[Fact]
		public void throws_UndefinedDefaultClientException()
		{
			// Arrange
			var (factory, _) = Setup(new() { Clients = Clients });

			// Act
			IRqliteClient act() => factory.CreateClient();

			// Assert
			Assert.Throws<UndefinedDefaultClientException>(act);
		}
	}
}

public class with_DefaultClientName
{
	public class without_Clients : RqliteClientFactoryTests
	{
		[Fact]
		public void returns_client_with_defaults()
		{
			// Arrange
			var (factory, v) = Setup(new() { DefaultClientName = Rnd.Str });

			// Act
			var result = factory.CreateClient();

			// Assert
			var client = Assert.IsType<RqliteClient>(result);
			Assert.Equal($"{v.ClientOptions.BaseAddress}/", client.HttpClient.BaseAddress!.AbsoluteUri);
			Assert.Equal(TimeSpan.FromSeconds(v.Options.TimeoutInSeconds), client.HttpClient.Timeout);
		}
	}

	public class with_Clients : RqliteClientFactoryTests
	{
		public string ClientName { get; set; }

		public Dictionary<string, RqliteOptions.Client> Clients { get; set; }

		public with_Clients()
		{
			ClientName = Rnd.Str;
			Clients = new()
			{
				{ ClientName, new() }
			};
		}

		[Fact]
		public void uses_named_HttpClient()
		{
			// Arrange
			var (factory, v) = Setup(new() { DefaultClientName = ClientName, Clients = Clients });

			// Act
			_ = factory.CreateClient();

			// Assert
			v.HttpClientFactory.Received().CreateClient(ClientName);
		}
	}
}
