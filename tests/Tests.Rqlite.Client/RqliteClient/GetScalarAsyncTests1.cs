// Rqlite: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

namespace Rqlite.Client.RqliteClientTests.GetScalarAsyncTests;

public class with_query : RqliteClientTests
{
	[Fact]
	public async Task passes_query_as_array()
	{
		// Arrange
		var (client, v) = Setup();
		var query = Rnd.Str;
		var expected = Json(new[] { query });

		// Act
		_ = await client.GetScalarAsync<int>(query);

		// Assert
		await v.HttpMessageHandler.Received().SendAsync(Arg.Is<HttpRequestMessage>(
			m => expected == m.Content!.ReadAsStringAsync().GetAwaiter().GetResult()
		));
	}
}

public class with_parameterised_query : RqliteClientTests
{
	[Fact]
	public async Task passes_query_and_param_as_array()
	{
		// Arrange
		var (client, v) = Setup();
		var query = Rnd.Str;
		var param = Rnd.Guid;
		var expected = Json(new[] { new object[] { query, param } });

		// Act
		_ = await client.GetScalarAsync<int>(query, param);

		// Assert
		await v.HttpMessageHandler.Received().SendAsync(Arg.Is<HttpRequestMessage>(
			m => expected == m.Content!.ReadAsStringAsync().GetAwaiter().GetResult()
		));
	}
}
