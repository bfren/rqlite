// Rqlite: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

namespace Rqlite.Client.RqliteClientTests.GetStatusAsyncTests;

public class when_called : RqliteClientTests
{
	[Fact]
	public async Task requests_status()
	{
		// Arrange
		var (client, v) = Setup();

		// Act
		_ = await client.GetStatusAsync();

		// Assert
		await v.HttpMessageHandler.Received().SendAsync(Arg.Is<HttpRequestMessage>(m =>
			m.Method == HttpMethod.Get
			&& m.RequestUri!.PathAndQuery.Contains("/status")
		));
	}
}
