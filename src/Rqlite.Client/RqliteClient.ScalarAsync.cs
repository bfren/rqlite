// Rqlite client for .NET.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using System;
using System.Net.Http;
using System.Threading.Tasks;
using Rqlite.Client.Internals;
using Rqlite.Client.Response;

namespace Rqlite.Client;

public sealed partial class RqliteClient : IRqliteClient
{
	/// <summary>
	/// Execute query and return a simple value.
	/// </summary>
	/// <typeparam name="TQuery">Query type.</typeparam>
	/// <typeparam name="TValue">Return value type.</typeparam>
	/// <param name="queries">Query to execute.</param>
	/// <param name="uriBuilder">URI builder.</param>
	/// <param name="send">Asynchronous send method.</param>
	/// <returns>Query results.</returns>
	internal static async Task<RqliteScalarResponse<TValue>> ExecuteScalarAsync<TQuery, TValue>(
		TQuery query,
		IUriBuilder uriBuilder,
		Func<HttpRequestMessage, Task<RqliteScalarResponse<TValue>>> send
	)
	{
		var request = new HttpRequestMessage
		{
			Content = new JsonContent(query),
			Method = HttpMethod.Post,
			RequestUri = uriBuilder.Build(),
		};

		try
		{
			return await send(request);
		}
		catch (Exception ex)
		{
			return new RqliteScalarResponse<TValue>(ex);
		}
	}

	/// <inheritdoc/>
	public Task<RqliteScalarResponse<T>> ScalarAsync<T>(string query) =>
		ExecuteScalarAsync(
			query: query,
			uriBuilder: QueryUri(),
			send: SendAsync<RqliteScalarResponse<T>>
		);

	/// <inheritdoc/>
	public Task<RqliteScalarResponse<T>> ScalarAsync<T>(string query, object param) =>
		ExecuteScalarAsync(
			query: new[] { new[] { query, param } },
			uriBuilder: QueryUri(),
			send: SendAsync<RqliteScalarResponse<T>>
		);

}
