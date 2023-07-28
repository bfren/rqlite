// Rqlite: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using NSubstitute.ExceptionExtensions;
using Rqlite.Client.Internals;
using Rqlite.Client.Response;

namespace Rqlite.Client.RqliteClientTests.QueryAsyncTests;

public class without_model
{
	public class when_queries_is_empty
	{
		[Fact]
		public async Task returns_response_with_message()
		{
			// Arrange
			var queries = Array.Empty<string>();
			var builder = Substitute.For<IUriBuilder>();
			var send = Substitute.For<Func<HttpRequestMessage, Task<RqliteQueryResponse>>>();
			var expected = "You must pass at least one query.";

			// Act
			var result = await RqliteClient.QueryAsync(queries, builder, send);

			// Assert
			Assert.Collection(result.Errors,
				x => Assert.Equal(expected, x.Value)
			);
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
			var send = Substitute.For<Func<HttpRequestMessage, Task<RqliteQueryResponse>>>();

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
				var send = Substitute.For<Func<HttpRequestMessage, Task<RqliteQueryResponse>>>();
				var expected = Json(queries);

				// Act
				_ = await RqliteClient.QueryAsync(queries, builder, send);

				// Assert
				await send.Received().Invoke(Arg.Is<HttpRequestMessage>(m =>
					expected == Assert.IsType<JsonContent>(m.Content).ReadAsStringAsync().GetAwaiter().GetResult()
				));
			}

			[Fact]
			public async Task as_http_post()
			{
				// Arrange
				var queries = new[] { Rnd.Str, Rnd.Str };
				var builder = Substitute.For<IUriBuilder>();
				var send = Substitute.For<Func<HttpRequestMessage, Task<RqliteQueryResponse>>>();

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
				var send = Substitute.For<Func<HttpRequestMessage, Task<RqliteQueryResponse>>>();

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
				var response = new RqliteQueryResponse();
				var send = Substitute.For<Func<HttpRequestMessage, Task<RqliteQueryResponse>>>();
				send.Invoke(default!).ReturnsForAnyArgs(response);

				// Act
				var result = await RqliteClient.QueryAsync(queries, builder, send);

				// Assert
				Assert.Same(response, result);
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
				var send = Substitute.For<Func<HttpRequestMessage, Task<RqliteQueryResponse>>>();
				send.Invoke(default!).ThrowsAsyncForAnyArgs(expected);

				// Act
				var result = await RqliteClient.QueryAsync(queries, builder, send);

				// Assert
				Assert.Collection(result.Errors,
					x => Assert.Equal(expected.ToString(), x.Value)
				);
			}
		}
	}
}

public class with_model
{
	public class when_queries_is_empty
	{
		[Fact]
		public async Task returns_response_with_message()
		{
			// Arrange
			var queries = Array.Empty<string>();
			var builder = Substitute.For<IUriBuilder>();
			var send = Substitute.For<Func<HttpRequestMessage, Task<RqliteQueryResponse<int>>>>();
			var expected = "You must pass at least one query.";

			// Act
			var result = await RqliteClient.QueryAsync(queries, builder, send);

			// Assert
			Assert.Collection(result.Errors,
				x => Assert.Equal(expected, x.Value)
			);
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
			var send = Substitute.For<Func<HttpRequestMessage, Task<RqliteQueryResponse<int>>>>();

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
				var send = Substitute.For<Func<HttpRequestMessage, Task<RqliteQueryResponse<int>>>>();
				var expected = Json(queries);

				// Act
				_ = await RqliteClient.QueryAsync(queries, builder, send);

				// Assert
				await send.Received().Invoke(Arg.Is<HttpRequestMessage>(m =>
					expected == Assert.IsType<JsonContent>(m.Content).ReadAsStringAsync().GetAwaiter().GetResult()
				));
			}

			[Fact]
			public async Task as_http_post()
			{
				// Arrange
				var queries = new[] { Rnd.Str, Rnd.Str };
				var builder = Substitute.For<IUriBuilder>();
				var send = Substitute.For<Func<HttpRequestMessage, Task<RqliteQueryResponse<int>>>>();

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
				var send = Substitute.For<Func<HttpRequestMessage, Task<RqliteQueryResponse<int>>>>();

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
				var response = new RqliteQueryResponse<int>();
				var send = Substitute.For<Func<HttpRequestMessage, Task<RqliteQueryResponse<int>>>>();
				send.Invoke(default!).ReturnsForAnyArgs(response);

				// Act
				var result = await RqliteClient.QueryAsync(queries, builder, send);

				// Assert
				Assert.Same(response, result);
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
				var send = Substitute.For<Func<HttpRequestMessage, Task<RqliteQueryResponse<int>>>>();
				send.Invoke(default!).ThrowsAsyncForAnyArgs(expected);

				// Act
				var result = await RqliteClient.QueryAsync(queries, builder, send);

				// Assert
				Assert.Collection(result.Errors,
					x => Assert.Equal(expected.ToString(), x.Value)
				);
			}
		}
	}
}
