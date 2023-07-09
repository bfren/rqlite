// Maybe: Rqlite Client for .NET.
// Copyright (c) bfren - licensed under https://mit.bfren.dev/2023

using System;
using System.Text.Json.Serialization;

namespace Rqlite.Client;

/// <summary>
/// Rqlite response from an execute request.
/// </summary>
public sealed record class RqliteExecuteResponse : RqliteResponse<RqliteExecuteResponse.Result>
{
	/// <inheritdoc/>
	public RqliteExecuteResponse() : base() { }

	/// <inheritdoc/>
	internal RqliteExecuteResponse(string error) : base(error) { }

	/// <inheritdoc/>
	internal RqliteExecuteResponse(Exception exception) : base(exception) { }

	/// <summary>
	/// Result properties.
	/// </summary>
	public sealed record class Result : RqliteResponseResult
	{
		/// <summary>
		/// For INSERT commands, the ID of the last item inserted.
		/// </summary>
		[JsonPropertyName("last_insert_id")]
		public int LastInsertId { get; init; }

		/// <summary>
		/// The number of rows affected by the command.
		/// </summary>
		[JsonPropertyName("rows_affected")]
		public int RowsAffected { get; init; }
	}
}
