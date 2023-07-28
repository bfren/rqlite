// Rqlite: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using NSubstitute.ExceptionExtensions;
using Rqlite.Client.Internals;
using Rqlite.Client.Response;

namespace Rqlite.Client.RqliteClientTests.ScalarAsyncTests;

public class when_called
{
	[Fact]
	public async Task builds_uri()
	{
		// Arrange
		var builder = Substitute.For<IUriBuilder>();
		var send = Substitute.For<Func<HttpRequestMessage, Task<RqliteScalarResponse<int>>>>();

		// Act
		_ = await RqliteClient.ScalarAsync(Rnd.Str, builder, send);

		// Assert
		builder.Received().Build();
	}

	public class sends_request : RqliteClientTests
	{
		[Fact]
		public async Task with_content_serialised_as_json()
		{
			// Arrange
			var command = Rnd.Str;
			var builder = Substitute.For<IUriBuilder>();
			var send = Substitute.For<Func<HttpRequestMessage, Task<RqliteScalarResponse<int>>>>();
			var expected = Json(command);

			// Act
			_ = await RqliteClient.ScalarAsync(command, builder, send);

			// Assert
			await send.Received().Invoke(Arg.Is<HttpRequestMessage>(m =>
				expected == Assert.IsType<JsonContent>(m.Content).ReadAsStringAsync().GetAwaiter().GetResult()
			));
		}

		[Fact]
		public async Task as_http_post()
		{
			// Arrange
			var builder = Substitute.For<IUriBuilder>();
			var send = Substitute.For<Func<HttpRequestMessage, Task<RqliteScalarResponse<int>>>>();

			// Act
			_ = await RqliteClient.ScalarAsync(Rnd.Str, builder, send);

			// Assert
			await send.Received().Invoke(Arg.Is<HttpRequestMessage>(m =>
				HttpMethod.Post == m.Method
			));
		}

		[Fact]
		public async Task with_built_uri()
		{
			// Arrange
			var uri = $"https://{Rnd.Str}.com/{Rnd.Str}".ToLowerInvariant();
			var builder = Substitute.For<IUriBuilder>();
			builder.Build().Returns(new Uri(uri));
			var send = Substitute.For<Func<HttpRequestMessage, Task<RqliteScalarResponse<int>>>>();

			// Act
			_ = await RqliteClient.ScalarAsync(Rnd.Str, builder, send);

			// Assert
			await send.Received().Invoke(Arg.Is<HttpRequestMessage>(m =>
				uri == m.RequestUri!.AbsoluteUri
			));
		}

		[Fact]
		public async Task returns_correct_RqliteScalarResponse()
		{
			// Arrange
			var commands = new[] { Rnd.Str, Rnd.Str };
			var builder = Substitute.For<IUriBuilder>();
			var response = new RqliteScalarResponse<int>();
			var send = Substitute.For<Func<HttpRequestMessage, Task<RqliteScalarResponse<int>>>>();
			send.Invoke(default!).ReturnsForAnyArgs(response);

			// Act
			var result = await RqliteClient.ScalarAsync(commands, builder, send);

			// Assert
			Assert.Same(response, result);
		}
	}

	public class and_send_throws_exception
	{
		[Fact]
		public async Task returns_RqliteScalarResponse_with_error()
		{
			// Arrange
			var builder = Substitute.For<IUriBuilder>();
			var expected = new ArgumentNullException(Rnd.Str);
			var send = Substitute.For<Func<HttpRequestMessage, Task<RqliteScalarResponse<int>>>>();
			send.Invoke(default!).ThrowsAsyncForAnyArgs(expected);

			// Act
			var result = await RqliteClient.ScalarAsync(Rnd.Str, builder, send);

			// Assert
			Assert.Collection(result.Errors,
				x => Assert.Equal(expected.ToString(), x.Value)
			);
		}
	}
}
