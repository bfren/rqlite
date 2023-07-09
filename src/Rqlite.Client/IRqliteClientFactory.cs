// Maybe: Rqlite Client for .NET.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

namespace Rqlite.Client;

/// <summary>
/// Create <see cref="IRqliteClient"/> instances.
/// </summary>
public interface IRqliteClientFactory
{
	/// <summary>
	/// Create a new <see cref="IRqliteClient"/> using the default named HttpClient (see <see cref="RqliteOptions.DefaultClientName"/>.
	/// </summary>
	/// <returns>IRqliteClient instance.</returns>
	IRqliteClient CreateClient();

	/// <summary>
	/// Create a new <see cref="IRqliteClient"/> using named HttpClient <paramref name="httpClientName"/>.
	/// </summary>
	/// <param name="httpClientName">Name of a preconfigured HttpClient.</param>
	/// <returns>IRqliteClient instance.</returns>
	IRqliteClient CreateClient(string httpClientName);
}
