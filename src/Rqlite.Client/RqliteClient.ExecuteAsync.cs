// Maybe: Rqlite Client for .NET.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Rqlite.Client;

public sealed partial class RqliteClient : IRqliteClient
{
	/// <summary>
	/// Returns the URI path for execute requests, including timings if <see cref="IncludeTimings"/> is true.
	/// </summary>
	internal Lazy<Internals.UriBuilder> ExecuteUri
	{
		get
		{
			var uriBuilder = new Internals.UriBuilder("/db/execute");
			if (IncludeTimings)
			{
				uriBuilder.AddQueryVar("timings");
			}

			return new(uriBuilder);
		}
	}

	internal static async Task<RqliteExecuteResponse> ExecuteAsync<T>(
		IEnumerable<T> commands,
		bool asSingleTransaction,
		Internals.UriBuilder uriBuilder,
		Func<HttpRequestMessage, Task<RqliteExecuteResponse>> send
	)
	{
		if (!commands.Any())
		{
			return new RqliteExecuteResponse("You must pass at least one command.");
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

		try
		{
			return await send(request);
		}
		catch (Exception ex)
		{
			return new RqliteExecuteResponse(ex);
		}
	}

	/// <inheritdoc/>
	public Task<RqliteExecuteResponse> ExecuteAsync(params string[] commands) =>
		ExecuteAsync(false, commands);

	/// <inheritdoc/>
	public Task<RqliteExecuteResponse> ExecuteAsync(bool asSingleTransaction, params string[] commands) =>
		ExecuteAsync(
			commands: commands,
			asSingleTransaction: asSingleTransaction,
			uriBuilder: ExecuteUri.Value,
			send: SendAsync<RqliteExecuteResponse>
		);

	/// <inheritdoc/>
	public Task<RqliteExecuteResponse> ExecuteAsync(string command, object param) =>
		ExecuteAsync(false, (command, param));

	/// <inheritdoc/>
	public Task<RqliteExecuteResponse> ExecuteAsync(params (string command, object param)[] commands) =>
		ExecuteAsync(false, commands);

	/// <inheritdoc/>
	public Task<RqliteExecuteResponse> ExecuteAsync(bool asSingleTransaction, params (string command, object param)[] commands) =>
		ExecuteAsync(
			commands: from c in commands select new[] { c.command, c.param },
			asSingleTransaction: asSingleTransaction,
			uriBuilder: ExecuteUri.Value,
			send: SendAsync<RqliteExecuteResponse>
		);
}
