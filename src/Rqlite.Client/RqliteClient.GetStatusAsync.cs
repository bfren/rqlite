// Rqlite client for .NET.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Rqlite.Client;

public sealed partial class RqliteClient : IRqliteClient
{
	/// <inheritdoc/>
	public async Task<RqliteStatus> GetStatusAsync()
	{
		var request = new HttpRequestMessage
		{
			Method = HttpMethod.Get,
			RequestUri = new Uri("/status", UriKind.Relative)
		};

		return await SendAsync<RqliteStatus>(request);
	}
}
