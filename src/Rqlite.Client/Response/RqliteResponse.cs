// Rqlite client for .NET.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace Rqlite.Client.Response;

/// <summary>
/// Common fields in Rqlite response results.
/// </summary>
public abstract record class RqliteResponseResult
{
	/// <summary>
	/// Set when there has been an error executing a request.
	/// </summary>
	public string? Error { get; init; }
}

/// <summary>
/// Common fields in Rqlite responses.
/// </summary>
/// <typeparam name="TResult">Result class type.</typeparam>
public abstract record class RqliteResponse<TResult>
	where TResult : RqliteResponseResult, new()
{
	/// <summary>
	/// Returns indexed list of errors while executing a request.
	/// </summary>
	[JsonIgnore]
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
	public List<TResult> Results { get; init; } = new();

	/// <summary>
	/// If IncludeTimings is set, will include the time taken to execute the request.
	/// </summary>
	public double Time { get; init; }

	/// <summary>
	/// Create an empty response.
	/// </summary>
	protected RqliteResponse() { }

	/// <summary>
	/// Create a response with an error message.
	/// </summary>
	/// <param name="error">Error message.</param>
	protected RqliteResponse(string error) : this() =>
		Results = new()
		{
			new() { Error = error }
		};

	/// <summary>
	/// Create a response from an Exception.
	/// </summary>
	/// <param name="exception">Exception object.</param>
	protected RqliteResponse(Exception exception) : this(exception.ToString()) { }
}
