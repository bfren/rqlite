// Rqlite client for .NET.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using System;
using System.Net.Http;
using System.Threading.Tasks;
using Rqlite.Internal;
using Wrap;

namespace Rqlite.Client;

public sealed partial class RqliteClient : IRqliteClient
{
	/// <inheritdoc/>
	public async Task<Maybe<RqliteStatus>> GetStatusAsync()
	{
		var request = new HttpRequestMessage
		{
			Method = HttpMethod.Get,
			RequestUri = new Uri("/status", UriKind.Relative)
		};

		return await SendAsync<RqliteStatus>(request).SwitchAsync(
			err: e =>
			{
				Logger.Err(e);
				return M.None;
			},
			ok: M.Wrap
		);
	}
}
