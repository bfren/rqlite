// Maybe: Rqlite Client for .NET.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using System.Collections.Generic;
using System.Net.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Rqlite.Client.Internals;

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
			string httpClientName =>
				CreateClient(httpClientName),

			_ =>
				throw new UndefinedDefaultClientException("Default HttpClient name must be defined in Rqlite settings.")
		};

	/// <inheritdoc/>
	public IRqliteClient CreateClient(string httpClientName) =>
		Options.Clients.GetValueOrDefault(httpClientName) switch
		{
			RqliteOptions.Client client =>
				new RqliteClient(
					httpClient: HttpClientFactory.CreateClient(httpClientName),
					includeTimings: client.IncludeTimings ?? Options.IncludeTimings,
					logger: Logger
				),

			_ =>
				throw new UnknownClientException($"Client '{httpClientName}' cannot be found in Rqlite settings.")
		};
}
