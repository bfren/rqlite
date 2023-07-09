// Maybe: Rqlite Client for .NET.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using System;
using System.Collections.Generic;
using System.Text.Json;

namespace Rqlite.Client;

/// <summary>
/// Rqlite response from a query request.
/// </summary>
public sealed record class RqliteQueryResponse : RqliteResponse<RqliteQueryResponse.Result>
{
	/// <inheritdoc/>
	public RqliteQueryResponse() : base() { }

	/// <inheritdoc/>
	internal RqliteQueryResponse(string error) : base(error) { }

	/// <inheritdoc/>
	internal RqliteQueryResponse(Exception exception) : base(exception) { }

	/// <summary>
	/// Result properties.
	/// </summary>
	public sealed record class Result : RqliteResponseResult
	{
		/// <summary>
		/// List of column types.
		/// </summary>
		public List<string>? Types { get; init; }

		/// <summary>
		/// List of column names.
		/// </summary>
		public List<string>? Columns { get; init; }

		/// <summary>
		/// List of values.
		/// </summary>
		public List<List<JsonElement>>? Values { get; init; }
	}
}

public sealed record class RqliteQueryResponse<T> : RqliteResponse<RqliteQueryResponse<T>.Result>
{
	/// <inheritdoc/>
	public RqliteQueryResponse() : base() { }

	/// <inheritdoc/>
	internal RqliteQueryResponse(string error) : base(error) { }

	/// <inheritdoc/>
	internal RqliteQueryResponse(Exception exception) : base(exception) { }

	/// <summary>
	/// Result properties.
	/// </summary>
	public sealed record class Result : RqliteResponseResult
	{
		/// <summary>
		/// Dictionary of column types, where the key is the column name and the value is the type.
		/// </summary>
		public Dictionary<string, string>? Types { get; init; }

		/// <summary>
		/// List of values.
		/// </summary>
		public List<T>? Rows { get; init; }
	}
}
