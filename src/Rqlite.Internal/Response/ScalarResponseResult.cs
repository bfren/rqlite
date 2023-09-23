// Rqlite client for .NET.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using System.Collections.Generic;

namespace Rqlite.Internal.Response;

/// <summary>
/// Result properties.
/// </summary>
public sealed record class ScalarResponseResult<T> : ResponseResult
{
	/// <summary>
	/// List of matching values.
	/// </summary>
	public List<List<T>>? Values { get; init; }
}
