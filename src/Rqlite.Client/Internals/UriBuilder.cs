// Maybe: Rqlite Client for .NET.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using System;
using System.Collections.Specialized;
using System.Text;

namespace Rqlite.Client.Internals;

/// <summary>
/// Enables building relative URIs with expandable query vars.
/// </summary>
internal sealed class UriBuilder
{
	/// <summary>
	/// URI path.
	/// </summary>
	internal string Path { get; }

	/// <summary>
	/// Query vars collection.
	/// </summary>
	internal NameValueCollection QueryVars { get; } = new();

	/// <summary>
	/// Create builder with a URI path.
	/// </summary>
	/// <param name="path">URI path.</param>
	internal UriBuilder(string path) =>
		Path = path;

	/// <summary>
	/// Create builder and optionally include timings with all requests.
	/// </summary>
	/// <param name="path">URI path.</param>
	/// <param name="includeTimings">Whether or not to include timings with each request.</param>
	internal UriBuilder(string path, bool includeTimings) : this(path)
	{
		if (includeTimings)
		{
			AddQueryVar("timings");
		}
	}

	/// <summary>
	/// Add a query var without a value.
	/// </summary>
	/// <param name="key">Query var key.</param>
	internal void AddQueryVar(string key) =>
		QueryVars.Add(key, null);

	/// <summary>
	/// Add a query var with a value.
	/// </summary>
	/// <param name="key">Query var key.</param>
	/// <param name="value">Query var value.</param>
	internal void AddQueryVar(string key, string value) =>
		QueryVars.Add(key, value);

	/// <summary>
	/// Build relative URI.
	/// </summary>
	/// <returns></returns>
	internal Uri Build()
	{
		// Start URI with path
		var uri = new StringBuilder(Path);

		// If there are query vars we need to add them
		if (QueryVars.Count > 0)
		{
			// Start with query
			_ = uri.Append('?');

			// Add each query var
			for (var i = 0; i < QueryVars.Count; i++)
			{
				// If this is not the first iteration, append &
				if (i > 0)
				{
					_ = uri.Append('&');
				}

				// Append key name
				var key = QueryVars.AllKeys[i];
				_ = uri.Append(key);

				// Append value with = if value is not null
				var values = QueryVars.Get(key);
				if (values is not null)
				{
					_ = uri.Append('=').Append(Uri.EscapeDataString(values));
				}
			}
		}

		// Return as relative URI - the base address is defined in the named HttpClient instances
		return new(uri.ToString(), UriKind.Relative);
	}
}
