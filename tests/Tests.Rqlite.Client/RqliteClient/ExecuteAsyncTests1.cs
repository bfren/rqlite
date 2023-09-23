// Rqlite: Unit Tests
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

namespace Rqlite.Client.RqliteClientTests.ExecuteAsyncTests;

public class with_commands_as_string_params : RqliteClientTests
{
	[Fact]
	public async Task passes_commands_without_modification()
	{
		// Arrange
		var (client, v) = Setup();
		var commands = new[] { Rnd.Str, Rnd.Str };
		var expected = Json(commands);

		// Act
		_ = await client.ExecuteAsync(commands);

		// Assert
		await v.HttpMessageHandler.Received().SendAsync(Arg.Is<HttpRequestMessage>(
			m => expected == Helpers.ReadContent(m)
		));
	}

	[Fact]
	public async Task does_not_add_transaction_to_QueryVars()
	{
		// Arrange
		var (client, v) = Setup();
		var commands = new[] { Rnd.Str, Rnd.Str };

		// Act
		_ = await client.ExecuteAsync(commands);

		// Assert
		await v.HttpMessageHandler.Received().SendAsync(Arg.Is<HttpRequestMessage>(
			m => false == m.RequestUri!.OriginalString.Contains("transaction")
		));
	}

	public class and_asSingleTransaction
	{
		[Fact]
		public async Task passes_asSingleTransaction_value()
		{
			// Arrange
			var (client, v) = Setup();
			var asTransaction = Rnd.Flip;
			var commands = new[] { Rnd.Str, Rnd.Str };

			// Act
			_ = await client.ExecuteAsync(asTransaction, commands);

			// Assert
			await v.HttpMessageHandler.Received().SendAsync(Arg.Is<HttpRequestMessage>(
				m => asTransaction == m.RequestUri!.OriginalString.Contains("transaction")
			));
		}
	}
}

public class with_parameterised_command : RqliteClientTests
{
	[Fact]
	public async Task passes_command_as_object_array()
	{
		// Arrange
		var (client, v) = Setup();
		var command = Rnd.Str;
		var param = Rnd.Guid;
		var expected = Json(new[] { new object[] { command, param } });

		// Act
		_ = await client.ExecuteAsync(command, param);

		// Assert
		await v.HttpMessageHandler.Received().SendAsync(Arg.Is<HttpRequestMessage>(
			m => expected == Helpers.ReadContent(m)
		));
	}
}

public class with_parameterised_commands_as_tuple_params : RqliteClientTests
{
	[Fact]
	public async Task passes_commands_as_object_array()
	{
		// Arrange
		var (client, v) = Setup();
		var c0 = Rnd.Str;
		var v0 = Rnd.Guid;
		var c1 = Rnd.Str;
		var v1 = Rnd.DateTime;
		var expected = Json(new[] { new object[] { c0, v0 }, new object[] { c1, v1 } });

		// Act
		_ = await client.ExecuteAsync((c0, v0), (c1, v1));

		// Assert
		await v.HttpMessageHandler.Received().SendAsync(Arg.Is<HttpRequestMessage>(
			m => expected == Helpers.ReadContent(m)
		));
	}

	[Fact]
	public async Task does_not_add_transaction_to_QueryVars()
	{
		// Arrange
		var (client, v) = Setup();

		// Act
		_ = await client.ExecuteAsync((Rnd.Str, Rnd.Guid), (Rnd.Str, Rnd.Dbl));

		// Assert
		await v.HttpMessageHandler.Received().SendAsync(Arg.Is<HttpRequestMessage>(
			m => false == m.RequestUri!.OriginalString.Contains("transaction")
		));
	}

	public class and_asSingleTransaction
	{
		[Fact]
		public async Task passes_asSingleTransaction_value()
		{
			// Arrange
			var (client, v) = Setup();
			var asTransaction = Rnd.Flip;

			// Act
			_ = await client.ExecuteAsync(asTransaction, (Rnd.Str, Rnd.Guid), (Rnd.Str, Rnd.Dbl));

			// Assert
			await v.HttpMessageHandler.Received().SendAsync(Arg.Is<HttpRequestMessage>(
				m => asTransaction == m.RequestUri!.OriginalString.Contains("transaction")
			));
		}
	}
}
