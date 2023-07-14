// Maybe: Rqlite Client for .NET.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Rqlite.Client.Internals;
using UriBuilder = Rqlite.Client.Internals.UriBuilder;

namespace Rqlite.Client;

public sealed partial class RqliteClient : IRqliteClient
{
	/// <inheritdoc cref="QueryAsync{TQuery, TModel}(IEnumerable{TQuery}, UriBuilder, Func{HttpRequestMessage, Task{RqliteQueryResponse{TModel}}})"/>
	internal static async Task<RqliteQueryResponse> QueryAsync<TQuery>(
		IEnumerable<TQuery> queries,
		UriBuilder uriBuilder,
		Func<HttpRequestMessage, Task<RqliteQueryResponse>> send
	)
	{
		if (!queries.Any())
		{
			return new RqliteQueryResponse("You must pass at least one query.");
		}

		var request = new HttpRequestMessage
		{
			Content = new JsonContent(queries),
			Method = HttpMethod.Post,
			RequestUri = uriBuilder.Build(),
		};

		try
		{
			return await send(request);
		}
		catch (Exception ex)
		{
			return new RqliteQueryResponse(ex);
		}
	}

	/// <summary>
	/// Execute multiple queries and return results.
	/// </summary>
	/// <typeparam name="TQuery">Query type.</typeparam>
	/// <typeparam name="TModel">Return model type.</typeparam>
	/// <param name="queries">Queries to execute.</param>
	/// <param name="uriBuilder">URI builder.</param>
	/// <param name="send">Asynchronous send method.</param>
	/// <returns>Query results.</returns>
	internal static async Task<RqliteQueryResponse<TModel>> QueryAsync<TQuery, TModel>(
		IEnumerable<TQuery> queries,
		UriBuilder uriBuilder,
		Func<HttpRequestMessage, Task<RqliteQueryResponse<TModel>>> send
	)
	{
		if (!queries.Any())
		{
			return new RqliteQueryResponse<TModel>("You must pass at least one query.");
		}

		uriBuilder.AddQueryVar("associative");

		var request = new HttpRequestMessage
		{
			Content = new JsonContent(queries),
			Method = HttpMethod.Post,
			RequestUri = uriBuilder.Build(),
		};

		try
		{
			return await send(request);
		}
		catch (Exception ex)
		{
			return new RqliteQueryResponse<TModel>(ex);
		}
	}

	/// <inheritdoc/>
	public Task<RqliteQueryResponse> QueryAsync(params string[] queries) =>
		QueryAsync(
			queries: queries,
			uriBuilder: QueryUri(),
			send: SendAsync<RqliteQueryResponse>
		);

	/// <inheritdoc/>
	public Task<RqliteQueryResponse> QueryAsync(string query, object param) =>
		QueryAsync((query, param));

	/// <inheritdoc/>
	public Task<RqliteQueryResponse> QueryAsync(params (string query, object param)[] queries) =>
		QueryAsync(
			queries: from q in queries select new[] { q.query, q.param },
			uriBuilder: QueryUri(),
			send: SendAsync<RqliteQueryResponse>
		);

	/// <inheritdoc/>
	public Task<RqliteQueryResponse<T>> QueryAsync<T>(params string[] queries) =>
		QueryAsync(
			queries: queries,
			uriBuilder: QueryUri(),
			send: SendAsync<RqliteQueryResponse<T>>
		);

	/// <inheritdoc/>
	public Task<RqliteQueryResponse<T>> QueryAsync<T>(string query, object param) =>
		QueryAsync<T>((query, param));

	/// <inheritdoc/>
	public Task<RqliteQueryResponse<T>> QueryAsync<T>(params (string query, object param)[] queries) =>
		QueryAsync(
			queries: from q in queries select new[] { q.query, q.param },
			uriBuilder: QueryUri(),
			send: SendAsync<RqliteQueryResponse<T>>
		);
}
