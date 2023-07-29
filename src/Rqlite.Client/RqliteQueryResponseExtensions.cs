// Rqlite client for .NET.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Rqlite.Client.Response;

namespace Rqlite.Client;

/// <summary>
/// <see cref="RqliteQueryResponse"/> extension methods.
/// </summary>
public static class RqliteQueryResponseExtensions
{
	/// <summary>
	/// Flatten a <see cref="RqliteQueryResponse{T}"/> by returning all results as a single list.
	/// Warning: any errors will be ignored and an empty list returned instead!
	/// </summary>
	/// <typeparam name="T">Return record type</typeparam>
	/// <param name="this">RqliteQueryResponse.</param>
	/// <returns>Flattened list of results.</returns>
	public static List<T> Flatten<T>(this RqliteQueryResponse<T> @this)
	{
		if (@this.Errors.Count > 0)
		{
			return new();
		}

		var rows = from result in @this.Results
				   from row in result.Rows ?? new()
				   select row;

		return rows.ToList();
	}

	/// <inheritdoc cref="Flatten{T}(RqliteQueryResponse{T})"/>
	public static async Task<List<T>> Flatten<T>(this Task<RqliteQueryResponse<T>> @this) =>
		Flatten(await @this);
}
