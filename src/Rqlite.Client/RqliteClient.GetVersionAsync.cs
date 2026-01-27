// Rqlite client for .NET.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using System.Threading.Tasks;

namespace Rqlite.Client;

public sealed partial class RqliteClient : IRqliteClient
{
	/// <inheritdoc/>
	public Task<Maybe<string>> GetVersionAsync() =>
		GetStatusAsync().MapAsync(x => x.Build.Version);
}
