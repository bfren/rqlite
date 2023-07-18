// Rqlite client for .NET.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using System;
using System.Collections.Generic;
using System.Text.Json;

namespace Rqlite.Client.Response;

/// <summary>
/// Rqlite response from an execute scalar request.
/// </summary>
public sealed record class RqliteScalarResponse<T> : RqliteResponse<RqliteScalarResponse<T>.Result>
{
	/// <inheritdoc/>
	public RqliteScalarResponse() : base() { }

	/// <inheritdoc/>
	internal RqliteScalarResponse(string error) : base(error) { }

	/// <inheritdoc/>
	internal RqliteScalarResponse(Exception exception) : base(exception) { }

	/// <summary>
	/// Result properties.
	/// </summary>
	public sealed record class Result : RqliteResponseResult
	{
		/// <summary>
		/// List of values.
		/// </summary>
		public List<List<JsonElement>>? Values { get; init; }
	}
}
