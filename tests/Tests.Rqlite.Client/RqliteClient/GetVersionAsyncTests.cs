// Rqlite: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

namespace Rqlite.Client.RqliteClientTests.GetVersionAsyncTests;

public class when_called : RqliteClientTests
{
	[Fact]
	public async Task requests_status()
	{
		// Arrange
		var (client, v) = Setup();

		// Act
		_ = await client.GetVersionAsync();

		// Assert
		await v.HttpMessageHandler.Received().SendAsync(Arg.Is<HttpRequestMessage>(m =>
			m.Method == HttpMethod.Get
			&& m.RequestUri!.PathAndQuery.Contains("/status")
		));
	}
}

public class when_response_returns_no_version_header : RqliteClientTests
{
	[Fact]
	public async Task returns_error_string()
	{
		// Arrange
		var (client, _) = Setup();
		var expected = "Unable to retrieve version - is the Rqlite instance running?";

		// Act
		var result = await client.GetVersionAsync();

		// Assert
		Assert.Equal(expected, result);
	}
}

public class when_response_returns_single_version_header : RqliteClientTests
{
	[Fact]
	public async Task returns_version_string()
	{
		// Arrange
		var (client, v) = Setup();
		var version = Rnd.Str;
		var response = new HttpResponseMessage();
		response.Headers.Add("X-Rqlite-Version", version);
		v.HttpMessageHandler.SendAsync(default!).ReturnsForAnyArgs(response);

		// Act
		var result = await client.GetVersionAsync();

		// Assert
		Assert.Equal(version, result);
	}
}
