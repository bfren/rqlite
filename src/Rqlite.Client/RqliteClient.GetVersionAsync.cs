// Rqlite client for .NET.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Rqlite.Client.Internals;

namespace Rqlite.Client;

public sealed partial class RqliteClient : IRqliteClient
{
	/// <inheritdoc/>
	public async Task<string> GetVersionAsync()
	{
		var response = await HttpClient.GetAsync("/status");
		var version = response.Headers.GetValues("X-Rqlite-Version").FirstOrDefault();
		Logger.Version(version);
		return version ?? "Unable to retrieve version - is the Rqlite instance running?";
	}
}
