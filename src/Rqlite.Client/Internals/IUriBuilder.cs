// Rqlite client for .NET.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using System;
using System.Collections.Specialized;

namespace Rqlite.Client.Internals;

/// <summary>
/// Enables building relative URIs with expandable query vars.
/// </summary>
internal interface IUriBuilder
{
	/// <summary>
	/// URI path.
	/// </summary>
	string Path { get; }

	/// <summary>
	/// Query vars collection.
	/// </summary>
	NameValueCollection QueryVars { get; }

	/// <summary>
	/// Add a query var without a value.
	/// </summary>
	/// <param name="key">Query var key.</param>
	void AddQueryVar(string key);

	/// <summary>
	/// Add a query var with a value.
	/// </summary>
	/// <param name="key">Query var key.</param>
	/// <param name="value">Query var value.</param>
	void AddQueryVar(string key, string value);

	/// <summary>
	/// Build relative URI.
	/// </summary>
	/// <returns>Relative URI built with <see cref="Path"/> and query from <see cref="QueryVars"/>.</returns>
	Uri Build();
}
