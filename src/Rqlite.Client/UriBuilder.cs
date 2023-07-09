// Maybe: Rqlite Client for .NET.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using System;
using System.Collections.Specialized;
using System.Text;

namespace Rqlite.Client;

internal sealed class UriBuilder
{
	private string Path { get; }

	private NameValueCollection QueryVars { get; } = new();

	internal UriBuilder(string path) =>
		Path = path;

	internal void AddQueryVar(string key) =>
		QueryVars.Add(key, null);

	internal void AddQueryVar(string key, string value) =>
		QueryVars.Add(key, value);

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
