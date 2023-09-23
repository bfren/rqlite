// Rqlite client for .NET.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Rqlite.Internal.Request;
using Rqlite.Internal.Response;
using Wrap;

namespace Rqlite.Client;

public sealed partial class RqliteClient : IRqliteClient
{
	/// <summary>
	/// Execute multiple commands and return results.
	/// </summary>
	/// <typeparam name="TCommand">Command type.</typeparam>
	/// <param name="commands">Commands to execute.</param>
	/// <param name="asSingleTransaction">If true, commands will be executed together as a single transaction.</param>
	/// <param name="uriBuilder">URI builder.</param>
	/// <param name="send">Asynchronous send method.</param>
	/// <returns>Command results.</returns>
	internal static async Task<Result<List<RqliteCommandResult>>> ExecuteAsync<TCommand>(
		IEnumerable<TCommand> commands,
		bool asSingleTransaction,
		IUriBuilder uriBuilder,
		Func<HttpRequestMessage, Task<Result<List<ExecuteResponseResult>>>> send
	)
	{
		if (!commands.Any())
		{
			return R.Err("You must pass at least one command.");
		}

		if (asSingleTransaction)
		{
			uriBuilder.AddQueryVar("transaction");
		}

		var request = new HttpRequestMessage
		{
			Content = new JsonContent(commands),
			Method = HttpMethod.Post,
			RequestUri = uriBuilder.Build(),
		};

		return await
			send(
				request
			)
			.MapAsync(
				x => x.Select(RqliteCommandResult.Create).ToList()
			);
	}

	/// <inheritdoc/>
	public Task<Result<List<RqliteCommandResult>>> ExecuteAsync(params string[] commands) =>
		ExecuteAsync(false, commands);

	/// <inheritdoc/>
	public Task<Result<List<RqliteCommandResult>>> ExecuteAsync(bool asSingleTransaction, params string[] commands) =>
		ExecuteAsync(
			commands: commands,
			asSingleTransaction: asSingleTransaction,
			uriBuilder: ExecuteUri(),
			send: GetResultsAsync<ExecuteResponseResult>
		);

	/// <inheritdoc/>
	public Task<Result<List<RqliteCommandResult>>> ExecuteAsync(string command, object param) =>
		ExecuteAsync(false, (command, param));

	/// <inheritdoc/>
	public Task<Result<List<RqliteCommandResult>>> ExecuteAsync(params (string command, object param)[] commands) =>
		ExecuteAsync(false, commands);

	/// <inheritdoc/>
	public Task<Result<List<RqliteCommandResult>>> ExecuteAsync(bool asSingleTransaction, params (string command, object param)[] commands) =>
		ExecuteAsync(
			commands: from c in commands select new[] { c.command, c.param },
			asSingleTransaction: asSingleTransaction,
			uriBuilder: ExecuteUri(),
			send: GetResultsAsync<ExecuteResponseResult>
		);
}
