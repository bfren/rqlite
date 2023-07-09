// Maybe: Rqlite Client for .NET.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using System.Text.Json.Serialization;
using static Rqlite.Client.RqliteStatus;
using static Rqlite.Client.RqliteStatus.RqliteStatusHttp;

namespace Rqlite.Client;

public sealed record class RqliteStatus(
	RqliteStatusBuild Build,
	RqliteStatusHttp Http,
	RqliteStatusNode Node,
	RqliteStatusStore Store
)
{
	public sealed record class RqliteStatusBuild(
		string Version
	);

	public sealed record class RqliteStatusHttp(
		string Auth,
		[property: JsonPropertyName("bind_addr")] string BindAddr,
		RqliteStatusHttpTls Tls
	)
	{
		public sealed record class RqliteStatusHttpTls(
			string Enabled
		);
	}

	public sealed record RqliteStatusNode(
		string Uptime
	);

	public sealed record RqliteStatusStore(
		[property: JsonPropertyName("dir_size")] int SizeInBytes
	);
}
