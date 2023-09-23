// Rqlite: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

namespace Rqlite.Client.RqliteClientTests.GetVersionAsyncTests;

public class when_called : RqliteClientTests
{
	[Fact]
	public async Task requests_status()
	{
		// Arrange
		var (client, v) = Setup(content: Helpers.GetStatusHttpContent());

		// Act
		_ = await client.GetVersionAsync();

		// Assert
		await v.HttpMessageHandler.Received().SendAsync(Arg.Is<HttpRequestMessage>(m =>
			m.Method == HttpMethod.Get
			&& m.RequestUri!.PathAndQuery.Contains("/status")
		));
	}
}

public class when_status_returns_build_version : RqliteClientTests
{
	[Fact]
	public async Task returns_version_string()
	{
		// Arrange
		var version = Rnd.Str;
		var (client, _) = Setup(content: Helpers.GetStatusHttpContent(version));

		// Act
		var result = await client.GetVersionAsync();

		// Assert
		Assert.Equal(version, result);
	}
}
