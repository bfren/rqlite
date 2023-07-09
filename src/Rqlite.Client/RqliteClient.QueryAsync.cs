// Maybe: Rqlite Client for .NET.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Rqlite.Client;

public sealed partial class RqliteClient : IRqliteClient
{
	/// <summary>
	/// Returns the URI path for query requests, including timings if <see cref="IncludeTimings"/> is true.
	/// </summary>
	internal Lazy<Internals.UriBuilder> QueryUri
	{
		get
		{
			var builder = new Internals.UriBuilder("/db/query");
			if (IncludeTimings)
			{
				builder.AddQueryVar("timings");
			}

			return new(builder);
		}
	}

	/// <inheritdoc/>
	public async Task<RqliteQueryResponse> QueryAsync(string query)
	{
		var builder = QueryUri.Value;
		builder.AddQueryVar("q", query);

		var request = new HttpRequestMessage
		{
			Method = HttpMethod.Get,
			RequestUri = builder.Build()
		};

		try
		{
			return await SendAsync<RqliteQueryResponse>(request);
		}
		catch (Exception ex)
		{
			return new RqliteQueryResponse(ex);
		}
	}

	/// <inheritdoc/>
	public async Task<RqliteQueryResponse<T>> QueryAsync<T>(string query)
	{
		var builder = QueryUri.Value;
		builder.AddQueryVar("associative");
		builder.AddQueryVar("q", query);

		var request = new HttpRequestMessage
		{
			Method = HttpMethod.Get,
			RequestUri = builder.Build()
		};

		try
		{
			return await SendAsync<RqliteQueryResponse<T>>(request);
		}
		catch (Exception ex)
		{
			return new RqliteQueryResponse<T>(ex);
		}
	}

	/// <inheritdoc/>
	public async Task<RqliteQueryResponse> QueryAsync(string query, object param)
	{
		var request = new HttpRequestMessage
		{
			Content = new JsonContent(new[] { new[] { query, param } }),
			Method = HttpMethod.Post,
			RequestUri = QueryUri.Value.Build()
		};

		try
		{
			return await SendAsync<RqliteQueryResponse>(request);
		}
		catch (Exception ex)
		{
			return new RqliteQueryResponse(ex);
		}
	}

	/// <inheritdoc/>
	public async Task<RqliteQueryResponse<T>> QueryAsync<T>(string query, object param)
	{
		var builder = QueryUri.Value;
		builder.AddQueryVar("associative");

		var request = new HttpRequestMessage
		{
			Content = new JsonContent(new[] { new[] { query, param } }),
			Method = HttpMethod.Post,
			RequestUri = builder.Build()
		};

		try
		{
			return await SendAsync<RqliteQueryResponse<T>>(request);
		}
		catch (Exception ex)
		{
			return new RqliteQueryResponse<T>(ex);
		}
	}
}
