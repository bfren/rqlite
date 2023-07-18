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
		string? version = null;
		if (response.Headers.TryGetValues("X-Rqlite-Version", out var values))
		{
			version = values.FirstOrDefault();
			Logger.Version(version);
		}

		return version ?? "Unable to retrieve version - is the Rqlite instance running?";
	}
}
