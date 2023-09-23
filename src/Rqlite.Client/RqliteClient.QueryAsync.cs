// Rqlite client for .NET.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Rqlite.Internal;
using Rqlite.Internal.Request;
using Rqlite.Internal.Response;
using Wrap;

namespace Rqlite.Client;

public sealed partial class RqliteClient : IRqliteClient
{
	/// <summary>
	/// Execute multiple queries and return results.
	/// </summary>
	/// <typeparam name="TQuery">Query type.</typeparam>
	/// <typeparam name="TModel">Return model type.</typeparam>
	/// <param name="queries">Queries to execute.</param>
	/// <param name="uriBuilder">URI builder.</param>
	/// <param name="send">Asynchronous send method.</param>
	/// <returns>Query results.</returns>
	internal static async Task<Result<List<TModel>>> QueryAsync<TQuery, TModel>(
		IEnumerable<TQuery> queries,
		IUriBuilder uriBuilder,
		Func<HttpRequestMessage, Task<Result<List<QueryResponseResult<TModel>>>>> send
	)
	{
		if (!queries.Any())
		{
			return R.Err("You must pass at least one query.");
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
			return await
				send(
					request
				)
				.MapAsync(
					x => x.SelectMany(y => y.Rows ?? new()).ToList()
				);
		}
		catch (Exception ex)
		{
			return R.Err(ex);
		}
	}

	/// <inheritdoc/>
	public Task<Result<List<T>>> QueryAsync<T>(params string[] queries) =>
		QueryAsync(
			queries: queries,
			uriBuilder: QueryUri(),
			send: GetResultsAsync<QueryResponseResult<T>>
		);

	/// <inheritdoc/>
	public Task<Result<List<T>>> QueryAsync<T>(string query, object param) =>
		QueryAsync<T>((query, param));

	/// <inheritdoc/>
	public Task<Result<List<T>>> QueryAsync<T>(params (string query, object param)[] queries) =>
		QueryAsync(
			queries: from q in queries select new[] { q.query, q.param },
			uriBuilder: QueryUri(),
			send: GetResultsAsync<QueryResponseResult<T>>
		);
}
