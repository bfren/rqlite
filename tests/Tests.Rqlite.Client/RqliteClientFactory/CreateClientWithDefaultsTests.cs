// Rqlite: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

namespace Rqlite.Client.RqliteClientFactoryTests.CreateClientWithDefaultTests;

/// <see cref="RqliteClientFactory.CreateClientWithDefaults"/>

public class when_called : RqliteClientFactoryTests
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
