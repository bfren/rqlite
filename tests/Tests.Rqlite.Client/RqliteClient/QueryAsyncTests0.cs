// Rqlite: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using NSubstitute.ExceptionExtensions;
using Rqlite.Internal.Request;
using Rqlite.Internal.Response;

namespace Rqlite.Client.RqliteClientTests.QueryAsyncTests;

public class when_queries_is_empty
{
	[Fact]
	public async Task returns_response_with_message()
	{
		// Arrange
		var queries = Array.Empty<string>();
		var builder = Substitute.For<IUriBuilder>();
		var send = Substitute.For<Func<HttpRequestMessage, Task<Result<List<QueryResponseResult<int>>>>>>();
		var expected = "You must pass at least one query.";

		// Act
		var result = await RqliteClient.QueryAsync(queries, builder, send);

		// Assert
		var err = result.AssertFail();
		Assert.Equal(expected, err.Message);
	}
}

public class when_queries_is_not_empty
{
	[Fact]
	public async Task builds_uri()
	{
		// Arrange
		var queries = new[] { Rnd.Str, Rnd.Str };
		var builder = Substitute.For<IUriBuilder>();
		var send = Substitute.For<Func<HttpRequestMessage, Task<Result<List<QueryResponseResult<int>>>>>>();

		// Act
		_ = await RqliteClient.QueryAsync(queries, builder, send);

		// Assert
		builder.Received().Build();
	}

	public class sends_request : RqliteClientTests
	{
		[Fact]
		public async Task with_content_serialised_as_json()
		{
			// Arrange
			var queries = new[] { Rnd.Str, Rnd.Str };
			var builder = Substitute.For<IUriBuilder>();
			var send = Helpers.GetQuerySubstitute<int>();
			var expected = Json(queries);

			// Act
			_ = await RqliteClient.QueryAsync(queries, builder, send);

			// Assert
			await send.Received().Invoke(Arg.Is<HttpRequestMessage>(m =>
				expected == Helpers.ReadContent(m)
			));
		}

		[Fact]
		public async Task as_http_post()
		{
			// Arrange
			var queries = new[] { Rnd.Str, Rnd.Str };
			var builder = Substitute.For<IUriBuilder>();
			var send = Helpers.GetQuerySubstitute<int>();

			// Act
			_ = await RqliteClient.QueryAsync(queries, builder, send);

			// Assert
			await send.Received().Invoke(Arg.Is<HttpRequestMessage>(m =>
				HttpMethod.Post == m.Method
			));
		}

		[Fact]
		public async Task with_built_uri()
		{
			// Arrange
			var queries = new[] { Rnd.Str, Rnd.Str };
			var uri = $"https://{Rnd.Str}.com/{Rnd.Str}".ToLowerInvariant();
			var builder = Substitute.For<IUriBuilder>();
			builder.Build().Returns(new Uri(uri));
			var send = Helpers.GetQuerySubstitute<int>();

			// Act
			_ = await RqliteClient.QueryAsync(queries, builder, send);

			// Assert
			await send.Received().Invoke(Arg.Is<HttpRequestMessage>(m =>
				uri == m.RequestUri!.AbsoluteUri
			));
		}

		[Fact]
		public async Task returns_correct_RqliteQueryResponse()
		{
			// Arrange
			var queries = new[] { Rnd.Str, Rnd.Str };
			var builder = Substitute.For<IUriBuilder>();
			var expected = new[] { Rnd.Int, Rnd.Int };
			var send = Helpers.GetQuerySubstitute(expected);

			// Act
			var result = await RqliteClient.QueryAsync(queries, builder, send);

			// Assert
			var actual = result.AssertOk();
			Assert.Equal(expected, actual);
		}
	}

	public class and_send_throws_exception
	{
		[Fact]
		public async Task returns_RqliteQueryResponse_with_error()
		{
			// Arrange
			var queries = new[] { Rnd.Str, Rnd.Str };
			var builder = Substitute.For<IUriBuilder>();
			var expected = new ArgumentNullException(Rnd.Str);
			var send = Helpers.GetQuerySubstitute<int>();
			send.Invoke(default!).ThrowsAsyncForAnyArgs(expected);

			// Act
			var result = await RqliteClient.QueryAsync(queries, builder, send);

			// Assert
			var err = result.AssertFail();
			Assert.Same(expected, err.Exception);
		}
	}
}
