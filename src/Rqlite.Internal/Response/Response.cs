// Rqlite client for .NET.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using System;
using System.Collections.Generic;
using System.Linq;

namespace Rqlite.Internal.Response;

/// <summary>
/// Common fields in Rqlite responses.
/// </summary>
/// <typeparam name="T">Result class type.</typeparam>
public sealed record class Response<T>
	where T : ResponseResult, new()
{
	/// <summary>
	/// Returns indexed list of errors while executing a request.
	/// </summary>
	public Dictionary<int, string> Errors =>
		(
			from x in Results
			let index = Results.IndexOf(x)
			let error = x.Error
			where !string.IsNullOrEmpty(error)
			select (index, error)
		)
		.ToDictionary(x => x.index, x => x.error);

	/// <summary>
	/// Request results.
	/// </summary>
	public List<T> Results { get; init; } = [];

	/// <summary>
	/// If IncludeTimings is set, will include the time taken to execute the request.
	/// </summary>
	public double Time { get; init; }

	/// <summary>
	/// Create an empty response.
	/// </summary>
	public Response() { }

	/// <summary>
	/// Create a response with an error message.
	/// </summary>
	/// <param name="error">Error message.</param>
	public Response(string error) : this() =>
		Results =
		[
			new() { Error = error }
		];

	/// <summary>
	/// Create a response from an Exception.
	/// </summary>
	/// <param name="exception">Exception object.</param>
	public Response(Exception exception) : this(exception.ToString()) { }
}
