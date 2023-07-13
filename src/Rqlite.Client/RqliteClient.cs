// Maybe: Rqlite Client for .NET.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Rqlite.Client.Internals;
using UriBuilder = Rqlite.Client.Internals.UriBuilder;

namespace Rqlite.Client;

/// <inheritdoc cref="IRqliteClient"/>
public sealed partial class RqliteClient : IRqliteClient
{
	/// <summary>
	/// Returns the URI path for execute requests, optionally including timings.
	/// </summary>
	internal Func<UriBuilder> ExecuteUri { get; private init; }

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
	internal Func<UriBuilder> QueryUri { get; private init; }

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
	/// <exception cref="JsonException">If the JSON response returns a null value.</exception>
	internal async Task<T> SendAsync<T>(HttpRequestMessage request)
	{
		Logger.Request(request);

		var response = await HttpClient.SendAsync(request);
		var json = await response.EnsureSuccessStatusCode().Content.ReadAsStringAsync();
		Logger.ResponseJson(json);

		return JsonSerializer.Deserialize<T>(json, JsonContent.SerialiserOptions)
			?? throw new JsonException($"'{json}' deserialised to a null value.");
	}

	/// <summary>
	/// Dispose <see cref="HttpClient"/>.
	/// </summary>
	public void Dispose() =>
		HttpClient.Dispose();
}
