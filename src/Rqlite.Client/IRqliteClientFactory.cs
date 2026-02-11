// Rqlite client for .NET.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using System.Text.Json;

namespace Rqlite.Client;

/// <summary>
/// Create <see cref="IRqliteClient"/> instances.
/// </summary>
public interface IRqliteClientFactory
{
	/// <summary>
	/// Allows custom JSON serialisation (e.g. type mapping, property naming).
	/// </summary>
	JsonSerializerOptions JsonOptions { get; set; }

	/// <summary>
	/// Create a new <see cref="IRqliteClient"/> using the default named HttpClient
	/// (see <see cref="RqliteOptions.DefaultClientName"/>.
	/// </summary>
	/// <returns>IRqliteClient instance.</returns>
	IRqliteClient CreateClient();

	/// <summary>
	/// Create a new <see cref="IRqliteClient"/> using named HttpClient <paramref name="httpClientName"/>.
	/// </summary>
	/// <param name="httpClientName">Name of a preconfigured HttpClient.</param>
	/// <returns>IRqliteClient instance.</returns>
	IRqliteClient CreateClient(string httpClientName);

	/// <summary>
	/// Create a client using default values in <see cref="RqliteOptions"/>.
	/// </summary>
	/// <returns>Default RqliteClient instance.</returns>
	IRqliteClient CreateClientWithDefaults();
}
