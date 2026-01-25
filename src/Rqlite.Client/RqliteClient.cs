// Rqlite client for .NET.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Rqlite.Internal;
using Rqlite.Internal.Request;
using Wrap;
using IUriBuilder = Rqlite.Internal.Request.IUriBuilder;
using UriBuilder = Rqlite.Internal.Request.UriBuilder;

namespace Rqlite.Client;

/// <inheritdoc cref="IRqliteClient"/>
public sealed partial class RqliteClient : IRqliteClient
{
	/// <summary>
	/// Returns the URI path for execute requests, optionally including timings.
	/// </summary>
	internal Func<IUriBuilder> ExecuteUri { get; private init; }

	/// <summary>
	/// Used to execute requests on Rqlite server.
	/// </summary>
	internal HttpClient HttpClient { get; private init; }

	/// <summary>
	/// Logger instance.
	/// </summary>
	internal ILogger<RqliteClient> Logger { get; private init; }

	/// <summary>
	/// Returns the URI path for query requests, optionally including timings.
	/// </summary>
	internal Func<IUriBuilder> QueryUri { get; private init; }

	/// <summary>
	/// Create database client instance using specified HttpClient.
	/// </summary>
	/// <param name="httpClient">HttpClient instance.</param>
	/// <param name="includeTimings">Whether or not to include timings with each request.</param>
	/// <param name="logger">ILogger instance.</param>
	internal RqliteClient(HttpClient httpClient, bool includeTimings, ILogger<RqliteClient> logger)
	{
		(HttpClient, Logger) = (httpClient, logger);
		ExecuteUri = () => new UriBuilder("/db/execute", includeTimings);
		QueryUri = () => new UriBuilder("/db/query", includeTimings);
	}

	/// <summary>
	/// Send a request and deserialise the JSON response.
	/// </summary>
	/// <typeparam name="T">Response type.</typeparam>
	/// <param name="request">Request message</param>
	/// <returns>Deserialised JSON response.</returns>
	internal async Task<Result<T>> SendAsync<T>(HttpRequestMessage request)
	{
		// Log the HTTP request
		Logger.Request(request);

		// Perform HTTP request and log HTTP response
		var httpResponse = await HttpClient.SendAsync(request);
		var json = await httpResponse.EnsureSuccessStatusCode().Content.ReadAsStringAsync();
		Logger.ResponseJson(json);

		// Attempt to parse response
		var rqliteResponse = JsonSerializer.Deserialize<T>(json, JsonContent.SerialiserOptions);
		if (rqliteResponse is null)
		{
			return R.Fail("'{JSON}' deserialised to a null value.", json)
				.Ctx(nameof(RqliteClient), nameof(SendAsync));
		}

		// Return response
		return rqliteResponse;
	}

	/// <summary>
	/// Dispose <see cref="HttpClient"/> property.
	/// </summary>
	public void Dispose() =>
		HttpClient.Dispose();
}
