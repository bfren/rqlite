// Maybe: Rqlite Client for .NET.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Rqlite.Client;

public sealed partial class RqliteClient : IRqliteClient
{
	/// <summary>
	/// Returns the URI path for execute requests, including timings if <see cref="IncludeTimings"/> is true.
	/// </summary>
	internal Lazy<UriBuilder> ExecuteUri
	{
		get
		{
			var builder = new UriBuilder("/db/execute");
			if (IncludeTimings)
			{
				builder.AddQueryVar("timings");
			}

			return new(builder);
		}
	}

	/// <inheritdoc/>
	public async Task<RqliteExecuteResponse> ExecuteAsync(params string[] commands)
	{
		if (commands.Length == 0)
		{
			return new RqliteExecuteResponse("You must pass at least one command.");
		}

		var request = new HttpRequestMessage
		{
			Content = new JsonContent(commands),
			Method = HttpMethod.Post,
			RequestUri = ExecuteUri.Value.Build(),
		};

		try
		{
			return await SendAsync<RqliteExecuteResponse>(request);
		}
		catch (Exception ex)
		{
			return new RqliteExecuteResponse(ex);
		}
	}

	/// <inheritdoc/>
	public Task<RqliteExecuteResponse> ExecuteAsync(string command, object param) =>
		ExecuteAsync(commands: (command, param));

	/// <inheritdoc/>
	public async Task<RqliteExecuteResponse> ExecuteAsync(params (string command, object param)[] commands)
	{
		var request = new HttpRequestMessage
		{
			Content = new JsonContent(from x in commands select new[] { x.command, x.param }),
			Method = HttpMethod.Post,
			RequestUri = ExecuteUri.Value.Build(),
		};

		try
		{
			return await SendAsync<RqliteExecuteResponse>(request);
		}
		catch (Exception ex)
		{
			return new RqliteExecuteResponse(ex);
		}
	}
}
