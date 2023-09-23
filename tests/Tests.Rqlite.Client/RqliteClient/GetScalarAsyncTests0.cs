// Rqlite: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using NSubstitute.ExceptionExtensions;
using Rqlite.Client.Internals;

namespace Rqlite.Client.RqliteClientTests.GetScalarAsyncTests;

public class when_called
{
	[Fact]
	public async Task builds_uri()
	{
		// Arrange
		var builder = Substitute.For<IUriBuilder>();
		var send = Helpers.GetScalarSubstitute<int>();

		// Act
		_ = await RqliteClient.GetScalarAsync(Rnd.Str, builder, send);

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
			var send = Helpers.GetScalarSubstitute<int>();
			var expected = Json(command);

			// Act
			_ = await RqliteClient.GetScalarAsync(command, builder, send);

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
			var send = Helpers.GetScalarSubstitute<int>();

			// Act
			_ = await RqliteClient.GetScalarAsync(Rnd.Str, builder, send);

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
			var send = Helpers.GetScalarSubstitute<int>();

			// Act
			_ = await RqliteClient.GetScalarAsync(Rnd.Str, builder, send);

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
			var expected = Rnd.Int;
			var send = Helpers.GetScalarSubstitute(expected);

			// Act
			var result = await RqliteClient.GetScalarAsync(commands, builder, send);

			// Assert
			var actual = result.AssertOk();
			Assert.Equivalent(expected, actual);
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
			var send = Helpers.GetScalarSubstitute<int>();
			send.Invoke(default!).ThrowsAsyncForAnyArgs(expected);

			// Act
			var result = await RqliteClient.GetScalarAsync(Rnd.Str, builder, send);

			// Assert
			var err = result.AssertErr();
			Assert.Same(expected, err.Exception);
		}
	}
}
