// Rqlite client for .NET.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using System;
using System.Net.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Rqlite.Client.Exceptions;
using Wrap;

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
	public IRqliteClient CreateClient() =>
		Options.DefaultClientName switch
		{
			// without any configured clients, we need to use default options
			_ when Options.Clients.Count == 0 =>
				CreateClientWithDefaults(),

			// attempt to create the specified client
			string httpClientName when Options.Clients.Count > 0 =>
				CreateClient(httpClientName),

			// we get here if:
			//   DefaultClientName is not set
			//   BUT at least one Client is configured
			_ =>
				throw new UndefinedDefaultClientException("Default HttpClient name must be defined in Rqlite settings.")
		};

	/// <inheritdoc/>
	public IRqliteClient CreateClient(string httpClientName) =>
		Options.Clients.GetValueOrNone(httpClientName).Match(
			none: () => throw new UnknownClientException($"Client '{httpClientName}' cannot be found in Rqlite settings."),
			some: x => new RqliteClient(
				httpClient: HttpClientFactory.CreateClient(httpClientName),
				includeTimings: x.IncludeTimings ?? Options.IncludeTimings,
				logger: Logger
			)
		);

	/// <inheritdoc/>
	public IRqliteClient CreateClientWithDefaults()
	{
		// get default options
		var options = new RqliteOptions();

		// create and configure the HttpClient
		var httpClient = HttpClientFactory.CreateClient();
		httpClient.BaseAddress = new(options.BaseAddress);
		httpClient.Timeout = TimeSpan.FromSeconds(options.TimeoutInSeconds);

		// return default RqliteClient
		return new RqliteClient(httpClient, options.IncludeTimings, Logger);
	}
}
