// Rqlite client for .NET.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using System.Text.Json.Serialization;
using static Rqlite.Client.RqliteStatus;
using static Rqlite.Client.RqliteStatus.RqliteStatusHttp;

namespace Rqlite.Client;

/// <summary>
/// Rqlite status values.
/// </summary>
/// <param name="Build">Build values.</param>
/// <param name="Http">HTTP values.</param>
/// <param name="Node">Node values.</param>
/// <param name="Store">Store values.</param>
public sealed record class RqliteStatus(
	RqliteStatusBuild Build,
	RqliteStatusHttp Http,
	RqliteStatusNode Node,
	RqliteStatusStore Store
)
{
	/// <summary>
	/// Rqlite build values.
	/// </summary>
	/// <param name="Version">Rqlite version.</param>
	public sealed record class RqliteStatusBuild(
		string Version
	);

	/// <summary>
	/// Rqlite HTTP values.
	/// </summary>
	/// <param name="Auth">Whether auth is enabled or disabled.</param>
	/// <param name="BindAddr">The bind address of the connected instance.</param>
	/// <param name="Tls">Whether TLS is enabled or disabled.</param>
	public sealed record class RqliteStatusHttp(
		string Auth,
		[property: JsonPropertyName("bind_addr")] string BindAddr,
		RqliteStatusHttpTls Tls
	)
	{
		/// <summary>
		/// Rqlite TLS values.
		/// </summary>
		/// <param name="Enabled">Whether TLS is enabled or disabled.</param>
		public sealed record class RqliteStatusHttpTls(
			string Enabled
		);
	}

	/// <summary>
	/// Rqlite node values.
	/// </summary>
	/// <param name="Uptime">The length of time this node has been up.</param>
	public sealed record RqliteStatusNode(
		string Uptime
	);

	/// <summary>
	/// Rqlite store values.
	/// </summary>
	/// <param name="SizeInBytes">The size of the attached SQLite store in bytes.</param>
	public sealed record RqliteStatusStore(
		[property: JsonPropertyName("dir_size")] int SizeInBytes
	);
}
