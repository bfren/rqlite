// Rqlite client for .NET.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using System.Collections.Generic;

namespace Rqlite.Internal.Response;

/// <summary>
/// Result properties.
/// </summary>
public sealed record class QueryResponseResult<T> : ResponseResult
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
