// Rqlite: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using NSubstitute.ExceptionExtensions;
using Rqlite.Client.Internals;
using Rqlite.Client.Response;

namespace Rqlite.Client.RqliteClientTests.ExecuteAsyncTests;

public class when_commands_is_empty
{
	[Fact]
	public async Task returns_response_with_message()
	{
		// Arrange
		var commands = Array.Empty<string>();
		var builder = Substitute.For<IUriBuilder>();
		var send = Substitute.For<Func<HttpRequestMessage, Task<RqliteExecuteResponse>>>();
		var expected = "You must pass at least one command.";

		// Act
		var result = await RqliteClient.ExecuteAsync(commands, Rnd.Flip, builder, send);

		// Assert
		Assert.Collection(result.Errors,
			x => Assert.Equal(expected, x.Value)
		);
	}
}

public class when_commands_is_not_empty
{
	[Fact]
	public async Task builds_uri()
	{
		// Arrange
		var commands = new[] { Rnd.Str, Rnd.Str };
		var builder = Substitute.For<IUriBuilder>();
		var send = Substitute.For<Func<HttpRequestMessage, Task<RqliteExecuteResponse>>>();

		// Act
		_ = await RqliteClient.ExecuteAsync(commands, Rnd.Flip, builder, send);

		// Assert
		builder.Received().Build();
	}

	public class sends_request : RqliteClientTests
	{
		[Fact]
		public async Task with_content_serialised_as_json()
		{
			// Arrange
			var commands = new[] { Rnd.Str, Rnd.Str };
			var builder = Substitute.For<IUriBuilder>();
			var send = Substitute.For<Func<HttpRequestMessage, Task<RqliteExecuteResponse>>>();
			var expected = Json(commands);

			// Act
			_ = await RqliteClient.ExecuteAsync(commands, Rnd.Flip, builder, send);

			// Assert
			await send.Received().Invoke(Arg.Is<HttpRequestMessage>(m =>
				expected == Assert.IsType<JsonContent>(m.Content).ReadAsStringAsync().GetAwaiter().GetResult()
			));
		}

		[Fact]
		public async Task as_http_post()
		{
			// Arrange
			var commands = new[] { Rnd.Str, Rnd.Str };
			var builder = Substitute.For<IUriBuilder>();
			var send = Substitute.For<Func<HttpRequestMessage, Task<RqliteExecuteResponse>>>();

			// Act
			_ = await RqliteClient.ExecuteAsync(commands, Rnd.Flip, builder, send);

			// Assert
			await send.Received().Invoke(Arg.Is<HttpRequestMessage>(m =>
				HttpMethod.Post == m.Method
			));
		}

		[Fact]
		public async Task with_built_uri()
		{
			// Arrange
			var commands = new[] { Rnd.Str, Rnd.Str };
			var uri = $"https://{Rnd.Str}.com/{Rnd.Str}".ToLowerInvariant();
			var builder = Substitute.For<IUriBuilder>();
			builder.Build().Returns(new Uri(uri));
			var send = Substitute.For<Func<HttpRequestMessage, Task<RqliteExecuteResponse>>>();

			// Act
			_ = await RqliteClient.ExecuteAsync(commands, Rnd.Flip, builder, send);

			// Assert
			await send.Received().Invoke(Arg.Is<HttpRequestMessage>(m =>
				uri == m.RequestUri!.AbsoluteUri
			));
		}

		[Fact]
		public async Task returns_correct_RqliteExecuteResponse()
		{
			// Arrange
			var commands = new[] { Rnd.Str, Rnd.Str };
			var builder = Substitute.For<IUriBuilder>();
			var response = new RqliteExecuteResponse();
			var send = Substitute.For<Func<HttpRequestMessage, Task<RqliteExecuteResponse>>>();
			send.Invoke(default!).ReturnsForAnyArgs(response);

			// Act
			var result = await RqliteClient.ExecuteAsync(commands, Rnd.Flip, builder, send);

			// Assert
			Assert.Same(response, result);
		}
	}

	public class and_send_throws_exception
	{
		[Fact]
		public async Task returns_RqliteExecuteResponse_with_error()
		{
			// Arrange
			var commands = new[] { Rnd.Str, Rnd.Str };
			var builder = Substitute.For<IUriBuilder>();
			var expected = new ArgumentNullException(Rnd.Str);
			var send = Substitute.For<Func<HttpRequestMessage, Task<RqliteExecuteResponse>>>();
			send.Invoke(default!).ThrowsAsyncForAnyArgs(expected);

			// Act
			var result = await RqliteClient.ExecuteAsync(commands, Rnd.Flip, builder, send);

			// Assert
			Assert.Collection(result.Errors,
				x => Assert.Equal(expected.ToString(), x.Value)
			);
		}
	}
}

public class when_asSingleTransaction_is_true
{
	[Fact]
	public async Task adds_transaction_to_QueryVars()
	{
		// Arrange
		var commands = new[] { Rnd.Str, Rnd.Str };
		var builder = Substitute.For<IUriBuilder>();
		var send = Substitute.For<Func<HttpRequestMessage, Task<RqliteExecuteResponse>>>();

		// Act
		_ = await RqliteClient.ExecuteAsync(commands, true, builder, send);

		// Assert
		builder.Received().AddQueryVar("transaction");
	}
}

public class when_asSingleTransaction_is_false
{
	[Fact]
	public async Task does_not_add_transaction_to_QueryVars()
	{
		// Arrange
		var commands = new[] { Rnd.Str, Rnd.Str };
		var builder = Substitute.For<IUriBuilder>();
		var send = Substitute.For<Func<HttpRequestMessage, Task<RqliteExecuteResponse>>>();

		// Act
		_ = await RqliteClient.ExecuteAsync(commands, false, builder, send);

		// Assert
		builder.DidNotReceive().AddQueryVar("transaction");
	}
}
