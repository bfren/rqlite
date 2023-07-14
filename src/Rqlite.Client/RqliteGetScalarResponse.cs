// Maybe: Rqlite Client for .NET.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using System;
using System.Collections.Generic;
using System.Text.Json;

namespace Rqlite.Client;

/// <summary>
/// Rqlite response from an execute scalar request.
/// </summary>
public sealed record class RqliteGetScalarResponse<T> : RqliteResponse<RqliteGetScalarResponse<T>.Result>
{
	/// <inheritdoc/>
	public RqliteGetScalarResponse() : base() { }

	/// <inheritdoc/>
	internal RqliteGetScalarResponse(string error) : base(error) { }

	/// <inheritdoc/>
	internal RqliteGetScalarResponse(Exception exception) : base(exception) { }

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
