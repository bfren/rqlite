// Rqlite client for .NET.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

namespace Rqlite.Internal.Response;

/// <summary>
/// Common fields in Rqlite response results.
/// </summary>
public abstract record class ResponseResult
{
	/// <summary>
	/// Set when there has been an error executing a request.
	/// </summary>
	public string? Error { get; init; }
}
