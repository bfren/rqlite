// Rqlite client for .NET.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using System;
using System.Net.Http;
using Microsoft.Extensions.Logging;
using IUriBuilder = Rqlite.Client.Internals.IUriBuilder;
using UriBuilder = Rqlite.Client.Internals.UriBuilder;

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
	/// Dispose <see cref="HttpClient"/>.
	/// </summary>
	public void Dispose() =>
		HttpClient.Dispose();
}
