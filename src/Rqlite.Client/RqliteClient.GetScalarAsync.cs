// Maybe: Rqlite Client for .NET.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using System;
using System.Net.Http;
using System.Threading.Tasks;
using Rqlite.Client.Internals;
using UriBuilder = Rqlite.Client.Internals.UriBuilder;

namespace Rqlite.Client;

public sealed partial class RqliteClient : IRqliteClient
{
	/// <summary>
	/// Execute query and return a simple value.
	/// </summary>
	/// <typeparam name="TQuery">Query type.</typeparam>
	/// <typeparam name="TModel">Return model type.</typeparam>
	/// <param name="queries">Query to execute.</param>
	/// <param name="uriBuilder">URI builder.</param>
	/// <param name="send">Asynchronous send method.</param>
	/// <returns>Query results.</returns>
	internal static async Task<RqliteGetScalarResponse<TModel>> ExecuteScalarAsync<TQuery, TModel>(
		TQuery query,
		UriBuilder uriBuilder,
		Func<HttpRequestMessage, Task<RqliteGetScalarResponse<TModel>>> send
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
			return new RqliteGetScalarResponse<TModel>(ex);
		}
	}

	/// <inheritdoc/>
	public Task<RqliteGetScalarResponse<T>> GetScalarAsync<T>(string query) =>
		ExecuteScalarAsync(
			query: query,
			uriBuilder: QueryUri(),
			send: SendAsync<RqliteGetScalarResponse<T>>
		);

	/// <inheritdoc/>
	public Task<RqliteGetScalarResponse<T>> GetScalarAsync<T>(string query, object param) =>
		ExecuteScalarAsync(
			query: new[] { new[] { query, param } },
			uriBuilder: QueryUri(),
			send: SendAsync<RqliteGetScalarResponse<T>>
		);

}
