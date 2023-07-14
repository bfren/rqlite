// Maybe: Rqlite Client for .NET.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.Json;

namespace Rqlite.Client;

/// <summary>
/// <see cref="RqliteGetScalarResponse{T}"/> extension methods.
/// </summary>
public static partial class RqliteGetScalarResponseExtensions
{
	/// <summary>
	/// Converts a <see cref="JsonElement"/> to <typeparamref name="T"/>.
	/// </summary>
	/// <typeparam name="T">Return type.</typeparam>
	/// <param name="element">Input JsonElement.</param>
	/// <param name="value">Output value.</param>
	/// <returns>True if conversion succeeds.</returns>
	internal delegate bool Convert<T>(JsonElement element, out T value);

	/// <summary>
	/// Flatten a <see cref="RqliteGetScalarResponse{T}"/> by returning first value or <paramref name="defaultValue"/>.
	/// Warning: any errors will be ignored and an empty list returned instead!
	/// </summary>
	/// <typeparam name="T">Return value type.</typeparam>
	/// <param name="response">RqliteGetScalarResponse.</param>
	/// <param name="convert">Conversion method.</param>
	/// <param name="defaultValue">Default value to return on error.</param>
	/// <returns>First value converted to <typeparamref name="T"/>, or <paramref name="defaultValue"/>.</returns>
	internal static T Flatten<T>(RqliteGetScalarResponse<T> response, Convert<T> convert, [DisallowNull] T defaultValue)
	{
		if (response.Errors.Count > 0)
		{
			return defaultValue;
		}

		var elements = from r in response.Results
					   from v in r.Values ?? new()
					   from e in v
					   select e;

		return elements.FirstOrDefault() switch
		{
			JsonElement e when convert(e, out T value) =>
				value,

			_ =>
				defaultValue
		};
	}
}
