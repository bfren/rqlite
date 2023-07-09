// Maybe: Rqlite Client for .NET.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using System.Collections.Generic;

namespace Rqlite.Client;

/// <summary>
/// Rqlite configuration options.
/// </summary>
public sealed record class RqliteOptions
{
	/// <summary>
	/// The name of the default configured HttpClient instance.
	/// </summary>
	public string? DefaultClientName { get; init; }

	/// <summary>
	/// If set to true, timings will be included with each request.
	/// </summary>
	public bool IncludeTimings { get; init; } = true;

	/// <summary>
	/// The default timeout in seconds.
	/// </summary>
	public int TimeoutInSeconds { get; init; } = 30;

	/// <summary>
	/// Rqlite clients to be configured.
	/// </summary>
	public Dictionary<string, Client> Clients { get; init; } = new();

	/// <summary>
	/// Rqlite client connection details.
	/// </summary>
	public sealed record class Client
	{
		/// <summary>
		/// The base address of the Rqlite database instance (without trailing slash).
		/// </summary>
		public string BaseAddress { get; init; } = "http://localhost:4001";

		/// <summary>
		/// Set to override the global setting to include timings with each request.
		/// </summary>
		public bool? IncludeTimings { get; init; }

		/// <summary>
		/// Set to override the global timeout in seconds for this client.
		/// </summary>
		public int? TimeoutInSeconds { get; init; }
	}
}
