// Maybe: Rqlite Client for .NET.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using System.Collections.Generic;
using System.Net.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Rqlite.Client.Exceptions;

namespace Rqlite.Client;

/// <inheritdoc cref="IRqliteClientFactory"/>
public sealed class RqliteClientFactory : IRqliteClientFactory
{
	/// <summary>
	/// IHttpClientFactory instance.
	/// </summary>
	internal IHttpClientFactory HttpClientFactory { get; private init; }

	/// <summary>
	/// ILogger instance for RqliteClients.
	/// </summary>
	internal ILogger<RqliteClient> Logger { get; private init; }

	/// <summary>
	/// RqliteOptions instance.
	/// </summary>
	internal RqliteOptions Options { get; private init; }

	/// <summary>
	/// Create RqliteClient factory instance.
	/// </summary>
	/// <param name="httpClientFactory">IHttpClientFactory instance.</param>
	/// <param name="logger">ILogger for RqliteClient instances.</param>
	/// <param name="options">RqliteOptions instance.</param>
	public RqliteClientFactory(IHttpClientFactory httpClientFactory, ILogger<RqliteClient> logger, IOptions<RqliteOptions> options) =>
		(HttpClientFactory, Logger, Options) = (httpClientFactory, logger, options.Value);

	/// <inheritdoc/>
	public IRqliteClient CreateClient()
	{
		if (Options.DefaultClientName is string httpClientName)
		{
			return CreateClient(httpClientName);
		}
		else
		{
			throw new UndefinedDefaultClientException("Default HttpClient name must be defined in Rqlite settings.");
		}
	}

	/// <inheritdoc/>
	public IRqliteClient CreateClient(string httpClientName)
	{
		if (Options.Clients.GetValueOrDefault(httpClientName) is RqliteOptions.Client client)
		{
			var httpClient = HttpClientFactory.CreateClient(httpClientName);
			var includeTimings = client.IncludeTimings ?? Options.IncludeTimings;
			return new RqliteClient(httpClient, includeTimings, Logger);
		}

		throw new UnknownClientException($"Client '{httpClientName}' cannot be found in Rqlite settings.");
	}
}
