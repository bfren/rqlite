// Rqlite client for .NET.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Rqlite.Client.Internals;
using Wrap;

namespace Rqlite.Client;

public sealed partial class RqliteClient : IRqliteClient
{

	/// <summary>
	/// Send a request and deserialise the JSON response.
	/// </summary>
	/// <typeparam name="T">Response type.</typeparam>
	/// <param name="request">Request message</param>
	/// <returns>Deserialised JSON response.</returns>
	/// <exception cref="JsonException">If the JSON response returns a null value.</exception>
	internal async Task<Result<List<T>>> GetResultsAsync<T>(HttpRequestMessage request)
		where T : ResponseResult, new()
	{
		try
		{
			return await
				SendAsync<Response<T>>(
					request
				)
				.BindAsync(
					x => x.Errors.Any() switch
					{
						false =>
							R.Wrap(x.Results),

						true =>
							R.Err(string.Join(Environment.NewLine, x.Errors))
					}
				);
		}
		catch (Exception ex)
		{
			// Capture any exceptions as an error result
			return R.Err(ex);
		}
	}
}
