// Rqlite: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace Rqlite.Client.RqliteClientTests.SendAsyncTests;

public class when_called : RqliteClientTests
{
	[Fact]
	public async Task logs_request()
	{
		// Arrange
		var (client, v) = Setup();
		var content = Rnd.Str;
		var method = HttpMethod.Delete;
		var uri = $"http://{Rnd.Str.ToLowerInvariant()}.com/";
		var request = new HttpRequestMessage()
		{
			Content = new StringContent(content),
			Method = method,
			RequestUri = new(uri)
		};
		var message = $"{method} {uri}: {content}";

		// Act
		_ = await client.SendAsync<Response>(request);

		// Assert
		v.Logger.Received().Log(LogLevel.Debug, message);
	}

	[Fact]
	public async Task sends_request()
	{
		// Arrange
		var (client, v) = Setup();
		var request = new HttpRequestMessage();

		// Act
		_ = await client.SendAsync<Response>(request);

		// Assert
		await v.HttpMessageHandler.Received().SendAsync(request);
	}

	[Fact]
	public async Task logs_response_json()
	{
		// Arrange
		var (client, v) = Setup();
		var message = $"Response JSON: {JsonSerializer.Serialize(v.HttpMessageHandler.Value)}";

		// Act
		_ = await client.SendAsync<Response>(new());

		// Assert
		v.Logger.Received().Log(LogLevel.Debug, message);
	}

	[Fact]
	public async Task returns_deserialised_value()
	{
		// Arrange
		var (client, v) = Setup();

		// Act
		var result = await client.SendAsync<Response>(new());

		// Assert
		Assert.Equal(v.HttpMessageHandler.Value.Foo, result.Foo);
		Assert.Equal(v.HttpMessageHandler.Value.Bar, result.Bar);
	}
}

public class when_request_is_not_successful : RqliteClientTests
{
	[Fact]
	public async Task throws_HttpRequestException()
	{
		// Arrange
		var (client, v) = Setup(status: System.Net.HttpStatusCode.InternalServerError);

		// Act
		async Task act() => await client.SendAsync<Response>(new());

		// Assert
		await Assert.ThrowsAsync<HttpRequestException>(act);
	}
}

public class when_request_returns_invalid_json : RqliteClientTests
{
	[Fact]
	public async Task throws_JsonException()
	{
		// Arrange
		var (client, v) = Setup(content: new StringContent("{This is invalid JSON.}"));

		// Act
		async Task act() => await client.SendAsync<Response>(new());

		// Assert
		await Assert.ThrowsAsync<JsonException>(act);
	}
}

public class when_request_returns_null_json : RqliteClientTests
{
	[Fact]
	public async Task throws_JsonException()
	{
		// Arrange
		var (client, v) = Setup(content: new StringContent(string.Empty));

		// Act
		async Task act() => await client.SendAsync<Response>(new());

		// Assert
		await Assert.ThrowsAsync<JsonException>(act);
	}
}
