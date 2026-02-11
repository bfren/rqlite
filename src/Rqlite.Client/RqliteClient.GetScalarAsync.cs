// Rqlite client for .NET.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Rqlite.Internal.Request;
using Rqlite.Internal.Response;

namespace Rqlite.Client;

public sealed partial class RqliteClient : IRqliteClient
{
	/// <summary>
	/// Execute query and return a simple value.
	/// </summary>
	/// <typeparam name="TQuery">Query type.</typeparam>
	/// <typeparam name="TValue">Return value type.</typeparam>
	/// <param name="query">Query to execute.</param>
	/// <param name="uriBuilder">URI builder.</param>
	/// <param name="send">Asynchronous send method.</param>
	/// <returns>Query results.</returns>
	internal async Task<Result<TValue>> GetScalarAsync<TQuery, TValue>(
		TQuery query,
		IUriBuilder uriBuilder,
		Func<HttpRequestMessage, Task<Result<List<ScalarResponseResult<TValue>>>>> send)
	{
		var request = new HttpRequestMessage
		{
			Content = new JsonContent(query, JsonOptions),
			Method = HttpMethod.Post,
			RequestUri = uriBuilder.Build(),
		};

		try
		{
			return await
				send(
					request
				)
				.BindAsync(
					x => x.SelectMany(y => y.Values ?? []).SelectMany(z => z).SingleOrNone().Match(
						fNone: () => R.Fail("Did not receive exactly one value.")
							.Ctx(nameof(RqliteClient), nameof(GetScalarAsync)),
						fSome: R.Wrap
					)
				);
		}
		catch (Exception ex)
		{
			return R.Fail(ex).Ctx(nameof(RqliteClient), nameof(GetScalarAsync));
		}
	}

	/// <inheritdoc/>
	public Task<Result<T>> GetScalarAsync<T>(string query) =>
		GetScalarAsync(
			query: new[] { query },
			uriBuilder: QueryUri(),
			send: GetResultsAsync<ScalarResponseResult<T>>
		);

	/// <inheritdoc/>
	public Task<Result<T>> GetScalarAsync<T>(string query, object param) =>
		GetScalarAsync(
			query: new[] { new[] { query, param } },
			uriBuilder: QueryUri(),
			send: GetResultsAsync<ScalarResponseResult<T>>
		);

}
